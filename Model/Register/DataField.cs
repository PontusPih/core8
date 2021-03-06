﻿using Core8.Model.Extensions;
using Core8.Model.Register.Abstract;

namespace Core8.Model.Register
{
    public class DataField : RegisterBase
    {
        protected override string ShortName => "DF";

        public void SetDF(uint value)
        {
            Set(value & Masks.DF);
        }

        public override string ToString()
        {
            return string.Format($"{base.ToString()} {Data.ToOctalString()}");
        }
    }
}
