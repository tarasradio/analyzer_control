using AnalyzerConfiguration;
using AnalyzerService;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PresentationWinForms.Forms
{
    public partial class ControlPanelWindow : Form
    {
        Analyzer analyzer;

        IList<Stepper> _stepperParams;
        public ControlPanelWindow(IList<Stepper> stepperParams)
        {
            _stepperParams = stepperParams;
            InitializeComponent();
        }

        public void Init(Analyzer analyzer)
        {
            this.analyzer = analyzer;
            initDriveControls();
        }

        private void initDriveControls()
        {
             stepperTurningView0.Init(analyzer); stepperTurningView0.SetStepperParams(_stepperParams[0]);  
             stepperTurningView1.Init(analyzer); stepperTurningView1.SetStepperParams(_stepperParams[1]);  
             stepperTurningView2.Init(analyzer); stepperTurningView2.SetStepperParams(_stepperParams[2]);  
             stepperTurningView3.Init(analyzer); stepperTurningView3.SetStepperParams(_stepperParams[3]);  
             stepperTurningView4.Init(analyzer); stepperTurningView4.SetStepperParams(_stepperParams[4]);  
             stepperTurningView5.Init(analyzer); stepperTurningView5.SetStepperParams(_stepperParams[5]);  
             stepperTurningView6.Init(analyzer); stepperTurningView6.SetStepperParams(_stepperParams[6]);  
             stepperTurningView7.Init(analyzer); stepperTurningView7.SetStepperParams(_stepperParams[7]);  
             stepperTurningView8.Init(analyzer); stepperTurningView8.SetStepperParams(_stepperParams[8]);  
             stepperTurningView9.Init(analyzer); stepperTurningView9.SetStepperParams(_stepperParams[9]);  
             stepperTurningView10.Init(analyzer); stepperTurningView10.SetStepperParams(_stepperParams[10]);
             stepperTurningView11.Init(analyzer); stepperTurningView11.SetStepperParams(_stepperParams[11]);
             stepperTurningView12.Init(analyzer); stepperTurningView12.SetStepperParams(_stepperParams[12]);
             stepperTurningView13.Init(analyzer); stepperTurningView13.SetStepperParams(_stepperParams[13]);
             stepperTurningView14.Init(analyzer); stepperTurningView14.SetStepperParams(_stepperParams[14]);
             stepperTurningView15.Init(analyzer); stepperTurningView15.SetStepperParams(_stepperParams[15]);
             stepperTurningView16.Init(analyzer); stepperTurningView16.SetStepperParams(_stepperParams[16]);
             stepperTurningView17.Init(analyzer); stepperTurningView17.SetStepperParams(_stepperParams[17]);
        }
    }
}
