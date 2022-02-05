using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacanciesApp.Models
{
    public class LanguageOption
    {
        public bool Checked { get; set; } = false;
        public string LanguageName { get; set; }

        public LanguageOption(string languageName)
        {
            LanguageName = languageName;
        }
    }
}
