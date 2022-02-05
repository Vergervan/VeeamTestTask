using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace VacanciesApp.Models
{
    public class VacanciesPage
    {
        private IWebDriver driver;
        private string pageUrl = "https://careers.veeam.ru/vacancies";
#pragma warning disable CS0649
        [FindsBy(How = How.XPath, Using = ".//div[@class='col-12 col-lg-4']/div/div[1]/input")]
        private IWebElement keyWordElement; //Ключевое слово
        [FindsBy(How = How.XPath, Using = ".//div[@class='col-12 col-lg-4']/div/div[2]/div/div")]
        private IWebElement departmentDropdownElement; //Отдел
        [FindsBy(How = How.XPath, Using = ".//div[@class='col-12 col-lg-4']/div/div[3]/div/div")]
        private IWebElement languageDropdownElement; //Язык
        [FindsBy(How = How.XPath, Using = ".//div[@class='col-12 col-lg-4']/div/div[4]/div/div")]
        private IWebElement experienceDropdownElement; //Опыт
        [FindsBy(How = How.XPath, Using = ".//div[@class='col-12 col-lg-4']/div/div[5]/div/div")]
        private IWebElement regionDropdownElement; //Регион
        [FindsBy(How = How.XPath, Using = ".//div[@class='col-12 col-lg-4']/div/div[6]/button")]
        private IWebElement resetButton; //Кнопка сброса фильтров
#pragma warning restore CS0649

        public VacanciesPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        public void GoToPage()
        {
            driver.Navigate().GoToUrl(pageUrl);
        }

        public void SetKeywordText(string keyWordText)
        {
            keyWordElement.SendKeys(keyWordText);
        }

        public IWebElement GetKeywordEditElement() => keyWordElement;

        public ICollection<IWebElement> GetLanguages()
        {
            //Без первичного нажатия на выпадающий список - в коде страницы не будет элементов с вариантами
            languageDropdownElement.Click();
            var elements = languageDropdownElement.FindElements(By.XPath(".//div/div"));
            Debug.WriteLine(elements.Count);
            languageDropdownElement.Click();
            return elements;
        }
        public ObservableCollection<LanguageOption> GetLanguageNames()
        {
            ObservableCollection<LanguageOption> languages = new ObservableCollection<LanguageOption>();
            foreach(var item in GetLanguages())
            {
                languages.Add(new LanguageOption(item.FindElement(By.ClassName("custom-control-label")).GetAttribute("innerText")));
            }
            return languages;
        }
        public ICollection<IWebElement> GetDepartments()
        {
            //Без первичного нажатия на выпадающий список - в коде страницы не будет элементов с вариантами
            departmentDropdownElement.Click();
            var elements = departmentDropdownElement.FindElements(By.XPath(".//div/a[@class='dropdown-item']"));
            departmentDropdownElement.Click();
            return elements;
        }
        public ObservableCollection<string> GetDepartmentNames()
        {
            ObservableCollection<string> departments = new ObservableCollection<string>();
            foreach(var item in GetDepartments())
            {
                departments.Add(item.GetAttribute("innerText"));
            }
            return departments;
        }
        public ICollection<IWebElement> GetExperience()
        {
            //Без первичного нажатия на выпадающий список - в коде страницы не будет элементов с вариантами
            experienceDropdownElement.Click();
            var elements = experienceDropdownElement.FindElements(By.XPath(".//div/a[@class='dropdown-item']"));
            experienceDropdownElement.Click();
            return elements;
        }
        public ObservableCollection<string> GetExperienceOptions()
        {
            ObservableCollection<string> experience = new ObservableCollection<string>();
            foreach (var item in GetExperience())
            {
                experience.Add(item.GetAttribute("innerText"));
            }
            return experience;
        }
        public ICollection<IWebElement> GetRegions()
        {
            //Без первичного нажатия на выпадающий список - в коде страницы не будет элементов с вариантами
            regionDropdownElement.Click();
            var elements = regionDropdownElement.FindElements(By.XPath(".//div/a[@class='dropdown-item']"));
            regionDropdownElement.Click();
            return elements;
        }
        public ObservableCollection<string> GetRegionNames()
        {
            ObservableCollection<string> region = new ObservableCollection<string>();
            foreach (var item in GetRegions())
            {
                region.Add(item.GetAttribute("innerText"));
            }
            return region;
        }
        public void ChooseDepartment(string name)
        {
            if (string.IsNullOrEmpty(name)) return;
            ClickDepartmentsDropdown();
            foreach (var item in GetDepartments())
            {
                if (item.GetAttribute("innerText") == name)
                {
                    item.Click();
                    break;
                }
            }
        }
        public void ChooseLanguage(ICollection<LanguageOption> options)
        {
            if (options.Count <= 0) return;
            ClickLanguagesDropdown();
            foreach (var item in GetLanguages())
            {
                foreach (var option in options)
                {
                    if (item.GetAttribute("innerText") == option.LanguageName)
                    {
                        try
                        {
                            string value = item.FindElement(By.XPath(".//input")).GetAttribute("checked");
                            if (value == null) throw new Exception();
                            if (!option.Checked)
                            {
                                item.Click();
                            }
                        }
                        catch (Exception)
                        {
                            if (option.Checked)
                            {
                                item.Click();
                            }
                        }
                        break;
                    }
                }
            }
            ClickLanguagesDropdown();
        }
        public void ChooseExperience(string name)
        {
            if (string.IsNullOrEmpty(name)) return;
            ClickExperienceDropdown();
            foreach (var item in GetExperience())
            {
                if (item.GetAttribute("innerText") == name)
                {
                    item.Click();
                    break;
                }
            }
        }
        public void ChooseRegion(string name)
        {
            if (string.IsNullOrEmpty(name)) return;
            ClickRegionsDropdown();
            foreach (var item in GetRegions())
            {
                if (item.GetAttribute("innerText") == name)
                {
                    item.Click();
                    break;
                }
            }
        }
        public void ClickDepartmentsDropdown()
        {
            departmentDropdownElement.Click();
        }
        public void ClickLanguagesDropdown()
        {
            languageDropdownElement.Click();
        }
        public void ClickExperienceDropdown()
        {
            experienceDropdownElement.Click();
        }
        public void ClickRegionsDropdown()
        {
            regionDropdownElement.Click();
        }
        public void ResetFilters()
        {
            resetButton.Click();
        }
    }
}
