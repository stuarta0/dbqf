using dbqf.Configuration;
using dbqf.Sql.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbqf.core.tests.Configuration
{
    [TestFixture]
    public class QueryConfig_Fixture
    {
        [Test]
        public void Fluent_config()
        {
            var sql = "SELECT * FROM Table";
            var config = new MatrixConfiguration();
            config
                .Subject((ISqlSubject)new SqlSubject()
                    .SqlQuery(sql)
                    .Field(new Field("column1", "display1", typeof(string)))
                    .Field(new Field("column2", "display2", typeof(int))));

            Assert.AreEqual(config.Count, 1);

            var s = (ISqlSubject)config[0];
            Assert.AreEqual(s.Sql, sql);
            Assert.AreEqual(s.Count, 2);

            var f1 = s[0];
            var f2 = s[1];
            Assert.AreEqual(f1.SourceName, "column1");
            Assert.AreEqual(f1.DisplayName, "display1");
            Assert.AreEqual(f1.DataType, typeof(string));

            Assert.AreEqual(f2.SourceName, "column2");
            Assert.AreEqual(f2.DisplayName, "display2");
            Assert.AreEqual(f2.DataType, typeof(int));
        }

        [Test]
        public void Fluent_id_field()
        {
            IField f;
            var subject = new Subject()
                .FieldId(f = new Field("column1", typeof(string)));

            Assert.AreSame(f, subject.IdField);
        }

        [Test]
        public void Fluent_default_field()
        {
            IField f;
            var subject = new Subject()
                .FieldDefault(f = new Field("column1", typeof(string)));

            Assert.AreSame(f, subject.DefaultField);
        }

        [Test]
        public void Fluent_config_complex_field()
        {
            ISqlSubject s1, s2;
            var config = new MatrixConfiguration();
            config
                .Subject(s1 = (ISqlSubject)new SqlSubject()
                    .Field(new Field("column1", "display1", typeof(string))))
                .Subject(s2 = (ISqlSubject)new SqlSubject()
                    .Field(new RelationField("column1", "display1", typeof(Guid), s1)));

            Assert.AreEqual(config.Count, 2);
            Assert.AreEqual(s2.Count, 1);
            Assert.AreEqual(s2[0].GetType(), typeof(RelationField));

            RelationField f1 = (RelationField)s2[0];
            Assert.AreEqual(f1.SourceName, "column1");
            Assert.AreEqual(f1.DisplayName, "display1");
            Assert.AreEqual(f1.DataType, typeof(Guid));
        }

        [Test]
        public void Fluent_config_matrix()
        {
            ISqlSubject s1, s2;
            var config = new MatrixConfiguration();
            config
                .Subject(s1 = (ISqlSubject)new SqlSubject()
                    .Field(new Field("column1", "display1", typeof(string))))
                .Subject(s2 = (ISqlSubject)new SqlSubject()
                    .Field(new RelationField("column1", "display1", typeof(Guid), s1)));

            MatrixNode n;
            n = config[s1, s1];
            n = config[s1, s2];
            n = config[s2, s1];
            n = config[s2, s2];
        }
    }
}
