using System.Collections.Generic;

namespace QuizApplication.Models
{
    public abstract class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string[] Options { get; set; }
        public string ImagePath { get; set; }
        public abstract string QuestionType { get; }

        public virtual bool CheckAnswer(int selectedIndex) => false;
        public virtual bool CheckAnswer(List<int> selectedIndexes) => false;
    }
}
