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
        public virtual DbSet<Bom> Bom { get; set; }
        public virtual DbSet<BomDole> BomDole { get; set; }
        public virtual DbSet<Companies> Companies { get; set; }
        public virtual DbSet<CompaniesSites> CompaniesSites { get; set; }
        public virtual DbSet<CompanyLocals> CompanyLocals { get; set; }
        public virtual DbSet<Component> Component { get; set; }
        public virtual DbSet<ComponentType> ComponentType { get; set; }
        public virtual DbSet<ComponentTypeLossLevel3> ComponentTypeLossLevel3 { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Locations> Locations { get; set; }
        public virtual DbSet<LossLevel1> LossLevel1 { get; set; }
        public virtual DbSet<LossLevel2> LossLevel2 { get; set; }
        public virtual DbSet<LossLevel3> LossLevel3 { get; set; }
        public virtual DbSet<Machine> Machine { get; set; }
        public virtual DbSet<MachineStatus> MachineStatus { get; set; }
        public virtual DbSet<MachineType> MachineType { get; set; }
        public virtual DbSet<MachineTypeComponentType> MachineTypeComponentType { get; set; }
        public virtual DbSet<MachineTypeLossLevel3> MachineTypeLossLevel3 { get; set; }
        public virtual DbSet<MachineTypeMaterial> MachineTypeMaterial { get; set; }
        public virtual DbSet<MaintenanceActivity> MaintenanceActivity { get; set; }
        public virtual DbSet<MaintenancePlan> MaintenancePlan { get; set; }
        public virtual DbSet<MaintenanceTeam> MaintenanceTeam { get; set; }
        public virtual DbSet<MaintenanceTeamMember> MaintenanceTeamMember { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<MaterialType> MaterialType { get; set; }
        public virtual DbSet<Name> Name { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductDetail> ProductDetail { get; set; }
        public virtual DbSet<ProductFamily> ProductFamily { get; set; }
        public virtual DbSet<ProductGroup> ProductGroup { get; set; }
        public virtual DbSet<ProductType> ProductType { get; set; }
        public virtual DbSet<ProductionPlan> ProductionPlan { get; set; }
        public virtual DbSet<ProductionStatus> ProductionStatus { get; set; }
        public virtual DbSet<RecordActiveProductionPlan> RecordActiveProductionPlan { get; set; }
        public virtual DbSet<RecordMachineComponentLoss> RecordMachineComponentLoss { get; set; }
        public virtual DbSet<RecordMachineStatus> RecordMachineStatus { get; set; }
        public virtual DbSet<RecordManufacturingLoss> RecordManufacturingLoss { get; set; }
        public virtual DbSet<RecordProductionPlanOutput> RecordProductionPlanOutput { get; set; }
        public virtual DbSet<RecordProductionPlanWaste> RecordProductionPlanWaste { get; set; }
        public virtual DbSet<RecordProductionPlanWasteMaterials> RecordProductionPlanWasteMaterials { get; set; }
        public virtual DbSet<Route> Route { get; set; }
        public virtual DbSet<RouteMachine> RouteMachine { get; set; }
        public virtual DbSet<RouteProductGroup> RouteProductGroup { get; set; }
        public virtual DbSet<Sensor> Sensor { get; set; }
        public virtual DbSet<Sites> Sites { get; set; }
        public virtual DbSet<SitesUsers> SitesUsers { get; set; }
        public virtual DbSet<StandardCostBrite> StandardCostBrite { get; set; }
        public virtual DbSet<Sysdiagrams> Sysdiagrams { get; set; }
        public virtual DbSet<Units> Units { get; set; }
        public virtual DbSet<UserAppTokens> UserAppTokens { get; set; }
        public virtual DbSet<UserGroups> UserGroups { get; set; }
        public virtual DbSet<UserGroupsAppFeatures> UserGroupsAppFeatures { get; set; }
        public virtual DbSet<UserGroupsApps> UserGroupsApps { get; set; }
        public virtual DbSet<UserPosition> UserPosition { get; set; }
        public virtual DbSet<UserProfiles> UserProfiles { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<WasteLevel1> WasteLevel1 { get; set; }
        public virtual DbSet<WasteLevel2> WasteLevel2 { get; set; }

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
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

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

            modelBuilder.Entity<Bom>(entity =>
            {
                entity.ToTable("BOM");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.F24).HasMaxLength(255);

                entity.Property(e => e.FruitGroup)
                    .HasColumnName("FRUIT GROUP")
                    .HasMaxLength(255);

                entity.Property(e => e.FruitType)
                    .HasColumnName("Fruit type")
                    .HasMaxLength(255);

                entity.Property(e => e.FruitTypeDescription)
                    .HasColumnName("Fruit type Description")
                    .HasMaxLength(255);

                entity.Property(e => e.Imdsc1ConsumeDescription)
                    .HasColumnName("Imdsc1_Consume Description")
                    .HasMaxLength(255);

                entity.Property(e => e.Imdsc1Description)
                    .HasColumnName("Imdsc1_Description")
                    .HasMaxLength(255);

                entity.Property(e => e.ImglptConsumeGlClass)
                    .HasColumnName("Imglpt_Consume GL Class")
                    .HasMaxLength(255);

                entity.Property(e => e.Imprp2).HasMaxLength(255);

                entity.Property(e => e.Imprp3ProductGrup)
                    .HasColumnName("Imprp3Product Grup")
                    .HasMaxLength(255);

                entity.Property(e => e.Imsrp2CanCount)
                    .HasColumnName("Imsrp2_Can Count")
                    .HasMaxLength(255);

                entity.Property(e => e.Imsrp7).HasMaxLength(255);

                entity.Property(e => e.Imsrp9CanSize)
                    .HasColumnName("Imsrp9_Can Size")
                    .HasMaxLength(255);

                entity.Property(e => e.Irdsc1Routing)
                    .HasColumnName("Irdsc1_Routing")
                    .HasMaxLength(255);

                entity.Property(e => e.Irmcu).HasMaxLength(255);

                entity.Property(e => e.Irtrt).HasMaxLength(255);

                entity.Property(e => e.IxcmcuConsumePartBP)
                    .HasColumnName("Ixcmcu_Consume part b/p")
                    .HasMaxLength(255);

                entity.Property(e => e.Ixfser).HasMaxLength(255);

                entity.Property(e => e.Ixfvbt).HasMaxLength(255);

                entity.Property(e => e.IxkitlProduceItem)
                    .HasColumnName("Ixkitl_ProduceITem")
                    .HasMaxLength(255);

                entity.Property(e => e.IxlitmConsumePart)
                    .HasColumnName("Ixlitm_Consume Part")
                    .HasMaxLength(255);

                entity.Property(e => e.IxmmcuBP)
                    .HasColumnName("Ixmmcu_B/P")
                    .HasMaxLength(255);

                entity.Property(e => e.IxqntyUsagePlusSpoilage).HasColumnName("Ixqnty_Usage plus spoilage");

                entity.Property(e => e.IxsbntSubstituteIf0PrimaryOtherAlternate).HasColumnName("Ixsbnt_Substitute(If 0=Primary , Other = Alternate)");

                entity.Property(e => e.IxscrpSpoilage).HasColumnName("Ixscrp_%Spoilage");

                entity.Property(e => e.Ixtbm).HasMaxLength(255);

                entity.Property(e => e.IxumUom)
                    .HasColumnName("Ixum_UOM")
                    .HasMaxLength(255);

                entity.Property(e => e.IxuomProductUom)
                    .HasColumnName("Ixuom_ProductUOM_")
                    .HasMaxLength(255);

                entity.Property(e => e.Sysdate).HasColumnName("sysdate");

                entity.Property(e => e.TodayJdate).HasColumnName("today_jdate");
            });

            modelBuilder.Entity<BomDole>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BOM_Dole");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(128);

                entity.Property(e => e.F24).HasMaxLength(255);

                entity.Property(e => e.FruitGroup)
                    .HasColumnName("FRUIT GROUP")
                    .HasMaxLength(255);

                entity.Property(e => e.FruitType)
                    .HasColumnName("Fruit type")
                    .HasMaxLength(255);

                entity.Property(e => e.FruitTypeDescription)
                    .HasColumnName("Fruit type Description")
                    .HasMaxLength(255);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Imdsc1ConsumeDescription)
                    .HasColumnName("Imdsc1_Consume Description")
                    .HasMaxLength(255);

                entity.Property(e => e.Imdsc1Description)
                    .HasColumnName("Imdsc1_Description")
                    .HasMaxLength(255);

                entity.Property(e => e.ImglptConsumeGlClass)
                    .HasColumnName("Imglpt_Consume GL Class")
                    .HasMaxLength(255);

                entity.Property(e => e.Imprp2).HasMaxLength(255);

                entity.Property(e => e.Imprp3ProductGrup)
                    .HasColumnName("Imprp3Product Grup")
                    .HasMaxLength(255);

                entity.Property(e => e.Imsrp2CanCount)
                    .HasColumnName("Imsrp2_Can Count")
                    .HasMaxLength(255);

                entity.Property(e => e.Imsrp7).HasMaxLength(255);

                entity.Property(e => e.Imsrp9CanSize)
                    .HasColumnName("Imsrp9_Can Size")
                    .HasMaxLength(255);

                entity.Property(e => e.Irdsc1Routing)
                    .HasColumnName("Irdsc1_Routing")
                    .HasMaxLength(255);

                entity.Property(e => e.Irmcu).HasMaxLength(255);

                entity.Property(e => e.Irtrt).HasMaxLength(255);

                entity.Property(e => e.IxcmcuConsumePartBP)
                    .HasColumnName("Ixcmcu_Consume part b/p")
                    .HasMaxLength(255);

                entity.Property(e => e.Ixfser).HasMaxLength(255);

                entity.Property(e => e.Ixfvbt).HasMaxLength(255);

                entity.Property(e => e.IxkitlProduceItem)
                    .HasColumnName("Ixkitl_ProduceITem")
                    .HasMaxLength(255);

                entity.Property(e => e.IxlitmConsumePart)
                    .HasColumnName("Ixlitm_Consume Part")
                    .HasMaxLength(255);

                entity.Property(e => e.IxmmcuBP)
                    .HasColumnName("Ixmmcu_B/P")
                    .HasMaxLength(255);

                entity.Property(e => e.IxqntyUsagePlusSpoilage).HasColumnName("Ixqnty_Usage plus spoilage");

                entity.Property(e => e.IxsbntSubstituteIf0PrimaryOtherAlternate).HasColumnName("Ixsbnt_Substitute(If 0=Primary , Other = Alternate)");

                entity.Property(e => e.IxscrpSpoilage).HasColumnName("Ixscrp_%Spoilage");

                entity.Property(e => e.Ixtbm).HasMaxLength(255);

                entity.Property(e => e.IxumUom)
                    .HasColumnName("Ixum_UOM")
                    .HasMaxLength(255);

                entity.Property(e => e.IxuomProductUom)
                    .HasColumnName("Ixuom_ProductUOM_")
                    .HasMaxLength(255);

                entity.Property(e => e.Sysdate).HasColumnName("sysdate");

                entity.Property(e => e.TodayJdate).HasColumnName("today_jdate");
            });

            modelBuilder.Entity<Companies>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

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

            modelBuilder.Entity<Component>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.MachineId).HasColumnName("Machine_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TypeId).HasColumnName("Type_Id");

                entity.Property(e => e.UpdateBy).HasMaxLength(128);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Machine)
                    .WithMany(p => p.Component)
                    .HasForeignKey(d => d.MachineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Machine_Component_Machine");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Component)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Machine_Component_Machine_ComponentType");
            });

            modelBuilder.Entity<ComponentType>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.Image).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<ComponentTypeLossLevel3>(entity =>
            {
                entity.ToTable("ComponentType_LossLevel3");

                entity.Property(e => e.ComponentTypeId).HasColumnName("ComponentType_Id");

                entity.Property(e => e.LossLevel3Id).HasColumnName("LossLevel3_Id");

                entity.HasOne(d => d.ComponentType)
                    .WithMany(p => p.ComponentTypeLossLevel3)
                    .HasForeignKey(d => d.ComponentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MachineTypeComponent_LossLevel3_MachineTypeComponent");

                entity.HasOne(d => d.LossLevel3)
                    .WithMany(p => p.ComponentTypeLossLevel3)
                    .HasForeignKey(d => d.LossLevel3Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LossLevel3_MachineTypeComponent_LossLevel3");
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.Ddname).HasMaxLength(50);

                entity.Property(e => e.Ddsnam).HasMaxLength(20);

                entity.Property(e => e.Education).HasMaxLength(50);

                entity.Property(e => e.EmLevel).HasMaxLength(20);

                entity.Property(e => e.EmNo).HasMaxLength(50);

                entity.Property(e => e.Empar).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Emsp2).HasMaxLength(20);

                entity.Property(e => e.HireDate).HasColumnType("date");

                entity.Property(e => e.Image).HasMaxLength(200);

                entity.Property(e => e.Sex)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.TermDate).HasColumnType("date");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

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
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

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
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

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
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.LossLevel1Id).HasColumnName("LossLevel1_Id");

                entity.Property(e => e.Name).HasMaxLength(500);

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
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

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
                entity.Property(e => e.CounterInTag).HasMaxLength(100);

                entity.Property(e => e.CounterOutTag).HasMaxLength(100);

                entity.Property(e => e.CounterResetTag).HasMaxLength(100);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.MachineTypeId).HasColumnName("MachineType_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StatusId).HasColumnName("Status_Id");

                entity.Property(e => e.StatusTag).HasMaxLength(100);

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

            modelBuilder.Entity<MachineStatus>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MachineType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.HasOee).HasColumnName("HasOEE");

                entity.Property(e => e.Image).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<MachineTypeComponentType>(entity =>
            {
                entity.ToTable("MachineType_ComponentType");

                entity.Property(e => e.ComponentTypeId).HasColumnName("ComponentType_Id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.MachineTypeId).HasColumnName("MachineType_Id");
            });

            modelBuilder.Entity<MachineTypeLossLevel3>(entity =>
            {
                entity.ToTable("MachineType_LossLevel3");

                entity.Property(e => e.LossLevel3Id).HasColumnName("LossLevel3_Id");

                entity.Property(e => e.MachineTypeId).HasColumnName("MachineType_Id");

                entity.HasOne(d => d.LossLevel3)
                    .WithMany(p => p.MachineTypeLossLevel3)
                    .HasForeignKey(d => d.LossLevel3Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MachineType_LossLevel3_LossLevel3");

                entity.HasOne(d => d.MachineType)
                    .WithMany(p => p.MachineTypeLossLevel3)
                    .HasForeignKey(d => d.MachineTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MachineType_LossLevel3_MachineType");
            });

            modelBuilder.Entity<MachineTypeMaterial>(entity =>
            {
                entity.ToTable("MachineType_Material");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

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

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Details).HasMaxLength(2000);

                entity.Property(e => e.MaintenanceId).HasColumnName("Maintenance_Id");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.Property(e => e.UpdatedDate).HasColumnType("date");

                entity.HasOne(d => d.Maintenance)
                    .WithMany(p => p.MaintenanceActivity)
                    .HasForeignKey(d => d.MaintenanceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Maintenance_Activity_Maintenance_Plan");
            });

            modelBuilder.Entity<MaintenancePlan>(entity =>
            {
                entity.ToTable("Maintenance_Plan");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastDate).HasColumnType("date");

                entity.Property(e => e.MachineId).HasColumnName("Machine_Id");

                entity.Property(e => e.MaintenanceTeamId).HasColumnName("MaintenanceTeam_Id");

                entity.Property(e => e.NextDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.MaintenanceTeam)
                    .WithMany(p => p.MaintenancePlan)
                    .HasForeignKey(d => d.MaintenanceTeamId)
                    .HasConstraintName("FK_Maintenance_Plan_Maintenance_Team");
            });

            modelBuilder.Entity<MaintenanceTeam>(entity =>
            {
                entity.ToTable("Maintenance_Team");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<MaintenanceTeamMember>(entity =>
            {
                entity.ToTable("MaintenanceTeam_Member");

                entity.Property(e => e.AddBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.AddDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MaintenanceTeamId).HasColumnName("MaintenanceTeam_Id");

                entity.Property(e => e.Response).HasMaxLength(50);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.UpdatedDate).HasColumnType("date");
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.Property(e => e.BhtperUnit)
                    .HasColumnName("BHTPerUnit")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.Icsgroup)
                    .HasColumnName("ICSGroup")
                    .HasMaxLength(50);

                entity.Property(e => e.Image).HasMaxLength(200);

                entity.Property(e => e.MaterialGroup).HasMaxLength(50);

                entity.Property(e => e.MaterialTypeId).HasColumnName("MaterialType_Id");

                entity.Property(e => e.ProductCategory).HasMaxLength(50);

                entity.Property(e => e.Uom)
                    .HasColumnName("UOM")
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.MaterialType)
                    .WithMany(p => p.Material)
                    .HasForeignKey(d => d.MaterialTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Material_MaterialType");
            });

            modelBuilder.Entity<MaterialType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
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
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.Igweight)
                    .HasColumnName("IGWeight")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Image).HasMaxLength(200);

                entity.Property(e => e.NetWeight).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PackingMedium).HasMaxLength(50);

                entity.Property(e => e.Pmweight)
                    .HasColumnName("PMWeight")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductFamilyId).HasColumnName("ProductFamily_Id");

                entity.Property(e => e.ProductGroupId).HasColumnName("ProductGroup_Id");

                entity.Property(e => e.ProductTypeId).HasColumnName("ProductType_Id");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.Property(e => e.WeightPerUom)
                    .HasColumnName("WeightPerUOM")
                    .HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.ProductFamily)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.ProductFamilyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_ProductFamily");

                entity.HasOne(d => d.ProductGroup)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.ProductGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_ProductGroup");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.ProductTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_ProductType");
            });

            modelBuilder.Entity<ProductDetail>(entity =>
            {
                entity.HasNoKey();

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

            modelBuilder.Entity<ProductFamily>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<ProductGroup>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<ProductType>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
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
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.PlanFinish).HasColumnType("datetime");

                entity.Property(e => e.PlanStart).HasColumnType("datetime");

                entity.Property(e => e.ProductId)
                    .HasColumnName("Product_Id")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.RouteId).HasColumnName("Route_Id");

                entity.Property(e => e.StatusId)
                    .HasColumnName("Status_Id")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UnitId).HasColumnName("Unit_Id");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductionPlan)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Production_Plan_Product");

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.ProductionPlan)
                    .HasForeignKey(d => d.RouteId)
                    .HasConstraintName("FK_Production_Plan_Route");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ProductionPlan)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_Production_Plan_Production_Status");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.ProductionPlan)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK_Production_Plan_Units");
            });

            modelBuilder.Entity<ProductionStatus>(entity =>
            {
                entity.ToTable("Production_Status");

                entity.Property(e => e.Name).HasMaxLength(20);
            });

            modelBuilder.Entity<RecordActiveProductionPlan>(entity =>
            {
                entity.ToTable("Record_Active_ProductionPlan");

                entity.Property(e => e.CreatedBy).HasMaxLength(128);

                entity.Property(e => e.Finish).HasColumnType("datetime");

                entity.Property(e => e.OperatorSetId).HasColumnName("OperatorSet_Id");

                entity.Property(e => e.ProductionPlanPlanId)
                    .IsRequired()
                    .HasColumnName("ProductionPlan_PlanId")
                    .HasMaxLength(50);

                entity.Property(e => e.RouteId).HasColumnName("Route_Id");

                entity.Property(e => e.Start).HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("Status_Id");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.RecordActiveProductionPlan)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Record_Active_ProductionPlan_Users");

                entity.HasOne(d => d.ProductionPlanPlan)
                    .WithMany(p => p.RecordActiveProductionPlan)
                    .HasForeignKey(d => d.ProductionPlanPlanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_Active_ProductionPlan_Production_Plan");

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.RecordActiveProductionPlan)
                    .HasForeignKey(d => d.RouteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_Active_ProductionPlan_Route");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.RecordActiveProductionPlan)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_Record_Active_ProductionPlan_Production_Status");
            });

            modelBuilder.Entity<RecordMachineComponentLoss>(entity =>
            {
                entity.ToTable("Record_Machine_Component_Loss");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.EndAt).HasColumnType("datetime");

                entity.Property(e => e.EndBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Guid).HasMaxLength(128);

                entity.Property(e => e.IsAuto)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LossLevel3Id).HasColumnName("LossLevel3_Id");

                entity.Property(e => e.MachineComponentId).HasColumnName("Machine_Component_Id");

                entity.Property(e => e.ProductionPlanId)
                    .HasColumnName("Production_Plan_Id")
                    .HasMaxLength(50);

                entity.Property(e => e.RecordMachineComponentStatusId).HasColumnName("Record_Machine_Component_Status_Id");

                entity.Property(e => e.StartedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.RecordMachineComponentLossCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_Machine_Component_Loss_Users");

                entity.HasOne(d => d.LossLevel3)
                    .WithMany(p => p.RecordMachineComponentLoss)
                    .HasForeignKey(d => d.LossLevel3Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_Machine_Component_Loss_LossLevel3");

                entity.HasOne(d => d.MachineComponent)
                    .WithMany(p => p.RecordMachineComponentLoss)
                    .HasForeignKey(d => d.MachineComponentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_Machine_Component_Loss_Machine_Component");

                entity.HasOne(d => d.ProductionPlan)
                    .WithMany(p => p.RecordMachineComponentLoss)
                    .HasForeignKey(d => d.ProductionPlanId)
                    .HasConstraintName("FK_Record_Machine_Component_Loss_Production_Plan");

                entity.HasOne(d => d.RecordMachineComponentStatus)
                    .WithMany(p => p.RecordMachineComponentLoss)
                    .HasForeignKey(d => d.RecordMachineComponentStatusId)
                    .HasConstraintName("FK_Record_Machine_Component_Loss_Record_Machine_Component_Status");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.RecordMachineComponentLossUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Record_Machine_Component_Loss_Users1");
            });

            modelBuilder.Entity<RecordMachineStatus>(entity =>
            {
                entity.ToTable("Record_Machine_Status");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.MachineId).HasColumnName("Machine_Id");

                entity.Property(e => e.MachineStatusId).HasColumnName("MachineStatus_Id");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.RecordMachineStatus)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Record_Machine_Component_Status_Users");

                entity.HasOne(d => d.Machine)
                    .WithMany(p => p.RecordMachineStatus)
                    .HasForeignKey(d => d.MachineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_Machine_Status_Machine");

                entity.HasOne(d => d.MachineStatus)
                    .WithMany(p => p.RecordMachineStatus)
                    .HasForeignKey(d => d.MachineStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_Machine_Component_Status_MachineStatus");
            });

            modelBuilder.Entity<RecordManufacturingLoss>(entity =>
            {
                entity.ToTable("Record_Manufacturing_Loss");

                entity.Property(e => e.ComponentTypeId).HasColumnName("ComponentType_Id");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.EndAt).HasColumnType("datetime");

                entity.Property(e => e.EndBy).HasMaxLength(128);

                entity.Property(e => e.Guid).HasMaxLength(128);

                entity.Property(e => e.LossLevel3Id).HasColumnName("LossLevel3_Id");

                entity.Property(e => e.MachineId).HasColumnName("Machine_Id");

                entity.Property(e => e.ProductionPlanId)
                    .HasColumnName("Production_Plan_Id")
                    .HasMaxLength(50);

                entity.Property(e => e.StartedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.ComponentType)
                    .WithMany(p => p.RecordManufacturingLoss)
                    .HasForeignKey(d => d.ComponentTypeId)
                    .HasConstraintName("FK_Record_Manufacturing_Loss_Machine_ComponentType");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.RecordManufacturingLossCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_Manufacturing_Loss_Users");

                entity.HasOne(d => d.LossLevel3)
                    .WithMany(p => p.RecordManufacturingLoss)
                    .HasForeignKey(d => d.LossLevel3Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_Manufacturing_Loss_LossLevel3");

                entity.HasOne(d => d.Machine)
                    .WithMany(p => p.RecordManufacturingLoss)
                    .HasForeignKey(d => d.MachineId)
                    .HasConstraintName("FK_Record_Manufacturing_Loss_Machine");

                entity.HasOne(d => d.ProductionPlan)
                    .WithMany(p => p.RecordManufacturingLoss)
                    .HasForeignKey(d => d.ProductionPlanId)
                    .HasConstraintName("FK_Record_ProductionPLan_Loss_Production_Plan");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.RecordManufacturingLossUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Record_Manufacturing_Loss_Users1");
            });

            modelBuilder.Entity<RecordProductionPlanOutput>(entity =>
            {
                entity.ToTable("Record_ProductionPlan_Output");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.IsCounterOut).HasDefaultValueSql("((1))");

                entity.Property(e => e.MachineId).HasColumnName("Machine_Id");

                entity.Property(e => e.ProductionPlanId)
                    .IsRequired()
                    .HasColumnName("Production_Plan_Id")
                    .HasMaxLength(50);

                entity.Property(e => e.Remark).HasMaxLength(200);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.RecordProductionPlanOutputCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_Production_Output_Users2");

                entity.HasOne(d => d.ProductionPlan)
                    .WithMany(p => p.RecordProductionPlanOutput)
                    .HasForeignKey(d => d.ProductionPlanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_Production_Output_Production_Plan");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.RecordProductionPlanOutputUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Record_Production_Output_Users");
            });

            modelBuilder.Entity<RecordProductionPlanWaste>(entity =>
            {
                entity.ToTable("Record_ProductionPlan_Waste");

                entity.Property(e => e.CauseMachineId).HasColumnName("CauseMachine_Id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.ProductionPlanId)
                    .IsRequired()
                    .HasColumnName("Production_Plan_Id")
                    .HasMaxLength(50);

                entity.Property(e => e.Reason).HasMaxLength(4000);

                entity.Property(e => e.RecordManufacturingLossId).HasColumnName("Record_Manufacturing_Loss_Id");

                entity.Property(e => e.RouteId).HasColumnName("Route_Id");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.Property(e => e.WasteLevel2Id).HasColumnName("WasteLevel2_Id");

                entity.HasOne(d => d.RecordManufacturingLoss)
                    .WithMany(p => p.RecordProductionPlanWaste)
                    .HasForeignKey(d => d.RecordManufacturingLossId)
                    .HasConstraintName("FK_Record_ProductionPlan_Waste_Record_Manufacturing_Loss");
            });

            modelBuilder.Entity<RecordProductionPlanWasteMaterials>(entity =>
            {
                entity.ToTable("Record_ProductionPlan_Waste_Materials");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MaterialId).HasColumnName("Material_Id");

                entity.Property(e => e.WasteId).HasColumnName("Waste_Id");

                entity.HasOne(d => d.Waste)
                    .WithMany(p => p.RecordProductionPlanWasteMaterials)
                    .HasForeignKey(d => d.WasteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_ProductionPlan_Waste_Materials_Record_ProductionPlan_Waste");
            });

            modelBuilder.Entity<Route>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

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
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.MachineId).HasColumnName("Machine_Id");

                entity.Property(e => e.RouteId).HasColumnName("Route_Id");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

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
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ProductGroupId).HasColumnName("ProductGroup_Id");

                entity.Property(e => e.RouteId).HasColumnName("Route_Id");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

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
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

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

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

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
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

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

            modelBuilder.Entity<StandardCostBrite>(entity =>
            {
                entity.ToTable("StandardCost_Brite");

                entity.Property(e => e.AllBhtperUnit)
                    .HasColumnName("AllBHTPerUnit")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ContainerSize)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.FruitBhtperUnit)
                    .HasColumnName("FruitBHTPerUnit")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.PackSize)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PackingMediumBhtperUnit)
                    .HasColumnName("PackingMediumBHTPerUnit")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductTypeId).HasColumnName("ProductType_Id");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.StandardCostBrite)
                    .HasForeignKey(d => d.ProductTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StandardCost_Brite_ProductType");
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

            modelBuilder.Entity<Units>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Uom)
                    .HasColumnName("UOM")
                    .HasMaxLength(10);
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
                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

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

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

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

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

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

            modelBuilder.Entity<WasteLevel1>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.ProductTypeId).HasColumnName("ProductType_Id");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.WasteLevel1)
                    .HasForeignKey(d => d.ProductTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WasteLevel1_ProductType");
            });

            modelBuilder.Entity<WasteLevel2>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetSystemGUID]())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.Property(e => e.WasteLevel1Id).HasColumnName("WasteLevel1_Id");

                entity.HasOne(d => d.WasteLevel1)
                    .WithMany(p => p.WasteLevel2)
                    .HasForeignKey(d => d.WasteLevel1Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WasteLevel2_WasteLevel1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
