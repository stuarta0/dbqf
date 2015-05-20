using dbqf.Criterion;
using ProtoBuf.Meta;
using Standalone.Core.Serialization.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Standalone.Core.Serialization.DTO.Criterion;
using dbqf.Display;

namespace Standalone.Core.Initialisers
{
    public class ProtoBufInitialiser : IInitialiser
    {
        public void Initialise()
        {
            RuntimeTypeModel.Default.Add(typeof(FieldPath), false).SetSurrogate(typeof(FieldPathDTO));
            RuntimeTypeModel.Default.Add(typeof(IParameter), false)
                .AddSubType(2, typeof(SimpleParameter))
                .AddSubType(3, typeof(LikeParameter))
                .AddSubType(4, typeof(NotParameter))
                .AddSubType(5, typeof(NullParameter))
                .AddSubType(6, typeof(Conjunction))
                .AddSubType(7, typeof(Disjunction));
            RuntimeTypeModel.Default.Add(typeof(SimpleParameter), false).SetSurrogate(typeof(SimpleParameterDTO));
            RuntimeTypeModel.Default.Add(typeof(LikeParameter), true);
            RuntimeTypeModel.Default.Add(typeof(NullParameter), true);
            RuntimeTypeModel.Default.Add(typeof(Conjunction), false).SetSurrogate(typeof(ConjunctionDTO));
            RuntimeTypeModel.Default.Add(typeof(Disjunction), true);
            
            RuntimeTypeModel.Default.Add(typeof(NotParameter), false)
                .Add(1, "_other")
                .UseConstructor = false;
            



            // For LikeParameter: must explictly use fully qualified name since inherited classes are private
            RuntimeTypeModel.Default.Add(typeof(MatchMode), true)
                .AddSubType(2, Type.GetType("dbqf.Criterion.MatchMode+ExactMatchMode, dbqf.core, Version=1.9.7.350, Culture=neutral, PublicKeyToken=null"))
                .AddSubType(3, Type.GetType("dbqf.Criterion.MatchMode+StartMatchMode, dbqf.core, Version=1.9.7.350, Culture=neutral, PublicKeyToken=null"))
                .AddSubType(4, Type.GetType("dbqf.Criterion.MatchMode+EndMatchMode, dbqf.core, Version=1.9.7.350, Culture=neutral, PublicKeyToken=null"))
                .AddSubType(5, Type.GetType("dbqf.Criterion.MatchMode+AnywhereMatchMode, dbqf.core, Version=1.9.7.350, Culture=neutral, PublicKeyToken=null"));

            // initialise all types allowed via MessageParam for IParameter values
            // Note: order is important.  If order changes, any existing serialised value will become invalid.
            // TODO: load all types from app.config
            var primitives = new string[] { "Byte", "Int16", "UInt16", "Int32", "UInt32", "Int64", "UInt64", "Single", "Double", "Boolean", "DateTime", "Char", "String" };
            for (int i = 0; i < primitives.Length; i++) primitives[i] = String.Concat("System.", primitives[i]);
            var customTypes = new string[] { 
                "dbqf.Display.DateValue, dbqf.core, Version=1.9.7.350, Culture=neutral, PublicKeyToken=null",
                "dbqf.Display.BetweenValue, dbqf.core, Version=1.9.7.350, Culture=neutral, PublicKeyToken=null"
            };

            // register custom types for serialisation using default behaviour
            RuntimeTypeModel.Default.Add(typeof(BetweenValue), false).SetSurrogate(typeof(BetweenValueDTO));
            foreach (var t in customTypes)
            {
                var type = Type.GetType(t);
                if (RuntimeTypeModel.Default.GetTypes().Cast<MetaType>().AsQueryable().Where(o => o.Type == type).Count() == 0)
                    RuntimeTypeModel.Default.Add(type, true);
            }

            // register subtypes for messages using primitives and our supplied custom types
            int messageCounter = 2;
            foreach (var t in primitives.Concat(customTypes))
                RuntimeTypeModel.Default[typeof(Standalone.Core.Serialization.MessageParam)].AddSubType(messageCounter++, 
                    typeof(Standalone.Core.Serialization.MessageParam<>).MakeGenericType(Type.GetType(t)));
        }
    }
}
