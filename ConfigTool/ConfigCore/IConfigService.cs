namespace ConfigTool.ConfigCore
{
    public interface IConfigService
    {
        T Load<T>(string path) where T : ConfigBase, new();

        void Save<T>(string path, T config) where T : ConfigBase;
    }
}
