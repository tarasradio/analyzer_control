using AnalyzerConfiguration;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PresentationWinForms.Forms
{
    public partial class ControlPanelForm : Form
    {
        IList<Stepper> _stepperParams;
        public ControlPanelForm(IList<Stepper> stepperParams)
        {
            _stepperParams = stepperParams;
            InitializeComponent();
            initDriveControls();
        }

        private void initDriveControls()
        {
            stepperTurningView0.SetStepperParams(_stepperParams[0]);
            stepperTurningView1.SetStepperParams(_stepperParams[1]);
            stepperTurningView2.SetStepperParams(_stepperParams[2]);
            stepperTurningView3.SetStepperParams(_stepperParams[3]);
            stepperTurningView4.SetStepperParams(_stepperParams[4]);
            stepperTurningView5.SetStepperParams(_stepperParams[5]);
            stepperTurningView6.SetStepperParams(_stepperParams[6]);
            stepperTurningView7.SetStepperParams(_stepperParams[7]);
            stepperTurningView8.SetStepperParams(_stepperParams[8]);
            stepperTurningView9.SetStepperParams(_stepperParams[9]);
            stepperTurningView10.SetStepperParams(_stepperParams[10]);
            stepperTurningView11.SetStepperParams(_stepperParams[11]);
            stepperTurningView12.SetStepperParams(_stepperParams[12]);
            stepperTurningView13.SetStepperParams(_stepperParams[13]);
            stepperTurningView14.SetStepperParams(_stepperParams[14]);
            stepperTurningView15.SetStepperParams(_stepperParams[15]);
            stepperTurningView16.SetStepperParams(_stepperParams[16]);
            stepperTurningView17.SetStepperParams(_stepperParams[17]);
        }
    }
}
