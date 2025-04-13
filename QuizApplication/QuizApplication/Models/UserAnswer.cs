using System.Collections.Generic;

namespace QuizApplication.Models
{
    public class UserAnswer
    {
        public Question Question { get; set; }
        public List<string> SelectedAnswers { get; set; }
        public bool IsCorrect { get; set; }
    }
}
