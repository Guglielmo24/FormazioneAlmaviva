using Animali.Interfaces;
namespace Animali.Services
{
    public class PersonaItaliaService: IPersonaService<PersonaItaliaService>
    {
        public string AggiungiPrefisso(string numeroTelefonico)
        {
            return "+39" + numeroTelefonico;
        }
    }
}
