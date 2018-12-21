
namespace GUIForFTP
{
    /// <summary>
    /// Класс инкапсулирует информацию об рассматриваемых объектах.
    /// </summary>
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
