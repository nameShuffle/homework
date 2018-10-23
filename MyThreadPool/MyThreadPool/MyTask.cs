using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    class MyTask<TResult>: IMyTask<TResult>
    {
        private Func<TResult> task;
        private bool isCompleted;
        private TResult result;
        private Queue<Action> poolQueue;
        private Queue<Action> continueQueue;

        private Object lockObject = new Object();

        public Action start;
        
        /// <summary>
        /// Конструктор класса задач без аргументов, инициализует значения для дальнейшей работы
        /// с задачей.
        /// </summary>
        /// <param name="func">Функция, на основе которой создается задача для пула потоков.</param>
        /// <param name="poolQueue">
        /// Ссылка на очередь задач из пула для добавления новых.
        /// Требуется для работы функции ContinueWith.
        /// </param>
        public MyTask(Func<TResult> func, ref Queue<Action> poolQueue)
        {
            this.task = func;
            this.isCompleted = false;
            this.start = Start;
            this.poolQueue = poolQueue;
            this.continueQueue = new Queue<Action>();
        }

        /// <summary>
        /// Данная функция запускает вычисление задачи. Вызывается свободным потоком
        /// из пула потоков, когда он забирает задачу себе.
        /// </summary>
        public void Start()
        {
            this.result = this.task();
            this.isCompleted = true;

            while(continueQueue.Count != 0)
            {
                Action continueTask = continueQueue.Dequeue();
                this.poolQueue.Enqueue(continueTask);
            }
        }
        
        /// <summary>
        /// Свойство, позваляющее пользователю проверить готовность задачи.
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                return isCompleted;
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
        public MyTaskWithArgs<TResult, TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func)
        {
            MyTaskWithArgs<TResult, TNewResult> continueTask = new MyTaskWithArgs<TResult, TNewResult>(func, this);
            lock(this.lockObject)
            {
                this.continueQueue.Enqueue(continueTask.start);
            }
            
            return continueTask;
        }
    }
}
