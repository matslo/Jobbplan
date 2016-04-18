using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jobbplan.Model;

namespace Jobbplan.BLL
{
    public interface IBrukerLogikk
    {
        bool RegistrerBruker(Registrer innBruker);
        bool GiBrukerAdminTilgang(Sjef innBruker, string brukernavn);
        bool FjernAdminTilgang(Sjef innBruker, string brukernavn);
        List<BrukerListe> HentBrukere(int ProsjektId, string b);
        bool BrukerIdb(LogInn innBruker);
        bool EmailDb(Registrer innBruker);
        int AntallMeldinger(string brukernavn);
        List<Timeliste> HentVakter(string Brukernavn);
        List<Profil> HentBruker(string Brukernavn);
        bool EndreBrukerInfo(Profil EndreBrukerInfo, string brukernavn);

    }
}
