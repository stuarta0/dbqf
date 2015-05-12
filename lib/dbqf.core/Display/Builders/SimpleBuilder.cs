using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Display.Builders
{
    public class SimpleBuilder : ParameterBuilder
    {
        public virtual string Operator { get; set; }

        public override string Label
        {
            get
            {
                if (base.Label == null)
                    return Operator;
                return base.Label;
            }
            set { base.Label = value; }
        }

        public SimpleBuilder()
        {
        }
        public SimpleBuilder(string op)
        {
            Operator = op;
        }

        /// <summary>
        /// Works with any value that can be interpreted by the data provider being used.
        /// </summary>
        public override IParameter Build(FieldPath path, params object[] values)
        {
            if (values == null)
                return null;

            Junction.Clear();
            foreach (var v in values)
                Junction.Add(new SimpleParameter(path, Operator, v));
            return Junction;
        }
    }
}
