using System;

namespace Lazy
{
    /// <summary>
    /// Класс, реализующий ленивые вычисления без гарантии 
    /// корректной работы в многопоточном режиме
    /// </summary>
    public class SimpleLazy<T>: ILazy<T>
    {
        private Func<T> func;
        private T result;
        private bool hasDecision;

        ///<param name="func">
        ///Функция, на основе которой будут 
        ///реализованы ленивые вычисления
        ///</param>
        public SimpleLazy(Func<T> func)
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
                    this.result = this.func();
                    this.func = null;
                    this.hasDecision = true;
                }
                return this.result;
            }
        }
    }
}
