using dbqf.Sql.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dbqf.tools
{
    /// <summary>
    /// Using INFORMATION_SCHEMA in SQL Server, creates a dbqf Configuration for all tables and columns.
    /// Note: 
    ///   - does not support composite keys.
    ///   - does not generate matrix queries (which is fine for non-advanced searches).
    /// </summary>
    public class SqlServerParser
    {
        public SqlServerParser(string connection)
        {
            _connection = connection;
        }

        private string _connection;

        public dbqf.Sql.Configuration.IMatrixConfiguration CreateConfiguration(IEnumerable<string> excludeSubjects = null)
        {
            var config = new HelperMatrixConfiguration();
            var builder = new SqlConnectionStringBuilder(_connection);
            var catalog = builder.InitialCatalog;
            
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Initialise subjects from tables
                    cmd.CommandText = @"SELECT TABLE_SCHEMA, TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG = @tableCatalog";
                    cmd.Parameters.AddWithValue("@tableCatalog", catalog);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var subject = new HelperSqlSubject(reader.GetString(0), reader.GetString(1));
                            subject.Sql = String.Format("SELECT * FROM {0}", subject.FullName);
                            
                            if (!excludeSubjects.Contains(subject.DisplayName))
                                config.Subject(subject);
                        }
                    }

                    // Determine PK for setting up fields shortly
                    // "[schema].[tableName]": "pkColumnName"
                    var pk = new Dictionary<string, string>();
                    cmd.CommandText = @"SELECT t.TABLE_SCHEMA, t.TABLE_NAME, COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS t 
INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE c ON t.TABLE_CATALOG = c.TABLE_CATALOG AND t.TABLE_SCHEMA = c.TABLE_SCHEMA AND t.CONSTRAINT_NAME = c.CONSTRAINT_NAME 
WHERE t.TABLE_CATALOG = @tableCatalog AND CONSTRAINT_TYPE = 'PRIMARY KEY'";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            pk.Add(HelperSqlSubject.ToFullName(reader.GetString(0), reader.GetString(1)), reader.GetString(2));
                    }

                    // Determine relationships
                    // "[schema].[tableName].[columnName]": "[schema].[tableName].[columnName]"
                    var fk = new Dictionary<string, string>();
                    cmd.CommandText = @"SELECT c1.TABLE_SCHEMA, c1.TABLE_NAME, c1.COLUMN_NAME, c2.TABLE_SCHEMA, c2.TABLE_NAME, c2.COLUMN_NAME
FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE c1 
INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS r ON c1.CONSTRAINT_NAME = r.CONSTRAINT_NAME 
INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE c2 ON c2.CONSTRAINT_NAME = r.UNIQUE_CONSTRAINT_NAME
WHERE c1.TABLE_CATALOG = @tableCatalog AND c2.TABLE_CATALOG = @tableCatalog";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fk.Add(
                                HelperField.ToFullName(reader.GetString(0), reader.GetString(1), reader.GetString(2)), 
                                HelperField.ToFullName(reader.GetString(3), reader.GetString(4), reader.GetString(5)));
                        }
                    }

                    // Configure fields for subjects based on columns and constraints
                    foreach (HelperSqlSubject subject in config)
                    {
                        var possibleNames = new List<string>() { "name", $"{subject.TableName}name".ToLower(), "title", $"{subject.TableName}title".ToLower(), subject.TableName.ToLower() };

                        cmd.CommandText = "SELECT COLUMN_NAME, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_CATALOG = @tableCatalog AND TABLE_SCHEMA = @tableSchema AND TABLE_NAME = @tableName";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@tableCatalog", catalog);
                        cmd.Parameters.AddWithValue("@tableSchema", subject.Schema);
                        cmd.Parameters.AddWithValue("@tableName", subject.TableName);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var field = new Configuration.Field()
                                {
                                    Subject = subject,
                                    SourceName = reader.GetString(0),
                                    DisplayName = SplitCamelCase(reader.GetString(0)),
                                    DataType = SqlToClr(reader.GetString(2)),
                                    DisplayFormat = ("money".Equals(reader.GetString(2)) ? "C0" : null)
                                };
                                
                                // set this field as the primary key if it's name matches the PK in the schema
                                if (pk.ContainsKey(subject.FullName) && pk[subject.FullName].Equals(field.SourceName))
                                    subject.IdField = field;

                                // set this field as a relationship if it's contained within the foreign keys in the schema
                                if (fk.ContainsKey(HelperField.ToFullName(field)))
                                {
                                    // redefine the field as a relation
                                    var target = HelperField.ParseFullName(fk[HelperField.ToFullName(field)]);
                                    var relation = new dbqf.Configuration.RelationField(field)
                                    {
                                        RelatedSubject = config[target.Subject.ToString()]
                                    };

                                    // now that we've recreated the field as a relation field, remove the original
                                    subject.Remove(field);
                                    field = relation;

                                    // strip postfix " id" off display string for relation fields
                                    if (field.DisplayName.ToLower().EndsWith(" id"))
                                        field.DisplayName = field.DisplayName.Substring(0, field.DisplayName.Length - 3);
                                }

                                // set this field as the default if it contains something resembling a name
                                if (subject.DefaultField == null && possibleNames.Contains(field.Name.ToLower()))
                                    subject.DefaultField = field;
                            }
                        }

                        if (subject.Count > 0)
                        {
                            // may not be correct, but the configuration isn't valid without it
                            if (subject.IdField == null)
                                subject.IdField = subject[0];
                            if (subject.DefaultField == null)
                                subject.DefaultField = subject[0];
                        }
                    }
                }
            }

            // remove any subjects that resulted in zero fields
            foreach (var subject in config.TakeWhile(s => s.Count == 0))
                config.Remove(subject);

            // hook up the easy matrix nodes based on the relation fields
            // (those that traverse many to many or are logically related via multiple relationships are omitted)
            var compiled = new Dictionary<Relationship, List<Configuration.IRelationField>>();
            foreach (HelperSqlSubject subject in config)
            {
                // we can't determine relationships without primary keys
                if (subject.IdField == null)
                    continue;

                // Bundle multiple relationships to the same subject into each query with a UNION ALL
                foreach (var field in subject)
                {
                    var related = field as Configuration.IRelationField;
                    if (related != null && related.RelatedSubject != null)
                    {
                        var key = new Relationship((HelperSqlSubject)related.Subject, (HelperSqlSubject)related.RelatedSubject);
                        if (!compiled.ContainsKey(key))
                            compiled.Add(key, new List<Configuration.IRelationField>());

                        compiled[key].Add(related);
                    }
                }
            }

            // our compiled list will now contain a lookup of relationship between two subjects (order irrelevant) and all fields from both subjects that relate to the other subject
            foreach (var pair in compiled)
            {
                // bi-directional, therefore process twice
                config[pair.Key.Subject1, pair.Key.Subject2].Query = HelperField.CreateMatrixSql(pair.Key.Subject1, pair.Value);
                config[pair.Key.Subject2, pair.Key.Subject1].Query = HelperField.CreateMatrixSql(pair.Key.Subject2, pair.Value);
            }
            
            return config;
        }

        public void UpdateListFields(IMatrixConfiguration config, int listLength, bool listDefault)
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();
                foreach (HelperSqlSubject subject in config)
                {
                    foreach (var field in subject)
                    {
                        // don't setup lists on relation fields
                        if (field is Configuration.IRelationField)
                            continue;

                        using (var cmd = conn.CreateCommand())
                        {
                            string extra = "";
                            if (field.DataType == typeof(string))
                                extra = $"WHERE LTRIM(RTRIM(COALESCE([{field.SourceName}], ''))) <> ''";
                            cmd.CommandText = $"SELECT COUNT(*) FROM (SELECT DISTINCT [{field.SourceName}] FROM {subject.FullName} {extra}) x";
                            try
                            {
                                var count = (int)cmd.ExecuteScalar();
                                if (count <= listLength || (listDefault && subject.DefaultField == field))
                                {
                                    field.List = new dbqf.Configuration.FieldList()
                                    {
                                        Type = Configuration.FieldListType.Suggested,
                                        Source = $"SELECT DISTINCT [{field.SourceName}] AS Value FROM {subject.FullName} {extra}"
                                    };
                                }
                            }
                            catch (SqlException ex)
                            {
                                // can occur when trying to run rtrim/ltrim/distinct on TEXT columns
                            }
                        }
                    }
                }
            }
        }

        public static string SplitCamelCase(string camelCase)
        {
            return Regex.Replace(camelCase, @"([a-z0-9])([A-Z])", "$1 $2");
        }

        private Type SqlToClr(string typename)
        {
            // https://msdn.microsoft.com/en-us/library/bb386947(v=vs.110).aspx

            //BIT System.Boolean
            //TINYINT System.Int16
            //INT System.Int32
            //BIGINT  System.Int64
            //SMALLMONEY  System.Decimal
            //MONEY   System.Decimal
            //DECIMAL System.Decimal
            //NUMERIC System.Decimal
            //REAL / FLOAT(24)  System.Single
            //FLOAT / FLOAT(53) System.Double

            //CHAR System.String
            //NCHAR   System.String
            //VARCHAR System.String
            //NVARCHAR    System.String
            //TEXT    System.String
            //NTEXT   System.String
            //XML System.Xml.Linq.XElement

            //SMALLDATETIME System.DateTime
            //DATETIME    System.DateTime
            //DATETIME2   System.DateTime
            //DATETIMEOFFSET  System.DateTimeOffset
            //DATE    System.DateTime
            //TIME    System.TimeSpan

            //BINARY(50)  System.Data.Linq.Binary
            //VARBINARY(50)   System.Data.Linq.Binary
            //VARBINARY(MAX)  System.Data.Linq.Binary
            //VARBINARY(MAX) with the FILESTREAM attribute    System.Data.Linq.Binary
            //IMAGE   System.Data.Linq.Binary
            //TIMESTAMP   System.Data.Linq.Binary

            //UNIQUEIDENTIFIER System.Guid
            //SQL_VARIANT System.Object

            switch (typename.ToUpper())
            {
                case "BIT": return typeof(System.Boolean); break;
                case "TINYINT": return typeof(System.Int16); break;
                case "INT": return typeof(System.Int32); break;
                case "BIGINT": return typeof(System.Int64); break;
                case "SMALLMONEY":
                case "MONEY":
                case "DECIMAL":
                case "NUMERIC": return typeof(System.Decimal); break;
                case "REAL": return typeof(System.Single); break;
                case "FLOAT": return typeof(System.Double); break;

                case "CHAR":
                case "NCHAR":
                case "VARCHAR":
                case "NVARCHAR":
                case "TEXT":
                case "NTEXT": return typeof(System.String); break;
                case "XML": return typeof(System.Xml.Linq.XElement); break;

                case "SMALLDATETIME":
                case "DATETIME":
                case "DATETIME2": return typeof(System.DateTime); break;
                case "DATETIMEOFFSET": return typeof(System.DateTimeOffset); break;
                case "DATE": return typeof(System.DateTime); break;
                case "TIME": return typeof(System.TimeSpan); break;

                case "BINARY": 
                case "VARBINARY":
                case "IMAGE": 
                case "TIMESTAMP": return typeof(object); break;

                case "UNIQUEIDENTIFIER": return typeof(System.Guid); break;
                case "SQL_VARIANT": return typeof(System.Object); break;
            }

            return typeof(object);
        }

        public class HelperMatrixConfiguration : MatrixConfiguration
        {
            public override Configuration.ISubject this[string fullname]
            {
                get
                {
                    var def = HelperSqlSubject.ParseFullName(fullname);
                    var result = _subjects.Find((s) => 
                    {
                        var subject = s as HelperSqlSubject;
                        if (subject != null)
                            return (def.Schema?.Equals(subject.Schema) ?? false) && (def.TableName?.Equals(subject.TableName) ?? false);
                        return false;
                    });

                    if (result != null)
                        return result;
                    return _subjects.Find((s) => { return s.DisplayName.Equals(fullname); });
                }
            }
        }

        public class HelperSqlSubject : SqlSubject
        {
            public struct SubjectDef
            {
                public string Schema;
                public string TableName;

                public override string ToString() => HelperSqlSubject.ToFullName(Schema, TableName);
            }

            public string Schema { get; set; }
            public string TableName { get; set; }

            public string FullName => ToFullName(Schema, TableName);

            public HelperSqlSubject(string schema, string name)
                : base()
            {
                Schema = schema;
                TableName = name;
                DisplayName = SqlServerParser.SplitCamelCase(name);
            }

            public override string ToString() => FullName;

            public static string ToFullName(string schema, string table) => $"[{schema}].[{table}]";

            public static SubjectDef ParseFullName(string name)
            {
                var parts = name?.Split('.') ?? new string[0];
                for (int i = 0; i < parts.Length; i++)
                    parts[i] = parts[i].TrimStart('[').TrimEnd(']');
                return parts.Length == 2 ? new SubjectDef()
                {
                    Schema = parts[0],
                    TableName = parts[1]
                } : new SubjectDef();
            }
        }

        public class HelperField : Configuration.Field
        {
            public struct FieldDef
            {
                public HelperSqlSubject.SubjectDef Subject;
                public string Name;

                public override string ToString() => HelperField.ToFullName(Subject.Schema, Subject.TableName, Name);
            }

            public static string CreateMatrixSql(HelperSqlSubject from, IEnumerable<Configuration.IRelationField> fields)
            {
                return String.Join(" UNION ALL ", fields.Select(f => CreateMatrixSql(from, f)));
            }

            public static string CreateMatrixSql(HelperSqlSubject from, Configuration.IRelationField field)
            {
                // either field.Subject == from, or field.RelatedSubject == from, anything else is exceptional
                if (field.Subject == from)
                    return $"SELECT [{from.IdField.SourceName}] FromID, [{field.SourceName}] ToID FROM {from.FullName}";
                else if (field.RelatedSubject == from)
                {
                    var target = field.Subject as HelperSqlSubject;
                    return $"SELECT [{field.SourceName}] FromID, [{from.IdField.SourceName}] ToID FROM {target.FullName}";
                }

                throw new ArgumentException("Cannot create matrix SQL from field if neither field.Subject or field.RelatedSubject are the source.");
            }

            public static string ToFullName(Configuration.IField field) => ToFullName(((HelperSqlSubject)field.Subject).Schema, ((HelperSqlSubject)field.Subject).TableName, field.SourceName);

            public static string ToFullName(string schema, string table, string column) => $"[{schema}].[{table}].[{column}]";

            public static FieldDef ParseFullName(string name)
            {
                var parts = name?.Split('.') ?? new string[0];
                for (int i = 0; i < parts.Length; i++)
                    parts[i] = parts[i].TrimStart('[').TrimEnd(']');
                return parts.Length == 3 ? new FieldDef() 
                {
                    Subject = new HelperSqlSubject.SubjectDef()
                    {
                        Schema = parts[0],
                        TableName = parts[1]
                    },
                    Name = parts[2]
                } : new FieldDef();
            }
        }

        public struct Relationship
        {
            public HelperSqlSubject Subject1;
            public HelperSqlSubject Subject2;

            public Relationship(HelperSqlSubject s1, HelperSqlSubject s2)
            {
                Subject1 = s1;
                Subject2 = s2;
            }

            public override int GetHashCode()
            {
                return (Subject1?.GetHashCode() ?? 0) + (Subject2?.GetHashCode() ?? 0);
            }

            public override bool Equals(object obj)
            {
                if (obj is Relationship)
                {
                    var other = (Relationship)obj;
                    return (this.Subject1 == other.Subject1 || this.Subject1 == other.Subject2)
                        && (this.Subject2 == other.Subject1 || this.Subject2 == other.Subject2);
                }
                return base.Equals(obj);
            }
        }
    }
}
