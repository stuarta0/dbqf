using NUnit.Framework;
using dbqf.Serialization.DTO;
using dbqf.Serialization.Assemblers.Criterion;
using dbqf.Serialization.DTO.Criterion;
using dbqf.Serialization.Assemblers;
using dbqf.Configuration;
using dbqf.Criterion;

namespace Standalone.Core.tests.Serialization
{
    [TestFixture]
    public class ParameterAssembler_Fixture
    {
        private class FieldPathAssemblerStub : FieldPathAssembler
        {
            public static FieldPath Source { get; private set; }
            public static FieldPathDTO DTO { get; private set; }
            static FieldPathAssemblerStub()
            {
                Source = new FieldPath();
                DTO = new FieldPathDTO();
            }

            public FieldPathAssemblerStub()
                : base(null)
            {
            }

            public override FieldPath Restore(FieldPathDTO dto)
            {
                return Source;
            }

            public override FieldPathDTO Create(FieldPath source)
            {
                return DTO;
            }
        }

        [Test]
        public void SimpleParameter_Restore()
        {
            // Arrange
            var mockPathAssembler = new FieldPathAssemblerStub();
            var assemblerUnderTest = new SimpleParameterAssembler(mockPathAssembler);
            var dto = new SimpleParameterDTO()
            {
                Path = FieldPathAssemblerStub.DTO,
                Operator = "=",
                Value = 1
            };

            // Act
            var restored = assemblerUnderTest.Restore(dto);

            // Assert
            Assert.IsNotNull(restored);
            Assert.IsInstanceOf<SimpleParameter>(restored);
            var p = (SimpleParameter)restored;
            Assert.AreSame(FieldPathAssemblerStub.Source, p.Path);
            Assert.AreEqual(dto.Operator, p.Operator);
            Assert.AreEqual(dto.Value, p.Value);
        }

        [Test]
        public void SimpleParameter_Create()
        {
            // Arrange
            var mockPathAssembler = new FieldPathAssemblerStub();
            var assemblerUnderTest = new SimpleParameterAssembler(mockPathAssembler);
            var p = new SimpleParameter(FieldPathAssemblerStub.Source, "=", 1);

            // Act
            var restored = assemblerUnderTest.Create(p);

            // Assert
            Assert.IsNotNull(restored);
            Assert.IsInstanceOf<SimpleParameterDTO>(restored);
            var dto = (SimpleParameterDTO)restored;
            Assert.AreSame(FieldPathAssemblerStub.DTO, dto.Path);
            Assert.AreEqual(p.Operator, dto.Operator);
            Assert.AreEqual(p.Value, dto.Value);
        }
    }
}
