namespace AssignmentGenerator.DataAccessLayer.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Question : Entity
    {
        [Required]
        [StringLength(255)]
        public string Text { get; set; }

        public int QuestionTypeId { get; set; }
        public QuestionType QuestionType { get; set; }


        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }

        public ICollection<Answer> Answers { get; set; }
    }
}
