using System;
using System.Collections.Generic;

namespace API_LCKR.Models;

public partial class CoachSalaryHistory
{
    public Guid Id { get; set; }

    public string CoachCpf { get; set; } = null!;

    public decimal Salario { get; set; }

    public DateOnly DataInicio { get; set; }

    public DateOnly? DataFim { get; set; }

    public string? Motivo { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Coach CoachCpfNavigation { get; set; } = null!;
}
