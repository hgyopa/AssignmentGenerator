namespace AssignmentGenerator.DataAccessLayer.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Answer : Entity
    {
        [Required]
        [StringLength(255)]
        public string Text { get; set; }

        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
