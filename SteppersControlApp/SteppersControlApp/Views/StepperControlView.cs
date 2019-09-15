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
        int _stepper = 0;

        public StepperControlView()
        {
            InitializeComponent();
        }

        public void SetStepperNumber(int number)
        {
            this._stepper = number;

            UpdateInformation();
        }

        public void UpdateInformation()
        {
            if (Core.GetConfig() == null)
                return;

            bool isReverse = Core.GetConfig().SteppersParams[_stepper].Reverse;

            if (isReverse)
            {
                buttonFwd.BackColor = Color.White;
                buttonRev.BackColor = Color.GreenYellow;
            }
            else
            {
                buttonFwd.BackColor = Color.GreenYellow;
                buttonRev.BackColor = Color.White;
            }

            labelStepperName.Text = $"{_stepper} - {Core.GetConfig().Steppers[_stepper].Name}";
            editNumberSteps.Value = Core.GetConfig().SteppersParams[_stepper].NumberSteps;
            editFullSpeed.Value = Core.GetConfig().SteppersParams[_stepper].FullSpeed;
        }

        public void SetSerialHelper(SerialHelper helper)
        {
            this._helper = helper;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopCommand.StopType type = StopCommand.StopType.SOFT_STOP;
            _helper.SendPacket(new StopCommand(_stepper, type, Protocol.GetPacketId()).GetBytes());
        }

        private void buttonRev_Click(object sender, EventArgs e)
        {
            uint countSteps = (uint)editNumberSteps.Value;
            if (useMoveStepsCheck.Checked)
                move(Protocol.Direction.REV, countSteps);
            else
                run(Protocol.Direction.REV);
        }

        private void buttonFwd_Click(object sender, EventArgs e)
        {
            uint countSteps = (uint)editNumberSteps.Value;
            if (useMoveStepsCheck.Checked)
                move(Protocol.Direction.FWD, countSteps);
            else
                run(Protocol.Direction.FWD);
        }

        private void move(Protocol.Direction direction, uint countSteps)
        {
            uint speed = (uint)editFullSpeed.Value;
            _helper.SendPacket(new StopCommand(_stepper, StopCommand.StopType.SOFT_STOP, Protocol.GetPacketId()).GetBytes());
            _helper.SendPacket(new SetSpeedCommand(_stepper, speed, Protocol.GetPacketId()).GetBytes());
            _helper.SendPacket(new MoveCommand(_stepper, direction, countSteps, Protocol.GetPacketId()).GetBytes());
        }

        private void run(Protocol.Direction direction)
        {
            uint speed = (uint)editFullSpeed.Value;
            _helper.SendPacket(new StopCommand(_stepper, StopCommand.StopType.SOFT_STOP, Protocol.GetPacketId()).GetBytes());
            _helper.SendPacket(new RunCommand(_stepper, direction, speed, Protocol.GetPacketId()).GetBytes());
        }

        private void StepperControlView_Load(object sender, EventArgs e)
        {

        }

        private void useMoveStepsCheck_CheckedChanged(object sender, EventArgs e)
        {
            editNumberSteps.Enabled = useMoveStepsCheck.Checked;
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            goUntil(Protocol.Direction.FWD);
        }

        private void goUntil(Protocol.Direction direction)
        {
            uint speed = (uint)editFullSpeed.Value;
            
            _helper.SendPacket(new StopCommand(_stepper, StopCommand.StopType.SOFT_STOP, Protocol.GetPacketId()).GetBytes());
            _helper.SendPacket(new GoUntilCommand(_stepper, direction, speed, Protocol.GetPacketId()).GetBytes());
        }
    }
}
