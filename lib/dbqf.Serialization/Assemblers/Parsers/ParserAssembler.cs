using System;
using dbqf.Parsers;
using dbqf.Serialization.DTO.Parsers;

namespace dbqf.Serialization.Assemblers.Parsers
{
    public class ParserAssembler : IAssembler<Parser, ParserDTO>
    {
        public Parser Restore(ParserDTO dto)
        {
            if (dto is ConvertParserDTO)
            {
                Type from = Type.GetType(((ConvertParserDTO)dto).FromType);
                Type to = Type.GetType(((ConvertParserDTO)dto).ToType);
                Type t = typeof(ConvertParser<,>);
                t = t.MakeGenericType(from, to);
                return Activator.CreateInstance(t) as Parser;
            }
            else if (dto is ChainedParserDTO)
            {
                var parser = new ChainedParser();
                foreach (var child in ((ChainedParserDTO)dto).Parsers)
                    parser.Add(this.Restore(child));
                return parser;
            }
            else if (dto is DelimitedParserDTO)
            {
                return new DelimitedParser(((DelimitedParserDTO)dto).GetDelimiters());
            }

            return null;
        }

        public ParserDTO Create(Parser source)
        {
            if (source is IConvertParser<object, object>)
            {
                Type[] generics = source.GetType().GetGenericArguments();
                var dto = new ConvertParserDTO();
                dto.FromType = generics[0].FullName;
                dto.ToType = generics[1].FullName;
                return dto;
            }
            else if (source is ChainedParser)
            {
                var dto = new ChainedParserDTO();
                foreach (var child in ((ChainedParser)source).Parsers)
                    dto.Parsers.Add(this.Create(child));
                return dto;
            }
            else if (source is DelimitedParser)
            {
                return new DelimitedParserDTO(((DelimitedParser)source).Delimiters);
            }

            return null;
        }
    }
}
