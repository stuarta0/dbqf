using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using dbqf.Criterion;
using dbqf.Configuration;

namespace Standalone.Core.Serialization.DTO
{
    [ProtoContract]
    public class FieldPathDTO
    {
        /// <summary>
        /// Required for implicit cast from DTO to FieldPath to rebuild references for ProtoBuf.
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        [ProtoMember(1)]
        public int SubjectIndex { get; set; }

        [ProtoMember(2)]
        public List<string> SourceNames { get; set; }

        public FieldPathDTO()
        {
            SourceNames = new List<string>();
        }

        public static implicit operator FieldPathDTO(FieldPath path)
        {
            if (path == null)
                return null;

            var dto = new FieldPathDTO();
            dto.SubjectIndex = path.Root.Configuration.IndexOf(path.Root);
            foreach (var f in path)
                dto.SourceNames.Add(f.SourceName);
            return dto;
        }

        public static implicit operator FieldPath(FieldPathDTO dto)
        {
            if (Configuration == null)
                throw new NullReferenceException("FieldPathDTO.Configuration not set.  Unable to resolve FieldPath using implicit cast.");

            if (dto == null)
                return null;

            var path = new FieldPath();
            ISubject curSubject = Configuration[dto.SubjectIndex];
            foreach (var name in dto.SourceNames)
            {
                var field = curSubject[name];
                path.Add(field);
                if (field is IRelationField)
                    curSubject = ((IRelationField)field).RelatedSubject;
            }
            return path;
        }
    }
}
