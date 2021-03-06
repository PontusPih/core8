﻿using Core8.Model.Enums;
using Core8.Model.Instructions.Abstract;
using Core8.Model.Interfaces;

namespace Core8.Model.Instructions
{
    public class Group2ORInstructions : Group2InstructionsBase
    {
        public Group2ORInstructions(IProcessor processor) : base(processor)
        {
        }

        protected override string OpCodeText => OpCodes == 0 ? base.OpCodeText : string.Join(' ', OpCodes.ToString(), base.OpCodeText);

        private Group2OROpCodes OpCodes => (Group2OROpCodes)(Data & Masks.GROUP_2_AND_OR_FLAGS);

        public override void Execute()
        {
            bool result = false;

            if (OpCodes.HasFlag(Group2OROpCodes.SMA))
            {
                result |= (Register.AC.Accumulator & Masks.AC_SIGN) != 0;
            }

            if (OpCodes.HasFlag(Group2OROpCodes.SZA))
            {
                result |= Register.AC.Accumulator == 0;
            }

            if (OpCodes.HasFlag(Group2OROpCodes.SNL))
            {
                result |= Register.AC.Link != 0;
            }

            if (result)
            {
                Register.PC.Increment();
            }

            if (OpCodes.HasFlag(Group2OROpCodes.CLA))
            {
                Register.AC.ClearAccumulator();
            }

            base.Execute();
        }
    }
}
