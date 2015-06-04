using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using dbqf.Display;
using dbqf.Serialization.Assemblers.Display;
using dbqf.Serialization.DTO.Builders;
using dbqf.Serialization.DTO.Display;

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
                    /* ParameterBuilders */
                    typeof(BetweenBuilderDTO),
                    typeof(BooleanBuilderDTO),
                    typeof(JunctionBuilderDTO),
                    typeof(LikeBuilderDTO),
                    typeof(NotBuilderDTO),
                    typeof(NullBuilderDTO),
                    typeof(SimpleBuilderDTO),

                    /* Value types */
                    typeof(BetweenValue),
                    typeof(DateValue)
                };
            return _universe;
        }
    }
}
