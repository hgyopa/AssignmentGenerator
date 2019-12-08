namespace AssignmentGenerator.Api.Models
{
    using System.Collections.Generic;

    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int QuestionTypeId { get; set; }
        public int AssignmentId { get; set; }
        public List<AnswerDTO> Answers { get; set; }
    }
}