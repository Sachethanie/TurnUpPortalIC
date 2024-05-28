using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V123.Network;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TurnUpPortalIC.Tests
{
    public class TM_Tests
    {
        IWebDriver driver;
        WebDriverWait wait;


        [SetUp]
        public void setUp()
        {
            driver = new ChromeDriver();
            
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(50));

            
            driver.Manage().Window.Maximize();  
           driver.Navigate().GoToUrl("http://horse.industryconnect.io/Account/Login?ReturnUrl=%2f");
            LoginPage();
            HomePage();
        } 

        [TearDown]
        public void TearDown()
        {
           driver.Quit();
        }


        public void LoginPage() 
        {
            driver.FindElement(By.Id("UserName")).SendKeys("hari");
            driver.FindElement(By.Id("Password")).SendKeys("123123");
            driver.FindElement(By.XPath("//*[@id=\"loginForm\"]/form/div[3]/input[1]")).Click();

        }

        public void HomePage()
        {
            
            driver.FindElement(By.XPath("/html/body/div[3]/div/div/ul/li[5]/a")).Click();
            driver.FindElement(By.XPath("/html/body/div[3]/div/div/ul/li[5]/ul/li[3]/a")).Click();

            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"tmsGrid\"]/div[4]/a[4]")));
        }

       
        [Test,Order(1)]
        public void CreateTMRecordTest()
        {
            string newCode = "Sasha";
            string newDescription = "Test Analysis";
            string newPrice = "100";
            
            driver.FindElement(By.XPath("//*[@id=\"container\"]/p/a")).Click();
            driver.FindElement(By.XPath("//*[@id=\"TimeMaterialEditForm\"]/div/div[1]/div/span[1]/span/span[1]")).Click();
            driver.FindElement(By.XPath("//*[@id=\"Code\"]")).SendKeys(newCode);
            driver.FindElement(By.Id("Description")).SendKeys(newDescription);
            driver.FindElement(By.XPath("//*[@id=\"TimeMaterialEditForm\"]/div/div[4]/div/span[1]/span/input[1]")).SendKeys(newPrice);
            driver.FindElement(By.XPath("//*[@id=\"SaveButton\"]")).Click();

            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"tmsGrid\"]/div[4]/a[4]")));

            driver.FindElement(By.XPath("//*[@id=\"tmsGrid\"]/div[4]/a[4]")).Click();
           
            Thread.Sleep(5000);
            //wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"tmsGrid\"]/div[4]/a[4]")));            

            var table = driver.FindElement(By.XPath("//*[@id=\"tmsGrid\"]/div[3]/table"));           

            // Locate the table body
            IWebElement tableBody = table.FindElement(By.TagName("tbody"));
            // Get all rows in the table body
            IList<IWebElement> rows = tableBody.FindElements(By.TagName("tr"));
            // Get the last row
            IWebElement lastRow = rows.Last(); 

            // Get all cells in the last row
            IList<IWebElement> cells = lastRow.FindElements(By.TagName("td"));
            String code = cells[0].Text;
            String typeCode = cells[1].Text;
            String description = cells[2].Text;
            String pricePerUnit = cells[3].Text;

           

           // String code = driver.FindElement(By.XPath("//*[@id=\"tmsGrid\"]/div[3]/table/tbody/tr[2]/td[1]")).Text;
            //String typeCode = driver.FindElement(By.XPath("//*[@id=\"tmsGrid\"]/div[3]/table/tbody/tr[2]/td[2]")).Text;
            //String description = driver.FindElement(By.XPath("//*[@id=\"tmsGrid\"]/div[3]/table/tbody/tr[2]/td[3]")).Text;
           // String pricePerUnit = driver.FindElement(By.XPath("//*[@id=\"tmsGrid\"]/div[3]/table/tbody/tr[2]/td[4]")).Text;
            
            Assert.That(code == newCode, "Code does not match");
            Assert.That(typeCode == "M", "typeCode does not match");
            Assert.That(description == newDescription, "description does not match");
            Assert.That(pricePerUnit.Contains (newPrice), "pricePerUnit does not match");

        }


     

        [Test, Order(2)]
        public void EditTMRecordTest()
           
        {

            string newCode = "Dinuka";
            string newDescription = "Business Analysis";
            string newPrice = "30";

            driver.FindElement(By.XPath("//*[@id=\"tmsGrid\"]/div[4]/a[4]")).Click();

            Thread.Sleep(5000);
            //wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"tmsGrid\"]/div[4]/a[4]")));

            //get last row of the table 

            var table = driver.FindElement(By.XPath("//*[@id=\"tmsGrid\"]/div[3]/table"));
            IWebElement tableBody = table.FindElement(By.TagName("tbody"));
            IList<IWebElement> raws = tableBody.FindElements(By.TagName("tr"));
            IWebElement lastRaw = raws.Last();

           
            // find the edit button 



            var editCell = lastRaw.FindElements(By.TagName("td"));
            IList<IWebElement> editElement = editCell[4].FindElements(By.TagName("a"));
            editElement.First().Click();

            // click edit button 

            //wait for edit page load 
            wait.Until(ExpectedConditions.ElementExists(By.Id("SaveButton")));

            //update edit row with new valus 
            var codeInput = driver.FindElement(By.XPath("//*[@id=\"Code\"]"));
            codeInput.Clear();
            codeInput.SendKeys(newCode);

            var descriptionInput = driver.FindElement(By.XPath("//*[@id=\"Description\"]"));
            descriptionInput.Clear();
            descriptionInput.SendKeys(newDescription);

            var pricePerUnit1 = driver.FindElement(By.XPath("//*[@id=\"TimeMaterialEditForm\"]/div/div[4]/div/span[1]/span/input[1]"));
            pricePerUnit1.Click();
            var PricePerUnit2 = driver.FindElement(By.XPath("//*[@id=\"Price\"]"));
            PricePerUnit2.Clear();
            pricePerUnit1.SendKeys(newPrice);

            //click save
            driver.FindElement(By.Id("SaveButton")).Click();

            //wait to load list page 
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"tmsGrid\"]/div[4]/a[4]")));

            //click pagination on last row 
            driver.FindElement(By.XPath("//*[@id=\"tmsGrid\"]/div[4]/a[4]")).Click();

            // wait to load data 
            Thread.Sleep(5000);

            // get the last row 
            var tableAfterEdit = driver.FindElement(By.XPath("//*[@id=\"tmsGrid\"]/div[3]/table"));
            IWebElement tableBodyAfterEdit = tableAfterEdit.FindElement(By.TagName("tbody"));
            IList<IWebElement> rawAfterEdit = tableBodyAfterEdit.FindElements(By.TagName("tr"));
            IWebElement lastRawAfterEdit = rawAfterEdit.Last();

            // assert new data available on the table 
            IList<IWebElement> cells = lastRawAfterEdit.FindElements(By.TagName("td"));
            String code = cells[0].Text;
            String typeCode = cells[1].Text;
            String description = cells[2].Text;
            String pricePerUnit = cells[3].Text;

            Assert.That(code == newCode, "Code does not match");
            Assert.That(description == newDescription, "description does not match");
            Assert.That(pricePerUnit.Contains(newPrice), "pricePerUnit does not match");





        }


        [Test, Order(3)]
        public void deleteTMRecordTest()
        {

            


            //click to the last page
            driver.FindElement(By.XPath("//*[@id=\"tmsGrid\"]/div[4]/a[4]")).Click();

            // wait until load the last page button
            Thread.Sleep(5000);

            ///get last row of the table 

            var table = driver.FindElement(By.XPath("//*[@id=\"tmsGrid\"]/div[3]/table"));
            IWebElement tableBody = table.FindElement(By.TagName("tbody"));
            IList<IWebElement> raws = tableBody.FindElements(By.TagName("tr"));
            IWebElement lastRaw = raws.Last();


            //find the delete buton

            var cells = lastRaw.FindElements(By.TagName("td"));
            String codeBeforeDelete = cells[0].Text;
            String typeCodeBeforeDelete = cells[1].Text;
            String descriptionBeforeDelete = cells[2].Text;
            String pricePerUnitBeforeDelete = cells[3].Text;
            IList<IWebElement> deleteElement = cells[4].FindElements(By.TagName("a"));
            
            deleteElement.Last().Click();

            
            //click the delete button


            driver.SwitchTo().Alert().Accept();

            Thread.Sleep(1000);

            //click the last raw pagination button
            driver.FindElement(By.XPath("//*[@id=\"tmsGrid\"]/div[4]/a[4]")).Click();

            //wait for page loading
            Thread.Sleep(5000);

            //find the last raw
            var tableAfterDelete = driver.FindElement(By.XPath("//*[@id=\"tmsGrid\"]/div[3]/table"));
            IWebElement tableBodyAfterDelete = tableAfterDelete.FindElement(By.TagName("tbody"));
            IList<IWebElement> rawsAfterDelete = tableBodyAfterDelete.FindElements(By.TagName("tr"));
            IWebElement lastRawAfterDelete = rawsAfterDelete.Last();

            // assert new data available on the table 
            IList<IWebElement> cellsAfterDelete = lastRawAfterDelete.FindElements(By.TagName("td"));
            String codeAfterDelete = cellsAfterDelete[0].Text;
            String typeCodeAfterDelete = cellsAfterDelete[1].Text;
            String descriptionAfterDelete = cellsAfterDelete[2].Text;
            String pricePerUnitAfterDelete = cellsAfterDelete[3].Text;

            Assert.That(codeBeforeDelete != codeAfterDelete, "Code does not match");
            Assert.That(descriptionBeforeDelete != descriptionAfterDelete, "description does not match");
            Assert.That(!pricePerUnitBeforeDelete.Contains(pricePerUnitAfterDelete), "pricePerUnit does not match");




        }
    }
}
