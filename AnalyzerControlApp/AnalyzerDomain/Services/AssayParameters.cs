using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerDomain.Services
{
    public enum CurveMathModelType
    {
        UNDEF = -1,
        POINT = 1,
        LINEAR = 2,
        RODBARD = 3,
        SPLINE = 4
    }

    public enum KineticAssayIdType
    {
        UNDEF = -1,
        NO = 0,
        YES = 1
    }

    public enum TestProcessType
    {
        ELISA,
        CLINCHEM
    }

    public enum PreDilution
    {
        NOT_DILUTED,
        DILUTED
    }

    public class GroupNumberTemplate
    {
        public TestProcessType testProcessType;
        public string id;
        public PreDilution preDilution;
    }

    public class GroupNumber
    {
        public bool isDefined = false;
        public GroupNumberTemplate sample;
        public GroupNumberTemplate control;
        public GroupNumberTemplate calibrator;
    }

    public enum AWtoNCSWType
    {
        UNDEF = -1,
        NO = 0,
        YES = 1
    }

    public class UsedVolumes
    {
        public bool isDefined = false;
        public int Sample_NDIL = 0;
        public int Sample_DIL = 0;
        public int TW1_1_Versatile = 0;
        public int TW1_2 = 0;
        public int TW1_3 = 0;
        public int TW2_Conjugate = 0;
        public int TW3_Substrate = 0;
        public int TACW = 0;
        public int ARW1_1 = 0;
        public int ARW1_2 = 0;
        public int ARW1_3 = 0;
        public int ARW2 = 0;
        public int ARW3 = 0;
        public int ARACW = 0;
        public int ARCUV = 0;
    }

    public class PrefilledVolumes
    {
        public bool isDefined = false;
        public int TW1 = 0;
        public int TW2 = 0;
        public int TW3 = 0;
        public int ARW1 = 0;
        public int ARW2 = 0;
        public int ARW3 = 0;
    }

    public class IncubationTimes
    {
        public bool isDefined = false;
        public int inc_1 = 0;
        public int inc_2 = 0;
        public int inc_3 = 0;
        public int inc_4 = 0;
        public int inc_5 = 0;
    }

    public class Repeats
    {
        public bool isDefined = false;
        public int rep_1 = 0;
        public int rep_2 = 0;
        public int rep_3 = 0;
        public int rep_4 = 0;
    }

    public class OpticalReads
    {
        public bool isDefined = false;
        public int start = 0;
        public int end = 0;
    }

    public class Wavelengths
    {
        public bool isDefined = false;
        public int primary = 0;
        public int secondary = 0;

        public static int[] wavelengthsValues = { 0, 340, 380, 405, 450, 480, 508, 546, 570, 600, 645, 700, 800 };
    }

    public class SelectableUnits
    {
        public string firstUnits = String.Empty;
        public string secondUnits = String.Empty;
        public string firdUnits = String.Empty;
    }

    public class UnitFactors
    {
        public double firstUnitFactor = 0;
        public double secondUnitFactor = 0;
        public double firdUnitFactor = 0;
    }

    public class ConcentrationLimits
    {
        public double lowerLimit = 0;
        public double upperLimit = 0;
    }

    public enum DilutionType
    {
        UNDEF = -1,
        ONLINE_DILUTION = 0,
        PRE_DELUTION = 1
    }

    public class Dilution
    {
        public bool isDefined = false;
        public int supported_dilution = 0;
        public int default_online_dilution = 0;
        public DilutionType dilutionType = DilutionType.UNDEF;
    }

    public class AssayVersion
    {
        public bool isDefined = false;
        public int major = 0;
        public int minor = 0;
        public int sub_minor = 0;
    }

    public class AssayParameters
    {
        public int barcodeVersion { get; set; } = -1;
        public string assayName { get; set; } = "";
        public string assayShortName { get; set; } = "";
        public int kitNumber { get; set; } = -1;
        public CurveMathModelType mathModelCurve { get; set; } = CurveMathModelType.UNDEF;
        public KineticAssayIdType useKinetic { get; set; } = KineticAssayIdType.UNDEF;
        public GroupNumber groupNumber { get; set; } = new GroupNumber();
        public AWtoNCSWType AWtoNCSW { get; set; } = AWtoNCSWType.UNDEF;
        public UsedVolumes usedVolumes { get; set; } = new UsedVolumes();
        public PrefilledVolumes prefilledVolumes { get; set; } = new PrefilledVolumes();
        public IncubationTimes incubationTimes { get; set; } = new IncubationTimes();
        public Repeats repeats { get; set; } = new Repeats();
        public OpticalReads opticalReads { get; set; } = new OpticalReads();
        public Wavelengths wavelengths { get; set; } = new Wavelengths();
        public SelectableUnits unitsStrings { get; set; } = new SelectableUnits();
        public UnitFactors unitFactors { get; set; } = new UnitFactors();
        public ConcentrationLimits concentrationLimits { get; set; } = new ConcentrationLimits();
        public string articleNumber = "";
        public int calibrationFrequency { get; set; } = -1;
        public int controlFrequency { get; set; } = -1;
        public Dilution dilution { get; set; } = new Dilution();
        public AssayVersion assayVersion { get; set; } = new AssayVersion();
        public int CRC { get; set; } = -1;

        public AssayParameters()
        {

        }
    }
}
