using Animali.Interfaces;
using Animali.Models;
using Animali.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
//using System.Text.Json;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using System.Text;

namespace Animali.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly IPersonaService<PersonaItaliaService> _personaServiceIt;
        private readonly IPersonaService<PersonaFranciaService> _personaServiceFr;
        public List<PersonaViewModel> persone = new List<PersonaViewModel>();
        public HomeController(ILogger<HomeController> logger, HttpClient httpClient, IPersonaService<PersonaItaliaService> personaServiceIt, IPersonaService<PersonaFranciaService> personaServiceFr)
        {
            _logger = logger;
            _httpClient = httpClient;
            _personaServiceIt = personaServiceIt;
            _personaServiceFr = personaServiceFr;
        }

        public /**/ IActionResult Index()
        {
            /*
            var telefonoIt = _personaServiceIt.AggiungiPrefisso("1234567890");
            _logger.LogInformation("Italia " + telefonoIt);
            ViewBag.TelefonoIt = telefonoIt;

            var telefonoFr = _personaServiceFr.AggiungiPrefisso("1234567890");
            _logger.LogInformation("Francia " + telefonoFr);
            ViewBag.TelefonoFr = telefonoFr;
            */


            /*
            for (int i = 0; i < 2; i++)
            {
                PersonaViewModel persona = new PersonaViewModel();
                persona.Nome = "Mario" + i;
                persona.Cognome = "Rossi" + i;
                persona.NumeroTelefonico = "1234567890(" + i+")";
                persone.Add(persona);
            }

            ViewBag.Count = persone.Count;
            */



            return View();
        }

        //partial view _FormPersone
        public IActionResult _FormPersone()
        {

            return PartialView();
        }

        //partial view _ListaPersone
        public async Task<IActionResult> _ListaPersone()
        {
            var response = await _httpClient.GetAsync("https://localhost:7062/api/Persona");
            
            if (response.IsSuccessStatusCode)
            {
                /*
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                persone = JsonSerializer.Deserialize<List<PersonaViewModel>>(responseContent, options);
                */
                var responseContent = await response.Content.ReadAsStringAsync();
                persone = JsonConvert.DeserializeObject<List<PersonaViewModel>>(responseContent);
            }

            ViewBag.Count = persone.Count;


            
            
            return PartialView(persone);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        [HttpPost]

        public async Task<IActionResult> SubmitForm(PersonaViewModel persona)
        {
            persona.ID_ComuneDiNascita = 1;
            var persona_json = JsonConvert.SerializeObject(persona);
            var data = new StringContent(persona_json.ToString(), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7062/api/Persona", data);
            var responseString = response.Content.ReadAsStringAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public void DeletePersona(int id)
        {
            var persona_json = JsonConvert.SerializeObject(id);
            var response = _httpClient.DeleteAsync("https://localhost:7062/api/Persona/"+id).Result;
        }

        /*partial view _PopupPersone*/
        [HttpPost]
        public async Task<IActionResult> _PopupPersona(int id)
        {
            var response = await _httpClient.GetAsync("https://localhost:7062/api/Persona/"+id);
            var persona = new PersonaViewModel();
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                persona = JsonConvert.DeserializeObject<PersonaViewModel>(responseContent);

            }
            return PartialView(persona);
        }
        /*Update Persona*/
        [HttpPost]

        public async Task<IActionResult> UpdateForm(PersonaViewModel persona)
        {
            var persona_json = JsonConvert.SerializeObject(persona);
            var data = new StringContent(persona_json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("https://localhost:7062/api/Persona/"+ persona.ID, data);

            return RedirectToAction("Index");
        }


    }
}
