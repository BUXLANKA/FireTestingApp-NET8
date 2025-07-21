using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireTestingApp_net8.Services
{
    public class SelectTabMessage
    {
        public int TabIndex { get; }

        public SelectTabMessage(int tabIndex)
        {
            TabIndex = tabIndex;
        }
    }
}
