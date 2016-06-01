namespace kaoshi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5294 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.es_class",
                c => new
                    {
                        no = c.Int(nullable: false),
                        name = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.no);
            
            CreateTable(
                "dbo.es_student",
                c => new
                    {
                        sno = c.Int(nullable: false),
                        pwd = c.String(nullable: false, maxLength: 40),
                        real_name = c.String(maxLength: 10),
                        sex = c.Byte(),
                        email = c.String(maxLength: 40),
                        create_at = c.DateTime(nullable: false),
                        update_at = c.DateTime(),
                        class_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.sno)
                .ForeignKey("dbo.es_class", t => t.class_id)
                .Index(t => t.class_id);
            
            CreateTable(
                "dbo.es_stu_exam",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        student = c.Int(nullable: false),
                        exam = c.Int(nullable: false),
                        begin_time = c.DateTime(),
                        end_time = c.DateTime(),
                        score = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.es_exam", t => t.exam, cascadeDelete: true)
                .ForeignKey("dbo.es_student", t => t.student, cascadeDelete: true)
                .Index(t => t.student)
                .Index(t => t.exam);
            
            CreateTable(
                "dbo.es_exam",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 30),
                        paper = c.Int(nullable: false),
                        status = c.Int(nullable: false),
                        stulist = c.String(),
                        begin_time = c.DateTime(),
                        end_time = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.es_paper", t => t.paper, cascadeDelete: true)
                .Index(t => t.paper);
            
            CreateTable(
                "dbo.es_paper",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 20),
                        ab_paging = c.String(nullable: false, maxLength: 1),
                        test_time = c.Int(nullable: false),
                        total_points = c.Int(nullable: false),
                        status = c.Int(nullable: false),
                        create_at = c.DateTime(nullable: false),
                        update_at = c.DateTime(),
                        subject = c.Int(nullable: false),
                        teacher = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.es_subject", t => t.subject, cascadeDelete: true)
                .ForeignKey("dbo.es_teacher", t => t.teacher)
                .Index(t => t.subject)
                .Index(t => t.teacher);
            
            CreateTable(
                "dbo.es_paper_compose",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        type = c.Int(nullable: false),
                        value = c.Single(nullable: false),
                        number = c.Int(nullable: false),
                        paper = c.Int(nullable: false),
                        tests = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.es_paper", t => t.paper, cascadeDelete: true)
                .ForeignKey("dbo.es_test_type", t => t.type, cascadeDelete: true)
                .Index(t => t.type)
                .Index(t => t.paper);
            
            CreateTable(
                "dbo.es_test_type",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.es_test",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        title = c.String(nullable: false, maxLength: 300),
                        type = c.Int(nullable: false),
                        result = c.String(maxLength: 100),
                        analyze = c.String(maxLength: 300),
                        subject = c.Int(nullable: false),
                        teacher = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.es_subject", t => t.subject, cascadeDelete: true)
                .ForeignKey("dbo.es_teacher", t => t.teacher)
                .ForeignKey("dbo.es_test_type", t => t.type, cascadeDelete: true)
                .Index(t => t.type)
                .Index(t => t.subject)
                .Index(t => t.teacher);
            
            CreateTable(
                "dbo.es_stu_test",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        exam = c.Int(nullable: false),
                        test = c.Int(nullable: false),
                        result = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.es_stu_exam", t => t.exam, cascadeDelete: true)
                .ForeignKey("dbo.es_test", t => t.test)
                .Index(t => t.exam)
                .Index(t => t.test);
            
            CreateTable(
                "dbo.es_subject",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.es_teacher",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        login_id = c.String(nullable: false, maxLength: 20),
                        login_pwd = c.String(nullable: false, maxLength: 50),
                        real_name = c.String(maxLength: 10),
                        sex = c.Byte(),
                        email = c.String(maxLength: 30),
                        create_at = c.DateTime(nullable: false),
                        update_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.es_test_option",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        abcd = c.String(nullable: false, maxLength: 2),
                        content = c.String(nullable: false, maxLength: 300),
                        test = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.es_test", t => t.test, cascadeDelete: true)
                .Index(t => t.test);
            
            CreateTable(
                "dbo.es_manager",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        login_id = c.String(nullable: false, maxLength: 20),
                        login_pwd = c.String(nullable: false, maxLength: 50),
                        real_name = c.String(maxLength: 10),
                        sex = c.Byte(),
                        email = c.String(maxLength: 30),
                        create_at = c.DateTime(),
                        update_at = c.DateTime(),
                        role = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.es_role", t => t.role)
                .Index(t => t.role);
            
            CreateTable(
                "dbo.es_role",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.es_notice",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        title = c.String(nullable: false, maxLength: 100),
                        content = c.String(nullable: false),
                        create_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.es_manager", "role", "dbo.es_role");
            DropForeignKey("dbo.es_student", "class_id", "dbo.es_class");
            DropForeignKey("dbo.es_stu_exam", "student", "dbo.es_student");
            DropForeignKey("dbo.es_stu_exam", "exam", "dbo.es_exam");
            DropForeignKey("dbo.es_exam", "paper", "dbo.es_paper");
            DropForeignKey("dbo.es_paper", "teacher", "dbo.es_teacher");
            DropForeignKey("dbo.es_paper", "subject", "dbo.es_subject");
            DropForeignKey("dbo.es_paper_compose", "type", "dbo.es_test_type");
            DropForeignKey("dbo.es_test", "type", "dbo.es_test_type");
            DropForeignKey("dbo.es_test_option", "test", "dbo.es_test");
            DropForeignKey("dbo.es_test", "teacher", "dbo.es_teacher");
            DropForeignKey("dbo.es_test", "subject", "dbo.es_subject");
            DropForeignKey("dbo.es_stu_test", "test", "dbo.es_test");
            DropForeignKey("dbo.es_stu_test", "exam", "dbo.es_stu_exam");
            DropForeignKey("dbo.es_paper_compose", "paper", "dbo.es_paper");
            DropIndex("dbo.es_manager", new[] { "role" });
            DropIndex("dbo.es_test_option", new[] { "test" });
            DropIndex("dbo.es_stu_test", new[] { "test" });
            DropIndex("dbo.es_stu_test", new[] { "exam" });
            DropIndex("dbo.es_test", new[] { "teacher" });
            DropIndex("dbo.es_test", new[] { "subject" });
            DropIndex("dbo.es_test", new[] { "type" });
            DropIndex("dbo.es_paper_compose", new[] { "paper" });
            DropIndex("dbo.es_paper_compose", new[] { "type" });
            DropIndex("dbo.es_paper", new[] { "teacher" });
            DropIndex("dbo.es_paper", new[] { "subject" });
            DropIndex("dbo.es_exam", new[] { "paper" });
            DropIndex("dbo.es_stu_exam", new[] { "exam" });
            DropIndex("dbo.es_stu_exam", new[] { "student" });
            DropIndex("dbo.es_student", new[] { "class_id" });
            DropTable("dbo.es_notice");
            DropTable("dbo.es_role");
            DropTable("dbo.es_manager");
            DropTable("dbo.es_test_option");
            DropTable("dbo.es_teacher");
            DropTable("dbo.es_subject");
            DropTable("dbo.es_stu_test");
            DropTable("dbo.es_test");
            DropTable("dbo.es_test_type");
            DropTable("dbo.es_paper_compose");
            DropTable("dbo.es_paper");
            DropTable("dbo.es_exam");
            DropTable("dbo.es_stu_exam");
            DropTable("dbo.es_student");
            DropTable("dbo.es_class");
        }
    }
}
