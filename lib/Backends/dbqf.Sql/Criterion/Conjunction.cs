
namespace dbqf.Sql.Criterion
{
    /// <summary>
    /// Combine parameters using AND.
    /// </summary>
    public class SqlConjunction : SqlJunction
    {
        protected override string Op
        {
            get { return "and"; }
        }

        public SqlConjunction()
        {
        }
    }
}
