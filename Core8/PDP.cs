﻿using Core8.Model.Extensions;
using Core8.Model.Interfaces;
using Serilog;
using System;
using System.Text;
using System.Threading;

namespace Core8
{
    public class PDP
    {
        private Thread cpuThread;

        private readonly MQRelay relay;

        public PDP()
        {
            Processor = new Processor(new Memory(), new Registers(), new Teleprinter());

            relay = new MQRelay(Processor.Teleprinter);

            relay.Start();

            ToggleRIMAndBinLoader();
        }

        public bool Running => cpuThread != null && cpuThread.IsAlive;

        public IProcessor Processor { get; }

        private void ToggleRIMAndBinLoader()
        {
            Load8(7617);

            Deposit8(1212); // TAD (7612)
            Deposit8(7402); // HLT
            Deposit8(7402); // HLT
            Deposit8(7402); // HLT
            Deposit8(7402); // HLT
            Deposit8(7402); // HLT
            Deposit8(7402); // HLT
            Deposit8(0000); // AND Z (0000)
            Deposit8(3212); // DCA (7612)
            Deposit8(4260); // JMS (7660)
            Deposit8(1300); // TAD (7700)
            Deposit8(7750); // SKP, SNA, SPA, CLA 
            Deposit8(5237); // JMP (7637)
            Deposit8(2212); // ISZ (7612)
            Deposit8(7040); // CMA
            Deposit8(5227); // JMP (7627)
            Deposit8(1212); // TAD (7612)
            Deposit8(7640); // SZA, CLA 
            Deposit8(5230); // JMP (7630)
            Deposit8(1214); // TAD (7614)
            Deposit8(0274); // AND (7674)
            Deposit8(1341); // TAD (7741)
            Deposit8(7510); // SKP, SPA 
            Deposit8(2226); // ISZ (7626)
            Deposit8(7750); // SKP, SNA, SPA, CLA 
            Deposit8(5626); // JMP I (7626)
            Deposit8(1214); // TAD (7614)
            Deposit8(0256); // AND (7656)
            Deposit8(1257); // TAD (7657)
            Deposit8(3213); // DCA (7613)
            Deposit8(5230); // JMP (7630)
            Deposit8(0070); // AND Z (0070)
            Deposit8(6201); // CDF
            Deposit8(0000); // AND Z (0000)
            Deposit8(0000); // AND Z (0000)
            Deposit8(6031); // KSF
            Deposit8(5262); // JMP (7662)
            Deposit8(6036); // KRB
            Deposit8(3214); // DCA (7614)
            Deposit8(1214); // TAD (7614)
            Deposit8(5660); // JMP I (7660)
            Deposit8(6011); //
            Deposit8(5270); // JMP (7670)
            Deposit8(6016); //
            Deposit8(5265); // JMP (7665)
            Deposit8(0300); // AND (7700)
            Deposit8(4343); // JMS (7743)
            Deposit8(7041); // IAC, CMA
            Deposit8(1215); // TAD (7615)
            Deposit8(7402); // HLT
            Deposit8(6032); // KCC
            Deposit8(6014); //
            Deposit8(6214); // RDF
            Deposit8(1257); // TAD (7657)
            Deposit8(3213); // DCA (7613)
            Deposit8(7604); // CLA OSR
            Deposit8(7700); // SMA, CLA 
            Deposit8(1353); // TAD (7753)
            Deposit8(1352); // TAD (7752)
            Deposit8(3261); // DCA (7661)
            Deposit8(4226); // JMS (7626)
            Deposit8(5313); // JMP (7713)
            Deposit8(3215); // DCA (7615)
            Deposit8(1213); // TAD (7613)
            Deposit8(3336); // DCA (7736)
            Deposit8(1214); // TAD (7614)
            Deposit8(3376); // DCA (7776)
            Deposit8(4260); // JMS (7660)
            Deposit8(3355); // DCA (7755)
            Deposit8(4226); // JMS (7626)
            Deposit8(5275); // JMP (7675)
            Deposit8(4343); // JMS (7743)
            Deposit8(7420); // SNL 
            Deposit8(5336); // JMP (7736)
            Deposit8(3216); // DCA (7616)
            Deposit8(1376); // TAD (7776)
            Deposit8(1355); // TAD (7755)
            Deposit8(1215); // TAD (7615)
            Deposit8(5315); // JMP (7715)
            Deposit8(6201); // CDF
            Deposit8(3616); // DCA I (7616)
            Deposit8(2216); // ISZ (7616)
            Deposit8(7600); // CLA 
            Deposit8(5332); // JMP (7732)
            Deposit8(0000); // AND Z (0000)
            Deposit8(1376); // TAD (7776)
            Deposit8(7106); // BSW, RAL, CLL
            Deposit8(7006); // BSW, RAL
            Deposit8(7006); // BSW, RAL
            Deposit8(1355); // TAD (7755)
            Deposit8(5743); // JMP I (7743)
            Deposit8(5262); // JMP (7662)
            Deposit8(0006); // AND Z (0006)
            Deposit8(0000); // AND Z (0000)
            Deposit8(0000); // AND Z (0000)

            // RIM -->
            Deposit8(6032); // KCC
            Deposit8(6031); // KSF
            Deposit8(5357); // JMP (7757)
            Deposit8(6036); // KRB
            Deposit8(7106); // BSW, RAL, CLL
            Deposit8(7006); // BSW, RAL
            Deposit8(7510); // SKP, SPA 
            Deposit8(5357); // JMP (7757)
            Deposit8(7006); // BSW, RAL
            Deposit8(6031); // KSF
            Deposit8(5367); // JMP (7767)
            Deposit8(6034); // KRS
            Deposit8(7420); // SNL 
            Deposit8(3776); // DCA I (7776)
            Deposit8(3376); // DCA (7776)
            Deposit8(5356); // JMP (7756)
            Deposit8(0000); // AND Z (0000)
            // <-- RIM

            Deposit8(5301); // JMP (7701)
        }

        public void DumpMemory()
        {
            var sb = new StringBuilder();

            uint zeroAddress = 0;
            bool zeroSet = false;

            void printZeroSpan()
            {
                if (zeroSet && zeroAddress != 0)
                {
                    sb.AppendLine($" --> {zeroAddress.ToOctalString(5)}");
                }
            };

            for (uint address = 0; address < Processor.Memory.Size; address++)
            {
                var instruction = Processor.Debug10(address);

                if (instruction.Data != 0)
                {
                    printZeroSpan();

                    sb.AppendLine(instruction.ToString());

                    zeroAddress = 0;
                    zeroSet = false;
                }
                else
                {
                    if (zeroSet)
                    {
                        zeroAddress = address;
                    }

                    if (!zeroSet)
                    {
                        sb.AppendLine(instruction.ToString());

                        zeroSet = true;
                    }
                }
            }

            printZeroSpan();

            Log.Information($"Memory dump:{Environment.NewLine}{sb.ToString()}");
        }

        public void Clear()
        {
            Processor.Clear();
        }

        public void Deposit8(uint data)
        {
            Deposit10(data.ToDecimal());
        }

        public void Deposit10(uint data)
        {
            Processor.Registers.SR.SetSR(data);

            Deposit();
        }

        private void Deposit()
        {
            var data = Processor.Registers.SR.Get;

            Processor.Memory.Write(Processor.Registers.PC.Address, data);

            Log.Information($"DEP: {Processor.Registers.PC} {data.ToOctalString()}");

            Processor.Registers.PC.Increment();
        }

        public void Load8(uint address)
        {
            Load10(address.ToDecimal());
        }

        public void Load10(uint address)
        {
            Processor.Registers.SR.SetSR(address);

            Load();
        }

        private void Load()
        {
            var address = Processor.Registers.SR.Get;

            Processor.Registers.PC.SetPC(address);

            Log.Information($"LOAD: {Processor.Registers.PC}");
        }

        public void Toggle8(uint word)
        {
            Toggle10(word.ToDecimal());
        }

        public void Toggle10(uint word)
        {
            Processor.Registers.SR.SetSR(word);
        }

        public void SetBreakpoint8(uint address)
        {
            Processor.SetBreakpoint(address.ToDecimal());
        }

        public void RemoveBreakpoint8(uint address)
        {
            Processor.RemoveBreakpoint(address.ToDecimal());
        }

        public void RemoveAllBreakpoints()
        {
            Processor.RemoveAllBreakpoints();
        }

        public void Exam()
        {
            Processor.Registers.AC.SetAccumulator(Processor.Memory.Read(Processor.Registers.PC.Address));

            Log.Information($"EXAM: {Processor.Registers.AC}");
        }

        public void Continue(bool waitForHalt = true)
        {
            cpuThread = new Thread(Processor.Run)
            {
                IsBackground = true,
                Priority = ThreadPriority.AboveNormal
            };

            cpuThread.Start();

            if (Running & waitForHalt)
            {
                cpuThread.Join();
            }
        }

        public void SingleStep(bool state)
        {
            Processor.SingleStep(state);
        }

        public void Halt(bool waitForHalt = true)
        {
            Processor.Halt();

            if (Running && waitForHalt)
            {
                cpuThread.Join();
            }
        }

        public void MountPaperTape(byte[] tape)
        {
            if (tape is null)
            {
                throw new ArgumentNullException(nameof(tape));
            }

            Processor.Teleprinter.MountPaperTape(tape);

            Log.Information($"TAPE: loaded {tape.Length} bytes");
        }

        public void LoadPaperTape(byte[] tape)
        {
            MountPaperTape(tape);

            Load8(7777);

            Continue();
        }
    }
}
