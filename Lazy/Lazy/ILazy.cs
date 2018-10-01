
namespace Lazy
{
    /// <summary>
    /// Интерфейс предоставляет ленивое вычисление
    /// </summary>
    /// <typeparam name="T">Тип результата, возвращаемого функцией, 
    /// которая будет использована для ленивого вычисления</typeparam>
    public interface ILazy <T>
    {
        T Get { get; }
    }
}
