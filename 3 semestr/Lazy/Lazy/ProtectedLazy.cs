using System;

namespace Lazy
{
    /// <summary>
    /// Класс, реализующий ленивые вычисления с гарантией
    /// корректной работы в многопоточном режиме.
    /// </summary>
    public class ProtectedLazy<T>:ILazy<T>
    {
        private Func<T> func;
        private T result;
        private volatile bool hasDecision;

        private Object lockObject = new Object();

        /// <summary>
        /// При создании объекта класса ProtectedLazy сохраняется передаваемая функция,
        /// на основе которой будут реализованы ленивые вычисления, и указывается,
        /// что изначально решение не найдено.
        /// </summary> 
        /// <param name="func">
        /// Функция, на основе которой будут реализованы ленивые вычисления.
        /// </param>
        public ProtectedLazy(Func<T> func)
        {
            this.func = func;
            this.hasDecision = false;
        }

        /// <summary>
        /// Свойство, возвращающее результат ленивого вычисления.
        /// Если данное свойство вызывается впервые, то исполняется переданная
        /// в конструкторе функция. После чего результат запоминается.
        /// При последующих вызовах функция не исполняется, свойство возвращает
        /// вычисленный ранее результат.
        /// При этом предусмотрена возможность работы в многопоточном режиме.
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
