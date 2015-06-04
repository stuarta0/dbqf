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
using Standalone.Core.Serialization.DTO.Builders;

namespace Standalone.Core.Export
{
    public class XmlViewPersistence : IViewPersistence
    {
        private PartViewAssembler _assembler;
        public XmlViewPersistence(PartViewAssembler assembler)
        {
            _assembler = assembler;
        }

        public void Save(string filename, IList<IPartView> parts)
        {
            if (parts == null)
                return;

            var dtos = new List<PartViewDTO>();
            foreach (var p in parts)
                dtos.Add(_assembler.Create(p));

            var ws = new XmlWriterSettings();
            ws.Indent = true;
            ws.IndentChars = "  ";
            ws.CheckCharacters = true;

            var serializer = new XmlSerializer(typeof(List<PartViewDTO>), GetUniverse());
            using (var writer = XmlWriter.Create(filename, ws))
                serializer.Serialize(writer, dtos);
        }

        public IList<IPartView> Load(string filename)
        {
            var rs = new XmlReaderSettings();
            rs.CheckCharacters = true; // required to read special characters like new line and tab

            var serializer = new XmlSerializer(typeof(List<PartViewDTO>), GetUniverse());
            using (var reader = XmlReader.Create(filename, rs))
            {
                List<IPartView> result = new List<IPartView>();
                var dtos = ((List<PartViewDTO>)serializer.Deserialize(reader));
                foreach (var dto in dtos)
                    result.Add(_assembler.Restore(dto));
                return result;
            }
        }

        private static Type[] _universe;
        private Type[] GetUniverse()
        {
            if (_universe == null)
                _universe = new Type[] { 
                    typeof(BetweenBuilderDTO),
                    typeof(BooleanBuilderDTO),
                    typeof(JunctionBuilderDTO),
                    typeof(LikeBuilderDTO),
                    typeof(NotBuilderDTO),
                    typeof(NullBuilderDTO),
                    typeof(SimpleBuilderDTO)
                };
            return _universe;
        }
    }
}
