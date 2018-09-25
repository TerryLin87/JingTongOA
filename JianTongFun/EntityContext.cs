using JianTongBLL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianTongFun
{
    public class EntityContext : DbContext
    {
        public EntityContext() : base("EntityContext") { }

        public DbSet<JianTongUserClass> JianTongUserClass { get; set; }

        public DbSet<Company> Company { get; set; }

        public DbSet<Department> Department { get; set; }

        public DbSet<Position> Position { get; set; }

        public DbSet<WorkInfoClass> WorkInfoClass { get; set; }

        public DbSet<WorkMemberClass> WorkMemberClass { get; set; }

        public DbSet<WorkProfessionReview> WorkProfessionReview { get; set; }

        public DbSet<WorkProjectReview> WorkProjectReview { get; set; }

        public DbSet<WorkFile> WorkFile { get; set; }

        public DbSet<WorkFileType> WorkFileType { get; set; }

        public DbSet<DevelopUnit> DevelopUnit { get; set; }

        public DbSet<ConstructionUnit> ConstructionUnit { get; set; }

        public DbSet<InvestmentType> InvestmentType { get; set; }

        public DbSet<ProductType> ProductType { get; set; }

        public DbSet<ConsultationType> ConsultationType { get; set; }

        public DbSet<ProductSource> ProductSource { get; set; }

        public DbSet<ValuationType> ValuationType { get; set; }

        public DbSet<workFlowHistory> workFlowHistory { get; set; }

        public DbSet<WorkFileInfoList> WorkFileInfoList { get; set; }

        public DbSet<WorkAndWorkFileInfo> WorkAndWorkFileInfo { get; set; }

        public DbSet<UserRole> UserRole { get; set; }

        public DbSet<RoleJurisdiction> RoleJurisdiction { get; set; }

        public DbSet<WorkIndex> WorkIndex { get; set; }

        public DbSet<WorkOtherIndex> WorkOtherIndex { get; set; }

        public DbSet<NormalConfig> NormalConfig { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();//移除复数表名的契约
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();//移除对MetaData表的查询验证，要不然每次都要访问EdmMetadata这个表

            //throw new UnintentionalCodeFirstException();
        }

        public DbSet<T> GetObjectByT<T>() where T : IntIdClass
        {
            Type tType = typeof(T);
            switch (tType.Name)
            {
                case "JianTongUserClass":
                    return JianTongUserClass as DbSet<T>;
                case "Company":
                    return Company as DbSet<T>;
                case "Department":
                    return Department as DbSet<T>;
                case "Position":
                    return Position as DbSet<T>;
                case "DevelopUnit":
                    return DevelopUnit as DbSet<T>;
                case "ConstructionUnit":
                    return ConstructionUnit as DbSet<T>;
                case "InvestmentType":
                    return InvestmentType as DbSet<T>;
                case "ProductType":
                    return ProductType as DbSet<T>;
                case "ConsultationType":
                    return ConsultationType as DbSet<T>;
                case "ProductSource":
                    return ProductSource as DbSet<T>;
                case "ValuationType":
                    return ValuationType as DbSet<T>;
                case "workFlowHistory":
                    return workFlowHistory as DbSet<T>;
                case "WorkFileInfoList":
                    return WorkFileInfoList as DbSet<T>;
                case "WorkAndWorkFileInfo":
                    return WorkAndWorkFileInfo as DbSet<T>;
                case "UserRole":
                    return UserRole as DbSet<T>;
                case "RoleJurisdiction":
                    return RoleJurisdiction as DbSet<T>;
                case "WorkIndex":
                    return WorkIndex as DbSet<T>;
                case "WorkOtherIndex":
                    return WorkOtherIndex as DbSet<T>;
                case "NormalConfig":
                    return NormalConfig as DbSet<T>;
            }
            return null;
        }

        public DbSet<T> GetObjectByTStringId<T>() where T : StringIdClass
        {
            Type tType = typeof(T);
            switch (tType.Name)
            {
                case "WorkInfoClass":
                    return WorkInfoClass as DbSet<T>;
                case "WorkMemberClass":
                    return WorkMemberClass as DbSet<T>;
                case "WorkProfessionReview":
                    return WorkProfessionReview as DbSet<T>;
                case "WorkProjectReview":
                    return WorkProjectReview as DbSet<T>;
                case "WorkFile":
                    return WorkFile as DbSet<T>;
                case "WorkFileType":
                    return WorkFileType as DbSet<T>;
            }
            return null;
        }

        public void AddTByIntId<T>(T t) where T : IntIdClass
        {
            Type tType = typeof(T);
            switch (tType.Name)
            {
                case "JianTongUserClass":
                    JianTongUserClass.Add(t as JianTongUserClass);
                    break;
                case "Company":
                    Company.Add(t as Company);
                    break;
                case "Department":
                    Department.Add(t as Department);
                    break;
                case "Position":
                    Position.Add(t as Position);
                    break;
                case "DevelopUnit":
                    DevelopUnit.Add(t as DevelopUnit);
                    break;
                case "ConstructionUnit":
                    ConstructionUnit.Add(t as ConstructionUnit);
                    break;
                case "InvestmentType":
                    InvestmentType.Add(t as InvestmentType);
                    break;
                case "ProductType":
                    ProductType.Add(t as ProductType);
                    break;
                case "ConsultationType":
                    ConsultationType.Add(t as ConsultationType);
                    break;
                case "ProductSource":
                    ProductSource.Add(t as ProductSource);
                    break;
                case "ValuationType":
                    ValuationType.Add(t as ValuationType);
                    break;
                case "workFlowHistory":
                    workFlowHistory.Add(t as workFlowHistory);
                    break;
                case "WorkFileInfoList":
                    WorkFileInfoList.Add(t as WorkFileInfoList);
                    break;
                case "WorkAndWorkFileInfo":
                    WorkAndWorkFileInfo.Add(t as WorkAndWorkFileInfo);
                    break;
                case "UserRole":
                    UserRole.Add(t as UserRole);
                    break;
                case "RoleJurisdiction":
                    RoleJurisdiction.Add(t as RoleJurisdiction);
                    break;
                case "WorkIndex":
                    WorkIndex.Add(t as WorkIndex);
                    break;
                case "WorkOtherIndex":
                    WorkOtherIndex.Add(t as WorkOtherIndex);
                    break;
                case "NormalConfig":
                    NormalConfig.Add(t as NormalConfig);
                    break;
            }
        }
    }
}
