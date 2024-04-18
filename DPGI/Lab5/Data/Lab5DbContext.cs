using System.Configuration;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Lab5.Data
{
    public partial class Lab5DbContext : DbContext
    {
        public Lab5DbContext()
        {
        }

        public Lab5DbContext(DbContextOptions<Lab5DbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["MyConnectionString"]
                    .ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Isbn)
                    .HasName("PK__Books__447D36EB170445C9");

                entity.Property(e => e.Isbn)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ISBN");

                entity.Property(e => e.Authors)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.PublisherCodeNavigation)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.PublisherCode)
                    .HasConstraintName("FK__Books__Publisher__398D8EEE");
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.HasKey(e => e.PublisherCode)
                    .HasName("PK__Publishe__DFB88E280543C19C");

                entity.Property(e => e.PublisherCode).ValueGeneratedNever();

                entity.Property(e => e.PublisherName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}