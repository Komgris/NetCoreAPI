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
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Locations> Locations { get; set; }
        public virtual DbSet<LossLevel1> LossLevel1 { get; set; }
        public virtual DbSet<LossLevel2> LossLevel2 { get; set; }
        public virtual DbSet<LossLevel3> LossLevel3 { get; set; }
        public virtual DbSet<Machine> Machine { get; set; }
        public virtual DbSet<MachineComponent> MachineComponent { get; set; }
        public virtual DbSet<MachineComponentType> MachineComponentType { get; set; }
        public virtual DbSet<MachineComponentTypeLossLevel3> MachineComponentTypeLossLevel3 { get; set; }
        public virtual DbSet<MachineStatus> MachineStatus { get; set; }
        public virtual DbSet<MachineType> MachineType { get; set; }
        public virtual DbSet<MachineTypeMaterial> MachineTypeMaterial { get; set; }
        public virtual DbSet<MaintenanceActivity> MaintenanceActivity { get; set; }
        public virtual DbSet<MaintenancePlan> MaintenancePlan { get; set; }
        public virtual DbSet<MaintenanceTeam> MaintenanceTeam { get; set; }
        public virtual DbSet<MaintenanceTeamMember> MaintenanceTeamMember { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<Name> Name { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductFamily> ProductFamily { get; set; }
        public virtual DbSet<ProductGroup> ProductGroup { get; set; }
        public virtual DbSet<ProductType> ProductType { get; set; }
        public virtual DbSet<ProductionDetails> ProductionDetails { get; set; }
        public virtual DbSet<ProductionPlan> ProductionPlan { get; set; }
        public virtual DbSet<Route> Route { get; set; }
        public virtual DbSet<RouteMachine> RouteMachine { get; set; }
        public virtual DbSet<RouteProductGroup> RouteProductGroup { get; set; }
        public virtual DbSet<Sensor> Sensor { get; set; }
        public virtual DbSet<Sites> Sites { get; set; }
        public virtual DbSet<SitesUsers> SitesUsers { get; set; }
        public virtual DbSet<Sysdiagrams> Sysdiagrams { get; set; }
        public virtual DbSet<UserAppTokens> UserAppTokens { get; set; }
        public virtual DbSet<UserGroups> UserGroups { get; set; }
        public virtual DbSet<UserGroupsAppFeatures> UserGroupsAppFeatures { get; set; }
        public virtual DbSet<UserGroupsApps> UserGroupsApps { get; set; }
        public virtual DbSet<UserPosition> UserPosition { get; set; }
        public virtual DbSet<UserProfiles> UserProfiles { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=103.70.6.198,1433;initial catalog=cim_db;persist security info=True;user id=cim;password=4dev@psec;MultipleActiveResultSets=True;");
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

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Ddname).HasMaxLength(50);

                entity.Property(e => e.Ddsnam).HasMaxLength(20);

                entity.Property(e => e.Education).HasMaxLength(50);

                entity.Property(e => e.EmLevel).HasMaxLength(20);

                entity.Property(e => e.EmNo).HasMaxLength(50);

                entity.Property(e => e.Empar).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Emsp2).HasMaxLength(20);

                entity.Property(e => e.HireDate).HasColumnType("date");

                entity.Property(e => e.Sex)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.TermDate).HasColumnType("date");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.Property(e => e.UserId)
                    .HasColumnName("User_Id")
                    .HasMaxLength(128);

                entity.Property(e => e.UserPositionId).HasColumnName("UserPosition_Id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Employees_Users");

                entity.HasOne(d => d.UserPosition)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.UserPositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employees_UserPosition1");
            });

            modelBuilder.Entity<Locations>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ParentId).HasColumnName("Parent_Id");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Locations_Locations");
            });

            modelBuilder.Entity<LossLevel1>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<LossLevel2>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.LossLevel1Id).HasColumnName("LossLevel1_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.LossLevel1)
                    .WithMany(p => p.LossLevel2)
                    .HasForeignKey(d => d.LossLevel1Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LossLevel2_LossLevel1");
            });

            modelBuilder.Entity<LossLevel3>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.LossLevel2Id).HasColumnName("LossLevel2_Id");

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.LossLevel2)
                    .WithMany(p => p.LossLevel3)
                    .HasForeignKey(d => d.LossLevel2Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LossLevel3_LossLevel2");
            });

            modelBuilder.Entity<Machine>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.MachineTypeId).HasColumnName("MachineType_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StatusId).HasColumnName("Status_Id");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.MachineType)
                    .WithMany(p => p.Machine)
                    .HasForeignKey(d => d.MachineTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Machine_MachineType");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Machine)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Machine_MachineStatus");
            });

            modelBuilder.Entity<MachineComponent>(entity =>
            {
                entity.ToTable("Machine_Component");

                entity.Property(e => e.MachineId).HasColumnName("Machine_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TypeId).HasColumnName("Type_Id");

                entity.HasOne(d => d.Machine)
                    .WithMany(p => p.MachineComponent)
                    .HasForeignKey(d => d.MachineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Machine_Component_Machine");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.MachineComponent)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Machine_Component_Machine_ComponentType");
            });

            modelBuilder.Entity<MachineComponentType>(entity =>
            {
                entity.ToTable("Machine_ComponentType");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.MachineTypeId).HasColumnName("MachineType_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.MachineType)
                    .WithMany(p => p.MachineComponentType)
                    .HasForeignKey(d => d.MachineTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Machine_ComponentType_MachineType");
            });

            modelBuilder.Entity<MachineComponentTypeLossLevel3>(entity =>
            {
                entity.ToTable("Machine_ComponentType_LossLevel3");

                entity.Property(e => e.LossLevel3Id).HasColumnName("LossLevel3_Id");

                entity.Property(e => e.MachineTypeComponentId).HasColumnName("MachineTypeComponent_Id");

                entity.HasOne(d => d.LossLevel3)
                    .WithMany(p => p.MachineComponentTypeLossLevel3)
                    .HasForeignKey(d => d.LossLevel3Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LossLevel3_MachineTypeComponent_LossLevel3");

                entity.HasOne(d => d.MachineTypeComponent)
                    .WithMany(p => p.MachineComponentTypeLossLevel3)
                    .HasForeignKey(d => d.MachineTypeComponentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MachineTypeComponent_LossLevel3_MachineTypeComponent");
            });

            modelBuilder.Entity<MachineStatus>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MachineType>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.HasOee).HasColumnName("HasOEE");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<MachineTypeMaterial>(entity =>
            {
                entity.ToTable("MachineType_Material");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.MachineTypeId).HasColumnName("MachineType_Id");

                entity.Property(e => e.MaterialId).HasColumnName("Material_Id");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.MachineType)
                    .WithMany(p => p.MachineTypeMaterial)
                    .HasForeignKey(d => d.MachineTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MachineType_Material_MachineType");
            });

            modelBuilder.Entity<MaintenanceActivity>(entity =>
            {
                entity.ToTable("Maintenance_Activity");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.Details).HasMaxLength(2000);

                entity.Property(e => e.MaintenanceId).HasColumnName("Maintenance_Id");

                entity.Property(e => e.UpdateDate).HasColumnType("date");

                entity.HasOne(d => d.Maintenance)
                    .WithMany(p => p.MaintenanceActivity)
                    .HasForeignKey(d => d.MaintenanceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Maintenance_Activity_Maintenance_Plan");
            });

            modelBuilder.Entity<MaintenancePlan>(entity =>
            {
                entity.ToTable("Maintenance_Plan");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.LastDate).HasColumnType("date");

                entity.Property(e => e.MachineId).HasColumnName("Machine_Id");

                entity.Property(e => e.MaintenanceTeamId).HasColumnName("MaintenanceTeam_Id");

                entity.Property(e => e.NextDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.MaintenanceTeam)
                    .WithMany(p => p.MaintenancePlan)
                    .HasForeignKey(d => d.MaintenanceTeamId)
                    .HasConstraintName("FK_Maintenance_Plan_Maintenance_Team");
            });

            modelBuilder.Entity<MaintenanceTeam>(entity =>
            {
                entity.ToTable("Maintenance_Team");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("date");
            });

            modelBuilder.Entity<MaintenanceTeamMember>(entity =>
            {
                entity.ToTable("MaintenanceTeam_Member");

                entity.Property(e => e.AddDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MaintenanceTeamId).HasColumnName("MaintenanceTeam_Id");

                entity.Property(e => e.Response).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("date");
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.Property(e => e.BhtperUnit)
                    .HasColumnName("BHTPerUnit")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Icsgroup)
                    .HasColumnName("ICSGroup")
                    .HasMaxLength(50);

                entity.Property(e => e.MaterialGroup).HasMaxLength(50);

                entity.Property(e => e.ProductCategory).HasMaxLength(50);

                entity.Property(e => e.Uom)
                    .HasColumnName("UOM")
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<Name>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.EmployeeId).HasColumnName("Employee_Id");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.LanguageId)
                    .IsRequired()
                    .HasColumnName("Language_id")
                    .HasMaxLength(2);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.Property(e => e.UserId)
                    .HasColumnName("User_Id")
                    .HasMaxLength(128);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Name)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Name_Employees");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Name)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Name_Users");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.BriteItemPerUpcitem)
                    .HasColumnName("BriteItemPerUPCItem")
                    .HasMaxLength(50);

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Igweight)
                    .HasColumnName("IGWeight")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.NetWeight).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PackingMedium).HasMaxLength(50);

                entity.Property(e => e.Pmweight)
                    .HasColumnName("PMWeight")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ProductFamily_Id).HasColumnName("ProductFamily_Id");

                entity.Property(e => e.ProductGroup_Id).HasColumnName("ProductGroup_Id");

                entity.Property(e => e.ProductType_Id).HasColumnName("ProductType_Id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.Property(e => e.WeightPerUom)
                    .HasColumnName("WeightPerUOM")
                    .HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.ProductFamily)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.ProductFamily_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_ProductFamily");

                entity.HasOne(d => d.ProductGroup)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.ProductGroup_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_ProductGroup");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.ProductType_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_ProductType");
            });

            modelBuilder.Entity<ProductFamily>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<ProductGroup>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<ProductType>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<ProductionDetails>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Production_Details");

                entity.Property(e => e.ProductDescription).HasMaxLength(50);

                entity.Property(e => e.ProductFamily)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.ProductGroup)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.ProductId)
                    .HasColumnName("Product_Id")
                    .HasMaxLength(20);

                entity.Property(e => e.ProductType)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Speed)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<ProductionPlan>(entity =>
            {
                entity.HasKey(e => e.PlanId);

                entity.ToTable("Production_Plan");

                entity.Property(e => e.PlanId).HasMaxLength(50);

                entity.Property(e => e.ActualFinish).HasColumnType("datetime");

                entity.Property(e => e.ActualStart).HasColumnType("datetime");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.PlanFinish).HasColumnType("datetime");

                entity.Property(e => e.PlanStart).HasColumnType("datetime");

                entity.Property(e => e.ProductId)
                    .HasColumnName("Product_Id")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.RouteId).HasColumnName("Route_Id");

                entity.Property(e => e.Status).HasMaxLength(10);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductionPlan)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Production_Plan_Product");
            });

            modelBuilder.Entity<Route>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<RouteMachine>(entity =>
            {
                entity.ToTable("Route_Machine");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.MachineId).HasColumnName("Machine_Id");

                entity.Property(e => e.RouteId).HasColumnName("Route_Id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.Machine)
                    .WithMany(p => p.RouteMachine)
                    .HasForeignKey(d => d.MachineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Route_Machine_Machine");

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.RouteMachine)
                    .HasForeignKey(d => d.RouteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Route_Machine_Route");
            });

            modelBuilder.Entity<RouteProductGroup>(entity =>
            {
                entity.ToTable("Route_ProductGroup");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.ProductGroupId).HasColumnName("ProductGroup_Id");

                entity.Property(e => e.RouteId).HasColumnName("Route_Id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.ProductGroup)
                    .WithMany(p => p.RouteProductGroup)
                    .HasForeignKey(d => d.ProductGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Route_ProductGroup_ProductGroup");

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.RouteProductGroup)
                    .HasForeignKey(d => d.RouteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Route_ProductGroup_Route");
            });

            modelBuilder.Entity<Sensor>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.MachineId).HasColumnName("Machine_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Plcaddress)
                    .HasColumnName("PLCAddress")
                    .HasMaxLength(50);

                entity.Property(e => e.Position).HasMaxLength(50);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Unit).HasMaxLength(50);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.Property(e => e.Value).HasMaxLength(50);

                entity.HasOne(d => d.Machine)
                    .WithMany(p => p.Sensor)
                    .HasForeignKey(d => d.MachineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sensor_Machine");
            });

            modelBuilder.Entity<Sites>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

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

            modelBuilder.Entity<Sysdiagrams>(entity =>
            {
                entity.HasKey(e => e.DiagramId)
                    .HasName("PK__sysdiagr__C2B05B61241CA7D3");

                entity.ToTable("sysdiagrams");

                entity.HasIndex(e => new { e.PrincipalId, e.Name })
                    .HasName("UK_principal_name")
                    .IsUnique();

                entity.Property(e => e.DiagramId).HasColumnName("diagram_id");

                entity.Property(e => e.Definition).HasColumnName("definition");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(128);

                entity.Property(e => e.PrincipalId).HasColumnName("principal_id");

                entity.Property(e => e.Version).HasColumnName("version");
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

            modelBuilder.Entity<UserPosition>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Role).HasMaxLength(50);
            });

            modelBuilder.Entity<UserProfiles>(entity =>
            {
                entity.ToTable("User_Profiles");

                entity.Property(e => e.Image).HasColumnType("image");

                entity.Property(e => e.UserId)
                    .HasColumnName("User_Id")
                    .HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserProfiles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_User_Profiles_Users");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("(newid())");

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
                    .HasMaxLength(500)
                    .HasDefaultValueSql("((1234))");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.Property(e => e.UserGroupId)
                    .HasColumnName("UserGroup_Id")
                    .HasDefaultValueSql("((2))");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'NoUserName')");

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
