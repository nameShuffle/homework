using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyThreadPool
{
    interface IMyTask <TResult>
    {
        bool IsCompleted { get; }
        TResult Result { get; }
    }
}
