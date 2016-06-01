namespace kaoshi.Models
{
   using System;
   using System.Data.Entity;
   using System.ComponentModel.DataAnnotations.Schema;
   using System.Linq;

   public partial class WebContext : DbContext
   {
      public WebContext()
          : base("name=WebContext")
      {
      }

      public virtual DbSet<es_role> es_role { get; set; }
      public virtual DbSet<es_manager> es_manager { get; set; }
      public virtual DbSet<es_teacher> es_teacher { get; set; }
      public virtual DbSet<es_class> es_class { get; set; }
      public virtual DbSet<es_student> es_student { get; set; }
      public virtual DbSet<es_notice> es_notice { get; set; }


      public virtual DbSet<es_subject> es_subject { get; set; }
      public virtual DbSet<es_paper> es_paper { get; set; }
      public virtual DbSet<es_test_type> es_test_type { get; set; }
      public virtual DbSet<es_paper_compose> es_paper_compose { get; set; }
      public virtual DbSet<es_test> es_test { get; set; }
      public virtual DbSet<es_test_option> es_test_option { get; set; }
      public virtual DbSet<es_exam> es_exam { get; set; }


      public virtual DbSet<es_stu_exam> es_stu_exam { get; set; }
      public virtual DbSet<es_stu_test> es_stu_test { get; set; }

      protected override void OnModelCreating(DbModelBuilder modelBuilder)
      {
         modelBuilder.Entity<es_test>()
            .HasMany(t => t.es_stu_test)
            .WithRequired(p => p.es_test)
            .WillCascadeOnDelete(false);
         modelBuilder.Entity<es_class>()
            .HasMany(t => t.es_student)
            .WithRequired(p => p.es_class)
            .WillCascadeOnDelete(false);
      }

      /**
      protected override void OnModelCreating(DbModelBuilder modelBuilder)
      {
        modelBuilder.Entity<es_class>()
            .Property(e => e.name)
            .IsUnicode(false);

        modelBuilder.Entity<es_class>()
            .HasMany(e => e.es_paper)
            .WithOptional(e => e.es_class)
            .HasForeignKey(e => e.subject_id);

        modelBuilder.Entity<es_class>()
            .HasMany(e => e.es_student)
            .WithOptional(e => e.es_class)
            .HasForeignKey(e => e.class_id);

        modelBuilder.Entity<es_exam>()
            .Property(e => e.score)
            .HasPrecision(18, 0);

        modelBuilder.Entity<es_manager>()
            .Property(e => e.login_id)
            .IsUnicode(false);

        modelBuilder.Entity<es_manager>()
            .Property(e => e.login_pwd)
            .IsUnicode(false);

        modelBuilder.Entity<es_manager>()
            .Property(e => e.real_name)
            .IsUnicode(false);

        modelBuilder.Entity<es_manager>()
            .Property(e => e.sex)
            .IsUnicode(false);

        modelBuilder.Entity<es_manager>()
            .Property(e => e.email)
            .IsUnicode(false);

        modelBuilder.Entity<es_paper>()
            .Property(e => e.name)
            .IsUnicode(false);

        modelBuilder.Entity<es_paper>()
            .Property(e => e.ab_paging)
            .IsFixedLength()
            .IsUnicode(false);

        modelBuilder.Entity<es_paper>()
            .HasMany(e => e.es_exam)
            .WithOptional(e => e.es_paper)
            .HasForeignKey(e => e.paper);

        modelBuilder.Entity<es_role>()
            .Property(e => e.name)
            .IsUnicode(false);

        modelBuilder.Entity<es_role>()
            .HasMany(e => e.es_manager)
            .WithOptional(e => e.es_role)
            .HasForeignKey(e => e.role_id);

        modelBuilder.Entity<es_student>()
            .Property(e => e.pwd)
            .IsUnicode(false);

        modelBuilder.Entity<es_student>()
            .Property(e => e.real_name)
            .IsUnicode(false);

        modelBuilder.Entity<es_student>()
            .Property(e => e.sex)
            .IsUnicode(false);

        modelBuilder.Entity<es_student>()
            .Property(e => e.email)
            .IsUnicode(false);

        modelBuilder.Entity<es_subject>()
            .Property(e => e.name)
            .IsUnicode(false);

        modelBuilder.Entity<es_teacher>()
            .Property(e => e.id)
            .IsUnicode(false);

        modelBuilder.Entity<es_teacher>()
            .Property(e => e.pwd)
            .IsUnicode(false);

        modelBuilder.Entity<es_teacher>()
            .Property(e => e.real_name)
            .IsUnicode(false);

        modelBuilder.Entity<es_teacher>()
            .Property(e => e.sex)
            .IsUnicode(false);

        modelBuilder.Entity<es_teacher>()
            .Property(e => e.email)
            .IsUnicode(false);

        modelBuilder.Entity<es_test>()
            .Property(e => e.title)
            .IsUnicode(false);

        modelBuilder.Entity<es_test>()
            .Property(e => e.option_a)
            .IsUnicode(false);

        modelBuilder.Entity<es_test>()
            .Property(e => e.option_b)
            .IsUnicode(false);

        modelBuilder.Entity<es_test>()
            .Property(e => e.option_c)
            .IsUnicode(false);

        modelBuilder.Entity<es_test>()
            .Property(e => e.option_d)
            .IsUnicode(false);
      }
    **/
   }
}
