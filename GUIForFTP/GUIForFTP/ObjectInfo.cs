
namespace GUIForFTP
{
    /// <summary>
    /// Класс инкапсулирует информацию об рассматриваемых объектах.
    /// </summary>
    public class ObjectInfo
    {
        public ObjectInfo(bool isDir, string name, string nameForList, string fullPath)
        {
            IsDir = isDir;
            Name = name;
            NameForList = nameForList;
            FullPath = fullPath;
        }

        public bool IsDir { get; }
        public string Name { get; }
        public string NameForList { get; }
        public string FullPath { get; }
    }
}
