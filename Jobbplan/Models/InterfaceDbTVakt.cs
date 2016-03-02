using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobbplan.Models
{
    interface InterfaceDbTVakt
    {
        bool RegistrerVakt(Vaktskjema innVakt);
        bool LedigVakt(Vaktskjema inn);
        List<Vaktkalender> hentAlleVakter(int id, string b);
        bool EndreVakt(Vaktskjema EndreVakt);
        
    }
}
