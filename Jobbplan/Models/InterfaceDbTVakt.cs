using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobbplan.Models
{
    public interface InterfaceDbTVakt
    {
        bool RegistrerVakt(Vaktskjema innVakt, string brukernavn);
        bool LedigVakt(Vaktskjema inn);
        List<Vaktkalender> hentAlleVakter(int id, string b);
        List<Vaktkalender> hentAlleVakterBruker(int id, string brukernavn);
        bool taLedigVakt(int id, string brukernavn);
        bool EndreVakt(Vaktskjema EndreVakt, string brukernavn);
        bool SlettVakt(int vaktId, string brukernavn);
        List<VaktRequestMelding> visVaktRequester(string Brukernavn);
        bool requestOk(int id);
        bool SlettVaktRequest(int Meldingid);


    }
}
