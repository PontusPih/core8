﻿using Core8.Model.Enums;
using Core8.Model.Interfaces;

namespace Core8.Model.Instructions.Abstract
{
    public abstract class Group2InstructionBase : InstructionBase
    {
        private readonly IProcessor processor;

        public Group2InstructionBase(uint address, uint data, IProcessor processor, IRegisters registers) : base(address, data)
        {
            this.processor = processor;
            Registers = registers;
        }

        protected IRegisters Registers { get; }

        protected override string OpCodeText => OpCodes != 0 ? OpCodes.ToString() : string.Empty;

        private Group2PrivilegedOpCodes OpCodes => (Group2PrivilegedOpCodes)(Data & Masks.PRIVILEGED_GROUP_2_FLAGS);

        public override void Execute()
        {
            if (OpCodes.HasFlag(Group2PrivilegedOpCodes.OSR))
            {
                Registers.LINK_AC.SetAccumulator(Registers.LINK_AC.Accumulator | Registers.Switch.Get);
            }

            if (OpCodes.HasFlag(Group2PrivilegedOpCodes.HLT))
            {
                processor.Halt();
            }
        }
    }
}