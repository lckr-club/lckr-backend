using API_LCKR.Models;
using System;
using System.Collections.Generic;

namespace API_LCKR.Models;

public partial class Coach
{
    public string Cpf { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public string Telefone { get; set; } = null!;

    public DateOnly DataAdmissao { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public string Especialidade { get; set; } = null!;

    public StatusCoach Status { get; set; }

    public decimal SalarioAtual { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string OrganizationCnpj { get; set; } = null!;

    public virtual ICollection<CoachSalaryHistory> CoachSalaryHistories { get; set; } = new List<CoachSalaryHistory>();

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Organization OrganizationCnpjNavigation { get; set; } = null!;
}
