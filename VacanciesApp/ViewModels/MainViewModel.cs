using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using VacanciesApp.Models;
using VacanciesApp.Utilities;

namespace VacanciesApp.ViewModels
{
    public class MainViewModel : BaseVM, IDisposable
    {
        private IWebDriver driver;
        private VacanciesPage page;
        private ChromeDriverService service = ChromeDriverService.CreateDefaultService();
        private ProcessChecker checker;
        private string _keywordText = string.Empty; //Значение ключевого слова
        private ObservableCollection<string> _departments, _experience, _regions; //Значения выпадающих списков на сайте
        private ObservableCollection<LanguageOption> _languages;

        private bool _isOpened = false;
        private string _selectedDepartment, _selectedExperience, _selectedRegion;

        public bool IsOpened
        {
            get => _isOpened;
            set
            {
                _isOpened = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanOpen));
            }
        }
        public bool CanOpen
        {
            get => !_isOpened;
        }
        #region FilterProperties
        public string KeywordText
        {
            get => _keywordText;
            set
            {
                _keywordText = value;
                OnPropertyChanged();
            }
        }

        public string SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                _selectedDepartment = value;
                OnPropertyChanged();
            }
        }
        public string SelectedExperience
        {
            get => _selectedExperience;
            set
            {
                _selectedExperience = value;
                OnPropertyChanged();
            }
        }
        public string SelectedRegion
        {
            get => _selectedRegion;
            set
            {
                _selectedRegion = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Commands
        public ICommand OpenBrowser
        {
            get => new ClickCommand((obj) =>
            {
                service.HideCommandPromptWindow = true;
                driver = new ChromeDriver(service);
                checker = new ProcessChecker(driver);
                checker.OnClose += (sender, args) => { IsOpened = false; };
                checker.StartChecking(5);
                driver.Manage().Window.Maximize();

                page = new VacanciesPage(driver);
                page.GoToPage();

                Debug.WriteLine("Process: " + service.ProcessId);

                _departments = page.GetDepartmentNames();
                OnPropertyChanged(nameof(Departments));
                _languages = page.GetLanguageNames();
                OnPropertyChanged(nameof(Languages));
                _experience = page.GetExperienceOptions();
                OnPropertyChanged(nameof(Experience));
                _regions = page.GetRegionNames();
                OnPropertyChanged(nameof(Regions));

                IsOpened = true;
            });
        }

        public ICommand CloseBrowser
        {
            get => new ClickCommand((obj) =>
            {
                try
                {
                    driver.Quit();
                }
                catch (Exception) { }
                IsOpened = false;
            });
        }

        public ICommand ApplyFilter
        {
            get => new ClickCommand((obj) =>
            {
                page.SetKeywordText(KeywordText);
                page.ChooseDepartment(SelectedDepartment);
                page.ChooseLanguage(Languages);
                page.ChooseExperience(SelectedExperience);
                page.ChooseRegion(SelectedRegion);
            });
        }
        public ICommand ResetFilter
        {
            get => new ClickCommand((obj) =>
            {
                page.ResetFilters();
            });
        }
        #endregion
        #region FilterOptions
        public ObservableCollection<string> Departments
        {
            get => _departments;
        }

        public ObservableCollection<LanguageOption> Languages
        {
            get => _languages;
        }
        public ObservableCollection<string> Experience
        {
            get => _experience;
        }
        public ObservableCollection<string> Regions
        {
            get => _regions;
        }
        #endregion

        public void Dispose()
        {
            Process.GetProcessById(service.ProcessId).Kill();
            driver.Quit();
            driver.Dispose();
        }
    }
}
