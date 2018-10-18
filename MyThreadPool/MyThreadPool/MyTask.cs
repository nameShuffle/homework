using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyThreadPool
{
    class MyTask<TResult>: IMyTask<TResult>
    {
        private Func<TResult> task;
        private bool isCompleted;
        private TResult result;
        private Queue<Action> poolQueue;
        public Action start;

        public MyTask(Func<TResult> func, ref Queue<Action> poolQueue)
        {
            this.task = func;
            this.isCompleted = false;
            this.start = Start;
            this.poolQueue = poolQueue;
        }

        public void Start()
        {
            this.result = this.task();
            this.isCompleted = true;
        }

        public bool IsCompleted
        {
            get
            {
                return isCompleted;
            }
        }

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
    }
}
