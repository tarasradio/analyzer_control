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
            stepperControlView0.setStepperNumber(0);
            stepperControlView1.setStepperNumber(1);
            stepperControlView2.setStepperNumber(2);
            stepperControlView3.setStepperNumber(3);
            stepperControlView4.setStepperNumber(4);
            stepperControlView5.setStepperNumber(5);
            stepperControlView6.setStepperNumber(6);
            stepperControlView7.setStepperNumber(7);
            stepperControlView8.setStepperNumber(8);
            stepperControlView9.setStepperNumber(9);
            stepperControlView10.setStepperNumber(10);
            stepperControlView11.setStepperNumber(11);
            stepperControlView12.setStepperNumber(12);
            stepperControlView13.setStepperNumber(13);
            stepperControlView14.setStepperNumber(14);
            stepperControlView15.setStepperNumber(15);
            stepperControlView16.setStepperNumber(16);
            stepperControlView17.setStepperNumber(17);

            stepperControlView0.SetSerialHelper(helper);
            stepperControlView1.SetSerialHelper(helper);
            stepperControlView2.SetSerialHelper(helper);
            stepperControlView3.SetSerialHelper(helper);
            stepperControlView4.SetSerialHelper(helper);
            stepperControlView5.SetSerialHelper(helper);
            stepperControlView6.SetSerialHelper(helper);
            stepperControlView7.SetSerialHelper(helper);
            stepperControlView8.SetSerialHelper(helper);
            stepperControlView9.SetSerialHelper(helper);
            stepperControlView10.SetSerialHelper(helper);
            stepperControlView11.SetSerialHelper(helper);
            stepperControlView12.SetSerialHelper(helper);
            stepperControlView13.SetSerialHelper(helper);
            stepperControlView14.SetSerialHelper(helper);
            stepperControlView15.SetSerialHelper(helper);
            stepperControlView16.SetSerialHelper(helper);
            stepperControlView17.SetSerialHelper(helper);
        }

        private void buttonEmmergencyStop_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < 18; i++)
                helper.SendPacket(new StopCommand(
                    i, StopCommand.StopType.HARD_STOP, 0).GetBytes());
        }
    }
}
