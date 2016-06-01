using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kaoshi.Areas.Teacher.Models
{
   public class TestModel
   {
      public int id { get; set; }
      public string title { get; set; }
      public int type { get; set; }
      public string result { get; set; }
      public int subject { get; set; }
      public bool addOptionFlag { get; set; }
      public bool editFlag { get; set; }
      public Array options { get; set; }
   }

   public class OptionModel
   {
      public int id { get; set; }
      public string abcd { get; set; }
      public string content { get; set; }
      public int test { get; set; }
      public bool editFlag { get; set; }
   }

   public class ComposeModel
   {
      public int id { get; set; }
      
      public int type { get; set; }

      public string tname { get; set; }
      
      public double value { get; set; }
      
      public int number { get; set; }
      
      public int paper { get; set; }
      
      public string tests { get; set; }
   }
}