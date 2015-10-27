
namespace dbqf.Sql.Criterion
{
    /// <summary>
    /// Combine parameters using OR.
    /// </summary>
    public class SqlDisjunction : SqlJunction
    {
        protected override string Op
        {
            get { return "or"; }
        }

        public SqlDisjunction()
        {
        }
    }
}
