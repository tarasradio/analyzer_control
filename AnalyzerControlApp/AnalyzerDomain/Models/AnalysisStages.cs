using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerDomain.Models
{
    public enum AnalysisStages : int
    {
        Sampling,
        Conjugate,
        EnzymeComplex,
        Substrate
    }
}
