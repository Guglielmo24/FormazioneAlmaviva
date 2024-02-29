using Animali.Interfaces;

namespace Animali.Services
{
    public class PersonaFranciaService : IPersonaService<PersonaFranciaService>
    {
        public string AggiungiPrefisso(string numeroTelefonico)
        {
            return "+33" + numeroTelefonico;
        }
    }
}
