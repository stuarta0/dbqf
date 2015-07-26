using dbqf.Configuration;
using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace dbqf.Processing
{
    public class PlaceholderParser
    {
        /// <summary>
        /// Parse text containing placeholders for fields delimited by bracket.  
        /// The first stop delimits the subject.field, subsequent stops traverse relationships.
        /// For example:
        /// [Products.Name] is the "Products" subject, field "Name"
        /// [Products.ProductCategoryID.Name] is the "Products" subject, field "ProductCategoryID" which is a RelationField to subject "Product Category" with a field "Name".
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="texts"></param>
        /// <returns></returns>
        public virtual Dictionary<string, IFieldPath> Parse(IConfiguration config, params string[] texts)
        {
            var fields = new Dictionary<string, IFieldPath>();

            foreach (var t in texts)
            {
                var matches = Regex.Matches(t, @"\[((?<subject>[^\]\.]+)(\.([^\]\.]+))+)\]");
                foreach (Match m in matches)
                {
                    // represent the entire path as the key to the dictionary
                    if (!fields.ContainsKey(m.Groups[1].Value))
                    {
                        // now create the FieldPath based on the Regex
                        var path = new FieldPath();
                        var subject = config[m.Groups["subject"].Value];
                        if (subject != null)
                        {
                            foreach (Capture capture in m.Groups[3].Captures)
                            {
                                var field = subject[capture.Value];
                                if (field == null)
                                {
                                    path.Clear();
                                    break;
                                }

                                path.Add(field);
                                if (field is IRelationField)
                                    subject = ((IRelationField)field).RelatedSubject;
                            }

                            if (path.Count > 0)
                                fields.Add(m.Groups[0].Value, path);
                        }
                    }
                }
            }

            return fields;
        }

        /// <summary>
        /// Given a resulting dictionary of placeholders and associated values, replace the placeholders in each of the texts with the associated value.
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="texts"></param>
        /// <returns></returns>
        public virtual string[] Replace(Dictionary<string, string> values, params string[] texts)
        {
            string[] result = new string[texts.Length];
            for (int i = 0; i < texts.Length; i++)
            {
                result[i] = Regex.Replace(texts[i], @"\[[^\]]+\]", e =>
                    {
                        if (values.ContainsKey(e.Value))
                            return values[e.Value];
                        return null;
                    });
            }

            return result;
        }
    }
}
