using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SteppersControlCore;
using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.CommunicationProtocol.StepperCommands;
using SteppersControlCore.SerialCommunication;

namespace SteppersControlApp.Views
{
    public partial class StepperControlView : UserControl
    {
        SerialHelper _helper;
        int stepperNumber = 0;

        public StepperControlView()
        {
            InitializeComponent();
        }

        public void setStepperNumber(int number)
        {
            this.stepperNumber = number;

            this.StepperName.Text = $"{number} - {Core._configuration.Steppers[number].Name}";
        }

        public void SetSerialHelper(SerialHelper helper)
        {
            this._helper = helper;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopCommand.StopType type = StopCommand.StopType.SOFT_STOP;
            _helper.SendPacket(new StopCommand(stepperNumber, type, Protocol.GetPacketId()).GetBytes());
        }

        private void buttonRev_Click(object sender, EventArgs e)
        {
            uint countSteps = (uint)EditCountSteps.Value;
            if (useMoveStepsCheck.Checked)
                move(Protocol.Direction.REV, countSteps);
            else
                run(Protocol.Direction.REV);
        }

        private void buttonFwd_Click(object sender, EventArgs e)
        {
            uint countSteps = (uint)EditCountSteps.Value;
            if (useMoveStepsCheck.Checked)
                move(Protocol.Direction.FWD, countSteps);
            else
                run(Protocol.Direction.FWD);
        }

        private void move(Protocol.Direction direction, uint countSteps)
        {
            uint speed = (uint)editFullSpeed.Value;
            _helper.SendPacket(new StopCommand(stepperNumber, StopCommand.StopType.SOFT_STOP, Protocol.GetPacketId()).GetBytes());
            _helper.SendPacket(new SetSpeedCommand(stepperNumber, speed, Protocol.GetPacketId()).GetBytes());
            _helper.SendPacket(new MoveCommand(stepperNumber, direction, countSteps, Protocol.GetPacketId()).GetBytes());
        }

        private void run(Protocol.Direction direction)
        {
            uint speed = (uint)editFullSpeed.Value;
            _helper.SendPacket(new StopCommand(stepperNumber, StopCommand.StopType.SOFT_STOP, Protocol.GetPacketId()).GetBytes());
            _helper.SendPacket(new RunCommand(stepperNumber, direction, speed, Protocol.GetPacketId()).GetBytes());
        }

        private void StepperControlView_Load(object sender, EventArgs e)
        {

        }

        private void useMoveStepsCheck_CheckedChanged(object sender, EventArgs e)
        {
            EditCountSteps.Enabled = useMoveStepsCheck.Checked;
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            goUntil(Protocol.Direction.FWD);
        }

        private void goUntil(Protocol.Direction direction)
        {
            uint speed = (uint)editFullSpeed.Value;
            
            _helper.SendPacket(new StopCommand(stepperNumber, StopCommand.StopType.SOFT_STOP, Protocol.GetPacketId()).GetBytes());
            _helper.SendPacket(new GoUntilCommand(stepperNumber, direction, speed, Protocol.GetPacketId()).GetBytes());
        }
    }
}
