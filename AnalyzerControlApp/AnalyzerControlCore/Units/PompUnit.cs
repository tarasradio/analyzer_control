﻿using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol.CncCommands;
using AnalyzerConfiguration;
using AnalyzerConfiguration.UnitsConfiguration;
using AnalyzerService.ExecutionControl;
using Infrastructure;
using System;
using System.Collections.Generic;

namespace AnalyzerService.Units
{
    public class PompUnit : UnitBase<PompConfiguration>
    {
        public PompUnit(ICommandExecutor executor, IConfigurationProvider provider) : base(executor, provider)
        {

        }

        public void CloseValves()
        {
            Logger.Debug($"[{nameof(PompUnit)}] - Close valves.");
            List<ICommand> commands = new List<ICommand>();

            commands.Add(new OffDeviceCncCommand(new List<int>() { 0 , 1 }));

            executor.WaitExecution(commands);
        }

        public void Home()
        {
            Logger.Debug($"[{nameof(PompUnit)}] - Start homing.");
            List<ICommand> commands = new List<ICommand>();
            
            //commands.Add(new OnDeviceCncCommand(new List<int>() { 0 }));
            
            //commands.Add(new OffDeviceCncCommand(new List<int>() { 1 }));

            steppers = new Dictionary<int, int>() {
                { Options.BigPistonStepper, Options.BigPistonSpeedAtHoming },
                { Options.SmallPistonStepper, Options.SmallPistonSpeedAtHoming }
            };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Options.BigPistonStepper, Options.BigPistonSpeedAtHoming },
                { Options.SmallPistonStepper, Options.SmallPistonSpeedAtHoming }
            };
            commands.Add(new HomeCncCommand(steppers));

            //commands.Add(new OffDeviceCncCommand(new List<int>() { 0 }));

            executor.WaitExecution(commands);
            Logger.Debug($"[{nameof(PompUnit)}] - Homing finished.");
        }

        public void AspirateBigPiston(int cycles)
        {
            List<ICommand> commands = new List<ICommand>();

            commands.Add(new OnDeviceCncCommand(new List<int>() { 0 }));

            steppers = new Dictionary<int, int>() {
                { Options.BigPistonStepper, Options.BigPistonSpeed }
            };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Options.BigPistonStepper, -25600 * cycles }
            };
            commands.Add(new MoveCncCommand(steppers));

            commands.Add(new OffDeviceCncCommand(new List<int>() { 0 }));

            executor.WaitExecution(commands);
        }

        public void DispenceBigPiston(int cycles)
        {
            List<ICommand> commands = new List<ICommand>();

            commands.Add(new OnDeviceCncCommand(new List<int>() { 0 }));

            steppers = new Dictionary<int, int>() {
                { Options.BigPistonStepper, Options.BigPistonSpeed }
            };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Options.BigPistonStepper, 25600 * cycles }
            };
            commands.Add(new MoveCncCommand(steppers));

            commands.Add(new OffDeviceCncCommand(new List<int>() { 0 }));

            executor.WaitExecution(commands);
        }


        // M = 100.48 uL за один оборот (диаметр 4 мм ход 8 мм)
        // 1 микролитр равен одному кубическому миллиметру
        // 200 шагов на один оборот мотора (без делителей)
        // используется делитель 128 (128 микрошагов на 1 шаг)
        // =>
        // 1 оборот малого плунжера = 128 * 200 = 25 600 шагов (SM)
        // расчет S шагов на N микролитров:
        // S = N / M * SM
        // Пример: на 200 uL -> 200 / 100.48 * 25600 = 50955 шагов
        // Пример: на 5 uL -> 5 / 100.48 * 25600 = 1274 шагов
        // Пример: на 1 uL -> 1 / 100.48 * 25600 = 255 шагов
        public void Pull(int value)
        {
            Logger.Debug($"[{nameof(PompUnit)}] - Start suction.");
            List<ICommand> commands = new List<ICommand>();

            commands.Add( new OnDeviceCncCommand(new List<int>() { 0 }) );
            //commands.Add( new OffDeviceCncCommand(new List<int>() { 1 }) );
            
            steppers = new Dictionary<int, int>() {
                { Options.SmallPistonStepper, Options.SmallPistonSpeedAtSuction }
            };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() {
                { Options.SmallPistonStepper, Options.SmallPistonStepsAtSuction }
            };
            commands.Add( new MoveCncCommand(steppers) );
            
            commands.Add( new OffDeviceCncCommand(new List<int>() { 0 }) );

            executor.WaitExecution(commands);
            Logger.Debug($"[{nameof(PompUnit)}] - Suction finished.");
        }

        public void Push(int value)
        {
            Logger.Debug($"[{nameof(PompUnit)}] - Start unsuction.");
            List<ICommand> commands = new List<ICommand>();
            
            commands.Add( new OnDeviceCncCommand(new List<int>() { 0 }) );
            commands.Add( new OffDeviceCncCommand(new List<int>() { 1 }) );

            steppers = new Dictionary<int, int>() {
                { Options.SmallPistonStepper, Options.SmallPistonSpeedAtSuction }
            };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() {
                { Options.SmallPistonStepper, Options.SmallPistonSpeedAtSuction }
            };
            commands.Add( new HomeCncCommand(steppers) );
            
            commands.Add( new OffDeviceCncCommand(new List<int>() { 0 }) );

            executor.WaitExecution(commands);
            Logger.Debug($"[{nameof(PompUnit)}] - Unsuction finished.");
        }

        public void WashTheNeedle(int cycles)
        {
            Logger.Debug($"[{nameof(PompUnit)}] - Start washing ({cycles} cycles).");
            
            for (int i = 0; i < cycles; i++)
            {
                WashingCycle();
            }
            Logger.Debug($"[{nameof(PompUnit)}] - Washing finished.");
        }

        private void WashingCycle()
        {
            List<ICommand> commands = new List<ICommand>();

            commands.Add(new OnDeviceCncCommand(new List<int>() { 0 }));
            commands.Add(new OffDeviceCncCommand(new List<int>() { 1 }));

            steppers = new Dictionary<int, int>() {
                    { Options.BigPistonStepper, Options.BigPistonSpeedAtHoming },
                    { Options.SmallPistonStepper, Options.SmallPistonSpeedAtHoming }
                };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                    { Options.BigPistonStepper, Options.BigPistonSpeedAtHoming },
                    { Options.SmallPistonStepper, Options.SmallPistonSpeedAtHoming }
                };
            commands.Add(new HomeCncCommand(steppers));

            commands.Add(new OffDeviceCncCommand(new List<int>() { 0 }));
            commands.Add(new OnDeviceCncCommand(new List<int>() { 1 }));

            steppers = new Dictionary<int, int>() {
                    { Options.BigPistonStepper, Options.BigPistonSpeedAtWashing },
                    { Options.SmallPistonStepper, Options.SmallPistonSpeedAtWashing }
                };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                    { Options.BigPistonStepper, Options.BigPistonStepsAtWashing },
                    { Options.SmallPistonStepper, Options.SmallPistonStepsAtWashing }
                };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
        }
    }
}
