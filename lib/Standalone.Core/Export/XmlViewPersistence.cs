using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using dbqf.Criterion.Values;
using dbqf.Serialization.DTO.Builders;
using dbqf.Serialization.DTO.Display;
using dbqf.Serialization.DTO.Parsers;
using Standalone.Core.Serialization.Assemblers;
using Standalone.Core.Serialization.DTO;

namespace Standalone.Core.Export
{
    public class XmlViewPersistence : IViewPersistence
    {
        private SearchDocumentAssembler _assembler;
        public XmlViewPersistence(SearchDocumentAssembler assembler)
        {
            _assembler = assembler;
        }

        public void Save(string filename, SearchDocument doc)
        {
            if (doc == null)
                return;

            var ws = new XmlWriterSettings();
            ws.Indent = true;
            ws.IndentChars = "  ";
            ws.CheckCharacters = true;

            var serializer = new XmlSerializer(typeof(SearchDocumentDTO), GetUniverse());
            using (var writer = XmlWriter.Create(filename, ws))
                serializer.Serialize(writer, _assembler.Create(doc));
        }

        public SearchDocument Load(string filename)
        {
            var rs = new XmlReaderSettings();
            rs.CheckCharacters = true; // required to read special characters like new line and tab

            var serializer = new XmlSerializer(typeof(SearchDocumentDTO), GetUniverse());
            using (var reader = XmlReader.Create(filename, rs))
                return _assembler.Restore((SearchDocumentDTO)serializer.Deserialize(reader));
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

                    /* Parsers */
                    typeof(DateParserDTO),
                    typeof(ChainedParserDTO),
                    typeof(DelimitedParserDTO),
                    typeof(ConvertParserDTO),

                    /* Value types */
                    typeof(BetweenValue),
                    typeof(DateValue)
                };
            return _universe;
        }
    }
}
