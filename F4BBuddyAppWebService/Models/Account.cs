using System;
using System.Collections.Generic;

namespace F4BBuddyAppWebService.Models;

public partial class Account
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public int? Money { get; set; }

    public string? LevelCompleted { get; set; }

    public virtual ICollection<Buddy> Buddies { get; set; } = new List<Buddy>();
}
