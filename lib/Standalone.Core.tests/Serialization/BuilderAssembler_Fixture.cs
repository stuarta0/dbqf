using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display.Builders;
using NUnit.Framework;
using Rhino.Mocks;
using Standalone.Core.Serialization.Assemblers;
using Standalone.Core.Serialization.Assemblers.Builders;
using Standalone.Core.Serialization.Assemblers.Criterion;
using Standalone.Core.Serialization.DTO;
using Standalone.Core.Serialization.DTO.Builders;
using Standalone.Core.Serialization.DTO.Criterion;

namespace Standalone.Core.tests.Serialization
{
    [TestFixture]
    public class BuilderAssembler_Fixture
    {
        [Test]
        public void SimpleBuilder_Restore()
        {
            // Arrange
            var mockJunctionAssembler = MockRepository.GenerateMock<JunctionParameterAssembler>((ParameterAssembler)null);
            mockJunctionAssembler.Expect(x => x.Restore(null)).Return(null);
            var assemblerUnderTest = new SimpleBuilderAssembler(null) { JunctionAssembler = mockJunctionAssembler };
            var dto = new SimpleBuilderDTO()
            {
                Label = "test",
                Junction = null,
                Operator = "="
            };

            // Act
            var restored = assemblerUnderTest.Restore(dto);

            // Assert
            Assert.IsNotNull(restored);
            Assert.IsInstanceOf<SimpleBuilder>(restored);
            var builder = (SimpleBuilder)restored;
            Assert.AreEqual(dto.Label, builder.Label);
            Assert.AreEqual(dto.Junction, builder.Junction);
            Assert.AreEqual(dto.Operator, builder.Operator);
            mockJunctionAssembler.VerifyAllExpectations();
        }

        [Test]
        public void SimpleBuilder_Create()
        {
            // Arrange
            var mockJunctionAssembler = MockRepository.GenerateMock<JunctionParameterAssembler>((ParameterAssembler)null);
            mockJunctionAssembler.Expect(x => x.Create(null)).Return(null);
            var assemblerUnderTest = new SimpleBuilderAssembler(null) { JunctionAssembler = mockJunctionAssembler };
            var builder = new SimpleBuilder()
            {
                Label = "test",
                Junction = null,
                Operator = "="
            };

            // Act
            var restored = assemblerUnderTest.Create(builder);

            // Assert
            Assert.IsNotNull(restored);
            Assert.IsInstanceOf<SimpleBuilderDTO>(restored);
            var dto = (SimpleBuilderDTO)restored;
            Assert.AreEqual(builder.Label, dto.Label);
            Assert.AreEqual(builder.Junction, dto.Junction);
            Assert.AreEqual(builder.Operator, dto.Operator);
            mockJunctionAssembler.VerifyAllExpectations();
        }

        [Test]
        public void NullBuilder_Restore()
        {
            // Arrange
            var mockJunctionAssembler = MockRepository.GenerateMock<JunctionParameterAssembler>((ParameterAssembler)null);
            mockJunctionAssembler.Expect(x => x.Restore(null)).Return(null);
            var assemblerUnderTest = new NullBuilderAssembler(null) { JunctionAssembler = mockJunctionAssembler };
            var dto = new NullBuilderDTO()
            {
                Label = "test",
                Junction = null
            };

            // Act
            var restored = assemblerUnderTest.Restore(dto);

            // Assert
            Assert.IsNotNull(restored);
            Assert.IsInstanceOf<NullBuilder>(restored);
            var builder = (NullBuilder)restored;
            Assert.AreEqual(dto.Label, builder.Label);
            Assert.AreEqual(dto.Junction, builder.Junction);
            mockJunctionAssembler.VerifyAllExpectations();
        }

        [Test]
        public void NullBuilder_Create()
        {
            // Arrange
            var mockJunctionAssembler = MockRepository.GenerateMock<JunctionParameterAssembler>((ParameterAssembler)null);
            mockJunctionAssembler.Expect(x => x.Create(null)).Return(null);
            var assemblerUnderTest = new NullBuilderAssembler(null) { JunctionAssembler = mockJunctionAssembler };
            var builder = new NullBuilder()
            {
                Label = "test",
                Junction = null
            };

            // Act
            var restored = assemblerUnderTest.Create(builder);

            // Assert
            Assert.IsNotNull(restored);
            Assert.IsInstanceOf<NullBuilderDTO>(restored);
            var dto = (NullBuilderDTO)restored;
            Assert.AreEqual(builder.Label, dto.Label);
            Assert.AreEqual(builder.Junction, dto.Junction);
            mockJunctionAssembler.VerifyAllExpectations();
        }

        [Test]
        public void NotBuilder_Restore()
        {
            // Arrange
            var mockJunctionAssembler = MockRepository.GenerateMock<JunctionParameterAssembler>((ParameterAssembler)null);
            mockJunctionAssembler.Expect(x => x.Restore(null)).Return(null);
            var mockOtherDTO = MockRepository.GenerateMock<ParameterBuilderDTO>();
            var mockOther = MockRepository.GenerateMock<ParameterBuilder>();
            var mockChain = MockRepository.GenerateMock<BuilderAssembler>((BuilderAssembler)null);
            mockChain.Expect(x => x.Restore(mockOtherDTO)).Return(mockOther);

            var assemblerUnderTest = new NotBuilderAssembler(null) { JunctionAssembler = mockJunctionAssembler, Chain = mockChain };
            var dto = new NotBuilderDTO()
            {
                Label = "test",
                Junction = null,
                Other = mockOtherDTO
            };

            // Act
            var restored = assemblerUnderTest.Restore(dto);

            // Assert
            Assert.IsNotNull(restored);
            Assert.IsInstanceOf<NotBuilder>(restored);
            var builder = (NotBuilder)restored;
            Assert.AreEqual(dto.Label, builder.Label);
            Assert.AreEqual(dto.Junction, builder.Junction);
            Assert.AreSame(mockOther, builder.Other);
            mockJunctionAssembler.VerifyAllExpectations();
            mockChain.VerifyAllExpectations();
        }

        [Test]
        public void NotBuilder_Create()
        {
            // Arrange
            var mockJunctionAssembler = MockRepository.GenerateMock<JunctionParameterAssembler>((ParameterAssembler)null);
            mockJunctionAssembler.Expect(x => x.Create(null)).Return(null);
            var mockOtherDTO = MockRepository.GenerateMock<ParameterBuilderDTO>();
            var mockOther = MockRepository.GenerateMock<ParameterBuilder>();
            var mockChain = MockRepository.GenerateMock<BuilderAssembler>((BuilderAssembler)null);
            mockChain.Expect(x => x.Create(mockOther)).Return(mockOtherDTO);

            var assemblerUnderTest = new NotBuilderAssembler(null) { JunctionAssembler = mockJunctionAssembler, Chain = mockChain };
            var builder = new NotBuilder()
            {
                Label = "test",
                Junction = null,
                Other = mockOther
            };

            // Act
            var restored = assemblerUnderTest.Create(builder);

            // Assert
            Assert.IsNotNull(restored);
            Assert.IsInstanceOf<NotBuilderDTO>(restored);
            var dto = (NotBuilderDTO)restored;
            Assert.AreEqual(builder.Label, dto.Label);
            Assert.AreEqual(builder.Junction, dto.Junction);
            Assert.AreSame(mockOtherDTO, dto.Other);
            mockJunctionAssembler.VerifyAllExpectations();
            mockChain.VerifyAllExpectations();
        }

        [Test]
        public void LikeBuilder_Restore()
        {
            // Arrange
            var mockJunctionAssembler = MockRepository.GenerateMock<JunctionParameterAssembler>((ParameterAssembler)null);
            mockJunctionAssembler.Expect(x => x.Restore(null)).Return(null);
            var assemblerUnderTest = new LikeBuilderAssembler(null) { JunctionAssembler = mockJunctionAssembler };
            var dto = new LikeBuilderDTO()
            {
                Label = "test",
                Junction = null,
                Mode = "Exact"
            };

            // Act
            var restored = assemblerUnderTest.Restore(dto);

            // Assert
            Assert.IsNotNull(restored);
            Assert.IsInstanceOf<LikeBuilder>(restored);
            var builder = (LikeBuilder)restored;
            Assert.AreEqual(dto.Label, builder.Label);
            Assert.AreEqual(dto.Junction, builder.Junction);
            Assert.AreSame(MatchMode.Exact, builder.Mode);
            mockJunctionAssembler.VerifyAllExpectations();
        }

        [Test]
        public void LikeBuilder_Create()
        {
            // Arrange
            var mockJunctionAssembler = MockRepository.GenerateMock<JunctionParameterAssembler>((ParameterAssembler)null);
            mockJunctionAssembler.Expect(x => x.Create(null)).Return(null);
            var assemblerUnderTest = new LikeBuilderAssembler(null) { JunctionAssembler = mockJunctionAssembler };
            var builder = new LikeBuilder()
            {
                Label = "test",
                Junction = null,
                Mode = MatchMode.Exact
            };

            // Act
            var restored = assemblerUnderTest.Create(builder);

            // Assert
            Assert.IsNotNull(restored);
            Assert.IsInstanceOf<LikeBuilderDTO>(restored);
            var dto = (LikeBuilderDTO)restored;
            Assert.AreEqual(builder.Label, dto.Label);
            Assert.AreEqual(builder.Junction, dto.Junction);
            Assert.AreEqual("Exact", dto.Mode);
            mockJunctionAssembler.VerifyAllExpectations();
        }

        [Test]
        public void BooleanBuilder_Restore()
        {
            // Arrange
            var mockJunctionAssembler = MockRepository.GenerateMock<JunctionParameterAssembler>((ParameterAssembler)null);
            mockJunctionAssembler.Expect(x => x.Restore(null)).Return(null);
            var assemblerUnderTest = new BooleanBuilderAssembler(null) { JunctionAssembler = mockJunctionAssembler };
            var dto = new BooleanBuilderDTO()
            {
                Label = "test",
                Junction = null,
                Value = true
            };

            // Act
            var restored = assemblerUnderTest.Restore(dto);

            // Assert
            Assert.IsNotNull(restored);
            Assert.IsInstanceOf<BooleanBuilder>(restored);
            var builder = (BooleanBuilder)restored;
            Assert.AreEqual(dto.Label, builder.Label);
            Assert.AreEqual(dto.Junction, builder.Junction);
            Assert.AreEqual(true, builder.Value);
            mockJunctionAssembler.VerifyAllExpectations();
        }

        [Test]
        public void BooleanBuilder_Create()
        {
            // Arrange
            var mockJunctionAssembler = MockRepository.GenerateMock<JunctionParameterAssembler>((ParameterAssembler)null);
            mockJunctionAssembler.Expect(x => x.Create(null)).Return(null);
            var assemblerUnderTest = new BooleanBuilderAssembler(null) { JunctionAssembler = mockJunctionAssembler };
            var builder = new BooleanBuilder()
            {
                Label = "test",
                Junction = null,
                Value = true
            };

            // Act
            var restored = assemblerUnderTest.Create(builder);

            // Assert
            Assert.IsNotNull(restored);
            Assert.IsInstanceOf<BooleanBuilderDTO>(restored);
            var dto = (BooleanBuilderDTO)restored;
            Assert.AreEqual(builder.Label, dto.Label);
            Assert.AreEqual(builder.Junction, dto.Junction);
            Assert.AreEqual(true, dto.Value);
            mockJunctionAssembler.VerifyAllExpectations();
        }

        [Test]
        public void BetweenBuilder_Restore()
        {
            // Arrange
            var mockJunctionAssembler = MockRepository.GenerateMock<JunctionParameterAssembler>((ParameterAssembler)null);
            mockJunctionAssembler.Expect(x => x.Restore(null)).Return(null);
            var assemblerUnderTest = new BetweenBuilderAssembler(null) { JunctionAssembler = mockJunctionAssembler };
            var dto = new BetweenBuilderDTO()
            {
                Label = "test",
                Junction = null
            };

            // Act
            var restored = assemblerUnderTest.Restore(dto);

            // Assert
            Assert.IsNotNull(restored);
            Assert.IsInstanceOf<BetweenBuilder>(restored);
            var builder = (BetweenBuilder)restored;
            Assert.AreEqual(dto.Label, builder.Label);
            Assert.AreEqual(dto.Junction, builder.Junction);
            mockJunctionAssembler.VerifyAllExpectations();
        }

        [Test]
        public void BetweenBuilder_Create()
        {
            // Arrange
            var mockJunctionAssembler = MockRepository.GenerateMock<JunctionParameterAssembler>((ParameterAssembler)null);
            mockJunctionAssembler.Expect(x => x.Create(null)).Return(null);
            var assemblerUnderTest = new BetweenBuilderAssembler(null) { JunctionAssembler = mockJunctionAssembler };
            var builder = new BetweenBuilder()
            {
                Label = "test",
                Junction = null
            };

            // Act
            var restored = assemblerUnderTest.Create(builder);

            // Assert
            Assert.IsNotNull(restored);
            Assert.IsInstanceOf<BetweenBuilderDTO>(restored);
            var dto = (BetweenBuilderDTO)restored;
            Assert.AreEqual(builder.Label, dto.Label);
            Assert.AreEqual(builder.Junction, dto.Junction);
            mockJunctionAssembler.VerifyAllExpectations();
        }
    }
}
