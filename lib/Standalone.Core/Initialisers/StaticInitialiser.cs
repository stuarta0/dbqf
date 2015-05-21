using dbqf.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Core.Initialisers
{
    public class StaticInitialiser : IInitialiser
    {
        private IConfiguration _configuration;
        public StaticInitialiser(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Initialise()
        {
            // required for ProtoBuf serialisation
            Standalone.Core.Serialization.DTO.FieldPathDTO.Configuration = _configuration;
        }
    }
}
