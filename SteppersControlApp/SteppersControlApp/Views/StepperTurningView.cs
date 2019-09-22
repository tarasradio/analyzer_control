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
using SteppersControlCore.SerialCommunication;
using SteppersControlCore.CommunicationProtocol.StepperCommands;

namespace SteppersControlApp.Views
{
    public partial class StepperTurningView : UserControl
    {
        private int _stepper = 0;
        bool isLoading = false;

        public StepperTurningView()
        {
            InitializeComponent();
        }

        public void SetStepper(int stepper)
        {
            _stepper = stepper;
            UpdateInformation();
        }

        public void ChangeStepper(int stepper)
        {
            SaveInformation();
            SetStepper(stepper);
        }

        public void SaveInformation()
        {
            if (isLoading)
                return;
            if (Core.GetConfig() == null)
                return;

            Core.GetConfig().SteppersParams[_stepper].Reverse = checkReverse.Checked;
            Core.GetConfig().Steppers[_stepper].Name = editStepperName.Text;
            Core.GetConfig().SteppersParams[_stepper].NumberSteps = (uint)editNumberSteps.Value;
            Core.GetConfig().SteppersParams[_stepper].FullSpeed = (uint)editFullSpeed.Value;
        }
        
        public void UpdateInformation()
        {
            isLoading = true;
            if (Core.GetConfig() == null)
                return;

            bool isReverse = Core.GetConfig().SteppersParams[_stepper].Reverse;
            checkReverse.Checked = isReverse;

            if(isReverse)
            {
                buttonFwd.BackColor = Color.White;
                buttonRev.BackColor = Color.GreenYellow;
            }
            else
            {
                buttonFwd.BackColor = Color.GreenYellow;
                buttonRev.BackColor = Color.White;
            }

            editStepperName.Text = Core.GetConfig().Steppers[_stepper].Name;
            editNumberSteps.Value = Core.GetConfig().SteppersParams[_stepper].NumberSteps;
            editFullSpeed.Value = Core.GetConfig().SteppersParams[_stepper].FullSpeed;
            isLoading = false;
        }

        private void checkReverse_CheckedChanged(object sender, EventArgs e)
        {
            SaveInformation();
            UpdateInformation();
        }

        private void buttonRev_Click(object sender, EventArgs e)
        {
            uint countSteps = (uint)editNumberSteps.Value;
            if (setNumberSteps.Checked)
                move(Protocol.Direction.REV, countSteps);
            else
                run(Protocol.Direction.REV);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopCommand.StopType type = StopCommand.StopType.SOFT_STOP;
            Core.Serial.SendPacket(new StopCommand(_stepper, type).GetBytes());
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            bool isReverse = Core.GetConfig().SteppersParams[_stepper].Reverse;
            Protocol.Direction direction = isReverse ? Protocol.Direction.REV : Protocol.Direction.FWD;
            goHome(direction);
        }

        private void buttonFwd_Click(object sender, EventArgs e)
        {
            uint countSteps = (uint)editNumberSteps.Value;
            if (setNumberSteps.Checked)
                move(Protocol.Direction.FWD, countSteps);
            else
                run(Protocol.Direction.FWD);
        }

        private void move(Protocol.Direction direction, uint countSteps)
        {
            uint speed = (uint)editFullSpeed.Value;
            Core.Serial.SendPacket(new StopCommand(_stepper, StopCommand.StopType.SOFT_STOP).GetBytes());
            Core.Serial.SendPacket(new SetSpeedCommand(_stepper, speed).GetBytes());
            Core.Serial.SendPacket(new MoveCommand(_stepper, direction, countSteps).GetBytes());
        }

        private void run(Protocol.Direction direction)
        {
            uint speed = (uint)editFullSpeed.Value;
            Core.Serial.SendPacket(new StopCommand(_stepper, StopCommand.StopType.SOFT_STOP).GetBytes());
            Core.Serial.SendPacket(new RunCommand(_stepper, direction, speed).GetBytes());
        }

        private void goHome(Protocol.Direction direction)
        {
            uint speed = (uint)editFullSpeed.Value;

            Core.Serial.SendPacket(new StopCommand(_stepper, StopCommand.StopType.SOFT_STOP).GetBytes());
            Core.Serial.SendPacket(new HomeCommand(_stepper, direction, speed).GetBytes());
        }

        private void editNumberSteps_ValueChanged(object sender, EventArgs e)
        {
            SaveInformation();
        }

        private void editFullSpeed_ValueChanged(object sender, EventArgs e)
        {
            SaveInformation();
        }

        private void editStepperName_TextChanged(object sender, EventArgs e)
        {
            SaveInformation();
        }
    }
}
