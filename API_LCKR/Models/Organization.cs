using System;
using System.Collections.Generic;

namespace API_LCKR.Models;

public partial class Organization
{
    public string Cnpj { get; set; } = null!;

    public string RazaoSocial { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Endereco { get; set; } = null!;

    public virtual ICollection<Coach> Coaches { get; set; } = new List<Coach>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
