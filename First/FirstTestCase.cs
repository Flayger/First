using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.Events;
using NUnit.Framework;


namespace First
{
    class FirstTestCase
    {


    public static void RunApp()
        {
            IWebDriver driver = new ChromeDriver();
            IWebElement ele;
            Screenshot image;

            try
            {

                driver.Url = "https://mail.yahoo.com/";
                ele = driver.FindElement(By.PartialLinkText("Sign in"));
                ele.Click();
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));


                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//*[@id='login-username']")));
                ele = driver.FindElement(By.XPath("//*[@id='login-username']"));
                ele.SendKeys("arthura635@yahoo.com");

                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//*[@id='login-signin']")));
                ele = driver.FindElement(By.XPath("//*[@id='login-signin']"));
                ele.Click();

                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//*[@id='login-passwd']")));
                ele = driver.FindElement(By.XPath("//*[@id='login-passwd']"));
                ele.SendKeys("21SDF4535146");

                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//*[@id='login-signin']")));
                ele = driver.FindElement(By.XPath("//*[@id='login-signin']"));
                ele.Click();

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
                image = ((ITakesScreenshot)driver).GetScreenshot();
                image.SaveAsFile("C:/tempTest/screenshot_" + timestamp + ".png");
                driver.Close();
                driver.Quit();
            }
            catch (Exception e)
            {
                driver.Close();
                driver.Quit();
                if (e.Source != null)
                    Console.WriteLine(e);
                Console.ReadLine();
            }
        }




        public static void Main(string[] args)
        {
            RunApp();

        }
    }
}
