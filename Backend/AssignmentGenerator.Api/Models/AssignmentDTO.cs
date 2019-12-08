namespace AssignmentGenerator.Api.Models
{
    using System;
    using System.Collections.Generic;

    public class AssignmentDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserId { get; set; }
        public List<QuestionDTO> Questions { get; set; }
    }
}