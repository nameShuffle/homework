using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MyThreadPool
{
    /// <summary>
    /// Класс реализует пул задач с фиксированным количеством 
    /// потоков. Потоки либо выполняют переданную задачу, либо
    /// находятся в режиме ожидания, если свободных задач нет.
    /// </summary>
    class MyThreadPool
    {
        private int threadsNumber;
        private Thread[] threads;
        private Queue<Action> tasks;

        private Object lockObject = new Object();

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
            while (true)
            {
                lock(this.lockObject)
                {
                    if (tasks.Count != 0)
                    {
                        var freeTask = tasks.Dequeue();
                        freeTask();
                    }
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
        public MyTask<TResult> AddTask<TResult> (Func<TResult> func)
        {
            var newTask = new MyTask<TResult>(func, ref tasks);
            lock(this.lockObject)
            {
                this.tasks.Enqueue(newTask.start);
                return newTask;
            }
        }

    }
}
