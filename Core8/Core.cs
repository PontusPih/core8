﻿using Core8.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core8
{
    public class Core : ICore
    {
        public Core(IProcessor processor, IMemory memory, IRegisters registers)
        {
            Memory = memory;
            Processor = processor;
            Registers = registers;
        }

        public IMemory Memory { get; }
        public IProcessor Processor { get; }
        public IRegisters Registers { get; }
    }
}