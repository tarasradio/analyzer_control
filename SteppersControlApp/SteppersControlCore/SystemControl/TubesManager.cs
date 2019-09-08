using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.SystemControl
{
    public class Tube
    {
        public int Id = 0;
        public bool IsHandle = false;
        public string BarCode = "";

        public Tube()
        {

        }
    };

    public class TubesManager
    {
        private int _tubesCount = 54; // общее число пробирок

        private Tube[] _tubes; // список пробирок
        
        public TubesManager()
        {
            _tubes = new Tube[_tubesCount];
        }

        public Tube[] GetTubes()
        {
            return _tubes;
        }
    }
}
