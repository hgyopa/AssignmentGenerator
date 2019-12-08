﻿namespace AssignmentGenerator.Api.Models
{
    public class AnswerDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
    }
}