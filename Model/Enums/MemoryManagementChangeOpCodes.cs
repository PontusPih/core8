﻿using System;

namespace Core8.Model.Enums
{
    [Flags]
    public enum MemoryManagementChangeOpCodes : uint
    {
        CDF = 1 << 0,
        CIF = 1 << 1
    }
}
