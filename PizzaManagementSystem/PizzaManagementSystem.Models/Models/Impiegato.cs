using System;
using System.Collections.Generic;

namespace PizzaManagementSystem.Models.Models;

public partial class Impiegato
{
    public int Id { get; set; }

    public int AreaId { get; set; }

    public string Firstname { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public bool Enabled { get; set; }

    public virtual Area Area { get; set; } = null!;
}
