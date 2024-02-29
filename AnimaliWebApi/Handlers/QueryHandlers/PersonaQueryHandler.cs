using MediatR;
using Microsoft.EntityFrameworkCore;
using AnimaliWebApi.Models.DB;
using Microsoft.Data.SqlClient;
using Dapper;

namespace AnimaliWebApi.Handlers.QueryHandlers
{
    public sealed record getPersonaQuery : IRequest<List<Persona>>;
    public sealed record getPersonaByIdQuery(int ID) : IRequest<Persona>;
    public sealed record getPersonaQueryDapper : IRequest<List<Persona>>;
    public sealed record getPersonaByIdQueryDapper(int ID) : IRequest<Persona>;
    public sealed class PersonaQueryHandler: 
        IRequestHandler<getPersonaQuery, List<Persona>>,
        IRequestHandler<getPersonaByIdQuery, Persona>,
        IRequestHandler<getPersonaQueryDapper, List<Persona>>,
        IRequestHandler<getPersonaByIdQueryDapper, Persona>
    {
        private readonly FormazioneDBContext _context;
        private readonly string _connectionString;
        public PersonaQueryHandler(FormazioneDBContext context)
        {
            _context = context;
            _connectionString = context.Database.GetConnectionString();
        }
        public async Task<List<Persona>> Handle(getPersonaQuery request, CancellationToken cancellationToken)
        {
            return await _context.Persona.ToListAsync();
        }

        public async Task<Persona> Handle(getPersonaByIdQuery request, CancellationToken cancellationToken)
        {
            var persona = await _context.Persona.FindAsync(request.ID);

            return persona;
        }

        public async Task<List<Persona>> Handle(getPersonaQueryDapper request, CancellationToken cancellationToken)
        {
            var query = "SELECT * FROM Persona";
            using var connection = new SqlConnection(_connectionString);
            var result = (await connection.QueryAsync<Persona>(query, new {param =(int?)1})).ToList();
            return result;
        }

        public async Task<Persona> Handle(getPersonaByIdQueryDapper request, CancellationToken cancellationToken)
        {
            var query = "SELECT * FROM Persona where persona.ID = @param";
            using var connection = new SqlConnection(_connectionString);
            var result = (await connection.QueryAsync<Persona>(query, new { param = request.ID })).SingleOrDefault();
            return result;
        }
    }
}
