using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HCILab.Website.Models
{
    public partial class hcilabContext : DbContext
    {
        public hcilabContext()
        {
        }

        public hcilabContext(DbContextOptions<hcilabContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Affiliation> Affiliation { get; set; }
        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<Articlekeyword> Articlekeyword { get; set; }
        public virtual DbSet<Author> Author { get; set; }
        public virtual DbSet<Authoraffiliation> Authoraffiliation { get; set; }
        public virtual DbSet<Authorarticles> Authorarticles { get; set; }
        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<Conference> Conference { get; set; }
        public virtual DbSet<ExternalId> ExternalId { get; set; }
        public virtual DbSet<Journal> Journal { get; set; }
        public virtual DbSet<Keyword> Keyword { get; set; }
        public virtual DbSet<Tableieee> Tableieee { get; set; }
        public virtual DbSet<Tableorcid> Tableorcid { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=12345;database=hcilab", x => x.ServerVersion("8.0.19-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Affiliation>(entity =>
            {
                entity.ToTable("affiliation");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address)
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Country)
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.InstitutionName)
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.InstitutionPosition)
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Website)
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(e => e.IdArticle)
                    .HasName("PRIMARY");

                entity.ToTable("article");

                entity.Property(e => e.IdArticle).HasColumnName("ID_Article");

                entity.Property(e => e.Abstract)
                    .HasColumnType("varchar(10000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.EndPage)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.StartPage)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Title)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Articlekeyword>(entity =>
            {
                entity.ToTable("articlekeyword");

                entity.HasIndex(e => e.ArticleId)
                    .HasName("ArticleId");

                entity.HasIndex(e => e.IdKeyword)
                    .HasName("IdKeyword");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.Articlekeyword)
                    .HasForeignKey(d => d.ArticleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("articlekeyword_ibfk_2");

                entity.HasOne(d => d.IdKeywordNavigation)
                    .WithMany(p => p.Articlekeyword)
                    .HasForeignKey(d => d.IdKeyword)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("articlekeyword_ibfk_1");
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.ToTable("author");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AuthorUrl)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Country)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Email)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IdOrcid)
                    .HasColumnName("ID_Orcid")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Authoraffiliation>(entity =>
            {
                entity.ToTable("authoraffiliation");

                entity.HasIndex(e => e.IdAffiliation)
                    .HasName("IdAffiliation");

                entity.HasIndex(e => e.IdAuthor)
                    .HasName("IdAuthor");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.IdAffiliationNavigation)
                    .WithMany(p => p.Authoraffiliation)
                    .HasForeignKey(d => d.IdAffiliation)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("authoraffiliation_ibfk_2");

                entity.HasOne(d => d.IdAuthorNavigation)
                    .WithMany(p => p.Authoraffiliation)
                    .HasForeignKey(d => d.IdAuthor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("authoraffiliation_ibfk_1");
            });

            modelBuilder.Entity<Authorarticles>(entity =>
            {
                entity.ToTable("authorarticles");

                entity.HasIndex(e => e.ArticleId)
                    .HasName("ArticleId");

                entity.HasIndex(e => e.AuthorId)
                    .HasName("AuthorId");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.Authorarticles)
                    .HasForeignKey(d => d.ArticleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("authorarticles_ibfk_2");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Authorarticles)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("authorarticles_ibfk_1");
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.IdBook)
                    .HasName("PRIMARY");

                entity.ToTable("book");

                entity.Property(e => e.BookName)
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Publisher)
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Volume)
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdBookNavigation)
                    .WithOne(p => p.Book)
                    .HasForeignKey<Book>(d => d.IdBook)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("book_ibfk_1");
            });

            modelBuilder.Entity<Conference>(entity =>
            {
                entity.HasKey(e => e.IdConference)
                    .HasName("PRIMARY");

                entity.ToTable("conference");

                entity.Property(e => e.ConferenceDates)
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ConferenceLocation)
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ConferenceName)
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdConferenceNavigation)
                    .WithOne(p => p.Conference)
                    .HasForeignKey<Conference>(d => d.IdConference)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("conference_ibfk_1");
            });

            modelBuilder.Entity<ExternalId>(entity =>
            {
                entity.ToTable("external_id");

                entity.HasIndex(e => e.ArticleId)
                    .HasName("ArticleId");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Type)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Value)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.ExternalId)
                    .HasForeignKey(d => d.ArticleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("external_id_ibfk_1");
            });

            modelBuilder.Entity<Journal>(entity =>
            {
                entity.HasKey(e => e.IdJournal)
                    .HasName("PRIMARY");

                entity.ToTable("journal");

                entity.Property(e => e.JournalTitle)
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdJournalNavigation)
                    .WithOne(p => p.Journal)
                    .HasForeignKey<Journal>(d => d.IdJournal)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("journal_ibfk_1");
            });

            modelBuilder.Entity<Keyword>(entity =>
            {
                entity.ToTable("keyword");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Tableieee>(entity =>
            {
                entity.HasKey(e => e.IdIeee)
                    .HasName("PRIMARY");

                entity.ToTable("tableieee");

                entity.HasIndex(e => e.ArticleId)
                    .HasName("ArticleId");

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.Tableieee)
                    .HasForeignKey(d => d.ArticleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tableieee_ibfk_1");
            });

            modelBuilder.Entity<Tableorcid>(entity =>
            {
                entity.HasKey(e => e.IdOrcid)
                    .HasName("PRIMARY");

                entity.ToTable("tableorcid");

                entity.HasIndex(e => e.ArticleId)
                    .HasName("ArticleId");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SourceName)
                    .IsRequired()
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.Tableorcid)
                    .HasForeignKey(d => d.ArticleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tableorcid_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
