using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Configuration;
using dbqf.Criterion;
using Standalone.Serialization.DTO;

namespace Standalone.Serialization.Assemblers
{
    /// <summary>
    /// TODO: see ProtoBuf surrogate for FieldPath and implicit operators on FieldPathDTO
    /// </summary>
    public class FieldPathAssembler : IAssembler<FieldPath, FieldPathDTO>
    {
        private IConfiguration _configuration;
        public FieldPathAssembler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public FieldPath Restore(FieldPathDTO dto)
        {
            var path = new FieldPath();
            var curSubject = _configuration[dto.SubjectIndex];
            foreach (var name in dto.SourceNames)
            {
                var field = curSubject[name];
                path.Add(field);
                if (field is IRelationField)
                    curSubject = ((IRelationField)field).RelatedSubject;
            }
            return path;
        }

        public FieldPathDTO Create(FieldPath source)
        {
            var dto = new FieldPathDTO();
            dto.SubjectIndex = _configuration.IndexOf(source.Root);
            foreach (var f in source)
                dto.SourceNames.Add(f.SourceName);
            return dto;
        }
    }
}
