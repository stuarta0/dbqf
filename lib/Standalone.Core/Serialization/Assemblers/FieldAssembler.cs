using dbqf.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Standalone.Core.Serialization.DTO;
using dbqf.Display.Parsers;

namespace Standalone.Core.Serialization.Assemblers
{
    public class FieldAssembler : IAssembler<IField, FieldDTO>
    {
        /// <summary>
        /// Gets a lookup of restored field references to their associated parser implementation.
        /// </summary>
        public Dictionary<IField, Parser> ParserLookup { get; protected set; }

        private ParserAssembler _parserAssembler;
        public FieldAssembler(ParserAssembler parserAssembler)
        {
            _parserAssembler = parserAssembler;
            ParserLookup = new Dictionary<IField, Parser>();
        }

        /// <summary>
        /// Restores a field from DTO.  Note: cannot restore RelationField.RelatedSubject as 
        /// we don't have reference to the restored configuration at this point.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IField Restore(FieldDTO dto)
        {
            var field = (dto.RelatedSubjectIndex >= 0 ? new RelationField() : new Field());
            field.SourceName = dto.SourceName;
            field.DataType = Type.GetType(dto.DataTypeFullName);
            field.DisplayFormat = dto.DisplayFormat;
            field.DisplayName = dto.DisplayName;

            if (dto.Parsers.Count > 0)
            {
                var parser = new ChainedParser();
                foreach (var p in dto.Parsers)
                    parser.Add(_parserAssembler.Restore(p));
                ParserLookup.Add(field, parser); // GetHashCode depends on Subject reference, removed for now
            }

            if (dto.List != null)
            {
                field.List = new FieldList()
                {
                    Source = dto.List.SourceSql,
                    Type = dto.List.Type
                };

                foreach (var i in dto.List.Items)
                    field.List.Add(i);
            }

            // RelationField subject will need to be configured once the subjects have been assembled (in ConfigurationAssembler)

            return field;
        }

        public FieldDTO Create(IField source)
        {
            FieldDTO dto = new FieldDTO();
            dto.SourceName = source.SourceName;
            dto.DataTypeFullName = source.DataType.FullName;
            dto.DisplayFormat = source.DisplayFormat;
            dto.DisplayName = source.SourceName.Equals(source.DisplayName) ? null : source.DisplayName;

            if (source.List != null)
            {
                dto.List = new FieldListDTO();
                dto.List.SourceSql = source.List.Source;
                dto.List.Type = source.List.Type;
                dto.List.Items = new List<object>(source.List);
            }

            if (source is IRelationField)
            {
                var related = (IRelationField)source;
                dto.RelatedSubjectIndex = related.RelatedSubject.Configuration.IndexOf(related.RelatedSubject);
            }
            return dto;
        }
    }
}
