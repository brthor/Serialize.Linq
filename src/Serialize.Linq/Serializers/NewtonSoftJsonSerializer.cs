using System.IO;
using System.Text;
using Newtonsoft.Json;
using Serialize.Linq.Interfaces;

namespace Serialize.Linq.Serializers
{
    public class NewtonSoftJsonSerializer: TextSerializer, IJsonSerializer, ITextSerializer, ISerializer
    {
        public override void Serialize<T>(Stream stream, T obj)
        {
            var serializer = new Newtonsoft.Json.JsonSerializer();
            serializer.Converters.Insert(0, (JsonConverter) new JsonPrimitiveConverter());
            serializer.TypeNameHandling = TypeNameHandling.All;
            
            using (var sr = new StreamWriter(stream, Encoding.UTF8, 4096, true))
            using (var jsonTextReader = new JsonTextWriter(sr))
            {
                serializer.Serialize(jsonTextReader, obj, typeof(T));
            }
        }

        public override T Deserialize<T>(Stream stream)
        {
            var serializer = new Newtonsoft.Json.JsonSerializer();
            serializer.Converters.Insert(0, (JsonConverter) new JsonPrimitiveConverter());
            serializer.TypeNameHandling = TypeNameHandling.All;
            
            using (var sr = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                var ret = serializer.Deserialize<T>(jsonTextReader);
                return ret;
            }
        }
    }
}