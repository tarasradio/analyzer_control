using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerConfiguration
{
    public interface IConfigurationProvider
    {
        void SaveConfiguration<T>(T configuration, string filename);
        T LoadConfiguration<T>(string filename);
    }
}
