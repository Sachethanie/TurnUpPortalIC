using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnUpPortalIC.Tests
{
    public class TM_Tests
    {
        IWebDriver driver;
        [SetUp]
        public void setUp()
        {

            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();  
            driver.Navigate().GoToUrl("http://horse.industryconnect.io/Account/Login?ReturnUrl=%2f");
            loginPage();
            homePage();
        }

        [TearDown]
        public void tearDown()
        {
            driver.Quit();
        }


        public void loginPage() 
        {
            driver.FindElement(By.Id("UserName")).SendKeys("hari");
            driver.FindElement(By.Id("Password")).SendKeys("123123");
            driver.FindElement(By.XPath("//*[@id=\"loginForm\"]/form/div[3]/input[1]")).Click();

        }

        public void homePage()
        {
            
            driver.FindElement(By.XPath("/html/body/div[3]/div/div/ul/li[5]/a")).Click();
            driver.FindElement(By.XPath("/html/body/div[3]/div/div/ul/li[5]/ul/li[3]/a")).Click();

        }

       
        [Test]
        public void createTMRecordTest()
        {           
            driver.FindElement(By.XPath("//*[@id=\"container\"]/p/a")).Click();
            driver.FindElement(By.XPath("//*[@id=\"TimeMaterialEditForm\"]/div/div[1]/div/span[1]/span/span[1]")).Click();
            driver.FindElement(By.XPath("//*[@id=\"Code\"]")).SendKeys("ABC");
            driver.FindElement(By.Id("Description")).SendKeys("123");
            driver.FindElement(By.XPath("//*[@id=\"TimeMaterialEditForm\"]/div/div[4]/div/span[1]/span/input[1]")).SendKeys("100");
            driver.FindElement(By.XPath("//*[@id=\"SaveButton\"]")).Click();           
        }

       
        [Test]
        public void editTMRecordTest() 
        { 
        
        
        }


        [Test]
        public void deleteTMRecordTest()
        { 
        
        
        }
    }
}
