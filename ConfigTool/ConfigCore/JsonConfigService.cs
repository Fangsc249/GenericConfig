using Newtonsoft.Json;
using System.IO;

namespace ConfigTool.ConfigCore
{
    public class JsonConfigService : IConfigService
    {
        public T Load<T>(string path) where T : ConfigBase, new()
        {
            if (!File.Exists(path))
            {
                T t = new T();
                this.Save(path, t);
                return t;
            }

            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public void Save<T>(string path, T config) where T : ConfigBase
        {
            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(path, json);
        }
    }
}
