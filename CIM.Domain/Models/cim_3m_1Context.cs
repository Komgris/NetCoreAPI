using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CIM.Domain.Models
{
    public partial class cim_3m_1Context : DbContext
    {
        public cim_3m_1Context()
        {
        }

        public cim_3m_1Context(DbContextOptions<cim_3m_1Context> options)
            : base(options)
        {
        }

        public virtual DbSet<CheckListMachine> CheckListMachine { get; set; }
        public virtual DbSet<CheckListType> CheckListType { get; set; }
        public virtual DbSet<Color> Color { get; set; }
        public virtual DbSet<LossLevel1> LossLevel1 { get; set; }
        public virtual DbSet<LossLevel2> LossLevel2 { get; set; }
        public virtual DbSet<LossLevel3> LossLevel3 { get; set; }
        public virtual DbSet<Machine> Machine { get; set; }
        public virtual DbSet<MachineCode> MachineCode { get; set; }
        public virtual DbSet<MachineStatus> MachineStatus { get; set; }
        public virtual DbSet<MachineType> MachineType { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<MaterialType> MaterialType { get; set; }
        public virtual DbSet<Pm2> Pm2 { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductMatMaster> ProductMatMaster { get; set; }
        public virtual DbSet<ProductMaterial> ProductMaterial { get; set; }
        public virtual DbSet<ProductionPlan> ProductionPlan { get; set; }
        public virtual DbSet<ProductionPlanCheckList> ProductionPlanCheckList { get; set; }
        public virtual DbSet<ProductionStatus> ProductionStatus { get; set; }
        public virtual DbSet<RecordActiveProductionPlan> RecordActiveProductionPlan { get; set; }
        public virtual DbSet<RecordMachineStatus> RecordMachineStatus { get; set; }
        public virtual DbSet<RecordManufacturingLoss> RecordManufacturingLoss { get; set; }
        public virtual DbSet<RecordProductionPlanCheckList> RecordProductionPlanCheckList { get; set; }
        public virtual DbSet<RecordProductionPlanCheckListDetail> RecordProductionPlanCheckListDetail { get; set; }
        public virtual DbSet<RecordProductionPlanColorOrder> RecordProductionPlanColorOrder { get; set; }
        public virtual DbSet<RecordProductionPlanColorOrderDetail> RecordProductionPlanColorOrderDetail { get; set; }
        public virtual DbSet<RecordProductionPlanInformation> RecordProductionPlanInformation { get; set; }
        public virtual DbSet<RecordProductionPlanInformationDetail> RecordProductionPlanInformationDetail { get; set; }
        public virtual DbSet<RecordProductionPlanOutput> RecordProductionPlanOutput { get; set; }
        public virtual DbSet<RecordProductionPlanWaste> RecordProductionPlanWaste { get; set; }
        public virtual DbSet<RecordProductionPlanWasteMaterials> RecordProductionPlanWasteMaterials { get; set; }
        public virtual DbSet<Units> Units { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Waste> Waste { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=103.245.164.12;initial catalog=cim_3m_1;persist security info=True;user id=cim;password=4dev@cim;MultipleActiveResultSets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CheckListMachine>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CheckList_Machine");

                entity.Property(e => e.CheckListId).HasColumnName("CheckList_Id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.MachineId).HasColumnName("Machine_Id");
            });

            modelBuilder.Entity<CheckListType>(entity =>
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

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
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

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.LossLevel1Id).HasColumnName("LossLevel1_Id");

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<LossLevel3>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(10);

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
            });

            modelBuilder.Entity<Machine>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.CounterOutTag).HasMaxLength(100);

                entity.Property(e => e.CounterResetTag).HasMaxLength(100);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Image).HasMaxLength(50);

                entity.Property(e => e.MachineTypeId).HasColumnName("MachineType_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SpeedTag).HasMaxLength(100);

                entity.Property(e => e.StatusId).HasColumnName("Status_Id");

                entity.Property(e => e.StatusTag).HasMaxLength(100);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<MachineCode>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MachineId).HasColumnName("Machine_Id");

                entity.Property(e => e.StatusId).HasColumnName("Status_Id");
            });

            modelBuilder.Entity<MachineStatus>(entity =>
            {
                entity.ToTable("Machine_Status");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MachineType>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.HasOee).HasColumnName("HasOEE");

                entity.Property(e => e.Image).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.Property(e => e.BhtperUnit)
                    .HasColumnName("BHTPerUnit")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ColorId).HasColumnName("Color_Id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.Image).HasMaxLength(200);

                entity.Property(e => e.MaterialGroup).HasMaxLength(50);

                entity.Property(e => e.MaterialTypeId).HasColumnName("MaterialType_Id");

                entity.Property(e => e.Name).HasMaxLength(15);

                entity.Property(e => e.UnitsId).HasColumnName("Units_Id");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<MaterialType>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<Pm2>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("PM2$");

                entity.Property(e => e.Bmscp).HasColumnName("BMSCP");

                entity.Property(e => e.BomChild)
                    .HasColumnName("BOM Child")
                    .HasMaxLength(255);

                entity.Property(e => e.BomChildDescription)
                    .HasColumnName("BOM Child Description")
                    .HasMaxLength(255);

                entity.Property(e => e.BomChildMaterial)
                    .HasColumnName("BOM Child Material")
                    .HasMaxLength(255);

                entity.Property(e => e.Bqreq).HasColumnName("BQREQ");

                entity.Property(e => e.ColorLayer1)
                    .HasColumnName("Color Layer 1")
                    .HasMaxLength(255);

                entity.Property(e => e.ColorLayer2)
                    .HasColumnName("Color Layer 2")
                    .HasMaxLength(255);

                entity.Property(e => e.ColorLayer3)
                    .HasColumnName("Color Layer 3")
                    .HasMaxLength(255);

                entity.Property(e => e.ColorLayer4)
                    .HasColumnName("Color Layer 4")
                    .HasMaxLength(255);

                entity.Property(e => e.ColorLayer5)
                    .HasColumnName("Color Layer 5")
                    .HasMaxLength(255);

                entity.Property(e => e.ColorLayer6)
                    .HasColumnName("Color Layer 6")
                    .HasMaxLength(255);

                entity.Property(e => e.Iump)
                    .HasColumnName("IUMP")
                    .HasMaxLength(255);

                entity.Property(e => e.Iums)
                    .HasColumnName("IUMS")
                    .HasMaxLength(255);

                entity.Property(e => e.Iums01)
                    .HasColumnName("IUMS01")
                    .HasMaxLength(255);

                entity.Property(e => e.Product).HasMaxLength(255);

                entity.Property(e => e.ProductDescription)
                    .HasColumnName("Product Description")
                    .HasMaxLength(255);

                entity.Property(e => e.WorkcenterDescription)
                    .HasColumnName("Workcenter Description")
                    .HasMaxLength(255);

                entity.Property(e => e.WorkcenterNo).HasColumnName("Workcenter No#");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Image).HasMaxLength(200);

                entity.Property(e => e.UnitId).HasColumnName("Unit_Id");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<ProductMatMaster>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Bmscp).HasColumnName("BMSCP");

                entity.Property(e => e.BomChild)
                    .HasColumnName("BOM Child")
                    .HasMaxLength(255);

                entity.Property(e => e.BomChildDescription)
                    .HasColumnName("BOM Child Description")
                    .HasMaxLength(255);

                entity.Property(e => e.BomChildMaterial)
                    .HasColumnName("BOM Child Material")
                    .HasMaxLength(255);

                entity.Property(e => e.Bqreq).HasColumnName("BQREQ");

                entity.Property(e => e.ColorLayer1)
                    .HasColumnName("Color Layer 1")
                    .HasMaxLength(255);

                entity.Property(e => e.ColorLayer2)
                    .HasColumnName("Color Layer 2")
                    .HasMaxLength(255);

                entity.Property(e => e.ColorLayer3)
                    .HasColumnName("Color Layer 3")
                    .HasMaxLength(255);

                entity.Property(e => e.ColorLayer4)
                    .HasColumnName("Color Layer 4")
                    .HasMaxLength(255);

                entity.Property(e => e.ColorLayer5)
                    .HasColumnName("Color Layer 5")
                    .HasMaxLength(255);

                entity.Property(e => e.ColorLayer6)
                    .HasColumnName("Color Layer 6")
                    .HasMaxLength(255);

                entity.Property(e => e.Iump)
                    .HasColumnName("IUMP")
                    .HasMaxLength(255);

                entity.Property(e => e.Iums)
                    .HasColumnName("IUMS")
                    .HasMaxLength(255);

                entity.Property(e => e.Iums01)
                    .HasColumnName("IUMS01")
                    .HasMaxLength(255);

                entity.Property(e => e.Product).HasMaxLength(255);

                entity.Property(e => e.ProductDescription)
                    .HasColumnName("Product Description")
                    .HasMaxLength(255);

                entity.Property(e => e.WorkcenterDescription)
                    .HasColumnName("Workcenter Description")
                    .HasMaxLength(255);

                entity.Property(e => e.WorkcenterNo).HasColumnName("Workcenter No#");
            });

            modelBuilder.Entity<ProductMaterial>(entity =>
            {
                entity.ToTable("Product_Material");

                entity.Property(e => e.Bqreq).HasColumnName("BQREQ");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.MaterialId).HasColumnName("Material_Id");

                entity.Property(e => e.ProductId).HasColumnName("Product_Id");

                entity.Property(e => e.UnitsId).HasColumnName("Units_Id");
            });

            modelBuilder.Entity<ProductionPlan>(entity =>
            {
                entity.HasKey(e => e.PlanId);

                entity.Property(e => e.PlanId).HasMaxLength(50);

                entity.Property(e => e.ActualFinish).HasColumnType("datetime");

                entity.Property(e => e.ActualStart).HasColumnType("datetime");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.MachineId).HasColumnName("Machine_Id");

                entity.Property(e => e.PlanFinish).HasColumnType("datetime");

                entity.Property(e => e.PlanStart).HasColumnType("datetime");

                entity.Property(e => e.ProductId)
                    .HasColumnName("Product_Id")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Sequence).HasColumnType("decimal(10, 3)");

                entity.Property(e => e.ShopNo)
                    .HasColumnName("Shop_No")
                    .HasMaxLength(128);

                entity.Property(e => e.StatusId)
                    .HasColumnName("Status_Id")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UnitId).HasColumnName("Unit_Id");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<ProductionPlanCheckList>(entity =>
            {
                entity.ToTable("ProductionPlan_CheckList");

                entity.Property(e => e.CheckListTypeId).HasColumnName("CheckListType_Id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.MachinTypeId).HasColumnName("MachinType_Id");

                entity.Property(e => e.Remark).HasMaxLength(200);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.CheckListType)
                    .WithMany(p => p.ProductionPlanCheckList)
                    .HasForeignKey(d => d.CheckListTypeId)
                    .HasConstraintName("FK_ProductionPlan_CheckList_CheckListType");
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

                entity.Property(e => e.EstimateFinish).HasColumnType("datetime");

                entity.Property(e => e.Finish).HasColumnType("datetime");

                entity.Property(e => e.OperatorSetId).HasColumnName("OperatorSet_Id");

                entity.Property(e => e.ProductionDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("([dbo].[fn_get_date_production](DEFAULT))");

                entity.Property(e => e.ProductionPlanPlanId)
                    .IsRequired()
                    .HasColumnName("ProductionPlan_PlanId")
                    .HasMaxLength(50);

                entity.Property(e => e.Start)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.StatusId).HasColumnName("Status_Id");
            });

            modelBuilder.Entity<RecordMachineStatus>(entity =>
            {
                entity.ToTable("Record_Machine_Status");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EndAt).HasColumnType("datetime");

                entity.Property(e => e.Hour).HasDefaultValueSql("([dbo].[fn_get_hr24number](DEFAULT))");

                entity.Property(e => e.MachineId).HasColumnName("Machine_Id");

                entity.Property(e => e.MachineStatusId).HasColumnName("MachineStatus_Id");

                entity.Property(e => e.Month).HasDefaultValueSql("([dbo].[fn_get_monthnumber](DEFAULT))");

                entity.Property(e => e.ProductionPlanId)
                    .HasColumnName("Production_Plan_Id")
                    .HasMaxLength(50);

                entity.Property(e => e.WeekNumber).HasDefaultValueSql("([dbo].[fn_get_weeknumber](DEFAULT))");

                entity.Property(e => e.Year).HasDefaultValueSql("([dbo].[fn_get_yearnumber](DEFAULT))");
            });

            modelBuilder.Entity<RecordManufacturingLoss>(entity =>
            {
                entity.ToTable("Record_Manufacturing_Loss");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.EndAt).HasColumnType("datetime");

                entity.Property(e => e.EndBy).HasMaxLength(128);

                entity.Property(e => e.Guid).HasMaxLength(128);

                entity.Property(e => e.LossLevel3Id).HasColumnName("LossLevel3_Id");

                entity.Property(e => e.MachineId).HasColumnName("Machine_Id");

                entity.Property(e => e.Month).HasDefaultValueSql("([dbo].[fn_get_monthnumber](DEFAULT))");

                entity.Property(e => e.ProductionPlanId)
                    .IsRequired()
                    .HasColumnName("Production_Plan_Id")
                    .HasMaxLength(50);

                entity.Property(e => e.Remark).HasMaxLength(200);

                entity.Property(e => e.StartedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.Property(e => e.WeekNumber).HasDefaultValueSql("([dbo].[fn_get_weeknumber](DEFAULT))");

                entity.Property(e => e.Year).HasDefaultValueSql("([dbo].[fn_get_yearnumber](DEFAULT))");
            });

            modelBuilder.Entity<RecordProductionPlanCheckList>(entity =>
            {
                entity.ToTable("Record_ProductionPlan_CheckList");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Date).HasDefaultValueSql("([dbo].[fn_get_datenumber](DEFAULT))");

                entity.Property(e => e.Hour).HasDefaultValueSql("([dbo].[fn_get_hr24number](DEFAULT))");

                entity.Property(e => e.Month).HasDefaultValueSql("([dbo].[fn_get_monthnumber](DEFAULT))");

                entity.Property(e => e.ProductionPlanId)
                    .IsRequired()
                    .HasColumnName("ProductionPlan_Id")
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.Property(e => e.WeekNumber).HasDefaultValueSql("([dbo].[fn_get_weeknumber](DEFAULT))");

                entity.Property(e => e.Year).HasDefaultValueSql("([dbo].[fn_get_yearnumber](DEFAULT))");
            });

            modelBuilder.Entity<RecordProductionPlanCheckListDetail>(entity =>
            {
                entity.ToTable("Record_ProductionPlan_CheckList_Detail");

                entity.Property(e => e.CheckListId).HasColumnName("CheckList_Id");

                entity.Property(e => e.CheckListTypeId).HasColumnName("CheckListType_Id");

                entity.Property(e => e.ExampleNumber).HasColumnName("Example_Number");

                entity.Property(e => e.RecordCheckListId).HasColumnName("Record_CheckList_Id");

                entity.Property(e => e.Remark).HasMaxLength(200);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.CheckList)
                    .WithMany(p => p.RecordProductionPlanCheckListDetail)
                    .HasForeignKey(d => d.CheckListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_ProductionPlan_CheckList_Detail_ProductionPlan_CheckList");

                entity.HasOne(d => d.CheckListType)
                    .WithMany(p => p.RecordProductionPlanCheckListDetail)
                    .HasForeignKey(d => d.CheckListTypeId)
                    .HasConstraintName("FK_Record_ProductionPlan_CheckList_Detail_CheckListType");

                entity.HasOne(d => d.RecordCheckList)
                    .WithMany(p => p.RecordProductionPlanCheckListDetail)
                    .HasForeignKey(d => d.RecordCheckListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_ProductionPlan_CheckList_Detail_Record_ProductionPlan_CheckList");
            });

            modelBuilder.Entity<RecordProductionPlanColorOrder>(entity =>
            {
                entity.ToTable("Record_ProductionPlan_Color_Order");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Date).HasDefaultValueSql("([dbo].[fn_get_datenumber](DEFAULT))");

                entity.Property(e => e.Hour).HasDefaultValueSql("([dbo].[fn_get_hr24number](DEFAULT))");

                entity.Property(e => e.Month).HasDefaultValueSql("([dbo].[fn_get_monthnumber](DEFAULT))");

                entity.Property(e => e.ProductId).HasColumnName("Product_Id");

                entity.Property(e => e.ProductionPlanId)
                    .IsRequired()
                    .HasColumnName("ProductionPlan_Id")
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.Property(e => e.WeekNumber).HasDefaultValueSql("([dbo].[fn_get_weeknumber](DEFAULT))");

                entity.Property(e => e.Year).HasDefaultValueSql("([dbo].[fn_get_yearnumber](DEFAULT))");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.RecordProductionPlanColorOrder)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_ProductionPlan_Color_Order_Product");

                entity.HasOne(d => d.ProductionPlan)
                    .WithMany(p => p.RecordProductionPlanColorOrder)
                    .HasForeignKey(d => d.ProductionPlanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_ProductionPlan_Color_Order_ProductionPlan");
            });

            modelBuilder.Entity<RecordProductionPlanColorOrderDetail>(entity =>
            {
                entity.ToTable("Record_ProductionPlan_Color_Order_Detail");

                entity.Property(e => e.ColorId).HasColumnName("Color_Id");

                entity.Property(e => e.RecordColorId).HasColumnName("Record_Color_Id");

                entity.Property(e => e.Remark).HasMaxLength(200);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.RecordProductionPlanColorOrderDetail)
                    .HasForeignKey(d => d.ColorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_ProductionPlan_Color_Order_Detail_Color");

                entity.HasOne(d => d.RecordColor)
                    .WithMany(p => p.RecordProductionPlanColorOrderDetail)
                    .HasForeignKey(d => d.RecordColorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_ProductionPlan_Color_Order_Detail_Record_ProductionPlan_Color_Order");
            });

            modelBuilder.Entity<RecordProductionPlanInformation>(entity =>
            {
                entity.ToTable("Record_ProductionPlan_Information");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Date).HasDefaultValueSql("([dbo].[fn_get_datenumber](DEFAULT))");

                entity.Property(e => e.Hour).HasDefaultValueSql("([dbo].[fn_get_hr24number](DEFAULT))");

                entity.Property(e => e.Month).HasDefaultValueSql("([dbo].[fn_get_monthnumber](DEFAULT))");

                entity.Property(e => e.ProductId).HasColumnName("Product_Id");

                entity.Property(e => e.ProductionPlanId)
                    .IsRequired()
                    .HasColumnName("ProductionPlan_Id")
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.Property(e => e.WeekNumber).HasDefaultValueSql("([dbo].[fn_get_weeknumber](DEFAULT))");

                entity.Property(e => e.Year).HasDefaultValueSql("([dbo].[fn_get_yearnumber](DEFAULT))");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.RecordProductionPlanInformation)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_ProductionPlan_Information_Product");

                entity.HasOne(d => d.ProductionPlan)
                    .WithMany(p => p.RecordProductionPlanInformation)
                    .HasForeignKey(d => d.ProductionPlanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_ProductionPlan_Information_ProductionPlan");
            });

            modelBuilder.Entity<RecordProductionPlanInformationDetail>(entity =>
            {
                entity.ToTable("Record_ProductionPlan_Information_Detail");

                entity.Property(e => e.LotNo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MaterialId).HasColumnName("Material_Id");

                entity.Property(e => e.RecordInformationId).HasColumnName("Record_Information_Id");

                entity.Property(e => e.Remark).HasMaxLength(200);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.HasOne(d => d.Material)
                    .WithMany(p => p.RecordProductionPlanInformationDetail)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_ProductionPlan_Information_Detail_Material");

                entity.HasOne(d => d.RecordInformation)
                    .WithMany(p => p.RecordProductionPlanInformationDetail)
                    .HasForeignKey(d => d.RecordInformationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_ProductionPlan_Information_Detail_Record_ProductionPlan_Information");
            });

            modelBuilder.Entity<RecordProductionPlanOutput>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Record_ProductionPlan_Output");

                entity.Property(e => e.AdditionalCounterOut).HasDefaultValueSql("((0))");

                entity.Property(e => e.CounterIn).HasDefaultValueSql("((0))");

                entity.Property(e => e.CounterOut).HasDefaultValueSql("((0))");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy).HasMaxLength(128);

                entity.Property(e => e.Date).HasDefaultValueSql("([dbo].[fn_get_datenumber](DEFAULT))");

                entity.Property(e => e.FactorDivide).HasDefaultValueSql("((1))");

                entity.Property(e => e.FactorMultiply).HasDefaultValueSql("((1))");

                entity.Property(e => e.Hour).HasDefaultValueSql("([dbo].[fn_get_hr24number](DEFAULT))");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.MachineId).HasColumnName("Machine_Id");

                entity.Property(e => e.Month).HasDefaultValueSql("([dbo].[fn_get_monthnumber](DEFAULT))");

                entity.Property(e => e.ProductionPlanId)
                    .IsRequired()
                    .HasColumnName("Production_Plan_Id")
                    .HasMaxLength(50);

                entity.Property(e => e.Remark).HasMaxLength(200);

                entity.Property(e => e.TotalIn).HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalOut).HasDefaultValueSql("((0))");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.Property(e => e.WeekNumber).HasDefaultValueSql("([dbo].[fn_get_weeknumber](DEFAULT))");

                entity.Property(e => e.Year).HasDefaultValueSql("([dbo].[fn_get_yearnumber](DEFAULT))");
            });

            modelBuilder.Entity<RecordProductionPlanWaste>(entity =>
            {
                entity.ToTable("Record_ProductionPlan_Waste");

                entity.Property(e => e.AmountUnit).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.CauseMachineId).HasColumnName("CauseMachine_Id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Date).HasDefaultValueSql("([dbo].[fn_get_datenumber](DEFAULT))");

                entity.Property(e => e.Hour).HasDefaultValueSql("([dbo].[fn_get_hr24number](DEFAULT))");

                entity.Property(e => e.Month).HasDefaultValueSql("([dbo].[fn_get_monthnumber](DEFAULT))");

                entity.Property(e => e.ProductionPlanId)
                    .IsRequired()
                    .HasColumnName("Production_Plan_Id")
                    .HasMaxLength(50);

                entity.Property(e => e.Reason).HasMaxLength(4000);

                entity.Property(e => e.RecordManufacturingLossId).HasColumnName("Record_Manufacturing_Loss_Id");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);

                entity.Property(e => e.WasteId).HasColumnName("Waste_Id");

                entity.Property(e => e.WeekNumber).HasDefaultValueSql("([dbo].[fn_get_weeknumber](DEFAULT))");

                entity.Property(e => e.Year).HasDefaultValueSql("([dbo].[fn_get_yearnumber](DEFAULT))");
            });

            modelBuilder.Entity<RecordProductionPlanWasteMaterials>(entity =>
            {
                entity.ToTable("Record_ProductionPlan_Waste_Materials");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Cost)
                    .HasColumnType("decimal(18, 3)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MaterialId).HasColumnName("Material_Id");

                entity.Property(e => e.WasteId).HasColumnName("Waste_Id");

                entity.HasOne(d => d.Waste)
                    .WithMany(p => p.RecordProductionPlanWasteMaterials)
                    .HasForeignKey(d => d.WasteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_ProductionPlan_Waste_Material_Record_ProductionPlan_Waste");
            });

            modelBuilder.Entity<Units>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).HasMaxLength(10);
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
                    .HasMaxLength(128);

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
            });

            modelBuilder.Entity<Waste>(entity =>
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

                entity.Property(e => e.MachineId).HasColumnName("Machine_Id");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
