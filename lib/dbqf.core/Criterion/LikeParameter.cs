using dbqf.Configuration;
using dbqf.Processing;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Criterion
{
    public class LikeParameter : IParameter
    {
        public FieldPath Path { get; private set; }
        public string Value { get; private set; }
        public MatchMode Mode { get; private set; }

        public LikeParameter(IField field, string value)
            : this(FieldPath.FromDefault(field), value)
        {
        }

        public LikeParameter(IField field, string value, MatchMode mode)
            : this(FieldPath.FromDefault(field), value, mode)
        {
        }

        public LikeParameter(FieldPath path, string value)
            : this(path, value, MatchMode.Anywhere)
        {
        }

        public LikeParameter(FieldPath path, string value, MatchMode mode)
        {
            Path = path;
            Value = value;
            Mode = mode;
        }

        public SqlString ToSqlString()
        {
            return new SqlString()
                .AddField(Path)
                .Add(" LIKE ")
                .AddParameter(Mode.ToMatchString(Value));
        }
    }
}
