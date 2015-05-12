using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
 
namespace Standalone.Serialization
{
    [XmlRoot("dictionary")]
    public class SerializableDictionary<TKey, TValue>
        : Dictionary<TKey, TValue>, IXmlSerializable
    {
		#region Constructors
		
		public SerializableDictionary()
           : base()
       {
       }
		
       public SerializableDictionary(IDictionary<TKey, TValue> dictionary)
           : base(dictionary)
       {
       }
		
       public SerializableDictionary(IEqualityComparer<TKey> comparer)
           : base(comparer)
       {
       }
		
       public SerializableDictionary(int capacity)
           : base(capacity)
       {
       }
		
       public SerializableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
           : base(dictionary, comparer)
       {
       }
		
       public SerializableDictionary(int capacity, IEqualityComparer<TKey> comparer)
           : base(capacity, comparer)
       {
       }
		
       protected SerializableDictionary(SerializationInfo info, StreamingContext context)
           : base(info, context)
       {
       }
		
		#endregion
		
        #region IXmlSerializable Members
		
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }
 
        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
 
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();
 
            if (wasEmpty)
                return;
 
            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");

                this.Add(
                    GetObject<TKey>(reader, keySerializer, "key"), 
                    GetObject<TValue>(reader, valueSerializer, "value"));

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        private T GetObject<T>(System.Xml.XmlReader reader, XmlSerializer deserializer, string element)
        {
            T value = default(T);
            if (reader.IsStartElement())
            {
                if (reader.IsEmptyElement)
                    reader.Read();
                else
                {
                    reader.ReadStartElement(element);
                    value = (T)deserializer.Deserialize(reader);
                    reader.ReadEndElement();
                }
            }

            return value;
        }
 
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
 
            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement("item");
 
                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();
 
                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();
 
                writer.WriteEndElement();
            }
        }
		
        #endregion
    }

    public class ConnectionDictionary : SerializableDictionary<Guid, string>
    {
    }
}