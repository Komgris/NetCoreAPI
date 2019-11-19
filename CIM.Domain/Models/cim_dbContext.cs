using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CIM.Domain.Models
{
    public partial class cim_dbContext : DbContext
    {
        public cim_dbContext()
        {
        }

        public cim_dbContext(DbContextOptions<cim_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AreaLocals> AreaLocals { get; set; }
        public virtual DbSet<Areas> Areas { get; set; }
        public virtual DbSet<Companies> Companies { get; set; }
        public virtual DbSet<CompaniesSites> CompaniesSites { get; set; }
        public virtual DbSet<CompanyLocals> CompanyLocals { get; set; }
        public virtual DbSet<SiteLocals> SiteLocals { get; set; }
        public virtual DbSet<Sites> Sites { get; set; }
        public virtual DbSet<SitesUsers> SitesUsers { get; set; }
        public virtual DbSet<UserProfiles> UserProfiles { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\sqlexpress;initial catalog=cim_db;persist security info=True;user id=cim;password=4dev@pse;MultipleActiveResultSets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AreaLocals>(entity =>
            {
                entity.ToTable("Area_Locals");

                entity.Property(e => e.FId).HasColumnName("F_Id");

                entity.Property(e => e.LanguageId)
                    .IsRequired()
                    .HasColumnName("Language_Id")
                    .HasMaxLength(2)
                    .IsFixedLength();

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.HasOne(d => d.F)
                    .WithMany(p => p.AreaLocals)
                    .HasForeignKey(d => d.FId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Area_Locals_Areas");
            });

            modelBuilder.Entity<Areas>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.ParentId).HasColumnName("Parent_Id");

                entity.Property(e => e.SiteId).HasColumnName("Site_Id");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.Areas)
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Areas_Sites");
            });

            modelBuilder.Entity<Companies>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<CompaniesSites>(entity =>
            {
                entity.ToTable("Companies_Sites");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CompanyId).HasColumnName("Company_Id");

                entity.Property(e => e.SiteId).HasColumnName("Site_Id");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompaniesSites)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Companies_Sites_Companies");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.CompaniesSites)
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Companies_Sites_Sites");
            });

            modelBuilder.Entity<CompanyLocals>(entity =>
            {
                entity.ToTable("Company_Locals");

                entity.Property(e => e.FId).HasColumnName("F_Id");

                entity.Property(e => e.LanguageId)
                    .IsRequired()
                    .HasColumnName("Language_Id")
                    .HasMaxLength(2)
                    .IsFixedLength();

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.HasOne(d => d.F)
                    .WithMany(p => p.CompanyLocals)
                    .HasForeignKey(d => d.FId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Company_Locals_Companies");
            });

            modelBuilder.Entity<SiteLocals>(entity =>
            {
                entity.ToTable("Site_Locals");

                entity.Property(e => e.FId).HasColumnName("F_Id");

                entity.Property(e => e.LanguageId)
                    .IsRequired()
                    .HasColumnName("Language_Id")
                    .HasMaxLength(2)
                    .IsFixedLength();

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.HasOne(d => d.F)
                    .WithMany(p => p.SiteLocals)
                    .HasForeignKey(d => d.FId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Site_Locals_Sites1");
            });

            modelBuilder.Entity<Sites>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<SitesUsers>(entity =>
            {
                entity.ToTable("Sites_Users");

                entity.Property(e => e.SiteId).HasColumnName("Site_Id");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("User_Id")
                    .HasMaxLength(128);

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.SitesUsers)
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sites_Users_Sites");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SitesUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sites_Users_Users");
            });

            modelBuilder.Entity<UserProfiles>(entity =>
            {
                entity.ToTable("User_Profiles");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Image).HasColumnType("image");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("User_Id")
                    .HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserProfiles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Profiles_Users");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Email).HasMaxLength(500);

                entity.Property(e => e.HashedPassword)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
