using dbqf.Configuration;
using dbqf.Processing;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Criterion
{
    public class SimpleParameter : IParameter
    {
        public IFieldPath Path { get; protected set; }
        public string Operator { get; protected set;}
        public object Value { get; protected set; }

        public SimpleParameter(IField field, string op, object value)
            : this(FieldPath.FromDefault(field), op, value)
        {
        }

        public SimpleParameter(IFieldPath path, string op, object value)
        {
            Path = path;
            Operator = op;
            Value = value;
        }

        public SqlString ToSqlString()
        {
            return new SqlString()
                .AddField(Path)
                .Add(Operator)
                .AddParameter(Value);
        }
    }
}
