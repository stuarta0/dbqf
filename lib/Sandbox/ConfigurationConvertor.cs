using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using dbqf.Serialization.Assemblers;
using dbqf.Serialization.Assemblers.Parsers;

namespace Sandbox
{
    static class ConfigurationConvertor
    {
        public static void Convert(string source, string dest)
        {
            var cv1 = DbQueryFramework_v1.Utils.Serialization.Deserialize<DbQueryFramework_v1.Configuration.QueryConfig>(source);
            var cdto = new dbqf.Serialization.DTO.MatrixConfigurationDTO(cv1.Subjects.Count);

            // convert old format into new
            for (int i = 0; i < cv1.Subjects.Count; i++)
            {
                var sv1 = cv1.Subjects[i];
                var sdto = new dbqf.Serialization.DTO.SubjectDTO();
                sdto.Sql = sv1.Source;
                sdto.DisplayName = sv1.DisplayName;
                sdto.IdFieldIndex = sv1.Fields.IndexOf(sv1.GetField("ID"));
                if (sdto.IdFieldIndex < 0)
                    throw new MissingFieldException(String.Concat("ID Field for ", sdto.DisplayName, " was not found."));
                sdto.DefaultFieldIndex = sv1.Fields.IndexOf(sv1.GetField(sv1.DefaultFieldName));
                if (sdto.DefaultFieldIndex < 0)
                    throw new MissingFieldException(String.Concat("Default field '", sv1.DefaultFieldName, "' for ", sdto.DisplayName, " was not found."));

                foreach (var fv1 in sv1.Fields)
                {
                    var fdto = new dbqf.Serialization.DTO.FieldDTO();
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
                        fdto.List = new dbqf.Serialization.DTO.FieldListDTO() { SourceSql = fv1.ListData.Source };
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
                    cdto[i, j] = new dbqf.Serialization.DTO.MatrixNodeDTO()
                    {
                        Query = nv1.Query,
                        ToolTip = nv1.ToolTip
                    };
                }
            }


            var assembler = new MatrixConfigurationAssembler(new SubjectAssembler(new FieldAssembler(new ParserAssembler())));
            var c = assembler.Restore(cdto);

            cdto = assembler.Create(c);


            // serialise new DTO
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Standalone.Core.Serialization.DTO.ProjectDTO));

            var ws = new System.Xml.XmlWriterSettings();
            ws.Indent = true;
            ws.IndentChars = "  ";
            ws.CheckCharacters = true;
            using (XmlWriter writer = XmlWriter.Create(dest, ws))
            {
                serializer.Serialize(writer, new Standalone.Core.Serialization.DTO.ProjectDTO()
                {
                    Id = Guid.NewGuid(),
                    Connections = new List<Standalone.Core.ProjectConnection>() { 
                        new Standalone.Core.SQLiteProjectConnection() 
                        { 
                            DisplayName = "Local Machine",
                            Identifier = "local",
                            ConnectionString = @"Data Source=C:\myData.db;Version=3;" 
                        },
                        new Standalone.Core.SqlProjectConnection() 
                        { 
                            DisplayName = "Remote Server",
                            Identifier = "remote",
                            ConnectionString = @"Server=(local);Database=AdventureWorks2008R2;Trusted_Connection=True;" 
                        }
                    },
                    Configuration = cdto
                });
            }
        }
    }
}
