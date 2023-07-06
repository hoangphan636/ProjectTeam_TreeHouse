using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace BusinessObject.DataAccess
{
    public partial class PRN231FamilyTreeContext : DbContext
    {
        public PRN231FamilyTreeContext()
        {
        }
        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", true, true)
               .Build();
            var strConn = config["ConnectionStrings:DefaultConnection"];

            return strConn;
        }

        public PRN231FamilyTreeContext(DbContextOptions<PRN231FamilyTreeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Family> Families { get; set; }
        public virtual DbSet<FamilyMember> FamilyMembers { get; set; }
        public virtual DbSet<Relationship> Relationships { get; set; }
        public virtual DbSet<Relative> Relatives { get; set; }
        public virtual DbSet<StudyPromotion> StudyPromotions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.HasOne(d => d.Member)
                    .WithOne(p => p.Accounts)
                    .HasConstraintName("FK__Accounts__Member__300424B4");
            });

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivityName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.FamilyId).HasColumnName("FamilyID");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Family)
                    .WithMany(p => p.Activities)
                    .HasForeignKey(d => d.FamilyId)
                    .HasConstraintName("FK__Activitie__Famil__32E0915F");
            });

            modelBuilder.Entity<Album>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AlbumName).HasMaxLength(200);

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.FamilyId).HasColumnName("FamilyID");

                entity.Property(e => e.UrlAlbum)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.HasOne(d => d.Family)
                    .WithMany(p => p.Albums)
                    .HasForeignKey(d => d.FamilyId)
                    .HasConstraintName("FK__Albums__FamilyID__38996AB5");
            });

            modelBuilder.Entity<Family>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.FamilyName)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<FamilyMember>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("DOB");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FamilyId).HasColumnName("FamilyID");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(12);

                entity.HasOne(d => d.Family)
                    .WithMany(p => p.FamilyMembers)
                    .HasForeignKey(d => d.FamilyId)
                    .HasConstraintName("FK__FamilyMem__Famil__267ABA7A");
            });

            modelBuilder.Entity<Relationship>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RelationType)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Relative>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FamilyId).HasColumnName("FamilyID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.MemberRelativeId).HasColumnName("MemberRelativeID");

                entity.Property(e => e.RelationId).HasColumnName("RelationID");

                entity.HasOne(d => d.Family)
                    .WithMany(p => p.Relatives)
                    .HasForeignKey(d => d.FamilyId)
                    .HasConstraintName("FK__Relatives__Famil__2D27B809");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Relatives)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK__Relatives__Membe__2B3F6F97");

                entity.HasOne(d => d.Relation)
                    .WithMany(p => p.Relatives)
                    .HasForeignKey(d => d.RelationId)
                    .HasConstraintName("FK__Relatives__Relat__2C3393D0");
            });

            modelBuilder.Entity<StudyPromotion>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.FamilyId).HasColumnName("FamilyID");

                entity.Property(e => e.PromotionName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Family)
                    .WithMany(p => p.StudyPromotions)
                    .HasForeignKey(d => d.FamilyId)
                    .HasConstraintName("FK__StudyProm__Famil__35BCFE0A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
