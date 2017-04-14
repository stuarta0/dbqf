using System;
using dbqf.Configuration;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace dbqf
{
	public class ItemData
	{
		/// <summary>
		/// Gets or sets the subject from which this item was retrieved.
		/// </summary>
		public ISubject Subject { get; set; }
		
		/// <summary>
		/// Gets or sets the identifier of the item from the subject.
		/// </summary>
		public object Id { get; set; }
		
		/// <summary>
		/// Gets or sets the fields of data for this item.  Note: fields should all belong to this object's Subject and be the base Field type.
		/// </summary>
		public Dictionary<IField, object> Data { get; set; }
		
		public ItemData ()
		{
			Data = new Dictionary<IField, object>();
		}

        public static Dictionary<string, IField> GetPlaceholders(ISubject subject, string placeholderText)
        {
            var fields = new Dictionary<string, IField>();

            var matches = Regex.Matches(placeholderText, @"\[[^\]]+\]");
            foreach (Match m in matches)
            {
                if (!fields.ContainsKey(m.Value))
                {
                    string key = m.Value.Trim('[', ']');
                    fields.Add(key, subject[key]);
                }
            }

            return fields;
        }

		protected virtual Dictionary<string, IField> GetPlaceholders(string placeholderText)
        {
            return GetPlaceholders(Subject, placeholderText);
        }
		
		/// <summary>
		/// Replaces the placeholders in the given string with data from this item.
		/// </summary>
		/// <returns>
		/// A string with data populated from this item.
		/// </returns>
		/// <param name='placeholderText'>
		/// Placeholder text with fields in [brackets].  If this item does not contain data for a field specified in the text, then it will be replaced with an empty string.
		/// </param>
		/// <exception cref='ArgumentException'>
		/// Is thrown when the Data collection contains fields from a different subject.
		/// </exception>
		public string ReplacePlaceholders(string placeholderText)
        {
			// ensure all fields are part of the same subject
			foreach (Field f in Data.Keys)
			{
				if (!f.Subject.Equals (Subject))
					throw new ArgumentException(String.Concat ("Field [", f.DisplayName, "] is not part of the subject [", Subject.DisplayName, "].  All fields must belong to the same subject."));
			}
			
			var placeholders = GetPlaceholders(placeholderText);
        	return Regex.Replace(placeholderText, @"\[[^\]]+\]", 
                new MatchEvaluator((m) =>
                {
                    // m.Value contains the field name with brackets
                    string fieldName = m.Value.Trim('[', ']');
				
					if (placeholders.ContainsKey (fieldName) && Data.ContainsKey (placeholders[fieldName]))
					{
						var field = placeholders[fieldName];
						if (Data[field] == null)
							return String.Empty;
						else if (!String.IsNullOrEmpty (field.DisplayFormat))
							return String.Format (String.Concat ("{0:", field.DisplayFormat, "}"), Data[field]);
						
						return Data[field].ToString ();
					}
				
					return String.Empty;
                }));
        }
	}
}

