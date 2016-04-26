using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jobbplan.DAL;
using Jobbplan.Model;

namespace Jobbplan.BLL
{
    public class ProsjektBLL : IProsjektLogikk
    {
        private InterfaceDbTProsjekt _repository;

        public ProsjektBLL()
        {
            _repository = new DbTransaksjonerProsjekt();
        }
        public ProsjektBLL(InterfaceDbTProsjekt moqs)
        {
            _repository = moqs;
        }
        public int BrukerId(string brukernavn)
        {
            return _repository.BrukerId(brukernavn);
        }

        public string BrukerNavn(int id)
        {
            return _repository.BrukerNavn(id);
        }

        public bool EndreProsjekt(Prosjekt EndreProsjekt, string brukernavn)
        {
            return _repository.EndreProsjekt(EndreProsjekt, brukernavn);
        }

        public bool ErAdmin(string brukernavn, int PId)
        {
            return _repository.ErAdmin(brukernavn, PId);
        }

        public bool ErEier(string brukernavn, int PId)
        {
            return _repository.ErEier(brukernavn, PId);
        }

        public string FultNavn(int brukerId)
        {
            return _repository.FultNavn(brukerId);
        }

        public List<ProsjektVis> HentProsjekter(string Brukernavn)
        {
            return _repository.HentProsjekter(Brukernavn); 
        }

        public bool LeggTilBrukerRequest(ProsjektrequestSkjema pReq, string brukernavn)
        {
            return _repository.LeggTilBrukerRequest(pReq, brukernavn);
        }

        public bool RegistrerProsjekt(Prosjekt innProsjekt, string brukernavn)
        {
            return _repository.RegistrerProsjekt(innProsjekt, brukernavn);
        }

        public bool RegistrerProsjektdeltakelse(ProsjektrequestMelding pid, string brukernavn)
        {
            return _repository.RegistrerProsjektdeltakelse(pid, brukernavn);
        }

        public List<ProsjektVis> SjekkTilgangProsjekt(string brukernavn)
        {
            return _repository.SjekkTilgangProsjekt(brukernavn);
        }

        public bool SlettBrukerFraProsjekt(string brukernavn, int PId)
        {
            return _repository.SlettBrukerFraProsjekt(brukernavn, PId);
        }

        public bool SlettProsjekt(string Brukernavn, int id)
        {
            return _repository.SlettProsjekt(Brukernavn, id);
        }

        public bool SlettRequest(int id, string brukernavn)
        {
            return _repository.SlettRequest(id, brukernavn);
        }

        public bool SlettRequestSomAdmin(string brukernavn, int id)
        {
            return _repository.SlettRequestSomAdmin(brukernavn, id);
        }

        public List<ProsjektrequestMelding> VisRequester(string Brukernavn)
        {
            return _repository.VisRequester(Brukernavn);
        }

        public List<ProsjektrequestMelding> VisRequesterForProsjekt(int id, string brukernavn)
        {
            return _repository.VisRequesterForProsjekt(id, brukernavn);
        }
    }
}
