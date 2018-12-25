?using System;

namespace MyThreadPool
{
    /// <summary>
    /// »нтерфейс задач, прин€тых к исполнению в пуле потоков. 
    /// ѕредоставл€ет возможность узнать результат задачи и ее состо€ние - 
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