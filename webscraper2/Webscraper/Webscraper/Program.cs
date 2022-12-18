using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Webscraper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Kies een scraper (0 = stop / 1 = Youtube / 2 = ictjob.be / 3 = Reddit): ");
            int keuze = Convert.ToInt32(Console.ReadLine());

            if (keuze == 0)
            {
                System.Environment.Exit(0);
            }
            else if (keuze == 1)
            {
                YTScraper();
            }
            else if (keuze == 2)
            {
                IctjobsScraper();
            }
            else if (keuze == 3)
            {
                RedditScraper();
            }
        }

        static void YTScraper()
        {
            Console.Write("Geef je zoekterm in: ");
            string zoekterm = Console.ReadLine();

            IWebDriver driver;
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Url = ("https://www.youtube.com/results?search_query=" + zoekterm + "&sp=CAI%253D");
            System.Threading.Thread.Sleep(2000);

            IWebElement element = driver.FindElement(By.XPath("//*[@aria-label='Akkoord gaan met het gebruik van cookies en andere gegevens voor de beschreven doeleinden']"));
            element.Click();
            System.Threading.Thread.Sleep(2000);

            List<Youtube> ytData = new List<Youtube>();

            for (int i = 1; i <= 5; i++)
            {
                IWebElement video = driver.FindElement(By.CssSelector("ytd-video-renderer:nth-child(" + i + ")"));
                IWebElement titel = video.FindElement(By.CssSelector("#title-wrapper"));
                System.Threading.Thread.Sleep(500);
                Console.WriteLine("Titel: " + titel.Text);

                System.Threading.Thread.Sleep(500);
                IWebElement link = video.FindElement(By.CssSelector("#video-title"));
                System.Threading.Thread.Sleep(500);
                Console.WriteLine("Link: " + link.GetAttribute("href"));

                System.Threading.Thread.Sleep(500);
                IWebElement uploader = video.FindElement(By.CssSelector("#channel-info"));
                System.Threading.Thread.Sleep(500);
                Console.WriteLine("Uploader: " + uploader.Text);

                System.Threading.Thread.Sleep(500);
                IWebElement views = video.FindElement(By.CssSelector("#metadata-line > span:nth-child(3)"));
                System.Threading.Thread.Sleep(500);
                Console.WriteLine("Aantal weergaven: " + views.Text);
                Console.WriteLine();
                System.Threading.Thread.Sleep(500);


                ytData.Add(new Youtube()
                {
                    titel = titel.Text,
                    link = link.GetAttribute("href"),
                    uploader = uploader.Text,
                    views = views.Text
                });

                using StreamWriter csvfile = new StreamWriter(@"C:\Users\Gebruiker\Documents\DEVOPS Project\webscraper2\json&csv\youtube.csv", true);
                csvfile.WriteLine("Titel: " + titel.Text);
                csvfile.WriteLine("Link: " + link.GetAttribute("href"));
                csvfile.WriteLine("Uploader: " + uploader.Text);
                csvfile.WriteLine("Aantal weergaven: " + views.Text);
                csvfile.WriteLine();
            }
            string jsonString = JsonSerializer.Serialize(ytData);
            File.WriteAllText(@"C:\Users\Gebruiker\Documents\DEVOPS Project\webscraper2\json&csv\youtube.json", jsonString);
        }
        static void IctjobsScraper()
        {
            Console.Write("Geef je zoekterm in: ");
            string zoekterm = Console.ReadLine();

            IWebDriver driver;
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Url = ("https://www.ictjob.be/nl/it-vacatures-zoeken?keywords=" + zoekterm);
            System.Threading.Thread.Sleep(5000);
            IWebElement element = driver.FindElement(By.CssSelector("#sort-by-date"));
            element.Click();

            List<Ictjob> ictData = new List<Ictjob>();

            for (int i = 1; i <= 6; i++)
            {
                if (i == 4)
                {

                }
                else
                {
                    IWebElement post = driver.FindElement(By.CssSelector("#search-result-body > div > ul > li:nth-child(" + i + ")"));
                    IWebElement titel = post.FindElement(By.CssSelector(".job-title"));
                    Console.WriteLine("Titel: " + titel.Text);

                    IWebElement bedrijf = post.FindElement(By.CssSelector(".job-company"));
                    Console.WriteLine("Bedrijf: " + bedrijf.Text);

                    IWebElement locatie = post.FindElement(By.CssSelector(".job-location"));
                    Console.WriteLine("Locatie: " + locatie.Text);

                    IWebElement keywords = post.FindElement(By.CssSelector(".job-keywords"));
                    Console.WriteLine("Keywords: " + keywords.Text);

                    IWebElement link = post.FindElement(By.CssSelector(".job-title"));
                    Console.WriteLine("Link naar de job: " + link.GetAttribute("href"));
                    Console.WriteLine();

                    ictData.Add(new Ictjob()
                    {
                        titel = titel.Text,
                        bedrijf = bedrijf.Text,
                        locatie = locatie.Text,
                        keywords = keywords.Text,
                        link2 = link.GetAttribute("href")
                    });


                    using StreamWriter csvfile = new StreamWriter(@"C:\Users\Gebruiker\Documents\DEVOPS Project\webscraper2\json&csv\ictjob.csv", true);
                    csvfile.WriteLine("Titel: " + titel.Text);
                    csvfile.WriteLine("Link: " + bedrijf.Text);
                    csvfile.WriteLine("Uploader: " + locatie.Text);
                    csvfile.WriteLine("Aantal weergaven: " + keywords.Text);
                    csvfile.WriteLine("Link: " + link.GetAttribute("href"));
                    csvfile.WriteLine();
                }
            }
            string jsonString = JsonSerializer.Serialize(ictData);
            File.WriteAllText(@"C:\Users\Gebruiker\Documents\DEVOPS Project\webscraper2\json&csv\ictjob.json", jsonString);
        }
        static void RedditScraper()
        {
            Console.Write("Geef je zoekterm in: ");
            string zoekterm = Console.ReadLine();

            IWebDriver driver;
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Url = ("https://www.reddit.com/search/?q=" + zoekterm + "&sort=new");

            IWebElement element = driver.FindElement(By.CssSelector("#SHORTCUT_FOCUSABLE_DIV > div:nth-child(6) > div._3q-XSJ2vokDQrvdG6mR__k > section > div > section._2BNSty-Ld4uppTeWGfEe8r > section:nth-child(1) > form > button"));
            element.Click();
            System.Threading.Thread.Sleep(1000);

            List<Reddit> redditData = new List<Reddit>();

            for (int i = 1; i <= 5; i++)
            {
                IWebElement post = driver.FindElement(By.CssSelector("#AppRouter-main-content > div > div > div._3ozFtOe6WpJEMUtxDOIvtU > div > div > div._2lzCpzHH0OvyFsvuESLurr._3SktesklDBwXt2pEl0sHY8 > div._1BJGsKulUQfhJyO19XsBph._3SktesklDBwXt2pEl0sHY8 > div._1MTbwSHIISfMYM16YhZ8kN > div.QBfRw7Rj8UkxybFpX-USO > div:nth-child(" + i + ") > div"));
                IWebElement uploader = post.FindElement(By.CssSelector("div > div > div._2n04GrCyhhQf-Kshn7akmH._37TF67KpZQl9SHbiAhz3mf._3xeOZ4NlqvpwzbB5E8QC6r > div._3AStxql1mQsrZuUIFP9xSg._1wxi9M8fCejzbsH0YGSer2 > div"));
                Console.WriteLine("Uploader: " + uploader.Text);
                IWebElement inhoud = post.FindElement(By.CssSelector("div > div > div._2n04GrCyhhQf-Kshn7akmH._19FzInkloQSdrf0rh3Omen"));
                Console.WriteLine("Inhoud: " + inhoud.Text);
                IWebElement minutes = post.FindElement(By.CssSelector("div > div > div._2n04GrCyhhQf-Kshn7akmH._37TF67KpZQl9SHbiAhz3mf._3xeOZ4NlqvpwzbB5E8QC6r > div._3AStxql1mQsrZuUIFP9xSg._1wxi9M8fCejzbsH0YGSer2 > span._2VF2J19pUIMSLJFky-7PEI"));
                Console.WriteLine("Aantal minuten geleden: " + minutes.Text);
                Console.WriteLine();


                redditData.Add(new Reddit()
                {
                   uploader = uploader.Text,
                   inhoud = inhoud.Text,
                   minutes = minutes.Text
                });

                using StreamWriter csvfile = new StreamWriter(@"C:\Users\Gebruiker\Documents\DEVOPS Project\webscraper2\json&csv\reddit.csv", true);
                csvfile.WriteLine("Uploader: " + uploader.Text);
                csvfile.WriteLine("Inhoud: " + inhoud.Text);
                csvfile.WriteLine("Aantal minuten geleden: " + minutes.Text);
                csvfile.WriteLine();
            }
            string jsonString = JsonSerializer.Serialize(redditData);
            File.WriteAllText(@"C:\Users\Gebruiker\Documents\DEVOPS Project\webscraper2\json&csv\reddit.json", jsonString);
        }
    }
}
