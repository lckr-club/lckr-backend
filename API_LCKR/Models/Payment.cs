using API_LCKR.Models;
using System;
using System.Collections.Generic;

namespace API_LCKR.Models;

public partial class Payment
{
    public Guid Id { get; set; }

    public decimal Valor { get; set; }

    public DateOnly DataPagamento { get; set; }

    public StatusPagamento Status { get; set; }

    public string MesReferencia { get; set; } = null!;

    public string Descricao { get; set; } = null!;

    public DateOnly? PagoEm { get; set; }

    public string CpfCoach { get; set; } = null!;

    public string OrganizationCnpj { get; set; } = null!;

    public virtual Coach CpfCoachNavigation { get; set; } = null!;

    public virtual Organization OrganizationCnpjNavigation { get; set; } = null!;
}
