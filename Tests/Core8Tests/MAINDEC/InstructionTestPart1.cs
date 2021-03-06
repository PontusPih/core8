using Core8.Model.Extensions;
using Core8.Tests.MAINDEC.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core8.Tests.MAINDEC
{
    [TestClass]
    public class InstructionTestPart1 : MAINDECTestsBase
    {
        protected override string TapeName => @"MAINDEC/tapes/MAINDEC-8E-D0AB-PB.bin";

        protected override string[] ExpectedOutput => new[] { "\u0007" };

        [TestMethod]
        public override void Start()
        {
            PDP.Load8(0200);

            PDP.Toggle8(7777);

            PDP.Continue();

            Assert.AreEqual(0147.ToDecimal(), PDP.Processor.Registers.PC.Address);

            StartAndWaitForCompletion();
        }
    }
}
