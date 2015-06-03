﻿using dbqf.Configuration;
using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Standalone.Core.Serialization.DTO;
using Standalone.Core.Serialization.Assemblers.Builders;
using Standalone.Core.Serialization.Assemblers.Parsers;
using Standalone.Core.Serialization.DTO.Display;
using dbqf.Display;
using Standalone.Core.Serialization.Assemblers.Criterion;
using dbqf.Display.Builders;

namespace Standalone.Core.Serialization.Assemblers.Display
{
    public class PartViewAssembler : IAssembler<IPartView, PartViewDTO>
    {
        private FieldPathAssembler _pathAssembler;
        private BuilderAssembler _builderAssembler;
        private ParserAssembler _parserAssembler;
        public PartViewAssembler(
            FieldPathAssembler pathAssembler,
            BuilderAssembler builderAssembler,
            ParserAssembler parserAssembler)
        {
            _pathAssembler = pathAssembler;
            _builderAssembler = builderAssembler;
            _parserAssembler = parserAssembler;
        }

        public IPartView Restore(PartViewDTO dto)
        {
            var part = new PartViewImpl();
            part.SelectedPath = _pathAssembler.Restore(dto.Path);
            part.SelectedBuilder = _builderAssembler.Restore(dto.Builder);
            part.Values = dto.Values;
            part.Parser = _parserAssembler.Restore(dto.Parser);
            return part;
        }

        public PartViewDTO Create(IPartView source)
        {
            var dto = new PartViewDTO();
            dto.Path = _pathAssembler.Create(source.SelectedPath);
            dto.Builder = _builderAssembler.Create(source.SelectedBuilder);
            dto.Values = source.Values;
            dto.Parser = _parserAssembler.Create(source.Parser);
            return dto;
        }

        private class PartViewImpl : IPartView
        {
            public dbqf.Criterion.FieldPath SelectedPath { get; set; }
            public dbqf.Display.Builders.ParameterBuilder SelectedBuilder { get; set; }
            public object[] Values { get; set; }
            public dbqf.Parsers.Parser Parser { get; set; }

            public void CopyFrom(IPartView other)
            {
                
            }

            public dbqf.Criterion.IParameter GetParameter()
            {
                throw new NotImplementedException();
            }

            public bool Equals(IPartView other)
            {
                throw new NotImplementedException();
            }
        }
    }
}
