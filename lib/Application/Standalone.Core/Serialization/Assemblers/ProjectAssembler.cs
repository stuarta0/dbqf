using dbqf.Serialization.Assemblers;
using Standalone.Core.Serialization.DTO;

namespace Standalone.Core.Serialization.Assemblers
{
    public class ProjectAssembler : IAssembler<Project, ProjectDTO>
    {
        private MatrixConfigurationAssembler _configurationAssembler;
        public ProjectAssembler(MatrixConfigurationAssembler configurationAssembler)
        {
            _configurationAssembler = configurationAssembler;
        }

        public Project Restore(ProjectDTO dto)
        {
            var prj = new Project();
            prj.Id = dto.Id;
            prj.Title = dto.Title;
            prj.Connections = dto.Connections;
            prj.Configuration = _configurationAssembler.Restore(dto.Configuration);
            return prj;
        }

        public ProjectDTO Create(Project source)
        {
            var dto = new ProjectDTO();
            dto.Id = source.Id;
            dto.Title = source.Title;
            dto.Connections = source.Connections;
            dto.Configuration = _configurationAssembler.Create(source.Configuration);
            return dto;
        }
    }
}
