using dbqf.Configuration;
using dbqf.Sql.Configuration;

namespace dbqf.core.tests
{
    public class Chinook : MatrixConfiguration
    {
        public Chinook()
            : base()
        {
            this
                .Subject(Artist)
                .Subject(Album)
                .Subject(Track)
                .Subject(Playlist)

                .Matrix(Artist, Album, "SELECT ArtistId FromID, AlbumID ToID FROM Album", "")
                .Matrix(Artist, Track, "SELECT ArtistId FromID, TrackID ToID FROM Album INNER JOIN Track ON Album.AlbumId = Track.AlbumId", "")
                .Matrix(Artist, Playlist, "SELECT ArtistId FromID, PlaylistId ToID FROM Album INNER JOIN Track ON Album.AlbumId = Track.AlbumId INNER JOIN PlaylistTrack ON PlaylistTrack.TrackId = Track.TrackId", "")
                
                .Matrix(Album, Artist, "SELECT AlbumId FromID, ArtistID ToID FROM Album", "")
                .Matrix(Album, Track, "SELECT AlbumId FromID, TrackID ToID FROM Track", "")
                .Matrix(Album, Playlist, "SELECT AlbumId FromID, PlaylistId ToID FROM Track INNER JOIN PlaylistTrack ON PlaylistTrack.TrackId = Track.TrackId", "")

                .Matrix(Track, Artist, "SELECT TrackId FromID, ArtistID ToID FROM Album INNER JOIN Track ON Album.AlbumId = Track.AlbumId", "")
                .Matrix(Track, Album, "SELECT TrackId FromID, AlbumId ToID FROM Track", "")
                .Matrix(Track, Playlist, "SELECT TrackId FromID, PlaylistId ToID FROM PlaylistTrack", "")

                .Matrix(Playlist, Artist, "SELECT PlaylistId FromID, ArtistId ToID FROM Album INNER JOIN Track ON Album.AlbumId = Track.AlbumId INNER JOIN PlaylistTrack ON PlaylistTrack.TrackId = Track.TrackId", "")
                .Matrix(Playlist, Album, "SELECT PlaylistId FromID, AlbumId ToID FROM Track INNER JOIN PlaylistTrack ON PlaylistTrack.TrackId = Track.TrackId", "")
                .Matrix(Playlist, Track, "SELECT PlaylistId FromID, TrackId ToID FROM PlaylistTrack", "")
                
                ;
        }

        private ISqlSubject _artist;
        public ISqlSubject Artist
        {
            get
            {
                if (_artist == null)
                    _artist = new SqlSubject("Artist")
                        .SqlQuery("Artist")
                        .FieldId(new Field("ArtistId", typeof(int)))
                        .FieldDefault(new Field("Name", typeof(string))
                        {
                            List = new FieldList() { Source = "SELECT DISTINCT Name AS Value FROM Artist ORDER BY Name" }
                        });
                return _artist;
            }
        }

        private ISqlSubject _album;
        public ISqlSubject Album
        {
            get
            {
                if (_album == null)
                    _album = new SqlSubject("Album")
                        .SqlQuery("Album")
                        .FieldId(new Field("AlbumId", typeof(int)))
                        .FieldDefault(new Field("Title", typeof(string))
                        {
                            List = new FieldList() { Source = "SELECT DISTINCT Title AS Value FROM Album ORDER BY Title" }
                        })
                        .Field(new RelationField("ArtistId", "Artist", Artist));
                return _album;
            }
        }

        private ISqlSubject _track;
        public ISqlSubject Track
        {
            get
            {
                if (_track == null)
                    _track = new SqlSubject("Track")
                        .SqlQuery(@"SELECT Track.*, MediaType.Name MTN, Genre.Name GN FROM Track 
LEFT OUTER JOIN MediaType ON MediaType.MediaTypeId = Track.MediaTypeId
LEFT OUTER JOIN Genre ON Genre.GenreId = Track.GenreId")
                        .FieldId(new Field("TrackId", typeof(int)))
                        .FieldDefault(new Field("Name", typeof(string))
                        {
                            List = new FieldList() { Source = "SELECT DISTINCT Name AS Value FROM Track ORDER BY Name" }
                        })
                        .Field(new RelationField("AlbumId", "Album", Album))
                        .Field(new Field("Composer", typeof(string)))
                        .Field(new Field("GN", "Genre", typeof(string)))
                        .Field(new Field("MTN", "Format", typeof(string)))
                        .Field(new Field("Milliseconds", typeof(long)))
                        .Field(new Field("Bytes", typeof(long)))
                        .Field(new Field("UnitPrice", "Price", typeof(double)));
                return _track;
            }
        }

        private ISqlSubject _playlist;
        public ISqlSubject Playlist
        {
            get
            {
                if (_playlist == null)
                    _playlist = new SqlSubject("Playlist")
                        .SqlQuery("Playlist")
                        .FieldId(new Field("PlaylistId", typeof(int)))
                        .FieldDefault(new Field("Name", typeof(string)));
                return _playlist;
            }
        }

        // Join can be handle via matrix
        //private ISqlSubject _playlistTrack;
        //public ISqlSubject PlaylistTrack
        //{
        //    get
        //    {
        //        if (_playlistTrack == null)
        //            _playlistTrack = new SqlSubject("PlaylistTrack")
        //                .SqlQuery("Album")
        //                .FieldId(new Field("PlaylistId", typeof(int)))
        //                .FieldDefault(new Field("Name", typeof(string)))
        //                .Field(new RelationField("ArtistId", "Artist", typeof(int), Artist, null, null));
        //        return _playlistTrack;
        //    }
        //}

        //private ISqlSubject _employee;
        //public ISqlSubject Employee
        //{
        //    get
        //    {
        //    }
        //}

        //private ISqlSubject _customer;
        //public ISqlSubject Customer
        //{
        //    get
        //    {
        //    }
        //}

        //private ISqlSubject _invoice;
        //public ISqlSubject Invoice
        //{
        //    get
        //    {
        //    }
        //}

        //private ISqlSubject _invoiceLine;
        //public ISqlSubject InvoiceLine
        //{
        //    get
        //    {
        //    }
        //}
    }
}
