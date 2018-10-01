using System;

namespace Lazy
{
    /// <summary>
    /// Класс, реализующий ленивые вычисления с гарантией
    /// корректной работы в многопоточном режиме
    /// </summary>
    public class ProtectedLazy<T>:ILazy<T>
    {
        private Func<T> func;
        private T result;
        private volatile bool hasDecision;

        private Object lockObject = new Object();

        ///<param name="func">
        ///Функция, на основе которой будут 
        ///реализованы ленивые вычисления
        ///</param>
        public ProtectedLazy(Func<T> func)
        {
            this.func = func;
            this.hasDecision = false;
        }

        /// <summary>
        /// Свойство, возвращающее результат ленивого вычисления
        /// </summary>
        public T Get
        {
            get
            {
                if (!this.hasDecision)
                {
                    lock (this.lockObject)
                    {
                        if (!this.hasDecision)
                        {
                            this.result = this.func();
                            this.func = null;

                            this.hasDecision = true;
                        }
                    }
                }

                return result;
            }
        }
    }
}
