using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xview
{
    public class SingleDataEventArgs<T> : EventArgs
    {
        public T Data { get; set; }
    }
}
