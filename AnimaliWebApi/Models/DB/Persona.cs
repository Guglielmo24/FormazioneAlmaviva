using System;
using System.Collections.Generic;

namespace AnimaliWebApi.Models.DB;

public partial class Persona
{
    public int ID { get; set; }

    public string Nome { get; set; } = null!;

    public string Cognome { get; set; } = null!;

    public string? NumeroTelefonico { get; set; }

    public int? ID_ComuneDiNascita { get; set; }

    public virtual Comune? ID_ComuneDiNascitaNavigation { get; set; }
}
