using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Standalone.Serialization.Assemblers;

namespace Sandbox
{
    static class ConfigurationConvertor
    {
        public static void Convert(string source, string dest)
        {
            var cv1 = DbQueryFramework_v1.Utils.Serialization.Deserialize<DbQueryFramework_v1.Configuration.QueryConfig>(source);
            var cdto = new Standalone.Serialization.DTO.ConfigurationDTO(cv1.Subjects.Count);

            // convert old format into new
            for (int i = 0; i < cv1.Subjects.Count; i++)
            {
                var sv1 = cv1.Subjects[i];
                var sdto = new Standalone.Serialization.DTO.SubjectDTO();
                sdto.Source = sv1.Source;
                sdto.DisplayName = sv1.DisplayName;
                sdto.IdFieldIndex = sv1.Fields.IndexOf(sv1.GetField("ID"));
                if (sdto.IdFieldIndex < 0)
                    throw new MissingFieldException(String.Concat("ID Field for ", sdto.DisplayName, " was not found."));
                sdto.DefaultFieldIndex = sv1.Fields.IndexOf(sv1.GetField(sv1.DefaultFieldName));
                if (sdto.DefaultFieldIndex < 0)
                    throw new MissingFieldException(String.Concat("Default field '", sv1.DefaultFieldName, "' for ", sdto.DisplayName, " was not found."));

                foreach (var fv1 in sv1.Fields)
                {
                    var fdto = new Standalone.Serialization.DTO.FieldDTO();
                    fdto.SourceName = fv1.SourceName;
                    fdto.DisplayName = fv1.DisplayName;
                    fdto.DisplayFormat = fv1.DisplayFormat;
                    fdto.DataTypeFullName = fv1.DataTypeName;

                    var cfv1 = fv1 as DbQueryFramework_v1.Configuration.ComplexField;
                    if (cfv1 != null)
                    {
                        fdto.RelatedSubjectIndex = cv1.Subjects.IndexOf(cv1.GetSubject(cfv1.LinkedSubjectID));
                        fdto.OutputDataTypeFullName = cfv1.OutputDataTypeName;
                        fdto.OutputSourceName = cfv1.OutputSourceName;
                    }

                    if (fv1.ListData != null)
                    {
                        fdto.List = new Standalone.Serialization.DTO.FieldListDTO() { SourceSql = fv1.ListData.Source };
                        fdto.List.Type = dbqf.Configuration.FieldListType.None;
                        if (fv1.ListData.Type == DbQueryFramework_v1.Configuration.FieldListType.LimitToList)
                            fdto.List.Type = dbqf.Configuration.FieldListType.Limited;
                        else
                            fdto.List.Type = dbqf.Configuration.FieldListType.Suggested;
                        fdto.List.Items = fv1.ListData.Items.ConvertAll<object>(o => o.Value);
                    }

                    sdto.Fields.Add(fdto);
                }

                cdto.Subjects[i] = sdto;
            }

            for (int i = 0; i < cdto.Subjects.Length; i++)
            {
                for (int j = 0; j < cdto.Subjects.Length; j++)
                {
                    var nv1 = cv1.SubjectMatrix[cv1.Subjects[i].ID][cv1.Subjects[j].ID];
                    cdto[i, j] = new Standalone.Serialization.DTO.MatrixNodeDTO()
                    {
                        Query = nv1.Query,
                        ToolTip = nv1.ToolTip
                    };
                }
            }


            var assembler = new ConfigurationAssembler(new SubjectAssembler(new FieldAssembler(new ParserAssembler())));
            var c = assembler.Restore(cdto);

            cdto = assembler.Create(c);


            // serialise new DTO
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Standalone.Serialization.DTO.ProjectDTO));
            using (TextWriter writer = new StreamWriter(dest))
            {
                serializer.Serialize(writer, new Standalone.Serialization.DTO.ProjectDTO()
                {
                    Id = Guid.NewGuid(),
                    Connections = new List<Standalone.Data.Connection>() { 
                        new Standalone.Data.Connection() 
                        { 
                            DisplayName = "Local Machine",
                            Identifier = "local",
                            ConnectionType = "SQLite", 
                            ConnectionString = @"Data Source=C:\myData.db;Version=3;" 
                        },
                        new Standalone.Data.Connection() 
                        { 
                            DisplayName = "Remote Server",
                            Identifier = "remote",
                            ConnectionType = "SqlClient", 
                            ConnectionString = @"Server=(local);Database=AdventureWorks2008R2;Trusted_Connection=True;" 
                        }
                    },
                    Configuration = cdto
                });
            }
        }
    }
}
