
using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ConfigTool.ConfigCore
{
    public class YamlConfigService : IConfigService // 2025-6-3
    {
        public T Load<T>(string path) where T : ConfigBase, new()
        {
            // 如果文件不存在，返回默认实例
            if (!File.Exists(path))
                return new T();

            // 读取YAML文件内容
            string yaml = File.ReadAllText(path);
            //YamlDotNet.Serialization.NamingConventions.
            // 创建YAML反序列化器
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(PascalCaseNamingConvention.Instance) // 
                .IgnoreUnmatchedProperties() // 忽略未匹配的属性
                .Build();

            // 反序列化为对象
            return deserializer.Deserialize<T>(yaml);
        }

        public void Save<T>(string path, T config) where T : ConfigBase
        {
            // 创建YAML序列化器
            var serializer = new SerializerBuilder()
                .WithNamingConvention(PascalCaseNamingConvention.Instance) // 
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull) // 忽略null值
                .Build();
            // 序列化为YAML字符串
            string yaml = serializer.Serialize(config);
            //Console.WriteLine($"保存配置到文件之前: {yaml}"); // 输出保存路径
            // 写入文件
            File.WriteAllText(path, yaml);
        }
    }
}
