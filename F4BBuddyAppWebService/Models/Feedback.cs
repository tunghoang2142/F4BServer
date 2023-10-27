using System;
using System.Collections.Generic;

namespace F4BBuddyAppWebService.Models;

public partial class Feedback
{
    public int Id { get; set; }

    public int? AllStarId { get; set; }

    public int? BuddyId { get; set; }

    public string Feedback1 { get; set; } = null!;

    public virtual AllStar? AllStar { get; set; }

    public virtual Buddy? Buddy { get; set; }
}
