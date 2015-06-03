using dbqf.Display;
using Standalone.Core.Serialization.Assemblers.Display;
using Standalone.Core.Serialization.DTO.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Standalone.Core.Export
{
    public class XmlViewPersistence : IViewPersistence
    {
        private PartViewAssembler _assembler;
        public XmlViewPersistence(PartViewAssembler assembler)
        {
            _assembler = assembler;
        }

        public void Save(string filename, IPartView part)
        {
            if (part == null)
                return;

            var ws = new XmlWriterSettings();
            ws.Indent = true;
            ws.IndentChars = "  ";
            ws.CheckCharacters = true;

            var serializer = new XmlSerializer(typeof(PartViewDTO));
            try
            {
                using (var writer = XmlWriter.Create(filename, ws))
                    serializer.Serialize(writer, _assembler.Create(part));
            }
            catch (Exception ex)
            {

            }
        }

        public IPartView Load(string filename)
        {
            var rs = new XmlReaderSettings();
            rs.CheckCharacters = true; // required to read special characters like new line and tab

            var serializer = new XmlSerializer(typeof(PartViewDTO));
            using (var reader = XmlReader.Create(filename, rs))
                return _assembler.Restore((PartViewDTO)serializer.Deserialize(reader));
        }
    }
}
