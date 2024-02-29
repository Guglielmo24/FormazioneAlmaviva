using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AnimaliWebApi.Models.DB;

public partial class FormazioneDBContext : DbContext
{
    public FormazioneDBContext()
    {
    }

    public FormazioneDBContext(DbContextOptions<FormazioneDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comune> Comune { get; set; }

    public virtual DbSet<Persona> Persona { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=AC-GLANGELLA2\\SQLEXPRESS;Initial Catalog=FormazioneDB;Integrated Security=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasOne(d => d.ID_ComuneDiNascitaNavigation).WithMany(p => p.Persona)
                .HasForeignKey(d => d.ID_ComuneDiNascita)
                .HasConstraintName("FK_Persona_Comune");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
