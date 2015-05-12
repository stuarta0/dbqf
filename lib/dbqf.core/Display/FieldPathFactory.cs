using System;
using System.Collections.Generic;
using System.Text;
using dbqf.Configuration;
using dbqf.Criterion;

namespace dbqf.Display
{
    public class FieldPathFactory : IFieldPathFactory
    {
        public virtual IList<FieldPath> GetFields(ISubject subject)
        {
            var result = new List<FieldPath>();
            foreach (var f in subject)
            {
                if (!f.Equals(subject.IdField))
                {
                    var path = FieldPath.FromDefault(f);

                    // when we generate a list of fields from a single subject, remove the subject name from the resulting path name
                    path.Description = path.Description.Substring((subject.DisplayName ?? string.Empty).Length + 1);
                    result.Add(path);
                }
            }
            return result;
        }
    }
}
