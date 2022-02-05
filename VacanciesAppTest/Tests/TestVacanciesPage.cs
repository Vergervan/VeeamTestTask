using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Diagnostics;
using VacanciesApp.Models;

namespace VacanciesApp.Tests
{
    [TestFixture]
    public class TestVacanciesPage
    {
        private IWebDriver driver;
        private ChromeDriverService service;

        [Test]
        public void CheckSetKeyword()
        {
            VacanciesPage vacanciesPage = new VacanciesPage(driver);
            vacanciesPage.GoToPage();
            string keywordText = "Test text";
            vacanciesPage.SetKeywordText(keywordText);
            Assert.AreEqual(keywordText, vacanciesPage.GetKeywordEditElement().GetAttribute("value"));
        }

        [Test]
        public void CheckGetDepartments()
        {
            VacanciesPage vacanciesPage = new VacanciesPage(driver);
            vacanciesPage.GoToPage();
            var departments = vacanciesPage.GetDepartmentNames();
            Assert.AreEqual(departments[0], "Все отделы");
        }

        [Test]
        public void CheckGetLanguages()
        {
            VacanciesPage vacanciesPage = new VacanciesPage(driver);
            vacanciesPage.GoToPage();
            var languages = vacanciesPage.GetLanguageNames();
            Debug.WriteLine(languages.Count);
            Debug.WriteLine("Language: " + languages[0]);
            Assert.AreEqual(languages[0], "Английский");
        }
        [Test]
        public void CheckGetExperience()
        {
            VacanciesPage vacanciesPage = new VacanciesPage(driver);
            vacanciesPage.GoToPage();
            var experience = vacanciesPage.GetExperienceOptions();
            Assert.AreEqual(experience[0], "Любой опыт");
        }
        [Test]
        public void CheckGetRegions()
        {
            VacanciesPage vacanciesPage = new VacanciesPage(driver);
            vacanciesPage.GoToPage();
            var regions = vacanciesPage.GetRegionNames();
            Assert.AreEqual(regions[0], "Любой регион");
        }
        [Test]
        public void LanguageDropdownClick()
        {
            VacanciesPage vacanciesPage = new VacanciesPage(driver);
            vacanciesPage.GoToPage();
            vacanciesPage.ClickLanguagesDropdown();
        }


        [TearDown]
        public void TearDownTest()
        {
            driver.Quit();
        }
        [SetUp]
        public void SetupTest()
        {
            service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            driver = new ChromeDriver(service);
        }

        //Для работы Debug в Visual Studio
        [OneTimeSetUp]
        public void StartTest()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
        }
        [OneTimeTearDown]
        public void EndTest()
        {
            Trace.Flush();
        }
    }
}
