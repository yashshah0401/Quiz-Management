using System;
using System.Collections.Generic;
using System.Linq;
using QuizApplication.Models;

namespace QuizApplication.Helpers
{
    public static class QuestionBank
    {
        public static List<Question> GetRandomizedQuestions()
        {
            var questions = new List<Question>
            {
                new MCQQuestion
                {
                    QuestionText = "What is the capital of France?",
                    Options = new string[] { "Berlin", "Madrid", "Paris", "Lisbon" },
                    CorrectAnswerIndex = 2,
                    ImagePath = "Assets/france.png"
                },
                new MCQQuestion
                {
                    QuestionText = "Which planet is known as the Red Planet?",
                    Options = new string[] { "Earth", "Mars", "Jupiter", "Saturn" },
                    CorrectAnswerIndex = 1,
                    ImagePath = "Assets/mars.png"
                },
                new MCQQuestion
                {
                    QuestionText = "What is the boiling point of water?",
                    Options = new string[] { "90°C", "100°C", "110°C", "80°C" },
                    CorrectAnswerIndex = 1,
                    ImagePath = "Assets/water.png"
                },
                new MCQQuestion
                {
                    QuestionText = "Which gas do humans need to breathe?",
                    Options = new string[] { "Oxygen", "Hydrogen", "Carbon Dioxide", "Nitrogen" },
                    CorrectAnswerIndex = 0,
                    ImagePath = "Assets/oxygen.png"
                },
                new MultiAnswerQuestion
                {
                    QuestionText = "Which of the following are programming languages?",
                    Options = new string[] { "Python", "HTML", "Java", "CSS" },
                    CorrectAnswerIndexes = new List<int> { 0, 2 },
                    ImagePath = ""
                },
                new MultiAnswerQuestion
                {
                    QuestionText = "Select the animals that are mammals:",
                    Options = new string[] { "Shark", "Tiger", "Whale", "Frog" },
                    CorrectAnswerIndexes = new List<int> { 1, 2 },
                    ImagePath = ""
                }
            };

            return questions.OrderBy(q => Guid.NewGuid()).ToList();
        }
    }
}