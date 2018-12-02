/*
//mytask нужно сделать прайват влож классом у пула
//переписать крутящее ожидание, использовать примитив синхронизации
//например, ManualResetEvent
using System;
using System.Collections.Generic;
using System.Threading;

namespace MyThreadPool
{
    /// <summary>
    /// Класс, реализующий Task. На основе переданной в пул потоков
    /// функции для удобства работы с ней создается объект данного класса.
    /// Класс предоставляет возможность проверить готовность вычислений,
    /// получить результат или добавить к данной задаче новую, результат которой
    /// зависит от результат начальной функции.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class MyTask<TResult> : IMyTask<TResult>
    {
        private Func<TResult> task;
        private volatile bool isCompleted;
        private TResult result;
        private Queue<Action> poolQueue;
        private Queue<Action> continueQueue;

        private Object lockObject = new Object();

        private bool error;
        private Exception exception;

        private Action start;

        private ManualResetEvent ready;

        /// <summary>
        /// Конструктор класса задач без аргументов, инициализует значения для дальнейшей работы
        /// с задачей.
        /// </summary>
        /// <param name="func">Функция, на основе которой создается задача для пула потоков.</param>
        /// <param name="poolQueue">
        /// Ссылка на очередь задач из пула для добавления новых.
        /// Требуется для работы функции ContinueWith.
        /// </param>
        public MyTask(Func<TResult> func, Queue<Action> poolQueue)
        {
            this.task = func;
            this.isCompleted = false;
            this.start = StartFunction;
            this.poolQueue = poolQueue;
            this.continueQueue = new Queue<Action>();
            this.error = false;
            ready = new ManualResetEvent(false);
        }


        /// <summary>
        /// Свойство, позваляющее пользователю проверить готовность задачи.
        /// </summary>
        public bool IsCompleted => isCompleted;

        /// <summary>
        /// Свойство, возвращает Action с нужной задачей
        /// </summary>
        public Action Start => start;

        /// <summary>
        /// Данная функция запускает вычисление задачи. Вызывается свободным потоком
        /// из пула потоков, когда он забирает задачу себе.
        /// </summary>
        public void StartFunction()
        {
            try
            {
                this.result = this.task();
            }
            catch (Exception e)
            {
                this.error = true;
                this.exception = e;
            }

            this.isCompleted = true;
            ready.Set();

            while (continueQueue.Count != 0)
            {
                lock (this.lockObject)
                {
                    Action continueTask = continueQueue.Dequeue();
                    this.poolQueue.Enqueue(continueTask);
                }
            }
        }

        /// <summary>
        /// Функция, которая позволяет пользователю получить результат
        /// вычисленной задачи. Однако, если результат задачи еще не известен,
        /// данная функция ожидает его и тем самым блокирует поток, откуда она была вызвана.
        /// </summary>
        public TResult Result
        {
            get
            {
                ready.WaitOne();
                if (error)
                {
                    throw new AggregateException(exception);
                }
                else
                {
                    return this.result;
                }
            }
        }

        /// <summary>
        /// Данная функция позволяет добавить новую задачу в пул потоков,
        /// вычисление которой будет зависеть от результат текущей задачи.
        /// </summary>
        /// <typeparam name="TNewResult">Результат новой переданной функции.</typeparam>
        /// <param name="func">Функция, на основе которой будет создана новая задача.</param>
        /// <returns>Экземпляр класса MyTaskWithArgs для дальнейшей работы с ним.</returns>
        public MyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func)
        {
            TNewResult ContinueFunction()
            {
                var arg = this.Result;
                return func(arg);
            }

            var continueTask = new MyTask<TNewResult>(ContinueFunction, this.poolQueue);

            if (this.IsCompleted)
            {
                lock (lockObject)
                {
                    this.poolQueue.Enqueue(continueTask.Start);
                }
            }
            else
            {
                lock (lockObject)
                {
                    this.continueQueue.Enqueue(continueTask.Start);
                }
            }

            return continueTask;
        }
    }
}*/