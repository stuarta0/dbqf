using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using dbqf.Configuration;

namespace Sandbox
{
    /// <summary>
    /// Generates a string that contains a fluent class definition for a configuration with the following structure:
    /// 
    /// public class Config : ConfigurationImpl
    /// {
    ///   public Config()
    ///     : base()
    ///   {
    ///     this
    ///       .Subject(Subject1)
    ///       .Subject(Subject2)
    ///       .Matrix(Subject1, Subject2, "...")
    ///       .Matrix(Subject2, Subject1, "...");
    ///   }
    /// 
    ///   private ISubject _subject1;
    ///   public ISubject Subject1
    ///   {
    ///       get
    ///       {
    ///           if (_subject1 == null)
    ///               _subject1 = new Subject("Subject1")
    ///                   .Sql("...")
    ///                   .FieldId(new Field("Field1", typeof(int)))
    ///                   .FieldDefault(new Field("Field2", typeof(string)))
    ///                   .Field(new RelationField("Field3", Subject2));
    ///           return _subject1;
    ///       }
    ///   }
    /// }
    /// </summary>
    public class FluentGenerator
    {
        private class SubjectName
        {
            public string Name;
            public Dictionary<IField, string> FieldNames;
            public string this[IField field]
            {
                get { return FieldNames[field]; }
            }

            public SubjectName(ISubject subject)
            {
                var reg = new Regex("[^A-Za-z0-9]");
                Name = reg.Replace(subject.DisplayName, "");
                FieldNames = new Dictionary<IField,string>();
                foreach (var field in subject)
                    FieldNames.Add(field, reg.Replace(field.DisplayName, ""));
            }
        }

        public string Generate(IConfiguration configuration, string namespaceName, string className)
        {
            // I could use T4, but that requires time spent learning it which I'm short on at the moment
            var sb = new StringBuilder(String.Format(@"
using dbqf.Configuration;

namespace {0}
{{
    public class {1} : ConfigurationImpl
    {{
        public {1}()
            : base()
        {{
            this
",
                namespaceName,
                className));


            // sanitise subject/field names
            var names = new Dictionary<ISubject, SubjectName>();

            // add constructor body
            foreach (var subject in configuration)
            {
                names.Add(subject, new SubjectName(subject));
                sb.AppendLine(String.Format("            .Subject({0})", names[subject].Name));
            }
            for (int i = 0; i < configuration.Count; i++)
            {
                for (int j = 0; j < configuration.Count; j++)
                {
                    var node = configuration[configuration[i], configuration[j]];
                    if (!String.IsNullOrWhiteSpace(node.Query))
                        sb.AppendLine(String.Format("            .Matrix({0}, {1}, @{2}, @{3})", 
                            names[configuration[i]].Name, names[configuration[j]].Name, 
                            Quote(node.Query), Quote(node.ToolTip)));
                }
            }
            sb.AppendLine(@"
            ;
        }

");

            // add subject properties
            foreach (var subject in configuration)
            {
                sb.AppendLine(String.Format(@"
        private ISubject {0};
        public ISubject {1}
        {{
            get
            {{
                if ({0} == null)
                    {0} = new Subject({2})
                        .Sql(@{3})",
                    String.Concat("_", names[subject].Name.ToLower()),
                    names[subject].Name,
                    Quote(subject.DisplayName),
                    Quote(subject.Source)
                    ));

                // fill in the fields
                foreach (var field in subject)
                {
                    string list = null;
                    if (field.List != null)
                        list = String.Format("{{ List = new FieldList() {{ Source = {0}, Type = FieldListType.{1} }}}}", Quote(field.List.Source), field.List.Type.ToString());

                    if (field is IRelationField)
                    {
                        sb.AppendLine(String.Format(@"                        .{0}(new RelationField({1}, {2}, {3}){4})",
                                field == subject.IdField ? "FieldId" : field == subject.DefaultField ? "FieldDefault" : "Field",
                                Quote(field.SourceName),
                                Quote(field.DisplayName),
                                names[((IRelationField)field).RelatedSubject].Name,
                                list
                                ));
                    }
                    else
                    {
                        sb.AppendLine(String.Format(@"                        .{0}(new Field({1}, {2}, typeof({3})){4})",
                                field == subject.IdField ? "FieldId" : field == subject.DefaultField ? "FieldDefault" : "Field",
                                Quote(field.SourceName),
                                Quote(field.DisplayName),
                                field.DataType.Name,
                                list
                                ));
                    }
                }
                sb.AppendLine(String.Format(@"
                return {0};
            }}
        }}
", String.Concat("_", names[subject].Name.ToLower())));
            }

            // end of file
            sb.AppendLine(@"
    }
}");

            return sb.ToString();
        }

        private string Quote(string value)
        {
            return String.Concat("\"", value, "\"");
        }
    }
}
