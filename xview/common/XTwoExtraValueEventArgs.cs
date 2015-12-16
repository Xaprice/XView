using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xview.common
{
    public class XTwoExtraValueEventArgs<T1, T2> : EventArgs
    {
        public T1 Data1 { get; set; }
        public T2 Data2 { get; set; }
    }
}
