using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Sql;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbqf.core.tests.Processing
{
    [TestFixture]
    public class Sqlite_Fixture
    {
        private Chinook _db;

        [SetUp]
        public void Setup()
        {
            _db = new Chinook();
        }

        //[Test]
        //public void Get_tracks()
        //{
        //    var cmd = new SqlGenerator(_db)
        //        .Target(_db.Track)
        //        .Column(FieldPath.FromDefault(_db.Track["Name"]))
        //        .ToCommand(typeof(SQLiteCommand));

        //    Console.WriteLine(cmd.CommandText);
        //}

        //[Test]
        //public void Get_tracks_for_artist()
        //{
        //    var cmd = new SqlGenerator(_db)
        //        .Target(_db.Track)
        //        .Column(FieldPath.FromDefault(_db.Track["Name"]))
        //        .Column(FieldPath.FromDefault(_db.Track["AlbumId"]))
        //        .Column(new FieldPath(new IField[] { _db.Track["AlbumId"], _db.Album["ArtistId"], _db.Artist.DefaultField }))
        //        .Where(new SimpleParameter(FieldPath.FromDefault(_db.Artist.DefaultField), "=", "Santana"))
        //        .ToCommand(typeof(SQLiteCommand));

        //    Console.WriteLine(cmd.CommandText);
        //}
    }
}
