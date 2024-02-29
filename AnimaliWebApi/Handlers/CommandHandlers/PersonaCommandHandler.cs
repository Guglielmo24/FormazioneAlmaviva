using MediatR;
using Microsoft.EntityFrameworkCore;
using AnimaliWebApi.Models.DB;
using AnimaliWebApi.Handlers.QueryHandlers;
using Azure.Core;
using Microsoft.Data.SqlClient;
using Dapper;

namespace AnimaliWebApi.Handlers.CommandHandlers
{
    public sealed record putPersonaCommand(int id, Persona persona) : IRequest<bool>;
    public sealed record deletePersonaCommand(int ID) : IRequest;
    public sealed record postPersonaCommand(Persona persona) : IRequest;
    public sealed record putPersonaCommandDapper(int id, Persona persona) : IRequest<bool>;
    public sealed record deletePersonaCommandDapper(int ID) : IRequest;
    public sealed record postPersonaCommandDapper(Persona persona) : IRequest;
    public class PersonaCommandHandler :
        IRequestHandler<putPersonaCommand, bool>,
        IRequestHandler<deletePersonaCommand>,
        IRequestHandler<postPersonaCommand>,
        IRequestHandler<putPersonaCommandDapper, bool>,
        IRequestHandler<deletePersonaCommandDapper>,
        IRequestHandler<postPersonaCommandDapper>
    {
        private readonly FormazioneDBContext _context;
        private readonly string _connectionString;

        public PersonaCommandHandler(FormazioneDBContext context)
        {
            _context = context;
            _connectionString = context.Database.GetConnectionString();
        }

        public async Task<bool> Handle(putPersonaCommand request, CancellationToken cancellationToken)
        {
            if (request.id != request.persona.ID)
            {
                return false;
            }

            _context.Entry(request.persona).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonaExists(request.id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

        }

        public async Task Handle(deletePersonaCommand request, CancellationToken cancellationToken)
        {
            var persona = await _context.Persona.FindAsync(request.ID);

            _context.Persona.Remove(persona);
            await _context.SaveChangesAsync();

        }

        public async Task Handle(postPersonaCommand request, CancellationToken cancellationToken)
        {
            _context.Persona.Add(request.persona);
            await _context.SaveChangesAsync();
        }


        public async Task<bool> Handle(putPersonaCommandDapper request, CancellationToken cancellationToken)
        {
            var query = "UPDATE Persona SET Nome = @Nome, Cognome = @Cognome ,NumeroTelefonico=@NumeroTelefonico WHERE Persona.ID= @ID";
            using var connection = new SqlConnection(_connectionString);

            if (request.id != request.persona.ID)
            {
                return false;
            }

            _context.Entry(request.persona).State = EntityState.Modified;

            try
            {
                await connection.ExecuteAsync(query, new { Nome = request.persona.Nome, Cognome = request.persona.Cognome, NumeroTelefonico = request.persona.NumeroTelefonico, ID= request.id });
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonaExists(request.id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

        }

        public async Task Handle(deletePersonaCommandDapper request, CancellationToken cancellationToken)
        {
            var query = "DELETE from Persona WHERE Persona.ID=@ID";
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(query, new { ID = request.ID});
            await _context.SaveChangesAsync();
        }

        public async Task Handle(postPersonaCommandDapper request, CancellationToken cancellationToken)
        {
            var query = "INSERT INTO Persona(Nome,Cognome,NumeroTelefonico,ID_ComuneDiNascita) VALUES (@Nome,@Cognome,@NumeroTelefonico,@ID_ComuneDiNascita)";
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(query, new { Nome = request.persona.Nome, Cognome = request.persona.Cognome, NumeroTelefonico = request.persona.NumeroTelefonico, ID_ComuneDiNascita=1 });
            await _context.SaveChangesAsync();
        }

        private bool PersonaExists(int id)
        {
            return _context.Persona.Any(e => e.ID == id);
        }
    }
}