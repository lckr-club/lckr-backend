using API_LCKR.Models;
using System;
using System.Collections.Generic;

namespace API_LCKR.Models;

public partial class Lesson
{
    public Guid Id { get; set; }

    public DateOnly DataAula { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFim { get; set; }

    public int Capacidade { get; set; }

    public StatusLesson Status { get; set; }

    public string Modalidade { get; set; } = null!;

    public string? Descricao { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string CoachCpf { get; set; } = null!;

    public string? CoachCpfPrevisto { get; set; }

    public virtual Coach CoachCpfNavigation { get; set; } = null!;

    public virtual Coach? CoachCpfPrevistoNavigation { get; set; }
}
