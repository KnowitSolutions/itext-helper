using NUnit.Framework;

namespace Tests.UnitTests
{
	[TestFixture]
    public class PassingTest 
    {
        [Test]
        public void Passes()
        {
			Assert.Pass();
        }
    }
}
