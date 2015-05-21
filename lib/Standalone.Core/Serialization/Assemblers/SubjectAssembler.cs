using dbqf.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Standalone.Core.Serialization.DTO;

namespace Standalone.Core.Serialization.Assemblers
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
            var subject = new Subject();
            subject.Source = dto.Source;
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
            dto.Source = source.Source;
            dto.DisplayName = source.DisplayName;
            dto.IdFieldIndex = source.IndexOf(source.IdField);
            dto.DefaultFieldIndex = source.IndexOf(source.DefaultField);
            foreach (var f in source)
                dto.Fields.Add(_fieldAssembler.Create(f));
            return dto;
        }
    }
}
