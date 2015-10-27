using dbqf.Configuration;
using dbqf.Processing;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Criterion
{
    public class LikeParameter : IParameter
    {
        public IFieldPath Path { get; private set; }
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

        public LikeParameter(IFieldPath path, string value)
            : this(path, value, MatchMode.Anywhere)
        {
        }

        public LikeParameter(IFieldPath path, string value, MatchMode mode)
        {
            Path = path;
            Value = value;
            Mode = mode;
        }
    }
}
