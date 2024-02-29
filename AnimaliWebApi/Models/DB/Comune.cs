using System;
using System.Collections.Generic;

namespace AnimaliWebApi.Models.DB;

public partial class Comune
{
    public int ID { get; set; }

    public string? Nome { get; set; }

    public virtual ICollection<Persona> Persona { get; set; } = new List<Persona>();
}
