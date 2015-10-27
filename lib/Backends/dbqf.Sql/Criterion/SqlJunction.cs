using dbqf.Criterion;
using System;

namespace dbqf.Sql.Criterion
{
    public abstract class SqlJunction : Junction, ISqlParameter
    {
        public SqlJunction()
            : base()
        {
        }

        /// <summary>
        /// Fluently add a parameter to this junction.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual SqlJunction Parameter(ISqlParameter parameter)
        {
            Add(parameter);
            return this;
        }

        #region Type-checking base class overrides

        private void TypeCheck(IParameter item)
        {
            if (!(item is ISqlParameter))
                throw new ArgumentException("Arguments must be of type ISqlParameter when using SqlJunction.");
        }

        public override void Insert(int index, IParameter item)
        {
            TypeCheck(item);
            base.Insert(index, item);
        }

        public override IParameter this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                TypeCheck(value);
                base[index] = value;
            }
        }

        public override void Add(IParameter item)
        {
            TypeCheck(item);
            base.Add(item);
        }

        #endregion

        public SqlString ToSqlString()
        {
            var sql = new SqlString();
            for (int i = 0; i < this.Count; i++)
            {
                // add the parameters if there's something to add
                var p = ((ISqlParameter)this[i]).ToSqlString().Flatten();
                if (p.Parts.Count > 0)
                    sql.Add(p);
            }

            // insert the operator between all the parts
            for (int i = 0; i < sql.Parts.Count - 1; i += 2)
                sql.Parts.Insert(i + 1, String.Concat(" ", Op, " "));

            // if we managed to produce something, wrap it in parens
            if (sql.Parts.Count > 0)
            {
                sql.Parts.Insert(0, "(");
                sql.Parts.Add(")");
            }

            return sql;
        }
    }
}
