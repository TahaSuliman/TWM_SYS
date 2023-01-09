namespace Curly_TWM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ndb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branchs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchsName = c.String(),
                        Section_id = c.Int(nullable: false),
                        empid = c.Int(nullable: false),
                        remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sections", t => t.Section_id, cascadeDelete: true)
                .Index(t => t.Section_id);
            
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SectiontName = c.String(),
                        Department_id = c.Int(nullable: false),
                        empid = c.Int(nullable: false),
                        remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.Department_id, cascadeDelete: true)
                .ForeignKey("dbo.emp_main", t => t.empid, cascadeDelete: false)
                .Index(t => t.Department_id)
                .Index(t => t.empid);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(),
                        MainDepartment_id = c.Int(nullable: false),
                        empid = c.Int(nullable: false),
                        remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.emp_main", t => t.empid, cascadeDelete: true)
                .ForeignKey("dbo.MainDepartments", t => t.MainDepartment_id, cascadeDelete: false)
                .Index(t => t.MainDepartment_id)
                .Index(t => t.empid);
            
            CreateTable(
                "dbo.emp_main",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        emp_arnm = c.String(),
                        emp_ennm = c.String(),
                        nat = c.String(),
                        Job_id = c.Int(),
                        emp_code = c.Int(nullable: false),
                        job_discription = c.String(),
                        Branch_id = c.Int(),
                        gender_Id = c.String(),
                        emp_mobile = c.String(),
                        emp_mobile2 = c.String(),
                        address = c.String(),
                        emp_class = c.String(),
                        remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branchs", t => t.Branch_id)
                .ForeignKey("dbo.Jobs", t => t.Job_id)
                .Index(t => t.Job_id)
                .Index(t => t.Branch_id);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JobName = c.String(),
                        remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MainDepartments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MainDepartmentName = c.String(),
                        empid = c.Int(nullable: false),
                        remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.emp_main", t => t.empid, cascadeDelete: true)
                .Index(t => t.empid);
            
            CreateTable(
                "dbo.countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        code = c.String(),
                        name = c.String(),
                        ar_name = c.String(),
                        en_name = c.String(),
                        calling_code = c.String(),
                        remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Initiatives",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InitiativeName = c.String(),
                        InitiativeDeatails = c.String(),
                        Initiative_Sdt = c.DateTime(nullable: false),
                        Initiative_Edt = c.DateTime(nullable: false),
                        Initiative_Entry = c.DateTime(nullable: false),
                        Targetid = c.Int(nullable: false),
                        empid = c.Int(nullable: false),
                        InitiativeStat = c.String(),
                        remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.emp_main", t => t.empid, cascadeDelete: true)
                .ForeignKey("dbo.Strategic_Targets", t => t.Targetid, cascadeDelete: false)
                .Index(t => t.Targetid)
                .Index(t => t.empid);
            
            CreateTable(
                "dbo.Strategic_Targets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TargetName = c.String(),
                        planid = c.Int(nullable: false),
                        remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Strategic_Plans", t => t.planid, cascadeDelete: true)
                .Index(t => t.planid);
            
            CreateTable(
                "dbo.Strategic_Plans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlanName = c.String(),
                        Plan_Sdt = c.DateTime(nullable: false),
                        Plan_Edt = c.DateTime(nullable: false),
                        Plan_Title = c.String(),
                        remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InitiativesTasksUploas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Initiativeid = c.Int(nullable: false),
                        TeamTasksid = c.Int(nullable: false),
                        docu_type = c.String(),
                        doc_url = c.String(),
                        doc_discription = c.String(),
                        remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Initiatives", t => t.Initiativeid, cascadeDelete: true)
                .ForeignKey("dbo.Team_Tasks", t => t.TeamTasksid, cascadeDelete: false)
                .Index(t => t.Initiativeid)
                .Index(t => t.TeamTasksid);
            
            CreateTable(
                "dbo.Team_Tasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TaskName = c.String(),
                        TaskDeatails = c.String(),
                        Task_Edt = c.DateTime(nullable: false),
                        Task_Ractdt = c.DateTime(nullable: false),
                        Initiative_Entry = c.DateTime(nullable: false),
                        Initiativeid = c.Int(nullable: false),
                        empid = c.Int(nullable: false),
                        TaskStat = c.String(),
                        remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Initiatives", t => t.Initiativeid, cascadeDelete: true)
                .ForeignKey("dbo.Teams", t => t.empid, cascadeDelete: false)
                .Index(t => t.Initiativeid)
                .Index(t => t.empid);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Initiativeid = c.Int(nullable: false),
                        empid = c.Int(nullable: false),
                        remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.emp_main", t => t.empid, cascadeDelete: true)
                .ForeignKey("dbo.Initiatives", t => t.Initiativeid, cascadeDelete: false)
                .Index(t => t.Initiativeid)
                .Index(t => t.empid);
            
            CreateTable(
                "dbo.InitiativesUploas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Initiativeid = c.Int(nullable: false),
                        docu_type = c.String(),
                        doc_url = c.String(),
                        doc_discription = c.String(),
                        remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Initiatives", t => t.Initiativeid, cascadeDelete: true)
                .Index(t => t.Initiativeid);
            
            CreateTable(
                "dbo.OperationalIndicators",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Initiativeid = c.Int(nullable: false),
                        Indicator = c.String(),
                        Indicator_Target = c.String(),
                        Indicator_Result = c.String(),
                        remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Initiatives", t => t.Initiativeid, cascadeDelete: true)
                .Index(t => t.Initiativeid);
            
            CreateTable(
                "dbo.Partners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Initiativeid = c.Int(nullable: false),
                        Partner = c.String(),
                        Partner_rol = c.String(),
                        Partner_add = c.String(),
                        remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Initiatives", t => t.Initiativeid, cascadeDelete: true)
                .Index(t => t.Initiativeid);
            
            CreateTable(
                "dbo.RiskLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Initiativeid = c.Int(nullable: false),
                        Risk = c.String(),
                        Risk_Type = c.String(),
                        Risk_class = c.String(),
                        Risk_Plan = c.String(),
                        remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Initiatives", t => t.Initiativeid, cascadeDelete: true)
                .Index(t => t.Initiativeid);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        RoleOwner = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.uploaddocs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        empid = c.Int(nullable: false),
                        docu_type = c.String(),
                        doc_url = c.String(),
                        doc_discription = c.String(),
                        remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.emp_main", t => t.empid, cascadeDelete: true)
                .Index(t => t.empid);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        emp_Id = c.Int(nullable: false),
                        user_fullname = c.String(),
                        Avatar = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.emp_main", t => t.emp_Id, cascadeDelete: true)
                .Index(t => t.emp_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "emp_Id", "dbo.emp_main");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.uploaddocs", "empid", "dbo.emp_main");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.RiskLists", "Initiativeid", "dbo.Initiatives");
            DropForeignKey("dbo.Partners", "Initiativeid", "dbo.Initiatives");
            DropForeignKey("dbo.OperationalIndicators", "Initiativeid", "dbo.Initiatives");
            DropForeignKey("dbo.InitiativesUploas", "Initiativeid", "dbo.Initiatives");
            DropForeignKey("dbo.InitiativesTasksUploas", "TeamTasksid", "dbo.Team_Tasks");
            DropForeignKey("dbo.Team_Tasks", "empid", "dbo.Teams");
            DropForeignKey("dbo.Teams", "Initiativeid", "dbo.Initiatives");
            DropForeignKey("dbo.Teams", "empid", "dbo.emp_main");
            DropForeignKey("dbo.Team_Tasks", "Initiativeid", "dbo.Initiatives");
            DropForeignKey("dbo.InitiativesTasksUploas", "Initiativeid", "dbo.Initiatives");
            DropForeignKey("dbo.Initiatives", "Targetid", "dbo.Strategic_Targets");
            DropForeignKey("dbo.Strategic_Targets", "planid", "dbo.Strategic_Plans");
            DropForeignKey("dbo.Initiatives", "empid", "dbo.emp_main");
            DropForeignKey("dbo.Branchs", "Section_id", "dbo.Sections");
            DropForeignKey("dbo.Sections", "empid", "dbo.emp_main");
            DropForeignKey("dbo.Sections", "Department_id", "dbo.Departments");
            DropForeignKey("dbo.Departments", "MainDepartment_id", "dbo.MainDepartments");
            DropForeignKey("dbo.MainDepartments", "empid", "dbo.emp_main");
            DropForeignKey("dbo.Departments", "empid", "dbo.emp_main");
            DropForeignKey("dbo.emp_main", "Job_id", "dbo.Jobs");
            DropForeignKey("dbo.emp_main", "Branch_id", "dbo.Branchs");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "emp_Id" });
            DropIndex("dbo.uploaddocs", new[] { "empid" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.RiskLists", new[] { "Initiativeid" });
            DropIndex("dbo.Partners", new[] { "Initiativeid" });
            DropIndex("dbo.OperationalIndicators", new[] { "Initiativeid" });
            DropIndex("dbo.InitiativesUploas", new[] { "Initiativeid" });
            DropIndex("dbo.Teams", new[] { "empid" });
            DropIndex("dbo.Teams", new[] { "Initiativeid" });
            DropIndex("dbo.Team_Tasks", new[] { "empid" });
            DropIndex("dbo.Team_Tasks", new[] { "Initiativeid" });
            DropIndex("dbo.InitiativesTasksUploas", new[] { "TeamTasksid" });
            DropIndex("dbo.InitiativesTasksUploas", new[] { "Initiativeid" });
            DropIndex("dbo.Strategic_Targets", new[] { "planid" });
            DropIndex("dbo.Initiatives", new[] { "empid" });
            DropIndex("dbo.Initiatives", new[] { "Targetid" });
            DropIndex("dbo.MainDepartments", new[] { "empid" });
            DropIndex("dbo.emp_main", new[] { "Branch_id" });
            DropIndex("dbo.emp_main", new[] { "Job_id" });
            DropIndex("dbo.Departments", new[] { "empid" });
            DropIndex("dbo.Departments", new[] { "MainDepartment_id" });
            DropIndex("dbo.Sections", new[] { "empid" });
            DropIndex("dbo.Sections", new[] { "Department_id" });
            DropIndex("dbo.Branchs", new[] { "Section_id" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.uploaddocs");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RiskLists");
            DropTable("dbo.Partners");
            DropTable("dbo.OperationalIndicators");
            DropTable("dbo.InitiativesUploas");
            DropTable("dbo.Teams");
            DropTable("dbo.Team_Tasks");
            DropTable("dbo.InitiativesTasksUploas");
            DropTable("dbo.Strategic_Plans");
            DropTable("dbo.Strategic_Targets");
            DropTable("dbo.Initiatives");
            DropTable("dbo.countries");
            DropTable("dbo.MainDepartments");
            DropTable("dbo.Jobs");
            DropTable("dbo.emp_main");
            DropTable("dbo.Departments");
            DropTable("dbo.Sections");
            DropTable("dbo.Branchs");
        }
    }
}
