using Microsoft.EntityFrameworkCore;
using API_LCKR.Models;
using System;
using System.Collections.Generic;

namespace API_LCKR.Models;

public partial class LckrDatabaseContext : DbContext
{
    public LckrDatabaseContext()
    {
    }

    public LckrDatabaseContext(DbContextOptions<LckrDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Coach> Coaches { get; set; }

    public virtual DbSet<CoachSalaryHistory> CoachSalaryHistories { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.HasPostgresEnum<StatusPagamento>(
            "public",
            "status_pagamento"
        );

        modelBuilder.HasPostgresEnum<StatusCoach>(
            "public",
            "status_coaches"
        );

        modelBuilder.HasPostgresEnum<StatusLesson>(
            "public",
            "status_lessons"
        );

        modelBuilder.Entity<Coach>(entity =>
        {
            entity.HasKey(e => e.Cpf).HasName("coaches_pkey");

            entity.ToTable("coaches");

            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .HasColumnName("cpf");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DataAdmissao).HasColumnName("data_admissao");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Especialidade)
                .HasMaxLength(50)
                .HasColumnName("especialidade");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasColumnType("status_coaches");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome");
            entity.Property(e => e.OrganizationCnpj)
                .HasMaxLength(14)
                .HasColumnName("organization_cnpj");
            entity.Property(e => e.SalarioAtual)
                .HasPrecision(6, 2)
                .HasColumnName("salario_atual");
            entity.Property(e => e.Senha)
                .HasMaxLength(50)
                .HasColumnName("senha");
            entity.Property(e => e.Telefone)
                .HasMaxLength(11)
                .HasColumnName("telefone");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.OrganizationCnpjNavigation).WithMany(p => p.Coaches)
                .HasForeignKey(d => d.OrganizationCnpj)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("coaches_organization_cnpj_fkey");
        });

        modelBuilder.Entity<CoachSalaryHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("coach_salary_history_pkey");

            entity.ToTable("coach_salary_history");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CoachCpf)
                .HasMaxLength(11)
                .HasColumnName("coach_cpf");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DataFim).HasColumnName("data_fim");
            entity.Property(e => e.DataInicio).HasColumnName("data_inicio");
            entity.Property(e => e.DataPagamento).HasColumnName("data_pagamento");
            entity.Property(e => e.Motivo)
                .HasMaxLength(100)
                .HasColumnName("motivo");
            entity.Property(e => e.Salario)
                .HasPrecision(6, 2)
                .HasColumnName("salario");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.CoachCpfNavigation).WithMany(p => p.CoachSalaryHistories)
                .HasForeignKey(d => d.CoachCpf)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("coach_salary_history_coach_cpf_fkey");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lessons_pkey");

            entity.ToTable("lessons");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Capacidade).HasColumnName("capacidade");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasColumnType("status_lessons");
            entity.Property(e => e.CoachCpf)
                .HasMaxLength(11)
                .HasColumnName("coach_cpf");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DataAula).HasColumnName("data_aula");
            entity.Property(e => e.Descricao)
                .HasMaxLength(50)
                .HasColumnName("descricao");
            entity.Property(e => e.HoraFim).HasColumnName("hora_fim");
            entity.Property(e => e.HoraInicio).HasColumnName("hora_inicio");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.CoachCpfNavigation).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.CoachCpf)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("lessons_coach_cpf_fkey");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(e => e.Cnpj).HasName("organization_pkey");

            entity.ToTable("organization");

            entity.Property(e => e.Cnpj)
                .HasMaxLength(14)
                .HasColumnName("cnpj");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Endereco)
                .HasMaxLength(100)
                .HasColumnName("endereco");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome");
            entity.Property(e => e.RazaoSocial)
                .HasMaxLength(50)
                .HasColumnName("razao_social");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payments_pkey");

            entity.ToTable("payments");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.DataPagamento).HasColumnName("data_pagamento");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasColumnType("status_pagamento");
            entity.Property(e => e.Descricao)
                .HasMaxLength(50)
                .HasColumnName("descricao");
            entity.Property(e => e.CpfCoach)
                .HasMaxLength(11)
                .HasColumnName("cpf_coach");
            entity.Property(e => e.MesReferencia)
                .HasMaxLength(10)
                .HasColumnName("mes_referencia");
            entity.Property(e => e.OrganizationCnpj)
                .HasMaxLength(14)
                .HasColumnName("organization_cnpj");
            entity.Property(e => e.PagoEm).HasColumnName("pago_em");
            entity.Property(e => e.Valor)
                .HasPrecision(5, 2)
                .HasColumnName("valor");

            entity.HasOne(d => d.OrganizationCnpjNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrganizationCnpj)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payments_organization_cnpj_fkey");

            entity.HasOne(d => d.CpfCoachNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CpfCoach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_payments_coaches");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome");
            entity.Property(e => e.OrganizationCnpj)
                .HasMaxLength(14)
                .HasColumnName("organization_cnpj");
            entity.Property(e => e.Senha)
                .HasMaxLength(50)
                .HasColumnName("senha");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.OrganizationCnpjNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.OrganizationCnpj)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_organization_cnpj_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
