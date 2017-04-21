using dbqf.Serialization.Assemblers.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Parsers;
using dbqf.Serialization.DTO.Parsers;
using Standalone.Core.Data;
using Standalone.Core.Serialization.DTO.Parsers;

namespace Standalone.Core.Serialization.Assemblers.Parsers
{
    public class ExtendedParserAssembler : ParserAssembler
    {
        public override ParserDTO Create(Parser source)
        {
            if (source is ExtendedDateParser)
                return new ExtendedDateParserDTO() { AllowNulls = ((DateParser)source).AllowNulls };

            return base.Create(source);
        }

        public override Parser Restore(ParserDTO dto)
        {
            if (dto is ExtendedDateParserDTO)
                return new ExtendedDateParser() { AllowNulls = ((ExtendedDateParserDTO)dto).AllowNulls };

            return base.Restore(dto);
        }
    }
}
