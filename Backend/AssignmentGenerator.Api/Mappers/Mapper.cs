namespace AssignmentGenerator.Api.Mappers
{
    using System.Linq;
    using DataAccessLayer.Entities;
    using Models;

    public static class Mapper
    {
        public static QuestionDTO MapFromTestGeneratorQuestionToDTO(TestGenerator.Models.Question question)
        {
            return new QuestionDTO
            {
                Text = question.Text,
                QuestionTypeId = (int)question.Type,
                Answers = question.Answers.Select(MapFromTestGeneratorAnswerToDTO).ToList()
            };
        }

        public static AnswerDTO MapFromTestGeneratorAnswerToDTO(TestGenerator.Models.Answer answer)
        {
            return new AnswerDTO
            {
                Text = answer.Text,
                IsCorrect = answer.IsCorrect
            };
        }

        public static AssignmentDTO MapFromDALAssignmentToDTO(Assignment assignment)
        {
            return new AssignmentDTO
            {
                Id = assignment.Id,
                UserId = assignment.UserId,
                CreationDate = assignment.CreationDate,
                Title = assignment.Title,
                Questions = assignment.Questions?.Select(MapFromDALQuestionToDTO).ToList()
            };
        }

        public static QuestionDTO MapFromDALQuestionToDTO(Question question)
        {
            return new QuestionDTO
            {
                Id = question.Id,
                Text = question.Text,
                QuestionTypeId = question.QuestionTypeId,
                AssignmentId = question.AssignmentId,
                Answers = question.Answers.Select(MapFromDALAnswerToDTO).ToList()
            };
        }

        public static AnswerDTO MapFromDALAnswerToDTO(Answer answer)
        {
            return new AnswerDTO
            {
                Id = answer.Id,
                Text = answer.Text,
                QuestionId = answer.QuestionId,
                IsCorrect = answer.IsCorrect
            };
        }

        public static Assignment MapFromAssignmentDTOToDAL(AssignmentDTO assignment)
        {
            return new Assignment
            {
                Id = assignment.Id,
                Title = assignment.Title,
                UserId = assignment.UserId,
                CreationDate = assignment.CreationDate,
                Questions = assignment.Questions.Select(MapFromQuestionDTOToDAL).ToList()
            };
        }

        public static Question MapFromQuestionDTOToDAL(QuestionDTO question)
        {
            return new Question
            {
                Id = question.Id,
                Text = question.Text,
                QuestionTypeId = question.QuestionTypeId,
                AssignmentId = question.AssignmentId,
                Answers = question.Answers.Select(MapFromAnswerDTOToDAL).ToList()
            };
        }

        public static Answer MapFromAnswerDTOToDAL(AnswerDTO answer)
        {
            return new Answer
            {
                Id = answer.Id,
                Text = answer.Text,
                QuestionId = answer.QuestionId,
                IsCorrect = answer.IsCorrect
            };
        }
    }
}