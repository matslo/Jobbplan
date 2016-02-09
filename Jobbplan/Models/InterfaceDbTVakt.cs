using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobbplan.Models
{
    interface InterfaceDbTVakt
    {
        bool RegistrerVakt(Vakt innVakt);
        bool LedigVakt(Vakt inn);

    }
}
