using System;

namespace Lazy
{
    /// <summary>
    /// Класс, предоставляющий возможность создавать объекты
    /// двух видов реализации ленивых вычислений:
    /// SimpleLazy - без гарантии корректной работы в многопоточном режиме
    /// ProtectedLazy - с гарантией корректной работы в многопоточном режиме
    /// </summary>
    public static class LazyFactory
    {
        public static SimpleLazy<T> CreateSimpleLazy<T>(Func<T> func)
            => new SimpleLazy<T>(func);

        public static ProtectedLazy<T> CreateProtectedLazy<T>(Func<T> func)
            => new ProtectedLazy<T>(func);
    }
}
