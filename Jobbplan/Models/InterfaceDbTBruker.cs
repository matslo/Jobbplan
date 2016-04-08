using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobbplan.Models
{
    public interface InterfaceDbTBruker
    {
        bool RegistrerBruker(Registrer innBruker);
        bool GiBrukerAdminTilgang(Sjef innBruker);
        bool FjernAdminTilgang(Sjef innBruker);
        List<BrukerListe> HentBrukere(int ProsjektId, string b);
        bool BrukerIdb(LogInn innBruker);
        bool EmailDb(Registrer innBruker);
        int AntallMeldinger(string brukernavn);
        List<Timeliste> HentVakter(string Brukernavn);
        List<Profil> HentBruker(string Brukernavn);
        bool EndreBrukerInfo(Profil EndreBrukerInfo, string brukernavn);

    }
}
