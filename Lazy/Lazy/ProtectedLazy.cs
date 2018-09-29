using System;

namespace Lazy
{
    public class ProtectedLazy<T>:ILazy<T>
    {
        private Func<T> func;
        private T result;
        private bool hasDecision;

        private Object lockObject = new Object();

        public ProtectedLazy(Func<T> func)
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
                    lock (this.lockObject)
                    {
                        if (!this.hasDecision)
                        {
                            this.result = this.func();
                            this.hasDecision = true;
                        }
                    }
                }

                return result;
            }
        }
    }
}
