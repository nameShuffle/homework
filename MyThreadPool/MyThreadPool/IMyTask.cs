using System;

namespace MyThreadPool
{
    /// <summary>
    /// Интерфейс задач, принятых к исполнению в пуле потоков. 
    /// Предоставляет возможность узнать результат задачи и ее состояние - 
    /// выполнена она или нет.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IMyTask<TResult>
    {
        /// <summary>
        /// Возвращает состояние задачи - выполнена она или нет.
        /// </summary>
        bool IsCompleted { get; }
        /// <summary>
        /// Возвращает результат задачи.
        /// </summary>
        TResult Result { get; }
        /// <summary>
        /// Позволяет добавить к задаче еще одну, при это ее аргументов
        /// будет результат выполнения основной задачи.
        /// </summary>
        /// <typeparam name="TNewResult">Тип результата новой задачи.</typeparam>
        /// <param name="func">Функция, на основе которой будет сформирована новая задача.</param>
        /// <returns>Возвращает новую задачу.</returns>
        IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func);
    }
}