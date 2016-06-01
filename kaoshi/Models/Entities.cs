using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace kaoshi.Models
{
   /// <summary>
   /// 角色表
   /// </summary>
   public partial class es_role
   {
      [Key]
      public int id { get; set; }

      [Required]
      [StringLength(10)]
      [Display(Name = "名称")]
      public string name { get; set; }

      public virtual ICollection<es_manager> es_manager { get; set; }
   }

   /// <summary>
   /// 管理员表
   /// </summary>
   public partial class es_manager
   {
      [Key]
      public int id { get; set; }

      [Required]
      [StringLength(20)]
      [Display(Name = "登录ID")]
      public string login_id { get; set; }

      [Required]
      [StringLength(50)]
      [Display(Name = "密码")]
      public string login_pwd { get; set; }

      [StringLength(10)]
      [Display(Name = "姓名")]
      public string real_name { get; set; }

      [Display(Name = "性别")]
      public Nullable<byte> sex { get; set; }

      [StringLength(30)]
      [DataType(DataType.EmailAddress)]
      [Display(Name = "邮箱")]
      public string email { get; set; }

      [DataType(DataType.DateTime)]
      [ScaffoldColumn(false)]
      [Display(Name = "创建时间")]
      public DateTime? create_at { get; set; }

      [DataType(DataType.DateTime)]
      [ScaffoldColumn(false)]
      [Display(Name = "更新时间")]
      public DateTime? update_at { get; set; }

      [ForeignKey("es_role")]
      [Display(Name = "角色")]
      public int? role { get; set; }

      public virtual es_role es_role { get; set; }
   }

   /// <summary>
   /// 教师表
   /// </summary>
   public partial class es_teacher
   {
      [Key]
      public int id { get; set; }
      [Required]
      [StringLength(20)]
      [Display(Name = "登录ID")]
      public string login_id { get; set; }

      [Required]
      [StringLength(50)]
      [Display(Name = "密码")]
      public string login_pwd { get; set; }

      [StringLength(10)]
      [Display(Name = "姓名")]
      public string real_name { get; set; }

      [Display(Name = "性别")]
      public Nullable<byte> sex { get; set; }

      [StringLength(30)]
      [DataType(DataType.EmailAddress)]
      [Display(Name = "邮箱")]
      public string email { get; set; }

      [DataType(DataType.DateTime)]
      [ScaffoldColumn(false)]
      [Display(Name = "创建时间")]
      public DateTime create_at { get; set; }

      [DataType(DataType.DateTime)]
      [ScaffoldColumn(false)]
      [Display(Name = "更新时间")]
      public DateTime? update_at { get; set; }
   }

   /// <summary>
   /// 班级表
   /// </summary>
   public partial class es_class
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.None)]
      [Display(Name = "班号")]
      public int no { get; set; }

      [Required]
      [StringLength(10)]
      [Display(Name = "班名")]
      public string name { get; set; }
      
      public virtual ICollection<es_student> es_student { get; set; }
   }

   /// <summary>
   /// 学生表
   /// </summary>
   public partial class es_student
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.None)]
      [Display(Name = "学号")]
      public int sno { get; set; }

      [Required]
      [StringLength(40)]
      [Display(Name = "密码")]
      public string pwd { get; set; }

      [StringLength(10)]
      [Display(Name = "姓名")]
      public string real_name { get; set; }

      [Display(Name = "性别")]
      public byte? sex { get; set; }

      [DataType(DataType.EmailAddress)]
      [StringLength(40)]
      [Display(Name = "邮箱")]
      public string email { get; set; }

      [DataType(DataType.DateTime)]
      [ScaffoldColumn(false)]
      [Display(Name = "创建时间")]
      public DateTime create_at { get; set; }

      [DataType(DataType.DateTime)]
      [ScaffoldColumn(false)]
      [Display(Name = "更新时间")]
      public DateTime? update_at { get; set; }

      [ForeignKey("es_class")]
      [Display(Name = "班级")]
      public int class_id { get; set; }

      public virtual es_class es_class { get; set; }

      public virtual ICollection<es_stu_exam> es_stu_exam { get; set; }
   }

   public partial class es_notice
   {
      [Key]
      public int id { get; set; }

      [Required]
      [StringLength(100)]
      [Display(Name = "标题")]
      public string title { get; set; }

      [Required]
      [DataType(DataType.Text)]
      [Display(Name = "内容")]
      public string content { get; set; }

      [DataType(DataType.DateTime)]
      [ScaffoldColumn(false)]
      [Display(Name = "发布时间")]
      public DateTime create_at { get; set; }

   }

   /**************** cut-line *****************/

   /// <summary>
   /// 科目表
   /// </summary>
   public partial class es_subject
   {
      [Key]
      public int id { get; set; }

      [Required]
      [StringLength(10)]
      [Display(Name = "名称")]
      public string name { get; set; }

      public virtual ICollection<es_test> es_text { get; set; }
      public virtual ICollection<es_paper> es_paper { get; set; }
   }

   /// <summary>
   /// 试卷表
   /// </summary>
   public partial class es_paper
   {
      [Key]
      public int id { get; set; }

      [Required]
      [StringLength(20)]
      [Display(Name = "名称")]
      public string name { get; set; }

      [Required]
      [StringLength(1)]
      [Display(Name = "卷面(A/B)")]
      public string ab_paging { get; set; }

      [Display(Name = "考试时长")]
      public int test_time { get; set; }

      [Display(Name = "卷面总分")]
      public int total_points { get; set; }

      /// <summary>
      /// 1.组卷中、2.组卷完成、3.试题筛选完成、0.删除
      /// </summary>
      [Display(Name = "状态")]
      public int status { get; set; }

      [DataType(DataType.DateTime)]
      [ScaffoldColumn(false)]
      [Display(Name = "创建时间")]
      public DateTime create_at { get; set; }

      [DataType(DataType.DateTime)]
      [ScaffoldColumn(false)]
      [Display(Name = "更新时间")]
      public DateTime? update_at { get; set; }

      [ForeignKey("es_subject")]
      [Display(Name = "科目")]
      public int subject { get; set; }

      [Display(Name = "教师")]
      [ForeignKey("es_teacher")]
      public Nullable<int> teacher { get; set; }

      public virtual es_subject es_subject { get; set; }
      public virtual es_teacher es_teacher { get; set; }

      public virtual ICollection<es_exam> es_exam { get; set; }
      public virtual ICollection<es_paper_compose> es_paper_compose { get; set; }
   }

   /// <summary>
   /// 试题(题目)类型
   /// </summary>
   public partial class es_test_type
   {
      [Key]
      public int id { get; set; }

      [Required]
      [StringLength(10)]
      [Display(Name = "名称")]
      public string name { get; set; }

      public virtual ICollection<es_test> es_test { get; set; }
      public virtual ICollection<es_paper_compose> es_paper_compose { get; set; }
   }

   /// <summary>
   /// 试卷题型合成(组卷)
   /// </summary>
   public partial class es_paper_compose
   {
      [Key]
      public int id { get; set; }

      [ForeignKey("es_test_type")]
      [Display(Name = "题型")]
      public int type { get; set; }

      [Display(Name = "分值")]
      public float value { get; set; }

      [Display(Name = "数量")]
      public int number { get; set; }

      [ForeignKey("es_paper")]
      [Display(Name = "试卷")]
      public int paper { get; set; }

      [StringLength(300)]
      [Display(Name = "试题库")]
      public string tests { get; set; }

      public virtual es_test_type es_test_type { get; set; }
      public virtual es_paper es_paper { get; set; }
   }

   /// <summary>
   /// 试题(题目)表
   /// </summary>
   public partial class es_test
   {
      [Key]
      public int id { get; set; }

      [Required]
      [StringLength(300)]
      [Display(Name = "题目")]
      public string title { get; set; }

      [ForeignKey("es_test_type")]
      [Display(Name = "类型")]
      public int type { get; set; }

      [StringLength(100)]
      [Display(Name = "答案")]
      public string result { get; set; }

      [StringLength(300)]
      [Display(Name = "解析")]
      public string analyze { get; set; }

      [Display(Name = "所属科目")]
      [ForeignKey("es_subject")]
      public int subject { get; set; }

      [Display(Name = "教师")]
      [ForeignKey("es_teacher")]
      public Nullable<int> teacher { get; set; }

      public virtual es_test_type es_test_type { get; set; }
      public virtual es_subject es_subject { get; set; }
      public virtual es_teacher es_teacher { get; set; }

      public virtual ICollection<es_test_option> es_test_option { get; set; }
      public virtual ICollection<es_stu_test> es_stu_test { get; set; }
   }

   /// <summary>
   /// 试题(题目)选项
   /// </summary>
   public partial class es_test_option
   {
      [Key]
      public int id { get; set; }

      [Required]
      [StringLength(2)]
      [Display(Name = "ABCD")]
      public string abcd { get; set; }

      [Required]
      [StringLength(300)]
      [Display(Name = "内容")]
      public string content { get; set; }

      [ForeignKey("es_test")]
      public int test { get; set; }

      public virtual es_test es_test { get; set; }
   }

   /// <summary>
   /// 考试记录表
   /// </summary>
   public partial class es_exam
   {
      [Key]
      public int id { get; set; }

      [Required]
      [StringLength(30)]
      [Display(Name = "名称")]
      public string name { get; set; }

      [ForeignKey("es_paper")]
      [Display(Name = "考卷")]
      public int paper { get; set; }

      /// <summary>
      /// 1.待考、2.正在考试、3.考试结束、0.考试取消
      /// </summary>
      [Display(Name = "状态")]
      public int status { get; set; }

      [Display(Name = "学生列表")]
      public string stulist { get; set; }

      [DataType(DataType.DateTime)]
      [Display(Name = "开始时间")]
      public DateTime? begin_time { get; set; }

      [DataType(DataType.DateTime)]
      [Display(Name = "结束时间")]
      public DateTime? end_time { get; set; }

      public virtual es_paper es_paper { get; set; }
      public virtual ICollection<es_stu_exam> es_stu_exam { get; set; }
   }

   /**************** cut-line *****************/

   /// <summary>
   /// 学生考试记录表
   /// </summary>
   public partial class es_stu_exam
   {
      [Key]
      public int id { get; set; }

      [ForeignKey("es_student")]
      [Display(Name = "学生")]
      public int student { get; set; }

      [ForeignKey("es_exam")]
      [Display(Name = "考卷")]
      public int exam { get; set; }

      [DataType(DataType.DateTime)]
      [Display(Name = "答题时间")]
      public DateTime? begin_time { get; set; }

      [DataType(DataType.DateTime)]
      [Display(Name = "提交时间")]
      public DateTime? end_time { get; set; }

      [Display(Name = "成绩")]
      public decimal? score { get; set; }

      public virtual es_exam es_exam { get; set; }
      public virtual es_student es_student { get; set; }

      public virtual ICollection<es_stu_test> es_stu_test { get; set; }
   }
   
   /// <summary>
   /// 学生答题临时记录表
   /// </summary>
   public partial class es_stu_test
   {
      [Key]
      public int id { get; set; }

      [Display(Name = "考试")]
      [ForeignKey("es_stu_exam")]
      public int exam { get; set; }

      [ForeignKey("es_test")]
      public int test { get; set; }

      [StringLength(10)]
      [Display(Name = "答案")]
      public string result { get; set; }

      public virtual es_stu_exam es_stu_exam { get; set; }
      public virtual es_test es_test { get; set; }
   }
}