using System;
using System.Collections.Generic;

namespace F4BBuddyAppWebService.Models;

public partial class AllStar
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string? Gender { get; set; }

    public int? Age { get; set; }

    public string? School { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
