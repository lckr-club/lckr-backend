using System;
using System.Collections.Generic;

namespace API_LCKR.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string OrganizationCnpj { get; set; } = null!;

    public virtual Organization OrganizationCnpjNavigation { get; set; } = null!;
}
