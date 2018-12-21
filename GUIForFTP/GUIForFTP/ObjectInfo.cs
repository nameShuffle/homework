
namespace GUIForFTP
{
    public class ObjectInfo
    {
        public ObjectInfo(bool isDir, string name)
        {
            IsDir = isDir;
            Name = name;
        }
        public bool IsDir { get; }
        public string Name { get; }
    }
}
