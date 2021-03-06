﻿using Core8.Model.Interfaces;
using Core8.Model.Register;

namespace Core8
{
    public class Registers : IRegisters
    {
        public Registers()
        {
            AC = new LinkAccumulator();
            PC = new InstructionFieldProgramCounter();
            SR = new SwitchRegister();
            MQ = new MultiplierQuotient();
            DF = new DataField();
            IB = new InstructionBuffer();
            UB = new UserBuffer();
            UF = new UserFlag();
            SF = new SaveField();
        }

        public LinkAccumulator AC { get; }

        public InstructionFieldProgramCounter PC { get; }

        public SwitchRegister SR { get; }

        public MultiplierQuotient MQ { get; }

        public DataField DF { get; }

        public InstructionBuffer IB { get; }

        public UserBuffer UB { get; }

        public UserFlag UF { get; }

        public SaveField SF { get; }
    }
}
