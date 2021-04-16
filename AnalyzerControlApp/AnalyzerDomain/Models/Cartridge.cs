using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AnalyzerDomain.Models
{
    public class Cartridge : ModelBase
    {
        public int Id { get; set; }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { 
                _description = value;
                OnPropertyChanged();
            }
        }

        private string _barcode;

        public string Barcode
        {
            get { return _barcode; }
            set { 
                _barcode = value;
                OnPropertyChanged();
            }
        }

        public Cartridge()
        {

        }
    }
}
