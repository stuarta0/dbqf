using dbqf.Configuration;
using dbqf.Serialization.DTO;
namespace dbqf.Serialization.Assemblers
{
    public class ConfigurationAssembler : IAssembler<IConfiguration, ConfigurationDTO>
    {
        private SubjectAssembler _subjectAssembler;
        public ConfigurationAssembler(SubjectAssembler subjectAssembler)
        {
            _subjectAssembler = subjectAssembler;
        }

        public IConfiguration Restore(ConfigurationDTO dto)
        {
            var configuration = new ConfigurationImpl();
            var count = dto.Subjects.Length;

            for (int i = 0; i < count; i++)
                configuration.Subject(_subjectAssembler.Restore(dto[i]));

            for (int i = 0; i < count; i++)
            {
                // finish wiring up the RelationFields now that our configuration is set up
                // assumes Subject and Field order is equivalent between DTO's and model objects
                for (int f = 0; f < configuration[i].Count; f++)
                {
                    if (configuration[i][f] is IRelationField)
                        ((IRelationField)configuration[i][f]).RelatedSubject = configuration[dto.Subjects[i].Fields[f].RelatedSubjectIndex];
                }

                // now complete the matrix nodes
                for (int j = 0; j < count; j++)
                {
                    var m = dto[i, j];
                    configuration.Matrix(configuration[i], configuration[j], m.Query, m.ToolTip);
                }
            }

            return configuration;
        }

        public ConfigurationDTO Create(IConfiguration source)
        {
            var dto = new ConfigurationDTO(source.Count);
            for (int i = 0; i < source.Count; i++)
            {
                dto[i] = _subjectAssembler.Create(source[i]);
                for (int j = 0; j < source.Count; j++)
                    dto[i, j] = Create(source[source[i], source[j]]);
            }
            return dto;
        }

        private MatrixNodeDTO Create(MatrixNode node)
        {
            return new MatrixNodeDTO()
            {
                Query = node.Query,
                ToolTip = node.ToolTip
            };
        }
    }
}
