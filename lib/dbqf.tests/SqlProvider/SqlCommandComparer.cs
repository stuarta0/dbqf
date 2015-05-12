using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Data.SqlClient;
using System.Data;

namespace dbqf.tests.SqlProvider
{
    class SqlCommandComparer
    {
        private string _connString;

        public SqlCommandComparer(string connectionString)
        {
            _connString = connectionString;
        }

        public void Test(List<object> searchResults, string commandText)
        {
            using (var conn = new SqlConnection(_connString))
            {
                int count = 0;
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = commandText;

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count++;
                            var id = reader.GetValue(0);
                            Assert.IsTrue(searchResults.Contains(id));
                        }
                    }
                }

                Assert.AreEqual(count, searchResults.Count);
            }
        }

        public void Test(string commandText1, string commandText2)
        {
            //var cmd = new SqlCommand();
            //cmd.CommandText = commandText1;
            //Test(cmd, commandText2);

            var results = new DataTable();
            using (var conn = new SqlConnection(_connString))
            {
                using (var adapter = new SqlDataAdapter(commandText1, conn))
                    adapter.Fill(results);
            }
            Test(results, commandText2);
        }

        public void Test(IDbCommand cmd, string commandText2)
        {
            var items = new List<object>();
            using (var conn = new SqlConnection(_connString))
            {
                cmd.Connection = conn;

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        items.Add(reader.GetValue(0));
                }
            }

            Test(items, commandText2);
        }

        public void Test(DataTable searchResults, string commandText)
        {
            var expected = new DataTable();
            using (var conn = new SqlConnection(_connString))
            {
                using (var adapter = new SqlDataAdapter(commandText, conn))
                    adapter.Fill(expected);
            }

            Assert.AreEqual(expected.Columns.Count, searchResults.Columns.Count);
            Assert.AreEqual(expected.Rows.Count, searchResults.Rows.Count);
            for (int r = 0; r < expected.Rows.Count; r++)
            {
                for (int c = 0; c < expected.Columns.Count; c++)
                    Assert.AreEqual(expected.Rows[r][c], searchResults.Rows[r][c]);
            }
        }
    }
}
