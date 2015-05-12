using dbqf.Configuration;
using dbqf.Processing;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Criterion
{
    public class NullParameter : IParameter
    {
        private FieldPath _path;

        public NullParameter(IField field)
            : this(FieldPath.FromDefault(field))
        {
        }

        public NullParameter(FieldPath path)
        {
            _path = path;
        }

        public SqlString ToSqlString()
        {
            return new SqlString()
                .AddField(_path)
                .Add(" IS NULL");
        }
    }
}
