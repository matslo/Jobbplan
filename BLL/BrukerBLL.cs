using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jobbplan.Model;
using Jobbplan.DAL;

namespace Jobbplan.BLL
{
    public class BrukerBLL : IBrukerLogikk
    {
        private InterfaceDbTBruker _repository;
        public BrukerBLL()
        {
            _repository = new DbTransaksjonerBruker();
        }
        public BrukerBLL(InterfaceDbTBruker moqs)
        {
            _repository = moqs;
        }
        public int AntallMeldinger(string brukernavn)
        {
            return _repository.AntallMeldinger(brukernavn);
        }
        public bool BrukerIdb(LogInn innBruker)
        {
            return _repository.BrukerIdb(innBruker);
        }
        public bool EmailDb(Registrer innBruker)
        {
            return _repository.EmailDb(innBruker);
        }
        public bool EndreBrukerInfo(Profil EndreBrukerInfo, string brukernavn)
        {
            return _repository.EndreBrukerInfo(EndreBrukerInfo, brukernavn);
        }
        public bool FjernAdminTilgang(Sjef innBruker, string brukernavn)
        {
            return _repository.FjernAdminTilgang(innBruker, brukernavn);
        }
        public bool GiBrukerAdminTilgang(Sjef innBruker, string brukernavn)
        {
            return _repository.GiBrukerAdminTilgang(innBruker, brukernavn);
        }
        public List<Profil> HentBruker(string Brukernavn)
        {
            return _repository.HentBruker(Brukernavn);
        }

        public List<BrukerListe> HentBrukere(int ProsjektId, string b)
        {
            return _repository.HentBrukere(ProsjektId, b);
        }

        public List<Timeliste> HentVakter(string Brukernavn, int Pid)
        {
            return _repository.HentVakter(Brukernavn, Pid);
        }

        public bool RegistrerBruker(Registrer innBruker)
        {
            return _repository.RegistrerBruker(innBruker);
        }
    }
}