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

        public virtual DbSet<App> App { get; set; }
        public virtual DbSet<AppFeatures> AppFeatures { get; set; }
        public virtual DbSet<AreaLocals> AreaLocals { get; set; }
        public virtual DbSet<Areas> Areas { get; set; }
        public virtual DbSet<Companies> Companies { get; set; }
        public virtual DbSet<CompaniesSites> CompaniesSites { get; set; }
        public virtual DbSet<CompanyLocals> CompanyLocals { get; set; }
        public virtual DbSet<SiteLocals> SiteLocals { get; set; }
        public virtual DbSet<Sites> Sites { get; set; }
        public virtual DbSet<SitesUsers> SitesUsers { get; set; }
        public virtual DbSet<UserAppTokens> UserAppTokens { get; set; }
        public virtual DbSet<UserGroups> UserGroups { get; set; }
        public virtual DbSet<UserGroupsAppFeatures> UserGroupsAppFeatures { get; set; }
        public virtual DbSet<UserGroupsApps> UserGroupsApps { get; set; }
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
            modelBuilder.Entity<App>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<AppFeatures>(entity =>
            {
                entity.HasKey(e => e.FeatureId);

                entity.ToTable("App_Features");

                entity.Property(e => e.FeatureId).HasColumnName("Feature_Id");

                entity.Property(e => e.AppId).HasColumnName("App_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.App)
                    .WithMany(p => p.AppFeatures)
                    .HasForeignKey(d => d.AppId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_App_Features_App");
            });

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

            modelBuilder.Entity<UserAppTokens>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_UserAppTokens_1");

                entity.Property(e => e.UserId)
                    .HasColumnName("User_Id")
                    .HasMaxLength(128);

                entity.Property(e => e.ExpiredAt).HasColumnType("datetime");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserAppTokens)
                    .HasForeignKey<UserAppTokens>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAppTokens_Users");
            });

            modelBuilder.Entity<UserGroups>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<UserGroupsAppFeatures>(entity =>
            {
                entity.HasKey(e => new { e.FeatureId, e.AppUserGroupId });

                entity.ToTable("UserGroups_AppFeatures");

                entity.Property(e => e.FeatureId).HasColumnName("Feature_Id");

                entity.Property(e => e.AppUserGroupId).HasColumnName("AppUserGroup_Id");

                entity.HasOne(d => d.Feature)
                    .WithMany(p => p.UserGroupsAppFeatures)
                    .HasForeignKey(d => d.FeatureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserGroups_AppFeatures_App_Features");
            });

            modelBuilder.Entity<UserGroupsApps>(entity =>
            {
                entity.ToTable("UserGroups_Apps");

                entity.Property(e => e.Id).HasDefaultValueSql("((1))");

                entity.Property(e => e.AppId).HasColumnName("App_Id");

                entity.Property(e => e.UserGroupId).HasColumnName("UserGroup_Id");

                entity.HasOne(d => d.App)
                    .WithMany(p => p.UserGroupsApps)
                    .HasForeignKey(d => d.AppId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserGroups_Apps_App");

                entity.HasOne(d => d.UserGroup)
                    .WithMany(p => p.UserGroupsApps)
                    .HasForeignKey(d => d.UserGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserGroups_Apps_UserGroups");

                entity.HasOne(d => d.UserGroupsAppFeatures)
                    .WithMany(p => p.UserGroupsApps)
                    .HasForeignKey(d => new { d.Id, d.UserGroupId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserGroups_Apps_UserGroups_AppFeatures");
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

                entity.Property(e => e.DefaultLanguageId)
                    .IsRequired()
                    .HasColumnName("DefaultLanguage_Id")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('en')");

                entity.Property(e => e.Email).HasMaxLength(500);

                entity.Property(e => e.HashedPassword)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.Property(e => e.UserGroupId).HasColumnName("UserGroup_Id");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.UserGroup)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_UserGroups");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
