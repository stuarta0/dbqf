using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

namespace DbQueryFramework_v1.Utils
{
    /// <summary>
    /// Helper class to (de)serialise objects; usually configurations.
    /// </summary>
    public class Serialization
    {
        public static XmlTextWriter Serialize(object instance, string filename)
        {
            XmlSerializer serializer = new XmlSerializer(instance.GetType());

            // Create an XmlTextWriter using a FileStream
            FileStream fs = new FileStream(filename, FileMode.Create);
            XmlTextWriter writer = new XmlTextWriter(fs, new UTF8Encoding());
            writer.Formatting = Formatting.Indented;
            writer.IndentChar = ' ';
            writer.Indentation = 3;

            // Serialize using the XmlTextWriter
            serializer.Serialize(writer, instance);
            writer.Close();

            return writer;
        }

        public static XmlTextWriter Serialize(object instance, Stream output)
        {
            XmlSerializer serializer = new XmlSerializer(instance.GetType());

            // Create an XmlTextWriter using a Stream
            XmlTextWriter writer = new XmlTextWriter(output, new UTF8Encoding());
            writer.Formatting = Formatting.Indented;
            writer.IndentChar = ' ';
            writer.Indentation = 4;

            // Serialize using the XmlTextWriter
            serializer.Serialize(writer, instance);
            writer.Flush();

            return writer;
        }

        public static object Deserialize(string filename, Type type)
        {
            object instance = null;

            XmlReader reader = null;
            XmlSerializer serializer = null;
            FileStream fs = null;
            try
            {
                // Create an instance of the XmlSerializer specifying type and namespace.
                serializer = new XmlSerializer(type);

                // A FileStream is needed to read the XML document.
                fs = new FileStream(filename, FileMode.Open);
                reader = new XmlTextReader(fs);

                instance = serializer.Deserialize(reader);
            }
            finally
            {
                if (fs != null)
                    fs.Close();

                if (reader != null)
                    reader.Close();
            }

            return instance;
        }

        public static T Deserialize<T>(string filename)
        {
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                return Deserialize<T>(fs);
        }

        public static T Deserialize<T>(Stream stream)
        {
            T instance = default(T);

            XmlReader reader = null;
            XmlSerializer serializer = null;
            try
            {
                // Create an instance of the XmlSerializer specifying type and namespace.
                serializer = new XmlSerializer(typeof(T));

                // A stream is needed to read the XML document.
                reader = new XmlTextReader(stream);
                instance = (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return instance;
        }

    }
}
