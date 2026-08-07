using System;
using System.Collections.Generic;
using EventPlus.WebAPI.Domains;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Contexts;

public partial class EventContext : DbContext
{
    public EventContext()
    {
    }

    public EventContext(DbContextOptions<EventContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comentario> Comentario { get; set; }

    public virtual DbSet<Evento> Evento { get; set; }

    public virtual DbSet<Instituicao> Instituicao { get; set; }

    public virtual DbSet<Presenca> Presenca { get; set; }

    public virtual DbSet<TipoEvento> TipoEvento { get; set; }

    public virtual DbSet<TipoUsuario> TipoUsuario { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=D23S20-1251881\\MSSQLSERVER2;Database=EventPlus;User Id=sa;Password=Senai@134;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.idComentario).HasName("PK__Comentar__C74515DA3496D6F0");

            entity.Property(e => e.idComentario).HasDefaultValueSql("(newid())");
            entity.Property(e => e.DataComentario).HasColumnType("datetime");
            entity.Property(e => e.Descricao)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.idEventoNavigation).WithMany(p => p.Comentario)
                .HasForeignKey(d => d.idEvento)
                .HasConstraintName("FK__Comentari__idEve__5FB337D6");

            entity.HasOne(d => d.idUsuarioNavigation).WithMany(p => p.Comentario)
                .HasForeignKey(d => d.idUsuario)
                .HasConstraintName("FK__Comentari__idUsu__5EBF139D");
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.idEvento).HasName("PK__Evento__C8DC7BDA2B5EE881");

            entity.Property(e => e.idEvento).HasDefaultValueSql("(newid())");
            entity.Property(e => e.DataEvento).HasColumnType("datetime");
            entity.Property(e => e.Descricao).HasColumnType("text");
            entity.Property(e => e.ImagemUrl)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.NomeEvento)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.idInstituicaoNavigation).WithMany(p => p.Evento)
                .HasForeignKey(d => d.idInstituicao)
                .HasConstraintName("FK__Evento__idInstit__59FA5E80");

            entity.HasOne(d => d.idTipoEventoNavigation).WithMany(p => p.Evento)
                .HasForeignKey(d => d.idTipoEvento)
                .HasConstraintName("FK__Evento__idTipoEv__5AEE82B9");
        });

        modelBuilder.Entity<Instituicao>(entity =>
        {
            entity.HasKey(e => e.idInstituicao).HasName("PK__Institui__8EA7AB0024CA1BD3");

            entity.HasIndex(e => e.CNPJ, "UQ__Institui__AA57D6B445CFDB94").IsUnique();

            entity.Property(e => e.idInstituicao).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CNPJ)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Endereço)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NomeFantasia)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Presenca>(entity =>
        {
            entity.HasKey(e => e.idPresenca).HasName("PK__Presenca__44CEA427DFA38397");

            entity.Property(e => e.idPresenca).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.idEventoNavigation).WithMany(p => p.Presenca)
                .HasForeignKey(d => d.idEvento)
                .HasConstraintName("FK__Presenca__idEven__6477ECF3");

            entity.HasOne(d => d.idUsuarioNavigation).WithMany(p => p.Presenca)
                .HasForeignKey(d => d.idUsuario)
                .HasConstraintName("FK__Presenca__idUsua__6383C8BA");
        });

        modelBuilder.Entity<TipoEvento>(entity =>
        {
            entity.HasKey(e => e.idTipoEvento).HasName("PK__TipoEven__09EED93A82F8EA5C");

            entity.Property(e => e.idTipoEvento).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Titulo)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.HasKey(e => e.idTipoUsuario).HasName("PK__TipoUsua__03006BFFDEFB1548");

            entity.Property(e => e.idTipoUsuario).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Titulo)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.idUsuario).HasName("PK__Usuario__645723A66438257B");

            entity.HasIndex(e => e.Email, "UQ__Usuario__A9D10534E4B87371").IsUnique();

            entity.Property(e => e.idUsuario).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Senha)
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.HasOne(d => d.idTipoUsuarioNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.idTipoUsuario)
                .HasConstraintName("FK__Usuario__idTipoU__5629CD9C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
