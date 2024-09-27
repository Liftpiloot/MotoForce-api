using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Route
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public double Distance { get; set; }

    public virtual ICollection<DataPoint> DataPoints { get; set; } = new List<DataPoint>();

    public virtual User User { get; set; } = null!;
}
