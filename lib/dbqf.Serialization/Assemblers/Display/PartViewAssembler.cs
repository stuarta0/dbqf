using System;
using dbqf.Display;
using dbqf.Serialization.Assemblers.Builders;
using dbqf.Serialization.Assemblers.Parsers;
using dbqf.Serialization.DTO.Display;
using System.Collections.Generic;

namespace dbqf.Serialization.Assemblers.Display
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
            if (!String.IsNullOrEmpty(dto.JunctionType))
            {
                var junc = new PartViewJunction();
                junc.Type = dto.JunctionType.Equals(JunctionType.Conjunction.ToString()) ? JunctionType.Conjunction : JunctionType.Disjunction;
                foreach (var j in dto.JunctionPartViews)
                    junc.Add(this.Restore(j));
                return junc;
            }
            else
            {
                var part = new PartViewNodeImpl();
                part.SelectedPath = _pathAssembler.Restore(dto.Path);
                part.SelectedBuilder = _builderAssembler.Restore(dto.Builder);
                part.Values = dto.Values;
                part.Parser = _parserAssembler.Restore(dto.Parser);
                return part;
            }
        }

        public PartViewDTO Create(IPartView source)
        {
            var dto = new PartViewDTO();
            if (source is IPartViewNode)
            {
                var node = (IPartViewNode)source;
                dto.Path = _pathAssembler.Create(node.SelectedPath);
                dto.Builder = _builderAssembler.Create(node.SelectedBuilder);
                dto.Values = node.Values;
                dto.Parser = _parserAssembler.Create(node.Parser);
            }
            else if (source is IPartViewJunction)
            {
                var junc = (IPartViewJunction)source;
                dto.JunctionType = junc.Type.ToString();
                dto.JunctionPartViews = new List<PartViewDTO>();
                foreach (var j in junc)
                    dto.JunctionPartViews.Add(this.Create(j));
            }
            return dto;
        }

        private class PartViewNodeImpl : IPartViewNode
        {
            public dbqf.Criterion.IFieldPath SelectedPath { get; set; }
            public dbqf.Criterion.Builders.ParameterBuilder SelectedBuilder { get; set; }
            public object[] Values { get; set; }
            public dbqf.Parsers.Parser Parser { get; set; }

            public void CopyFrom(IPartView other)
            {
                
            }

            public dbqf.Criterion.IParameter GetParameter()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Considered equal when the path, builder and parser are equal.
            /// Note: value is ignored in equality test.
            /// </summary>
            public bool Equals(IPartView other)
            {
                if (other == null || !(other is IPartViewNode))
                    return false;

                var node = (IPartViewNode)other;
                return SelectedPath.Equals(node.SelectedPath)
                    && SelectedBuilder.Equals(node.SelectedBuilder)
                    && dbqf.Parsers.Parser.Equals(Parser, node.Parser);
            }

            public override bool Equals(object obj)
            {
                if (obj is IPartView)
                    return Equals((IPartView)obj);
                return base.Equals(obj);
            }

            public override string ToString()
            {
                return String.Format("{0} {1} {2}",
                    SelectedPath.Description,
                    SelectedBuilder.Label,
                    Values != null ?
                        String.Join(", ", Values.Convert<object, string>(v => v != null ? v.ToString() : string.Empty).ToArray())
                        : string.Empty);
            }
        }
    }
}
