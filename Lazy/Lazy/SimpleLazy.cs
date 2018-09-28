using System;
using System.Linq;

namespace Lazy
{
    public class SimpleLazy<T>: ILazy<T>
    {
        private Func<T> func;
        private T result;
        private bool hasDecision;

        public SimpleLazy(Func<T> func)
        {
            this.func = func;
            this.hasDecision = false;
        }

        public T Get
        {
            get
            {
                if (!this.hasDecision)
                {
                    this.result = this.func();
                    this.hasDecision = true;
                }
                return this.result;
            }
        }
    }
}
