using AnalyzerCommunication.CommunicationProtocol.StepperCommands;
using AnalyzerConfiguration;
using AnalyzerService;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PresentationWinForms.Views
{
    public partial class StepperTurningView : UserControl
    {
        public enum Direction
        {
            Inverce,
            Forward
        }

        private Stepper stepperParams = null;
        bool isLoading = false;

        public StepperTurningView()
        {
            InitializeComponent();
        }

        public void SetStepperParams(Stepper stepperParams)
        {
            this.stepperParams = stepperParams;
            UpdateInformation();
        }

        public void ChangeStepperParams(Stepper stepperParams)
        {
            SaveInformation();
            SetStepperParams(stepperParams);
        }

        public void SaveInformation()
        {
            if (isLoading)
                return;
            if (Analyzer.AppConfig == null)
                return;

            stepperParams.Reverse = checkReverse.Checked;
            stepperParams.Name = editStepperName.Text;
            stepperParams.NumberSteps = (uint)editNumberSteps.Value;
            stepperParams.FullSpeed = (uint)editFullSpeed.Value;
        }
        
        public void UpdateInformation()
        {
            isLoading = true;
            if (Analyzer.AppConfig == null)
                return;

            bool isReverse = stepperParams.Reverse;
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

            editStepperName.Text = $"{stepperParams.Number} - {stepperParams.Name}";
            editNumberSteps.Value = stepperParams.NumberSteps;
            editFullSpeed.Value = stepperParams.FullSpeed;
            isLoading = false;
        }

        private void checkReverse_CheckedChanged(object sender, EventArgs e)
        {
            SaveInformation();
            UpdateInformation();
        }

        private void buttonRev_Click(object sender, EventArgs e)
        {
            int countSteps = (int)editNumberSteps.Value;
            if (setNumberSteps.Checked)
                move(Direction.Inverce, countSteps);
            else
                run(Direction.Inverce);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            Analyzer.Serial.SendPacket(new StopCommand(stepperParams.Number, StopCommand.StopType.SOFT_STOP).GetBytes());
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            bool isReverse = stepperParams.Reverse;
            Direction direction = isReverse ? Direction.Inverce : Direction.Forward;
            goHome(direction);
        }

        private void buttonFwd_Click(object sender, EventArgs e)
        {
            int countSteps = (int)editNumberSteps.Value;
            if (setNumberSteps.Checked)
                move(Direction.Forward, countSteps);
            else
                run(Direction.Forward);
        }

        private void move(Direction direction, int countSteps)
        {
            int speed = (int)editFullSpeed.Value;
            int stepper = stepperParams.Number;

            Analyzer.Serial.SendPacket(new StopCommand(stepper, StopCommand.StopType.SOFT_STOP).GetBytes());
            Analyzer.Serial.SendPacket(new SetSpeedCommand(stepper, (uint)speed).GetBytes());

            countSteps = direction == Direction.Forward ? countSteps : -countSteps;

            Analyzer.Serial.SendPacket(new MoveCommand(stepper, countSteps).GetBytes());
        }

        private void run(Direction direction)
        {
            int speed = (int)editFullSpeed.Value;
            int stepper = stepperParams.Number;
            Analyzer.Serial.SendPacket(new StopCommand(stepper, StopCommand.StopType.SOFT_STOP).GetBytes());
            speed = direction == Direction.Forward ? speed : -speed;
            Analyzer.Serial.SendPacket(new RunCommand(stepper, speed).GetBytes());
        }

        private void goHome(Direction direction)
        {
            int speed = (int)editFullSpeed.Value;
            int stepper = stepperParams.Number;
            Analyzer.Serial.SendPacket(new StopCommand(stepper, StopCommand.StopType.SOFT_STOP).GetBytes());
            speed = direction == Direction.Forward ? speed : -speed;
            Analyzer.Serial.SendPacket(new HomeCommand(stepper, speed).GetBytes());
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
