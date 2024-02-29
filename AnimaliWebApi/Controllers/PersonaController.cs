using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnimaliWebApi.Models.DB;
using MediatR;
using AnimaliWebApi.Handlers.QueryHandlers;
using AnimaliWebApi.Handlers.CommandHandlers;
namespace AnimaliWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private readonly FormazioneDBContext _context;
        private readonly IMediator _mediator;
        public PersonaController(FormazioneDBContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        // GET: api/Persona
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Persona>>> GetPersona()
        {
            var result= await _mediator.Send(new getPersonaQueryDapper());
            return result;
        }

        // GET: api/Persona/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Persona>> GetPersona(int id)
        {
            var result = await _mediator.Send(new getPersonaByIdQueryDapper(id));
            return result;
        }

        // PUT: api/Persona/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersona(int id, Persona persona)
        {
            await _mediator.Send(new putPersonaCommandDapper(id, persona));

            return NoContent();
        }

        // POST: api/Persona
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Persona>> PostPersona(Persona persona)
        {

            await _mediator.Send(new postPersonaCommandDapper(persona));
            return CreatedAtAction("GetPersona", new { id = persona.ID }, persona);
        }

        // DELETE: api/Persona/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersona(int id)
        {
            await _mediator.Send(new deletePersonaCommandDapper(id));
            return NoContent();
        }

        private bool PersonaExists(int id)
        {
            return _context.Persona.Any(e => e.ID == id);
        }
    }
}
