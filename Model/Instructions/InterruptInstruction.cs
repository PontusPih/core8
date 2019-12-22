﻿using Core8.Model.Enums;
using Core8.Model.Instructions.Abstract;
using Core8.Model.Interfaces;
using System;

namespace Core8.Model.Instructions
{
    public class InterruptInstruction : InstructionBase
    {
        public InterruptInstruction(uint address, uint data) : base(address, data)
        {
        }

        protected override string OpCodeText => OpCode.ToString();

        private InterruptOpCode OpCode => (InterruptOpCode)(Data & Masks.IO_OPCODE);

        public override void Execute(IHardware hardware)
        {
            switch (OpCode)
            {
                case InterruptOpCode.SKON:
                    SKON(hardware);
                    break;
                case InterruptOpCode.ION:
                    ION(hardware);
                    break;
                case InterruptOpCode.IOF:
                    IOF(hardware);
                    break;
                default:
                    break;
            }
        }

        private void SKON(IHardware hardware)
        {
        }

        private void ION(IHardware hardware)
        {
        }

        private void IOF(IHardware hardware)
        {
        }
    }
}
