using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Standalone.Serialization
{
    // http://stackoverflow.com/questions/2678249/in-protobuf-net-how-can-i-pass-an-array-of-type-object-with-objects-of-different
    [ProtoContract]
    public abstract class MessageParam
    {
        public abstract object UntypedValue { get; set; }
        public static MessageParam<T> Create<T>(T value)
        {
            return new MessageParam<T> { Value = value };
        }
        public static MessageParam CreateDynamic(object value)
        {
            Type type = value.GetType();
            switch (Type.GetTypeCode(value.GetType()))
            {
                // special cases
                case TypeCode.Int32: return Create((int)value);
                case TypeCode.Int64: return Create((long)value);
                case TypeCode.Single: return Create((float)value);
                case TypeCode.DateTime: return Create((DateTime)value);
                // fallback in case we forget to add one, or it isn't a TypeCode
                default:
                    MessageParam param = (MessageParam)Activator.CreateInstance(
                        typeof(MessageParam<>).MakeGenericType(type));
                    param.UntypedValue = value;
                    return param;
            }
        }
    }

    [ProtoContract]
    public sealed class MessageParam<T> : MessageParam
    {
        [ProtoMember(1)]
        public T Value { get; set; }
        public override object UntypedValue
        {
            get { return Value; }
            set { Value = (T)value; }
        }
    }
}
