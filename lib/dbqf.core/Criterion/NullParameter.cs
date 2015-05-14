using dbqf.Configuration;
using dbqf.Processing;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Criterion
{
    public class NullParameter : IParameter
    {
        public FieldPath Path { get; private set; }

        public NullParameter(IField field)
            : this(FieldPath.FromDefault(field))
        {
        }

        public NullParameter(FieldPath path)
        {
            Path = path;
        }

        public SqlString ToSqlString()
        {
            return new SqlString()
                .AddField(Path)
                .Add(" IS NULL");
        }
    }
}
