using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using SteppersControlCore.CommunicationProtocol.StepperCommands;
using SteppersControlCore.SerialCommunication;

namespace SteppersControlApp
{
    public partial class ControlPanelForm : Form
    {
        SerialHelper helper;

        public ControlPanelForm()
        {
            InitializeComponent();
        }

        public void setSerialHelper(SerialHelper helper)
        {
            this.helper = helper;
            initDriveControls();
        }

        private void initDriveControls()
        {
            stepperTurningView0.SetStepper(0);
            stepperTurningView1.SetStepper(1);
            stepperTurningView2.SetStepper(2);
            stepperTurningView3.SetStepper(3);
            stepperTurningView4.SetStepper(4);
            stepperTurningView5.SetStepper(5);
            stepperTurningView6.SetStepper(6);
            stepperTurningView7.SetStepper(7);
            stepperTurningView8.SetStepper(8);
            stepperTurningView9.SetStepper(9);
            stepperTurningView10.SetStepper(10);
            stepperTurningView11.SetStepper(11);
            stepperTurningView12.SetStepper(12);
            stepperTurningView13.SetStepper(13);
            stepperTurningView14.SetStepper(14);
            stepperTurningView15.SetStepper(15);
            stepperTurningView16.SetStepper(16);
            stepperTurningView17.SetStepper(17);

            stepperTurningView0.SetSerialHelper(helper);
            stepperTurningView1.SetSerialHelper(helper);
            stepperTurningView2.SetSerialHelper(helper);
            stepperTurningView3.SetSerialHelper(helper);
            stepperTurningView4.SetSerialHelper(helper);
            stepperTurningView5.SetSerialHelper(helper);
            stepperTurningView6.SetSerialHelper(helper);
            stepperTurningView7.SetSerialHelper(helper);
            stepperTurningView8.SetSerialHelper(helper);
            stepperTurningView9.SetSerialHelper(helper);
            stepperTurningView10.SetSerialHelper(helper);
            stepperTurningView11.SetSerialHelper(helper);
            stepperTurningView12.SetSerialHelper(helper);
            stepperTurningView13.SetSerialHelper(helper);
            stepperTurningView14.SetSerialHelper(helper);
            stepperTurningView15.SetSerialHelper(helper);
            stepperTurningView16.SetSerialHelper(helper);
            stepperTurningView17.SetSerialHelper(helper);
        }
    }
}
