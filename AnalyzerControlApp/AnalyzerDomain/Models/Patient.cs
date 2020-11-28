using System;
using System.Collections.Generic;

namespace AnalyzerDomain.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string MiddleName { get; set; }

        public List<Analysis> Analyses { get; set; }

        public Patient()
        {
            FirstName = string.Empty;
            SecondName = string.Empty;
            MiddleName = string.Empty;

            Analyses = new List<Analysis>();
        }
    }
}
