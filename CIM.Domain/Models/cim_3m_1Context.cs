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

        public virtual DbSet<CheckListType> CheckListType { get; set; }
        public virtual DbSet<LossLevel1> LossLevel1 { get; set; }
        public virtual DbSet<LossLevel2> LossLevel2 { get; set; }
        public virtual DbSet<LossLevel3> LossLevel3 { get; set; }
        public virtual DbSet<Machine> Machine { get; set; }
        public virtual DbSet<MachineStatus> MachineStatus { get; set; }
        public virtual DbSet<MachineType> MachineType { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductMaterial> ProductMaterial { get; set; }
        public virtual DbSet<ProductionPlan> ProductionPlan { get; set; }
        public virtual DbSet<ProductionPlanCheckList> ProductionPlanCheckList { get; set; }
        public virtual DbSet<ProductionStatus> ProductionStatus { get; set; }
        public virtual DbSet<RecordActiveProductionPlan> RecordActiveProductionPlan { get; set; }
        public virtual DbSet<RecordMachineStatus> RecordMachineStatus { get; set; }
        public virtual DbSet<RecordManufacturingLoss> RecordManufacturingLoss { get; set; }
        public virtual DbSet<RecordProductionPlanCheckList> RecordProductionPlanCheckList { get; set; }
        public virtual DbSet<RecordProductionPlanCheckListDetail> RecordProductionPlanCheckListDetail { get; set; }
        public virtual DbSet<RecordProductionPlanOutput> RecordProductionPlanOutput { get; set; }
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

                entity.Property(e => e.CounterInTag).HasMaxLength(100);

                entity.Property(e => e.CounterOutTag).HasMaxLength(100);

                entity.Property(e => e.CounterResetTag).HasMaxLength(100);

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

                entity.Property(e => e.StatusTag).HasMaxLength(100);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            modelBuilder.Entity<MachineStatus>(entity =>
            {
                entity.ToTable("Machine_Status");

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

            modelBuilder.Entity<ProductMaterial>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Product_Material");

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

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Date).HasDefaultValueSql("([dbo].[fn_get_datenumber](DEFAULT))");

                entity.Property(e => e.FactorDivide).HasDefaultValueSql("((1))");

                entity.Property(e => e.FactorMultiply).HasDefaultValueSql("((1))");

                entity.Property(e => e.Hour).HasDefaultValueSql("([dbo].[fn_get_hr24number](DEFAULT))");

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

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(128);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
