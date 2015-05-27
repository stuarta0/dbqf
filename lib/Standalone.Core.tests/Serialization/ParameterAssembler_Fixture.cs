using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Configuration;
using dbqf.Criterion;
using NUnit.Framework;
using Rhino.Mocks;
using Standalone.Core.Serialization.Assemblers;
using Standalone.Core.Serialization.Assemblers.Criterion;
using Standalone.Core.Serialization.DTO;
using Standalone.Core.Serialization.DTO.Criterion;

namespace Standalone.Core.tests.Serialization
{
    [TestFixture]
    public class ParameterAssembler_Fixture
    {
        [Test]
        public void SimpleParameter_Restore()
        {
            // Arrange
            var mockPath = MockRepository.GenerateMock<FieldPath>();
            var mockPathDTO = MockRepository.GenerateMock<FieldPathDTO>();
            var mockPathAssembler = MockRepository.GenerateMock<FieldPathAssembler>((IConfiguration)null);
            mockPathAssembler.Stub(x => x.Restore(mockPathDTO)).Return(mockPath);
            var assemblerUnderTest = new SimpleParameterAssembler(null, mockPathAssembler);
            var dto = new SimpleParameterDTO()
            {
                Path = mockPathDTO,
                Operator = "=",
                Value = 1
            };

            // Act
            var restored = assemblerUnderTest.Restore(dto);

            // Assert
            Assert.IsNotNull(restored);
            Assert.IsInstanceOf<SimpleParameter>(restored);
            var p = (SimpleParameter)restored;
            Assert.AreSame(mockPath, p.Path);
            Assert.AreEqual(dto.Operator, p.Operator);
            Assert.AreEqual(dto.Value, p.Value);
            mockPathAssembler.VerifyAllExpectations();
        }

        [Test]
        public void SimpleParameter_Create()
        {
            // Arrange
            var mockPath = MockRepository.GenerateMock<FieldPath>();
            var mockPathDTO = MockRepository.GenerateMock<FieldPathDTO>();
            var mockPathAssembler = MockRepository.GenerateMock<FieldPathAssembler>((IConfiguration)null);
            mockPathAssembler.Stub(x => x.Create(mockPath)).Return(mockPathDTO);
            var assemblerUnderTest = new SimpleParameterAssembler(null, mockPathAssembler);
            var p = new SimpleParameter(mockPath, "=", 1);

            // Act
            var restored = assemblerUnderTest.Create(p);

            // Assert
            Assert.IsNotNull(restored);
            Assert.IsInstanceOf<SimpleParameterDTO>(restored);
            var dto = (SimpleParameterDTO)restored;
            Assert.AreSame(mockPathDTO, dto.Path);
            Assert.AreEqual(p.Operator, dto.Operator);
            Assert.AreEqual(p.Value, dto.Value);
            mockPathAssembler.VerifyAllExpectations();
        }
    }
}
