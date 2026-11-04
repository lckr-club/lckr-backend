using System;
using System.Collections.Generic;

namespace API_LCKR.Models;

public partial class PayrollPeriod
{
    public Guid Id { get; set; }

    public string MesReferencia { get; set; } = null!;

    public DateOnly DataInicio { get; set; }

    public DateOnly DataFim { get; set; }

    public StatusPeriodo Status { get; set; }

    public DateTime? DataFechamento { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string OrganizationCnpj { get; set; } = null!;

    public virtual Organization OrganizationCnpjNavigation { get; set; } = null!;
}
