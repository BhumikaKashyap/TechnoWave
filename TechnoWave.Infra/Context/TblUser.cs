using System;
using System.Collections.Generic;

namespace TechnoWave.Infra.Context;

public partial class TblUser
{
    public Guid UserId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }
}
