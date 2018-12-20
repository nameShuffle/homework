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

        private AutoResetEvent readyTask;

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

            this.readyTask = new AutoResetEvent(false);

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
            while (true)
            {
                this.readyTask.WaitOne();

                bool newTask = false;

                lock (this.lockObject)
                {
                    if (tasks.Count != 0)
                    {
                        freeTask = tasks.Dequeue();
                        newTask = true;
                    }
                }

                if (newTask)
                {
                    freeTask();
                }

                if (this.token.IsCancellationRequested)
                {
                    this.readyTask.Set();
                    return;
                }
                    
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
        public IMyTask<TResult> AddTask<TResult> (Func<TResult> func)
        {
            var newTask = new MyTask<TResult>(func, this);
            lock (this.lockObject)
            {
                this.tasks.Enqueue(newTask.Start);
                this.readyTask.Set();
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
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Завершает работу всех потоков в пуле, как только они завершили вычисления.
        /// </summary>
        public void Shutdown()
        {
            this.cancelTokenSource.Cancel();
            this.readyTask.Set();
            while (true)
            {
                if (ThreadsCount() == 0)
                {
                    return;
                }
            }
        }

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
            private MyThreadPool pool;
            private Queue<Action> continueQueue;

            private Object lockObject = new Object();

            private bool error;
            private Exception exception;

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
            public MyTask(Func<TResult> func, MyThreadPool pool)
            {
                this.task = func;
                this.isCompleted = false;
                this.pool = pool;
                this.continueQueue = new Queue<Action>();
                this.error = false;
                this.ready = new ManualResetEvent(false);
            }


            /// <summary>
            /// Свойство, позваляющее пользователю проверить готовность задачи.
            /// </summary>
            public bool IsCompleted => isCompleted;

            /// <summary>
            /// Свойство, возвращает Action с нужной задачей
            /// </summary>
            public Action Start =>
                () =>
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
                            this.pool.tasks.Enqueue(continueTask);
                            this.pool.readyTask.Set();
                        }
                    }
                };

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
            public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func)
            {
                TNewResult ContinueFunction()
                {
                    var arg = this.Result;
                    return func(arg);
                }

                
                if (this.IsCompleted)
                {
                    var continueTask = this.pool.AddTask(ContinueFunction);
                    return continueTask;
                }
                else
                {
                    var continueTask = new MyTask<TNewResult>(ContinueFunction, this.pool);
                    lock (this.lockObject)
                    {
                        this.continueQueue.Enqueue(continueTask.Start);
                    }
                    return continueTask;
                }
            }
        }
    }
}
