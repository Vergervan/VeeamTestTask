using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacanciesApp.Utilities
{
    public class ProcessChecker
    {
        private IWebDriver driver; 
        private bool stopCheck = false;
        public event EventHandler OnClose;
        public ProcessChecker(IWebDriver driver)
        {
            this.driver = driver;
        }
        public void StartChecking(int secondsInterval)
        {
            stopCheck = false;
            Task.Run(async () =>
            {
                while (!stopCheck)
                {
                    Debug.WriteLine("Checking");
                    try
                    {
                        var val = driver.Title;
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("Exception while checking");
                        stopCheck = true;
                        OnClose?.Invoke(this, new EventArgs());
                        break;  
                    }
                    await Task.Delay(secondsInterval * 1000);
                }
            });
        }
        public void StopChecking()
        {
            stopCheck = true;
        }
    }
}
