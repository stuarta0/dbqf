using dbqf.Serialization.Assemblers.Criterion;
using NUnit.Framework;

namespace dbqf.Serialization.tests
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

        private class TestAssemblyLineEnd : TestAssemblyLine
        {
            public int Restored { get { return 42; } }
            public string Created { get { return "fourty-two"; } }

            public TestAssemblyLineEnd()
                : base(null)
            {
            }

            public override int Restore(string dto)
            {
                return Restored;
            }

            public override string Create(int p)
            {
                return Created;
            }
        }

        [Test]
        public void Successor_test()
        {
            // Arrange
            var assembler1 = new TestAssemblyLineEnd();
            var assembler2 = new TestAssemblyLine(assembler1);
            var assembler3 = new TestAssemblyLine(assembler2);

            // Act
            var restored = assembler3.Restore("one");
            var created = assembler3.Create(2);

            // Assert
            Assert.AreEqual(assembler1.Restored, restored);
            Assert.AreEqual(assembler1.Created, created);
        }
    }
}
