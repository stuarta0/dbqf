using System;
using System.Xml.Serialization;

namespace dbqf.Serialization.DTO.Parsers
{
    [XmlRoot("DelimitedParser")]
    public class DelimitedParserDTO : ParserDTO
    {
        public DelimitedParserDTO()
        {
        }

        public DelimitedParserDTO(string delimiters)
        {
            DelimiterString = delimiters;
        }

        /// <summary>
        /// Creates a DTO with the array of delimiters.
        /// If there is at least one delimiter with more than 1 character it will use the array element.
        /// If all delimiters are only 1 character it will use the string attribute.
        /// </summary>
        /// <param name="delimiters"></param>
        public DelimitedParserDTO(string[] delimiters)
        {
            if (delimiters == null)
                return;

            delimiters.RemoveAll(s => s == null);
            if (delimiters.Length > 0)
            {
                bool allSingleChar = true;
                foreach (var d in delimiters)
                    if (d != null && d.Length > 1)
                        allSingleChar = false;

                if (allSingleChar)
                    DelimiterString = String.Join("", delimiters);
                else
                    Delimiters = delimiters;
            }
        }

        [XmlAttribute("Delimiters")]
        public string DelimiterString { get; set; }

        [XmlElement("Delimiter")]
        public string[] Delimiters { get; set; }

        [XmlIgnore]
        public bool DelimiterStringSpecified
        {
            get { return !String.IsNullOrEmpty(DelimiterString); }
        }

        public string[] GetDelimiters()
        {
            if (DelimiterStringSpecified)
            {
                string[] result = new string[DelimiterString.Length];
                for (int i = 0; i < result.Length; i++)
                    result[i] = DelimiterString[i].ToString();
                return result;
            }
            else
                return Delimiters;
        }
    }
}
