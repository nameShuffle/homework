using System;
using System.Collections.Generic;
using System.Threading;

namespace MyThreadPool
{
    /// <summary>
    /// Класс реализует пул задач с фиксированным количеством 
    /// потоков. Потоки либо выполняют переданную задачу, либо
    /// находятся в режиме ожидания, если свободных задач нет.
    /// </summary>
    public class MyThreadPool
    {
        private int threadsNumber;
        private Thread[] threads;
        private Queue<Action> tasks;

        private Object lockObject = new Object();
        private CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        private CancellationToken token;

        /// <summary>
        /// Конструктор класса создает указанное количество потоков,
        /// потоки сразу же запускаются и переходят в режим ожидания
        /// </summary>
        /// <param name="n">количество потоков в пуле</param>
        public MyThreadPool(int n)
        {
            this.threads = new Thread[n];
            this.threadsNumber = n;
            this.tasks = new Queue<Action>();
            this.token = cancelTokenSource.Token;

            for (int i = 0; i < threadsNumber; i++)
            {
                threads[i] = new Thread(Working)
                {
                    IsBackground = true
                };
            }
            foreach (var thread in threads)
            {
                thread.Start();
            }
        }

        /// <summary>
        /// Работа потоков представлена в виде данной функции
        /// Поток находится в "режиме ожидания" до тех пор, пока 
        /// в очереди появится новая задача
        /// </summary>
        private void Working()
        {
            Action freeTask = null;
            bool newTask = false;
            while (true)
            {
                if (tasks.Count != 0)
                {
                    lock (this.lockObject)
                    {
                        if (tasks.Count != 0)
                        {
                            freeTask = tasks.Dequeue();
                            newTask = true;
                        }
                    }
                }
                if (newTask)
                {
                    freeTask();
                    newTask = false;
                }
                if (this.token.IsCancellationRequested)
                    return;
            }
        }

        /// <summary>
        /// Функция добавления новой задачи в пул потоков. 
        /// Action-функция, которая отвечает за выполнения переданной задачи
        /// добавляется в очередь задач и ожидает свободного потока.
        /// </summary>
        /// <typeparam name="TResult">Результат, возвращаемый func.</typeparam>
        /// <param name="func">
        /// Функция-сама задача, которую пользователь хочет добавить
        /// в пул потоков.
        /// </param>
        /// <returns>
        /// Возвращает ссылку на экземпляр класса MyTask,
        /// в который для удобства была обернута переданная задача.
        /// </returns>
        public MyTask<TResult> AddTask<TResult> (Func<TResult> func)
        {
            var newTask = new MyTask<TResult>(func, tasks);
            lock (this.lockObject)
            {
                this.tasks.Enqueue(newTask.Start);
                return newTask;
            }
        }

        /// <summary>
        /// Функция, возвращающая количество работающих потоков в пуле.
        /// </summary>
        public int ThreadsCount()
        {
            var count = 0;
            foreach (var thread in this.threads)
            {
                if (thread.IsAlive)
                    count++;
            }

            return count;
        }

        /// <summary>
        /// Завершает работу всех потоков в пуле, как только они завершили вычисления.
        /// </summary>
        public void Shutdown()
        {
            cancelTokenSource.Cancel();
        }

        /*
        /// <summary>
        /// Класс, реализующий Task. На основе переданной в пул потоков
        /// функции для удобства работы с ней создается объект данного класса.
        /// Класс предоставляет возможность проверить готовность вычислений,
        /// получить результат или добавить к данной задаче новую, результат которой
        /// зависит от результат начальной функции.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        private class MyTask<TResult> : IMyTask<TResult>
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
                    while (true)
                    {
                        if (isCompleted)
                        {
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
        }*/
    }
}
