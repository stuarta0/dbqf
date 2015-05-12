using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Display.Parsers;
using Standalone.Serialization.DTO;
using Standalone.Serialization.DTO.Display;

namespace Standalone.Serialization.Assemblers
{
    public class ParserAssembler : IAssembler<Parser, ParserDTO>
    {
        public Parser Restore(ParserDTO dto)
        {
            // assumes anything responsible for creating the DTO will create a valid type name
            var parser = (Parser)Activator.CreateInstance(Type.GetType(dto.TypeName));

            if (parser is DelimitedParser)
                ((DelimitedParser)parser).Delimiters = dto.Delimiters;

            return parser;
        }

        public ParserDTO Create(Parser source)
        {
            throw new NotImplementedException();
        }
    }
}
