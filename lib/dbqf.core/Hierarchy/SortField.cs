using System;
using System.Xml.Serialization;
using System.Diagnostics;

namespace dbqf
{
	[DebuggerDisplay("{FieldSourceName} {SortDirection}")]
	public class SortField
	{
		public enum Direction { Ascending, Descending }
		
		/// <summary>
		/// Gets or sets the source name of the field.
		/// </summary>
		[XmlAttribute]
		public string FieldSourceName { get; set; }
		
		/// <summary>
		/// Gets or sets the sort direction.
		/// </summary>
		[XmlAttribute]
		public Direction SortDirection { get; set; }
		
		public SortField ()
		{
		}
		
		public override string ToString ()
		{
			return string.Format ("{0} {1}", FieldSourceName, SortDirection);
		}
	}
}

