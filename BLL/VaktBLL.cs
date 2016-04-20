using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jobbplan.BLL;
using Jobbplan.DAL;
using Jobbplan.Model;

namespace Jobbplan.BLL
{
    public class VaktBLL : IVaktLogikk
    {
        private InterfaceDbTVakt _repository;

        public VaktBLL()
        {
            _repository = new DbTransaksjonerVakt();
        }
        public VaktBLL(InterfaceDbTVakt moqs)
        {
            _repository = moqs;
        }
        public bool EndreVakt(Vaktskjema EndreVakt, string brukernavn)
        {
            return _repository.EndreVakt(EndreVakt, brukernavn);
        }

        public List<VisMaler> hentAlleMaler(int id, string brukernavn)
        {
            return _repository.hentAlleMaler(id, brukernavn);
        }

        public List<Vaktkalender> hentAlleVakter(int id, string b)
        {
            return _repository.hentAlleVakter(id, b);
        }

        public List<Vaktkalender> hentAlleVakterBruker(int id, string brukernavn)
        {
            return _repository.hentAlleVakterBruker(id, brukernavn);
        }

        public List<Vaktkalender> hentAlleVakterForBruker(string brukernavn)
        {
            return _repository.hentAlleVakterForBruker(brukernavn);
        }

        public List<Vaktkalender> hentAlleLedigeVakter(int id, string brukernavn)
        {
            return _repository.hentAlleLedigeVakter(id, brukernavn);
        }
        public bool LedigVakt(Vaktskjema inn)
        {
            return _repository.LedigVakt(inn);
        }

        public bool RegistrerMal(MalerSkjema mal, string brukernavn)
        {
            return _repository.RegistrerMal(mal, brukernavn);
        }

        public bool RegistrerVakt(Vaktskjema innVakt, string brukernavn)
        {
            return _repository.RegistrerVakt(innVakt, brukernavn);
        }

        public bool requestOk(int id)
        {
            return _repository.requestOk(id);
        }

        public bool SlettVakt(int vaktId, string brukernavn)
        {
            return _repository.SlettVakt(vaktId, brukernavn);
        }

        public bool SlettVaktRequest(int Meldingid)
        {
            return _repository.SlettVaktRequest(Meldingid);
        }

        public bool taLedigVakt(int id, string brukernavn)
        {
            return _repository.taLedigVakt(id, brukernavn);
        }

        public List<VaktRequestMelding> visVaktRequester(string Brukernavn)
        {
            return _repository.visVaktRequester(Brukernavn);
        }
    }
}
