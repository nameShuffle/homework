?using System;
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
    