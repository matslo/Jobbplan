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
        List<Vaktkalender> hentAlleVakterBruker(int id, string brukernavn);
        bool taLedigVakt(int id, string brukernavn);
        bool EndreVakt(Vaktskjema EndreVakt);
        List<VaktRequestMelding> visVaktRequester(string Brukernavn);
        bool requestOk(int id);

    }
}
