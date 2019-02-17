using System;

namespace Lazy
{
    /// <summary>
    /// Класс, предоставляющий возможность создавать объекты
    /// двух видов реализации ленивых вычислений:
    /// SimpleLazy - без гарантии корректной работы в многопоточном режиме;
    /// ProtectedLazy - с гарантией корректной работы в многопоточном режиме.
    /// </summary>
    public static class LazyFactory
    {
        /// <summary>
        /// Метод создает объект класса SimpleLazy, реализующего ленивые вычисления
        /// без гарантии корректной работы в многопоточном режиме.
        /// </summary>
        /// <typeparam name="T">Тип результата, возвращаемого ленивым вычислением.</typeparam>
        /// <param name="func">
        /// Функция, на основе которой будут реализованы ленивые вычислния.
        /// </param>
        public static SimpleLazy<T> CreateSimpleLazy<T>(Func<T> func)
            => new SimpleLazy<T>(func);

        /// <summary>
        /// Метод создает объект класса ProtectedLazy, предоставляющего ленивые вычисления
        /// с гарантией корректной работы в многопоточном режиме.
        /// </summary>
        /// <typeparam name="T">Тип результат, возвращаемого ленивым вычислением.</typeparam>
        /// <param name="func">
        /// Функция, на основе которой будут реализованы ленивые вычисления.</param>
        public static ProtectedLazy<T> CreateProtectedLazy<T>(Func<T> func)
            => new ProtectedLazy<T>(func);
    }
}
