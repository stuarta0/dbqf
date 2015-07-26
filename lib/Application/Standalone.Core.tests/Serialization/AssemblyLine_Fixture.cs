
using NUnit.Framework;
namespace Standalone.Core.tests.Serialization
{
    [TestFixture]
    public class AssemblyLine_Fixture
    {
        private class TestAssemblyLine : AssemblyLine<int, string>
        {
            public TestAssemblyLine(AssemblyLine<int, string> successor)
                : base(successor)
            {
            }
        }

        [Test]
        public void Successor_test()
        {
            // Arrange
            var assembler1 = MockRepository.GenerateMock<AssemblyLine<int, string>>((AssemblyLine<int,string>)null);
            var assembler2 = new TestAssemblyLine(assembler1);
            var assembler3 = new TestAssemblyLine(assembler2);
            assembler1.Expect(x => x.Restore("one")).Return(default(int));
            assembler1.Expect(x => x.Create(2)).Return(default(string));

            // Act
            var restored = assembler3.Restore("one");
            var created = assembler3.Create(2);

            // Assert
            Assert.AreEqual(default(int), restored);
            Assert.AreEqual(default(string), created);
            assembler1.VerifyAllExpectations();
        }
    }
}
