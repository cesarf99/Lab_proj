using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HCILab.Projects.Models
{
    public partial class hcilabprojectContext : DbContext
    {
        public hcilabprojectContext()
        {
        }

        public hcilabprojectContext(DbContextOptions<hcilabprojectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Connectionpublication> Connectionpublication { get; set; }
        public virtual DbSet<Connectionresearcher> Connectionresearcher { get; set; }
        public virtual DbSet<Project> Project { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=12345;database=hcilabproject", x => x.ServerVersion("8.0.19-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Connectionpublication>(entity =>
            {
                entity.ToTable("connectionpublication");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Doi)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            });

            modelBuilder.Entity<Connectionresearcher>(entity =>
            {
                entity.ToTable("connectionresearcher");

                entity.HasIndex(e => e.ProjectId)
                    .HasName("ProjectID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.OrcidId).HasColumnName("OrcidID");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Connectionresearcher)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("connectionresearcher_ibfk_1");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.IdProject)
                    .HasName("PRIMARY");

                entity.ToTable("project");

                entity.Property(e => e.IdProject).HasColumnName("ID_Project");

                entity.Property(e => e.Country)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Description)
                    .HasColumnType("varchar(2000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Email)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Financer)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.GrantNumber)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Putcode)
                    .HasColumnType("mediumtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Tipo)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Url)
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
