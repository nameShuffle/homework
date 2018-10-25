namespace MyThreadPool
{
    /// <summary>
    /// Интерфейс задач, принятых к исполнению в пуле потоков. 
    /// Предоставляет возможность узнать результат задачи и ее состояние - 
    /// выполнена она или нет.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    interface IMyTask <TResult>
    {
        bool IsCompleted { get; }
        TResult Result { get; }
    }
}
