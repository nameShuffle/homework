using System;

namespace MyThreadPool
{
    public class MyTaskWithArgs<TResult, TNewResult> : IMyTask<TNewResult>
    {
        private Func<TResult, TNewResult> task;
        private bool isCompleted;
        private TNewResult result;

        private bool error;
        private Exception exception;

        private TResult ownerResult;

        public Action start;

        /// <summary>
        /// Конструктор класса задач с аргументами, инициализует значения для дальнейшей работы
        /// с задачей.
        /// </summary>
        /// <param name="func">Функция, на основе которой создается задача для пула потоков.</param>
        /// <param name="owner">
        /// Задача, от результат которой зависит результат данной задачи.
        /// </param>
        public MyTaskWithArgs(Func<TResult, TNewResult> func, MyTask<TResult> owner)
        {
            this.task = func;
            this.isCompleted = false;
            this.ownerResult = owner.Result;
            this.start = Start;
            this.error = false;
        }

        /// <summary>
        /// Данная функция запускает вычисление задачи. Вызывается свободным потоком
        /// из пула потоков, когда он забирает задачу себе.
        /// </summary>
        public void Start()
        {
            try
            {
                this.result = this.task(ownerResult);
            }
            catch (Exception e)
            {
                this.error = true;
                this.exception = e;
            }

            this.isCompleted = true;
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
        public TNewResult Result
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
                            return this.result;
                    }
                }
            }
        }
    }
}
