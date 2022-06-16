using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnalyzerDomain.Services
{
    public class AssayParametersHandlers
    {
        static public void AssayNameHandler(AssayParameters parameters, string paramContent)
        {
            parameters.assayName = paramContent.Trim('?');
        }

        static public void AssayShortNameHandler(AssayParameters parameters, string paramContent)
        {
            parameters.assayShortName = paramContent.Trim('?');
        }

        static public void KitNumberHandler(AssayParameters parameters, string paramContent)
        {
            parameters.kitNumber = int.Parse(paramContent.Trim('?'));
        }

        static public void MathModelCurveHandler(AssayParameters parameters, string paramContent)
        {
            parameters.mathModelCurve = (CurveMathModelType)int.Parse(paramContent.Trim('?'));
        }

        static public void KineticAssayIdHandler(AssayParameters parameters, string paramContent)
        {
            string useKinetic = paramContent.Trim('?');
            if(useKinetic.StartsWith("N")) {
                parameters.useKinetic = KineticAssayIdType.NO;
            } else if(useKinetic.StartsWith("Y")) {
                parameters.useKinetic = KineticAssayIdType.YES;
            }
        }

        static public void GroupNumberHandler(AssayParameters parameters, string paramContent)
        {
            Regex paramRegex = new Regex(@"([A-Z])([0-9][A-Z])([A-Z])");

            MatchCollection collection = paramRegex.Matches(paramContent);

            parameters.groupNumber.sample = new GroupNumberTemplate();
            parameters.groupNumber.control = new GroupNumberTemplate();
            parameters.groupNumber.calibrator = new GroupNumberTemplate();

            FillGroupTemplate(collection[0], parameters.groupNumber.sample);
            FillGroupTemplate(collection[1], parameters.groupNumber.control);
            FillGroupTemplate(collection[2], parameters.groupNumber.calibrator);
        }

        static void FillGroupTemplate(Match match, GroupNumberTemplate groupNumber)
        {
            string testProcessType = match.Groups[1].Value;
            string id = match.Groups[2].Value;
            string preDelution = match.Groups[3].Value;

            if (testProcessType.StartsWith("E")) {
                groupNumber.testProcessType = TestProcessType.ELISA;
            } else {
                groupNumber.testProcessType = TestProcessType.CLINCHEM;
            }

            groupNumber.id = id;

            if(preDelution.StartsWith("N")) {
                groupNumber.preDilution = PreDilution.NOT_DILUTED;
            } else  {
                groupNumber.preDilution = PreDilution.DILUTED;
            }
        }

        static public void AWtoNCSWHandler(AssayParameters parameters, string paramContent)
        {
            string AWtoNCSW = paramContent.Trim('?');
            if (AWtoNCSW.StartsWith("N"))
            {
                parameters.AWtoNCSW = AWtoNCSWType.NO;
            }
            else if (AWtoNCSW.StartsWith("Y"))
            {
                parameters.AWtoNCSW = AWtoNCSWType.YES;
            }
        }

        static public void UsedVolumesHandler(AssayParameters parameters, string paramContent)
        {
            Regex paramRegex = new Regex(@"([0-9]{3})");

            MatchCollection collection = paramRegex.Matches(paramContent);

            parameters.usedVolumes.Sample_NDIL = int.Parse(collection[0].Groups[1].Value);
            parameters.usedVolumes.Sample_DIL = int.Parse(collection[1].Groups[1].Value);

            parameters.usedVolumes.TW1_1_Versatile = int.Parse(collection[2].Groups[1].Value);
            parameters.usedVolumes.TW1_2 = int.Parse(collection[0].Groups[1].Value);
            parameters.usedVolumes.TW1_3 = int.Parse(collection[0].Groups[1].Value);

            parameters.usedVolumes.TW2_Conjugate = int.Parse(collection[5].Groups[1].Value);
            parameters.usedVolumes.TW3_Substrate = int.Parse(collection[6].Groups[1].Value);
            parameters.usedVolumes.TACW = int.Parse(collection[7].Groups[1].Value);

            parameters.usedVolumes.ARW1_1 = int.Parse(collection[8].Groups[1].Value);
            parameters.usedVolumes.ARW1_2 = int.Parse(collection[9].Groups[1].Value);
            parameters.usedVolumes.ARW1_3 = int.Parse(collection[10].Groups[1].Value);

            parameters.usedVolumes.ARW2 = int.Parse(collection[11].Groups[1].Value);
            parameters.usedVolumes.ARW3 = int.Parse(collection[12].Groups[1].Value);
            parameters.usedVolumes.ARACW = int.Parse(collection[13].Groups[1].Value);
            parameters.usedVolumes.ARCUV = int.Parse(collection[14].Groups[1].Value);
        }

        static public void PrefilledVolumesHandler(AssayParameters parameters, string paramContent)
        {
            Regex paramRegex = new Regex(@"([0-9]{3})");

            MatchCollection collection = paramRegex.Matches(paramContent);

            parameters.prefilledVolumes.TW1 = int.Parse(collection[0].Groups[1].Value);
            parameters.prefilledVolumes.TW2 = int.Parse(collection[1].Groups[1].Value);
            parameters.prefilledVolumes.TW3 = int.Parse(collection[2].Groups[1].Value);

            parameters.prefilledVolumes.ARW1 = int.Parse(collection[3].Groups[1].Value);
            parameters.prefilledVolumes.ARW2 = int.Parse(collection[4].Groups[1].Value);
            parameters.prefilledVolumes.ARW3 = int.Parse(collection[5].Groups[1].Value);
        }

        static public void IncubationTimesHandler(AssayParameters parameters, string paramContent)
        {
            Regex paramRegex = new Regex(@"([0-9]{4})");

            MatchCollection collection = paramRegex.Matches(paramContent);

            parameters.incubationTimes.inc_1 = int.Parse(collection[0].Groups[1].Value);
            parameters.incubationTimes.inc_2 = int.Parse(collection[1].Groups[1].Value);
            parameters.incubationTimes.inc_3 = int.Parse(collection[2].Groups[1].Value);
            parameters.incubationTimes.inc_4 = int.Parse(collection[3].Groups[1].Value);
            parameters.incubationTimes.inc_5 = int.Parse(collection[4].Groups[1].Value);
        }

        static public void RepeatsHandler(AssayParameters parameters, string paramContent)
        {
            Regex paramRegex = new Regex(@"([0-9]{1})");

            MatchCollection collection = paramRegex.Matches(paramContent);

            parameters.repeats.rep_1 = int.Parse(collection[0].Groups[1].Value);
            parameters.repeats.rep_2 = int.Parse(collection[1].Groups[1].Value);
            parameters.repeats.rep_3 = int.Parse(collection[2].Groups[1].Value);
            parameters.repeats.rep_4 = int.Parse(collection[3].Groups[1].Value);
        }

        static public void OpticalReadsHandler(AssayParameters parameters, string paramContent)
        {
            Regex paramRegex = new Regex(@"([0-9]{2})");

            MatchCollection collection = paramRegex.Matches(paramContent);

            parameters.opticalReads.start = int.Parse(collection[0].Groups[1].Value);
            parameters.opticalReads.end = int.Parse(collection[1].Groups[1].Value);
        }

        static public void WaveLengthsHandler(AssayParameters parameters, string paramContent)
        {
            Regex paramRegex = new Regex(@"([0-9]{2})");

            MatchCollection collection = paramRegex.Matches(paramContent);

            parameters.wavelengths.primary = int.Parse(collection[0].Groups[1].Value);
            parameters.wavelengths.secondary = int.Parse(collection[1].Groups[1].Value);
        }

        static public void UnitStringsHandler(AssayParameters parameters, string paramContent)
        {
            Regex paramRegex = new Regex(@"([A-z\//?]{10})");

            MatchCollection collection = paramRegex.Matches(paramContent);

            parameters.unitsStrings.firstUnits = collection[0].Groups[1].ToString().Trim('?');
            parameters.unitsStrings.secondUnits = collection[1].Groups[1].ToString().Trim('?');
            parameters.unitsStrings.firdUnits = collection[2].Groups[1].ToString().Trim('?');
        }

        static public void UnitFactorsHandler(AssayParameters parameters, string paramContent)
        {
            Regex paramRegex = new Regex(@"([0-9]{5})");

            MatchCollection collection = paramRegex.Matches(paramContent);

            parameters.unitFactors.firstUnitFactor = UnsignedFloatStringParcer.Parse(collection[0].Groups[1].Value);
            parameters.unitFactors.secondUnitFactor = UnsignedFloatStringParcer.Parse(collection[1].Groups[1].Value);
            parameters.unitFactors.firdUnitFactor = UnsignedFloatStringParcer.Parse(collection[2].Groups[1].Value);
        }

        static public void ConcentrationLimitsHandler(AssayParameters parameters, string paramContent)
        {
            Regex paramRegex = new Regex(@"([0-9]{5})");

            MatchCollection collection = paramRegex.Matches(paramContent);

            parameters.concentrationLimits.lowerLimit = UnsignedFloatStringParcer.Parse(collection[0].Groups[1].Value);
            parameters.concentrationLimits.upperLimit = UnsignedFloatStringParcer.Parse(collection[1].Groups[1].Value);
        }

        static public void ArticleNumberHandler(AssayParameters parameters, string paramContent)
        {
            parameters.articleNumber = paramContent.Trim('?');
        }

        static public void CalibrationControlFrequencyHandler(AssayParameters parameters, string paramContent)
        {
            Regex paramRegex = new Regex(@"([0-9]{4})");

            MatchCollection collection = paramRegex.Matches(paramContent);

            parameters.calibrationFrequency = int.Parse(collection[0].Groups[1].Value);
            parameters.controlFrequency = int.Parse(collection[1].Groups[1].Value);
        }

        static public void DilutionHandler(AssayParameters parameters, string paramContent)
        {
            Regex paramRegex = new Regex(@"([0-9]{2})([0-9]{1})([0-9]{1})");

            MatchCollection collection = paramRegex.Matches(paramContent);

            parameters.dilution.supported_dilution = int.Parse(collection[0].Groups[1].Value);
            parameters.dilution.default_online_dilution = int.Parse(collection[0].Groups[2].Value);
            parameters.dilution.dilutionType = (DilutionType)int.Parse(collection[0].Groups[3].Value);
        }

        static public void AssayVersionHandler(AssayParameters parameters, string paramContent)
        {
            Regex paramRegex = new Regex(@"([0-9]{2})([0-9]{2})([0-9]{2})");

            MatchCollection collection = paramRegex.Matches(paramContent);

            parameters.assayVersion.major = int.Parse(collection[0].Groups[1].Value);
            parameters.assayVersion.minor = int.Parse(collection[0].Groups[2].Value);
            parameters.assayVersion.sub_minor = int.Parse(collection[0].Groups[3].Value);
        }

        static public void CRCHandler(AssayParameters parameters, string paramContent)
        {
            Console.WriteLine("Handling CRCHandler");
        }
    }

    public class AssayParametersBarcodeParser
    {
        delegate void paramHandler(AssayParameters barcode, string paramContent);

        static private Dictionary<string, paramHandler> assayParamHandlers = new Dictionary<string, paramHandler>()
        {
            { "01", new paramHandler(AssayParametersHandlers.AssayNameHandler) },
            { "02", new paramHandler(AssayParametersHandlers.AssayShortNameHandler) },
            { "03", new paramHandler(AssayParametersHandlers.KitNumberHandler) },
            { "04", new paramHandler(AssayParametersHandlers.MathModelCurveHandler) },
            { "05", new paramHandler(AssayParametersHandlers.KineticAssayIdHandler) },
            { "06", new paramHandler(AssayParametersHandlers.GroupNumberHandler) },
            { "07", new paramHandler(AssayParametersHandlers.AWtoNCSWHandler) },
            { "10", new paramHandler(AssayParametersHandlers.UsedVolumesHandler) },
            { "11", new paramHandler(AssayParametersHandlers.PrefilledVolumesHandler) },
            { "12", new paramHandler(AssayParametersHandlers.IncubationTimesHandler) },
            { "13", new paramHandler(AssayParametersHandlers.RepeatsHandler) },
            { "15", new paramHandler(AssayParametersHandlers.OpticalReadsHandler) },
            { "16", new paramHandler(AssayParametersHandlers.WaveLengthsHandler) },
            { "20", new paramHandler(AssayParametersHandlers.UnitStringsHandler) },
            { "21", new paramHandler(AssayParametersHandlers.UnitFactorsHandler) },
            { "22", new paramHandler(AssayParametersHandlers.ConcentrationLimitsHandler) },
            { "30", new paramHandler(AssayParametersHandlers.ArticleNumberHandler) },
            { "31", new paramHandler(AssayParametersHandlers.CalibrationControlFrequencyHandler) },
            { "32", new paramHandler(AssayParametersHandlers.DilutionHandler) },
            { "90", new paramHandler(AssayParametersHandlers.AssayVersionHandler) },
            { "99", new paramHandler(AssayParametersHandlers.CRCHandler) },
        };

        /// <summary>
        /// Parse assay parameters string and return AssayParameters object if success or null if barcode is incorrect
        /// </summary>
        /// <param name="barcode">Raw barcode string</param>
        /// <returns>Assay parameters object</returns>
        public static AssayParameters Parse(String barcode)
        {
            AssayParameters parameters = null;

            Regex generalRegex = new Regex(@"DRGTP(\d{2})(\(\d{2}\)[A-Za-z0-9-\s?/]+)+?DRGTP");
            Regex paramRegex = new Regex(@"\((\d{2})\)([A-Za-z0-9-\s?/]+)");
            MatchCollection generalCollection = generalRegex.Matches(barcode);

            if (generalCollection.Count == 1 &&
                generalCollection[0].Groups.Count == 3 &&
                generalCollection[0].Groups[2].Captures.Count == 21)
            {
                parameters = new AssayParameters();

                parameters.barcodeVersion = int.Parse(generalCollection[0].Groups[1].Value);
                foreach (Capture paramCapture in generalCollection[0].Groups[2].Captures)
                {
                    MatchCollection paramCollection = paramRegex.Matches(paramCapture.Value);
                    if (paramCollection.Count == 1 &&
                        paramCollection[0].Groups.Count == 3)
                    {
                        assayParamHandlers[paramCollection[0].Groups[1].Value](parameters, paramCollection[0].Groups[2].Value);
                    }
                }
                
                const string backupDateTimeFormat = "dd_MM_yyyy_#_HH_mm_ss";
                string filename = $"./barcode_{DateTime.Now.ToString(backupDateTimeFormat)}.txt";
                
                printBarcode(parameters, filename);

                //Process.Start("notepad.exe", filename);
            }
            return parameters;
        }

        public static void printBarcode (AssayParameters parameters, string filename)
        {
            // This is invariant
            NumberFormatInfo format = new NumberFormatInfo();
            // Set the decimal seperator
            format.NumberDecimalSeparator = ".";

            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"- Barcode version: {parameters.barcodeVersion}");
            builder.AppendLine($"- Assay name: {parameters.assayName}");
            builder.AppendLine($"- Assay short name: {parameters.assayShortName}");
            builder.AppendLine($"- Kit number: {parameters.kitNumber}");
            builder.AppendLine($"- Curve math model: {parameters.mathModelCurve}");
            builder.AppendLine($"- Use kinetic: {parameters.useKinetic}");

            builder.AppendLine("- Groups:");

            builder.AppendLine($"- Sample:");
            builder.AppendLine($"-- [test proccess type: {parameters.groupNumber.sample.testProcessType}]");
            builder.AppendLine($"-- [id: {parameters.groupNumber.sample.id}]");
            builder.AppendLine($"-- [pre dilution: {parameters.groupNumber.sample.preDilution}]");

            builder.AppendLine($"- Control:");
            builder.AppendLine($"-- [test proccess type: {parameters.groupNumber.control.testProcessType}]");
            builder.AppendLine($"-- [id: {parameters.groupNumber.control.id}]");
            builder.AppendLine($"-- [pre dilution: {parameters.groupNumber.control.preDilution}]");

            builder.AppendLine($"- Calibrator:");
            builder.AppendLine($"-- [test proccess type: {parameters.groupNumber.calibrator.testProcessType}]");
            builder.AppendLine($"-- [id: {parameters.groupNumber.calibrator.id}]");
            builder.AppendLine($"-- [pre dilution: {parameters.groupNumber.calibrator.preDilution}]");

            builder.AppendLine($"- AW to NCSW: {parameters.AWtoNCSW}");

            builder.AppendLine($"- Used Volumes:");
            builder.AppendLine($"-- Sample_NDIL: {parameters.usedVolumes.Sample_NDIL}");
            builder.AppendLine($"-- Sample_DIL: {parameters.usedVolumes.Sample_DIL}");
            builder.AppendLine($"-- TW1_1_Versatile: {parameters.usedVolumes.TW1_1_Versatile}");
            builder.AppendLine($"-- TW1_2: {parameters.usedVolumes.TW1_2}");
            builder.AppendLine($"-- TW1_3: {parameters.usedVolumes.TW1_3}");
            builder.AppendLine($"-- TW2_Conjugate: {parameters.usedVolumes.TW2_Conjugate}");
            builder.AppendLine($"-- TW3_Substrate: {parameters.usedVolumes.TW3_Substrate}");
            builder.AppendLine($"-- TACW: {parameters.usedVolumes.TACW}");
            builder.AppendLine($"-- ARW1_1: {parameters.usedVolumes.ARW1_1}");
            builder.AppendLine($"-- ARW1_2: {parameters.usedVolumes.ARW1_2}");
            builder.AppendLine($"-- ARW1_3: {parameters.usedVolumes.ARW1_3}");
            builder.AppendLine($"-- ARW2: {parameters.usedVolumes.ARW2}");
            builder.AppendLine($"-- ARW3: {parameters.usedVolumes.ARW3}");
            builder.AppendLine($"-- ARACW: {parameters.usedVolumes.ARACW}");
            builder.AppendLine($"-- ARCUV: {parameters.usedVolumes.ARCUV}");

            builder.AppendLine($"- Prefilled Volumes:");
            builder.AppendLine($"-- TW1: {parameters.prefilledVolumes.TW1}");
            builder.AppendLine($"-- TW2: {parameters.prefilledVolumes.TW2}");
            builder.AppendLine($"-- TW3: {parameters.prefilledVolumes.TW3}");
            builder.AppendLine($"-- ARW1: {parameters.prefilledVolumes.ARW1}");
            builder.AppendLine($"-- ARW2: {parameters.prefilledVolumes.ARW2}");
            builder.AppendLine($"-- ARW3: {parameters.prefilledVolumes.ARW3}");

            builder.AppendLine($"- Incubation times:");
            builder.AppendLine($"-- inc_1: {parameters.incubationTimes.inc_1}");
            builder.AppendLine($"-- inc_2: {parameters.incubationTimes.inc_2}");
            builder.AppendLine($"-- inc_3: {parameters.incubationTimes.inc_3}");
            builder.AppendLine($"-- inc_4: {parameters.incubationTimes.inc_4}");
            builder.AppendLine($"-- inc_5: {parameters.incubationTimes.inc_5}");

            builder.AppendLine($"- Wash and mix repeats:");
            builder.AppendLine($"-- Pipette needle: {parameters.repeats.rep_1}");
            builder.AppendLine($"-- ACW: {parameters.repeats.rep_2}");
            builder.AppendLine($"-- Mix: {parameters.repeats.rep_3}");
            builder.AppendLine($"-- Reserved: {parameters.repeats.rep_4}");

            builder.AppendLine($"- Optical Reading number to use in OD calculation:");
            builder.AppendLine($"-- start: {parameters.opticalReads.start}");
            builder.AppendLine($"-- end: {parameters.opticalReads.end}");

            builder.AppendLine($"- Primary and secondary wavelengths for optical reading:");
            builder.AppendLine($"-- primary: {Wavelengths.wavelengthsValues[parameters.wavelengths.primary]} nm");
            builder.AppendLine($"-- secondary: {Wavelengths.wavelengthsValues[parameters.wavelengths.secondary]} nm");

            builder.AppendLine($"- Three selectable units:");
            builder.AppendLine($"-- first units: {parameters.unitsStrings.firstUnits}");
            builder.AppendLine($"-- second units: {parameters.unitsStrings.secondUnits}");
            builder.AppendLine($"-- fird units: {parameters.unitsStrings.firdUnits}");

            builder.AppendLine($"- Three selectable unit factors:");
            builder.AppendLine($"-- first unit factor: {parameters.unitFactors.firstUnitFactor.ToString(format)}");
            builder.AppendLine($"-- second unit factor: {parameters.unitFactors.secondUnitFactor.ToString(format)}");
            builder.AppendLine($"-- fird unit factor: {parameters.unitFactors.firdUnitFactor.ToString(format)}");

            builder.AppendLine($"- Lower and upper concentration limits: ");
            builder.AppendLine($"-- lower: {parameters.concentrationLimits.lowerLimit.ToString(format)}");
            builder.AppendLine($"-- upper: {parameters.concentrationLimits.upperLimit.ToString(format)}");

            builder.AppendLine($"- External DRG article number: {parameters.articleNumber}");

            builder.AppendLine($"- Calibration and control frequency (hours): ");
            builder.AppendLine($"-- calibration frequency: {parameters.calibrationFrequency} hours");
            builder.AppendLine($"-- control frequency: {parameters.controlFrequency} hours");

            builder.AppendLine($"- Dilution: ");
            builder.AppendLine($"-- supported dilution: {parameters.dilution.supported_dilution}");
            builder.AppendLine($"-- default online dilution: {parameters.dilution.default_online_dilution}");
            builder.AppendLine($"-- dilution type: {parameters.dilution.dilutionType}");

            builder.AppendLine($"- Assay version: {parameters.assayVersion.major.ToString("D2")}:{parameters.assayVersion.minor.ToString("D2")}:{parameters.assayVersion.sub_minor.ToString("D2")}");

            String result = builder.ToString();
            System.IO.File.WriteAllText(filename, result);
        }
    }
}
