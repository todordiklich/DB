using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models
{
    public class Homework
    {
        public int HomeworkId { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string Content { get; set; }

        public ContentType ContentType { get; set; }

        public DateTime SubmissionTime { get; set; }

        public int StudentId { get; set; }
        public Student Strudent { get; set; }


        public int CourseId { get; set; }
        public Course Course { get; set; }


        //HomeworkId
        //Content(string, linking to a file, not unicode)
        //ContentType(enum – can be Application, Pdf or Zip)
        //SubmissionTime
        //StudentId
        //CourseId
    }
}
