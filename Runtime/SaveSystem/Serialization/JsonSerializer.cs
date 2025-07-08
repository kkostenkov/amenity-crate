using Newtonsoft.Json;

namespace Amenity.SaveSystem
{
    public class JsonSerializer<T> : ISerializer<T>
    {
        public string FileExtension => "json";

        public string SerializeToString(T data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            return json;
        }

        public T Deserialize(string input)
        {
            var data = JsonConvert.DeserializeObject<T>(input);
            return data;
        }

        public TData DeepCopy<TData>(TData data)
        {
            var from = JsonConvert.SerializeObject(data, Formatting.Indented);
            var to = JsonConvert.DeserializeObject<TData>(from);
            return to;
        }
    }
}