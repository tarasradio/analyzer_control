using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerDomain.Models
{
    public class CartridgeCassette: ModelBase
    {
        private string _barcode;

        public string Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value;
                OnPropertyChanged();
            }
        }

        private bool _inserted;

        public bool Inserted
        {
            get => _inserted;
            set
            {
                _inserted = value;
                OnPropertyChanged();
            }
        }

        private int _countLeft;

        public int CountLeft
        {
            get => _countLeft;
            set
            {
                _countLeft = value;
                OnPropertyChanged();
            }
        }

        public CartridgeCassette()
        {
            Barcode = string.Empty;
            Inserted = false;
        }
    }
}
