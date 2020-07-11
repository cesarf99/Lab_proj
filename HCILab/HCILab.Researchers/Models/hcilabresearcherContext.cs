using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HCILab.Researchers.Models
{
    public partial class hcilabresearcherContext : DbContext
    {
        public hcilabresearcherContext()
        {
        }

        public hcilabresearcherContext(DbContextOptions<hcilabresearcherContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Researcher> Researcher { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=12345;database=hcilabresearcher", x => x.ServerVersion("8.0.19-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Researcher>(entity =>
            {
                entity.HasKey(e => e.IdResearcher)
                    .HasName("PRIMARY");

                entity.ToTable("researcher");

                entity.Property(e => e.IdResearcher).HasColumnName("ID_Researcher");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.OrcidId)
                    .IsRequired()
                    .HasColumnName("OrcidID")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ScopusId)
                    .HasColumnName("ScopusID")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
