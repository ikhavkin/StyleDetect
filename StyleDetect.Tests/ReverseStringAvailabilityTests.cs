using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Intentions.CSharp.Test;
using NUnit.Framework;

namespace Codevolve.StyleDetect.Tests
{
    [TestFixture]
    public class ReverseStringAvailabilityTests : CSharpContextActionAvailabilityTestBase
    {
        protected override IContextAction CreateContextAction(ICSharpContextActionDataProvider dataProvider)
        {
            return new ReverseStringAction(dataProvider);
        }

        protected override string ExtraPath
        {
            get { return "ReverseStringAction"; }
        }

        protected override string RelativeTestDataPath
        {
            get { return "ReverseStringAction"; }
        }

        [Test]
        public void AvailabilityTest()
        {
            DoTestFiles("availability01.cs");
        }
    }
}