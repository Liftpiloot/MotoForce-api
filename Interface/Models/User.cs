using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? ProfilePic { get; set; }

    public virtual ICollection<Route> Routes { get; set; } = new List<Route>();
}
