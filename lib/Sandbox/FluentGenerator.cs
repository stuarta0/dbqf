using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using dbqf.Configuration;
using dbqf.Sql.Configuration;

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
    ///   private ISqlSubject _subject1;
    ///   public ISqlSubject Subject1
    ///   {
    ///       get
    ///       {
    ///           if (_subject1 == null)
    ///               _subject1 = new SqlSubject("Subject1")
    ///                   .SqlQuery("...")
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
            public string MemberName;
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
                MemberName = String.Concat("_", Name.ToLower());
            }
        }

        public string Generate(IMatrixConfiguration configuration, string namespaceName, string className)
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
                    var node = configuration[(ISqlSubject)configuration[i], (ISqlSubject)configuration[j]];
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
            foreach (ISqlSubject subject in configuration)
            {
                sb.AppendLine(String.Format(@"
        private ISqlSubject {0};
        public ISqlSubject {1}
        {{
            get
            {{
                if ({0} == null)
                {{
                    {0} = new SqlSubject({2})
                        .SqlQuery(@{3})
                        .{4};",
                    names[subject].MemberName,
                    names[subject].Name,
                    Quote(subject.DisplayName),
                    Quote(subject.Sql),
                    FieldText(subject.IdField, names) // we need to set this up before continuing with the remaining fields as RelationFields reference the related subject IdField for DataType
                    ));

                sb.AppendLine(String.Format(@"
                    {0}", names[subject].MemberName));

                // fill in the fields (ID field is already taken care of)
                foreach (var field in subject)
                {
                    if (field != subject.IdField)
                        sb.AppendLine(String.Concat(@"                        .", FieldText(field, names)));
                }
                sb.AppendLine(String.Format(@"
                ;}}
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

        private string FieldText(IField field, Dictionary<ISubject, SubjectName> names)
        {
            var subject = field.Subject;
            string list = null;
            if (field.List != null)
                list = String.Format("{{ List = new FieldList() {{ Source = {0}, Type = FieldListType.{1} }}}}", Quote(field.List.Source), field.List.Type.ToString());

            if (field is IRelationField)
            {
                return String.Format(@"{0}(new RelationField({1}, {2}, {3}){4})",
                        field == subject.IdField ? "FieldId" : field == subject.DefaultField ? "FieldDefault" : "Field",
                        Quote(field.SourceName),
                        Quote(field.DisplayName),
                        names[((IRelationField)field).RelatedSubject].Name,
                        list);
            }
            else
            {
                return String.Format(@"{0}(new Field({1}, {2}, typeof({3})){4})",
                        field == subject.IdField ? "FieldId" : field == subject.DefaultField ? "FieldDefault" : "Field",
                        Quote(field.SourceName),
                        Quote(field.DisplayName),
                        field.DataType.FullName,
                        list);
            }
        }

        private string Quote(string value)
        {
            return String.Concat("\"", value, "\"");
        }
    }
}
