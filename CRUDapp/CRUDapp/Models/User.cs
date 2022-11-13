using System;
using System.Collections.Generic;

namespace CRUDapp.Models;

public partial class User
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public long? Age { get; set; }

    public byte[]? Image { get; set; }
}
