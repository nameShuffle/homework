﻿using System;

namespace Lazy
{
    public static class LazyFactory
    {
        public static SimpleLazy<T> CreateSimpleLazy<T>(Func<T> func)
        {
            return new SimpleLazy<T>(func);
        }

        public static ProtectedLazy<T> CreateProtectedLazy<T>(Func<T> func)
        {
            return new ProtectedLazy<T>(func);
        }
    }
}
