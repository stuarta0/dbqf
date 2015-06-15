using dbqf.Configuration;

namespace dbqf.tests
{
    public class Chinook : ConfigurationImpl
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

        private ISubject _artist;
        public ISubject Artist
        {
            get
            {
                if (_artist == null)
                    _artist = new Subject("Artist")
                        .Sql("Artist")
                        .FieldId(new Field("ArtistId", typeof(int)))
                        .FieldDefault(new Field("Name", typeof(string)));
                return _artist;
            }
        }

        private ISubject _album;
        public ISubject Album
        {
            get
            {
                if (_album == null)
                    _album = new Subject("Album")
                        .Sql("Album")
                        .FieldId(new Field("AlbumId", typeof(int)))
                        .FieldDefault(new Field("Title", typeof(string)))
                        .Field(new RelationField("ArtistId", "Artist", Artist));
                return _album;
            }
        }

        private ISubject _track;
        public ISubject Track
        {
            get
            {
                if (_track == null)
                    _track = new Subject("Track")
                        .Sql(@"SELECT Track.*, MediaType.Name MTN, Genre.Name GN FROM Track 
LEFT OUTER JOIN MediaType ON MediaType.MediaTypeId = Track.MediaTypeId
LEFT OUTER JOIN Genre ON Genre.GenreId = Track.GenreId")
                        .FieldId(new Field("TrackId", typeof(int)))
                        .FieldDefault(new Field("Name", typeof(string)))
                        .Field(new RelationField("AlbumId", "Album", Album))
                        .Field(new Field("Composer", typeof(string)))
                        .Field(new Field("GN", "Genre", typeof(string)))
                        .Field(new Field("MTN", "Format", typeof(string)))
                        .Field(new Field("Milliseconds", typeof(long)))
                        .Field(new Field("Bytes", typeof(long)))
                        .Field(new Field("UnitPrice", typeof(double)));
                return _track;
            }
        }

        private ISubject _playlist;
        public ISubject Playlist
        {
            get
            {
                if (_playlist == null)
                    _playlist = new Subject("Playlist")
                        .Sql("Playlist")
                        .FieldId(new Field("PlaylistId", typeof(int)))
                        .FieldDefault(new Field("Name", typeof(string)));
                return _playlist;
            }
        }

        // Join can be handle via matrix
        //private ISubject _playlistTrack;
        //public ISubject PlaylistTrack
        //{
        //    get
        //    {
        //        if (_playlistTrack == null)
        //            _playlistTrack = new Subject("PlaylistTrack")
        //                .Sql("Album")
        //                .FieldId(new Field("PlaylistId", typeof(int)))
        //                .FieldDefault(new Field("Name", typeof(string)))
        //                .Field(new RelationField("ArtistId", "Artist", typeof(int), Artist, null, null));
        //        return _playlistTrack;
        //    }
        //}

        //private ISubject _employee;
        //public ISubject Employee
        //{
        //    get
        //    {
        //    }
        //}

        //private ISubject _customer;
        //public ISubject Customer
        //{
        //    get
        //    {
        //    }
        //}

        //private ISubject _invoice;
        //public ISubject Invoice
        //{
        //    get
        //    {
        //    }
        //}

        //private ISubject _invoiceLine;
        //public ISubject InvoiceLine
        //{
        //    get
        //    {
        //    }
        //}
    }
}
