using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jobbplan.Model;
namespace Jobbplan.DAL
{
    public interface InterfaceDbTProsjekt
    {
        bool RegistrerProsjekt(Prosjekt innProsjekt, string brukernavn);
        bool LeggTilBrukerRequest(ProsjektrequestSkjema pReq, string brukernavn);
        bool RegistrerProsjektdeltakelse(ProsjektrequestMelding pid, string brukernavn);
        List<ProsjektrequestMelding> VisRequester(string Brukernavn); 
        List<ProsjektrequestMelding> VisRequesterForProsjekt(int id, string brukernavn);
        List<ProsjektVis> HentProsjekter(string Brukernavn);
        bool SlettBrukerFraProsjekt(string brukernavn, int PId);
        bool ErAdmin(string brukernavn, int PId);
        bool ErEier(string brukernavn, int PId);
        int BrukerId(string brukernavn);
        string FultNavn(int brukerId);
        List<ProsjektVis> SjekkTilgangProsjekt(string brukernavn);
        bool SlettRequest(int id, string brukernavn);   
        bool SlettRequestSomAdmin(string brukernavn, int id);
        bool SlettProsjekt(string Brukernavn, int id); //Mangler å slette vakter, deltakelser, admin 
        string BrukerNavn(int id);
        bool EndreProsjekt(Prosjekt EndreProsjekt, string brukernavn);

    }
}
