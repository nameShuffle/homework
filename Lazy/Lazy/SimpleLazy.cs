using System;

namespace Lazy
{
    /// <summary>
    /// Класс, реализующий ленивые вычисления без гарантии 
    /// корректной работы в многопоточном режиме.
    /// </summary>
    public class SimpleLazy<T>: ILazy<T>
    {
        private Func<T> func;
        private T result;
        private bool hasDecision;

        /// <summary>
        /// При создании объекта класса SimpleLazy сохраняется передаваемая функция,
        /// на основе которой будут реализованы ленивые вычисления, и указывается,
        /// что изначально решение не найдено.
        /// </summary> 
        /// <param name="func">
        /// Функция, на основе которой будут реализованы ленивые вычисления.
        /// </param>
        public SimpleLazy(Func<T> func)
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
