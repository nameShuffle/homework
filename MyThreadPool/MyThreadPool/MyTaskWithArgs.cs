using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyThreadPool
{
    class MyTaskWithArgs<TResult, TNewResult> : IMyTask<TNewResult>
    {
        private Func<TResult, TNewResult> task;
        private bool isCompleted;
        private TNewResult result;
        
        public Action start;
        public MyTask<TResult> owner;

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
            this.start = Start;
            this.owner = owner;
        }

        /// <summary>
        /// Данная функция запускает вычисление задачи. Вызывается свободным потоком
        /// из пула потоков, когда он забирает задачу себе.
        /// </summary>
        public void Start()
        {
            this.result = this.task(owner.Result);
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
                        return this.result;
                }
            }
        }
    }
}
