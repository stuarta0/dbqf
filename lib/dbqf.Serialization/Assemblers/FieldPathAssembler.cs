using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Serialization.DTO;

namespace dbqf.Serialization.Assemblers
{
    /// <summary>
    /// TODO: see ProtoBuf surrogate for FieldPath and implicit operators on FieldPathDTO
    /// </summary>
    public class FieldPathAssembler : IAssembler<IFieldPath, FieldPathDTO>
    {
        private IConfiguration _configuration;
        public FieldPathAssembler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual IFieldPath Restore(FieldPathDTO dto)
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

        public virtual FieldPathDTO Create(IFieldPath source)
        {
            var dto = new FieldPathDTO();
            dto.SubjectIndex = _configuration.IndexOf(source.Root);
            foreach (var f in source)
                dto.SourceNames.Add(f.SourceName);
            return dto;
        }
    }
}
