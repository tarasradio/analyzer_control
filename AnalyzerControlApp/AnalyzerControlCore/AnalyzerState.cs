using AnalyzerDomain.Services;
using Infrastructure;
using System;

namespace AnalyzerService
{
    public class AnalyzerState : IAnalyzerState
    {
        private object locker = new object();

        private ushort[] sensorsValues;
        private ushort[] steppersStates;

        private string tubeBarcode;
        private string cartridgeBarcode;

        private static string firmwareVersion;

        public AnalyzerState(int sensorsNumber, int steppersNumber)
        {
            this.sensorsValues = new ushort[sensorsNumber];
            this.steppersStates = new ushort[steppersNumber];
        }

        public string TubeBarcode
        {
            get
            {
                string barCode;

                lock (locker)
                {
                    barCode = tubeBarcode;
                    tubeBarcode = String.Empty;
                }

                return barCode;
            }
            set
            {
                lock (locker)
                {
                    tubeBarcode = value;
                    Logger.Debug($"New tube barcode received: {value} [{value.Length}]");
                }
            }
        }

        public string CartridgeBarcode
        {
            get
            {
                string barCode;

                lock (locker)
                {
                    barCode = cartridgeBarcode;
                    cartridgeBarcode = String.Empty;
                }

                return barCode;
            }
            set
            {
                lock (locker)
                {
                    cartridgeBarcode = value;
                    Logger.Debug($"New cartridge barcode received: {value} [{value.Length}]");

                    AssayParameters parameters = AssayParametersBarcodeParser.Parse(CartridgeBarcode);
                    if(parameters != null) {
                        Logger.Debug("Analysis parameters have been read!");
                        Logger.Debug($"- Barcode version: {parameters.barcodeVersion}");
                        Logger.Debug($"- Assay name: {parameters.assayName}");
                        Logger.Debug($"- Assay short name: {parameters.assayShortName}");
                        Logger.Debug($"- Kit number: {parameters.kitNumber}");
                        Logger.Debug($"- Curve math model: {parameters.mathModelCurve}");
                        Logger.Debug($"- Use kinetic: {parameters.useKinetic}");

                        Logger.Debug("- Groups:");

                        Logger.Debug($"- Sample:");
                        Logger.Debug($"-- [test proccess type: {parameters.groupNumber.sample.testProcessType}]");
                        Logger.Debug($"-- [id: {parameters.groupNumber.sample.id}]");
                        Logger.Debug($"-- [pre dilution: {parameters.groupNumber.sample.preDilution}]");

                        Logger.Debug($"- Control:");
                        Logger.Debug($"-- [test proccess type: {parameters.groupNumber.control.testProcessType}]");
                        Logger.Debug($"-- [id: {parameters.groupNumber.control.id}]");
                        Logger.Debug($"-- [pre dilution: {parameters.groupNumber.control.preDilution}]");

                        Logger.Debug($"- Calibrator:");
                        Logger.Debug($"-- [test proccess type: {parameters.groupNumber.calibrator.testProcessType}]");
                        Logger.Debug($"-- [id: {parameters.groupNumber.calibrator.id}]");
                        Logger.Debug($"-- [pre dilution: {parameters.groupNumber.calibrator.preDilution}]");

                        Logger.Debug($"- AW to NCSW: {parameters.AWtoNCSW}");

                        Logger.Debug($"- Used Volumes:");
                        Logger.Debug($"-- Sample_NDIL: {parameters.usedVolumes.Sample_NDIL}");
                        Logger.Debug($"-- Sample_DIL: {parameters.usedVolumes.Sample_DIL}");
                        Logger.Debug($"-- TW1_1_Versatile: {parameters.usedVolumes.TW1_1_Versatile}");
                        Logger.Debug($"-- TW1_2: {parameters.usedVolumes.TW1_2}");
                        Logger.Debug($"-- TW1_3: {parameters.usedVolumes.TW1_3}");
                        Logger.Debug($"-- TW2_Conjugate: {parameters.usedVolumes.TW2_Conjugate}");
                        Logger.Debug($"-- TW3_Substrate: {parameters.usedVolumes.TW3_Substrate}");
                        Logger.Debug($"-- TACW: {parameters.usedVolumes.TACW}");
                        Logger.Debug($"-- ARW1_1: {parameters.usedVolumes.ARW1_1}");
                        Logger.Debug($"-- ARW1_2: {parameters.usedVolumes.ARW1_2}");
                        Logger.Debug($"-- ARW1_3: {parameters.usedVolumes.ARW1_3}");
                        Logger.Debug($"-- ARW2: {parameters.usedVolumes.ARW2}");
                        Logger.Debug($"-- ARW3: {parameters.usedVolumes.ARW3}");
                        Logger.Debug($"-- ARACW: {parameters.usedVolumes.ARACW}");
                        Logger.Debug($"-- ARCUV: {parameters.usedVolumes.ARCUV}");

                        Logger.Debug($"- Prefilled Volumes:");
                        Logger.Debug($"-- TW1: {parameters.prefilledVolumes.TW1}");
                        Logger.Debug($"-- TW2: {parameters.prefilledVolumes.TW2}");
                        Logger.Debug($"-- TW3: {parameters.prefilledVolumes.TW3}");
                        Logger.Debug($"-- ARW1: {parameters.prefilledVolumes.ARW1}");
                        Logger.Debug($"-- ARW2: {parameters.prefilledVolumes.ARW2}");
                        Logger.Debug($"-- ARW3: {parameters.prefilledVolumes.ARW3}");

                        Logger.Debug($"- Incubation times:");
                        Logger.Debug($"-- inc_1: {parameters.incubationTimes.inc_1}");
                        Logger.Debug($"-- inc_2: {parameters.incubationTimes.inc_2}");
                        Logger.Debug($"-- inc_3: {parameters.incubationTimes.inc_3}");
                        Logger.Debug($"-- inc_4: {parameters.incubationTimes.inc_4}");
                        Logger.Debug($"-- inc_5: {parameters.incubationTimes.inc_5}");

                        Logger.Debug($"- Wash and mix repeats:");
                        Logger.Debug($"-- Pipette needle: {parameters.repeats.rep_1}");
                        Logger.Debug($"-- ACW: {parameters.repeats.rep_2}");
                        Logger.Debug($"-- Mix: {parameters.repeats.rep_3}");
                        Logger.Debug($"-- Reserved: {parameters.repeats.rep_4}");

                        Logger.Debug($"- Optical Reading number to use in OD calculation:");
                        Logger.Debug($"-- start: {parameters.opticalReads.start}");
                        Logger.Debug($"-- end: {parameters.opticalReads.end}");

                        Logger.Debug($"- Primary and secondary wavelengths for optical reading:");
                        Logger.Debug($"-- primary: {Wavelengths.wavelengthsValues[parameters.wavelengths.primary]} nm");
                        Logger.Debug($"-- secondary: {Wavelengths.wavelengthsValues[parameters.wavelengths.secondary]} nm");

                        Logger.Debug($"- Three selectable units:");
                        Logger.Debug($"-- first units: {parameters.unitsStrings.firstUnits}");
                        Logger.Debug($"-- second units: {parameters.unitsStrings.secondUnits}");
                        Logger.Debug($"-- fird units: {parameters.unitsStrings.firdUnits}");

                        Logger.Debug($"- Three selectable unit factors:");
                        Logger.Debug($"-- first unit factor: {parameters.unitFactors.firstUnitFactor}");
                        Logger.Debug($"-- second unit factor: {parameters.unitFactors.secondUnitFactor}");
                        Logger.Debug($"-- fird unit factor: {parameters.unitFactors.firdUnitFactor}");

                        Logger.Debug($"- Lower and upper concentration limits: ");
                        Logger.Debug($"-- lower: {parameters.concentrationLimits.lowerLimit}");
                        Logger.Debug($"-- upper: {parameters.concentrationLimits.upperLimit}");

                        Logger.Debug($"- External DRG article number: {parameters.articleNumber}");

                        Logger.Debug($"- Calibration and control frequency (hours): ");
                        Logger.Debug($"-- calibration frequency: {parameters.calibrationFrequency} hours");
                        Logger.Debug($"-- control frequency: {parameters.controlFrequency} hours");

                        Logger.Debug($"- Dilution: ");
                        Logger.Debug($"-- supported dilution: {parameters.dilution.supported_dilution}");
                        Logger.Debug($"-- default online dilution: {parameters.dilution.default_online_dilution}");
                        Logger.Debug($"-- dilution type: {parameters.dilution.dilutionType}");

                        Logger.Debug($"- Assay version: {parameters.assayVersion.major.ToString("D2")}:{parameters.assayVersion.minor.ToString("D2")}:{parameters.assayVersion.sub_minor.ToString("D2")}");

                    } else {
                        Logger.Debug("Invalid barcode! The analysis parameters have not been read!");
                    }
                }
            }
        }

        public ushort[] SensorsValues
        {
            get
            {
                ushort[] values = new ushort[sensorsValues.Length];

                lock (locker)
                {
                    Array.Copy(sensorsValues, values, sensorsValues.Length);
                }

                return values;
            }
            set
            {
                if (value.Length != sensorsValues.Length)
                    return;

                lock (locker)
                {
                    Array.Copy(value, sensorsValues, value.Length);
                }
            }
        }

        public ushort[] SteppersStates
        {
            get
            {
                ushort[] states = new ushort[steppersStates.Length];

                lock (locker)
                {
                    Array.Copy(steppersStates, states, steppersStates.Length);
                }

                return states;
            }
            set
            {
                if (value.Length != steppersStates.Length)
                    return;

                lock (locker)
                {
                    Array.Copy(value, steppersStates, value.Length);
                }
            }
        }

        public string FirmwareVersion
        {
            get
            {
                return firmwareVersion;
            }
            set
            {
                firmwareVersion = value;
            }
        }
    }
}
