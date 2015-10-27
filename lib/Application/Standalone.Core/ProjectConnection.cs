using System.Diagnostics;
using System.Xml.Serialization;

namespace Standalone.Core
{
    [DebuggerDisplay("{Identifier}: {DisplayName}")]
    public abstract class ProjectConnection
    {
        /// <summary>
        /// Gets or sets a name to display to the user to identify this connection.
        /// </summary>
        [XmlAttribute]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier for the connection within a project.
        /// </summary>
        [XmlAttribute]
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the connection string to use with the corresponding type of connection.
        /// </summary>
        [XmlText]
        public string ConnectionString { get; set; }
    }

    [XmlRoot("SqlConnection")]
    public class SqlProjectConnection : ProjectConnection
    {
    }

    [XmlRoot("SQLiteConnection")]
    public class SQLiteProjectConnection : ProjectConnection
    {
    }

    [XmlRoot("MsAccessConnection")]
    public class MsAccessProjectConnection : ProjectConnection
    {
    }
}
