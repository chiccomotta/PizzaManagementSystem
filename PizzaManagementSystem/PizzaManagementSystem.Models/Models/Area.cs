using System;
using System.Collections.Generic;

namespace PizzaManagementSystem.Models.Models;

public partial class Area
{
    public int AreaId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime? InsertDate { get; set; }

    public string? InsertBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public bool Enabled { get; set; }

    public int? Codice { get; set; }

    public virtual ICollection<Impiegato> Impiegatos { get; set; } = new List<Impiegato>();
}
