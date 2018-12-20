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
        bool IsCompleted { get; }
        TResult Result { get; }
        IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func);
    }
}