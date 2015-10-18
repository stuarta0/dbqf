using dbqf.Configuration;
using dbqf.Serialization.DTO;
using dbqf.Sql.Configuration;
using System;

namespace dbqf.Serialization.Assemblers
{
    public class SubjectAssembler : IAssembler<ISubject, SubjectDTO>
    {
        private FieldAssembler _fieldAssembler;
        public SubjectAssembler(FieldAssembler fieldAssember)
        {
            _fieldAssembler = fieldAssember;
        }

        public ISubject Restore(SubjectDTO dto)
        {
            Subject subject;
            if (String.IsNullOrEmpty(dto.Sql))
                subject = new Subject();
            else
            {
                subject = new SqlSubject();
                ((SqlSubject)subject).Sql = dto.Sql;
            }
            subject.DisplayName = dto.DisplayName;
            foreach (var f in dto.Fields)
                subject.Field(_fieldAssembler.Restore(f));
            subject.IdField = subject[dto.IdFieldIndex];
            subject.DefaultField = subject[dto.DefaultFieldIndex];
            return subject;
        }

        public SubjectDTO Create(ISubject source)
        {
            var dto = new SubjectDTO();
            if (source is ISqlSubject)
                dto.Sql = ((ISqlSubject)source).Sql;
            dto.DisplayName = source.DisplayName;
            dto.IdFieldIndex = source.IndexOf(source.IdField);
            dto.DefaultFieldIndex = source.IndexOf(source.DefaultField);
            foreach (var f in source)
                dto.Fields.Add(_fieldAssembler.Create(f));
            return dto;
        }
    }
}
