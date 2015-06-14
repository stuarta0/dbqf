using dbqf;
using dbqf.Criterion;
using dbqf.Display;
using dbqf.Serialization.Assemblers;
using dbqf.Serialization.Assemblers.Display;
using dbqf.Serialization.DTO;
using dbqf.Serialization.DTO.Display;
using Standalone.Core.Serialization.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standalone.Core.Serialization.Assemblers
{
    public class SearchDocumentAssembler : IAssembler<SearchDocument, SearchDocumentDTO>
    {
        private Project _project;
        private FieldPathAssembler _pathAssembler;
        private PartViewAssembler _partAssembler;
        public SearchDocumentAssembler(Project project, FieldPathAssembler pathAssembler, PartViewAssembler partAssembler)
        {
            _project = project;
            _pathAssembler = pathAssembler;
            _partAssembler = partAssembler;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="Standalone.Core.Serialization.Assemblers.SearchDocumentRestoreException">Thrown if the project details cannot be matched to the assemblers project instance.</exception>
        /// <returns></returns>
        public SearchDocument Restore(SearchDocumentDTO dto)
        {
            var doc = new SearchDocument()
            {
                Project = new ProjectDetails()
                {
                    FileHint = dto.Project.FileHint,
                    Id = dto.Project.Id,
                    Version = (dto.Project.Version == null ? null : new System.Version(dto.Project.Version))
                },
                SearchType = dto.SearchType
            };
            
            // verify project before continuing
            var errors = new StringBuilder();
            if (!_project.Id.Equals(doc.Project.Id))
                errors.AppendLine("This search document was saved for a different project.");
            int compare = 0;
            if ((doc.Project.Version == null && _project.Version != null) || (doc.Project.Version != null && _project.Version == null))
                errors.AppendLine("This search document was saved for a different version of the project.");
            else if (!(doc.Project.Version == null && _project.Version == null))
                compare = doc.Project.Version.CompareTo(_project.Version);
            if (compare != 0)
                errors.AppendLine(compare > 0 ? "The search document was saved for a newer version of the project." : "The search document was saved for an older version of the project.");
            if (errors.Length > 0)
                throw new SearchDocumentRestoreException(errors.ToString());

            // if the project was OK, try to restore the subject
            if (dto.SubjectIndex >= 0 && dto.SubjectIndex < _project.Configuration.Count)
                doc.Subject = _project.Configuration[dto.SubjectIndex];
            else
                throw new SearchDocumentRestoreException(String.Format("Search index {0} does not exist in the project.", dto.SubjectIndex));

            // now restore output/parts
            if (dto.Outputs != null)
            {
                doc.Outputs = new List<FieldPath>();
                foreach (var f in dto.Outputs)
                    doc.Outputs.Add(_pathAssembler.Restore(f));
            }

            if (dto.Parts != null)
            {
                var parts = _partAssembler.Restore(dto.Parts);
                if (parts is IPartViewNode)
                    parts = new PartViewJunction(JunctionType.Conjunction) { parts };
                doc.Parts = (IPartViewJunction)parts;
            }

            return doc;
        }

        public SearchDocumentDTO Create(SearchDocument source)
        {
            var dto = new SearchDocumentDTO()
            {
                Project = new ProjectDetailsDTO()
                {
                    FileHint = source.Project.FileHint,
                    Id = source.Project.Id,
                    Version = (source.Project.Version == null ? null : source.Project.Version.ToString())
                },
                SearchType = source.SearchType,
                SubjectIndex = source.Subject.Configuration.IndexOf(source.Subject)
            };

            if (source.Outputs != null)
            {
                dto.Outputs = new List<FieldPathDTO>();
                foreach (var f in source.Outputs)
                    dto.Outputs.Add(_pathAssembler.Create(f));
            }

            if (source.Parts != null)
                dto.Parts = _partAssembler.Create(source.Parts);

            return dto;
        }
    }
}
