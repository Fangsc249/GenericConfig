using System;

namespace ConfigTool.ConfigCore
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigAttribute : Attribute
    {
        public ConfigAttribute(string Category,string DisplayName)
        {
            this.Category = Category;
            this.DisplayName = DisplayName;
        }

        public string Category { get; set; } = "默认";
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public int Order { get; set; } = 99;
    }

    public enum PathType { File, Directory }

    [AttributeUsage(AttributeTargets.Property)]
    public class PathSelectorAttribute : Attribute
    {
        public PathType Type { get; }
        public string Filter { get; set; } = "All Files|*.*";

        public PathSelectorAttribute(PathType type) => Type = type;
    }

}
