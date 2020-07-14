using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerConfiguration
{
    public interface IConfigurationProvider<T>
    {
        void SaveConfiguration(T configuration, string filename);
        T LoadConfiguration(string filename);
    }
}
