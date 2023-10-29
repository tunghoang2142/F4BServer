using System;
using System.Collections.Generic;

namespace F4BBuddyAppWebService.Models;

public partial class Buddy
{
    public int Id { get; set; }

    public int? AccountId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string? Gender { get; set; }

    public int? Age { get; set; }

    public string School { get; set; } = null!;

    public int SchoolYear { get; set; }

    public string GuardianEmail { get; set; } = null!;

    public string? IsGuardianConsented { get; set; }

    public string? Hobbies { get; set; }

    public string? Motivation { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
