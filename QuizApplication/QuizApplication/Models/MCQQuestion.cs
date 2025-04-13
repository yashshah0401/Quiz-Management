using QuizApplication.Models;

namespace QuizApplication.Models
{
    public class MCQQuestion : Question
    {
        public override string QuestionType => "MCQ";

        public int CorrectAnswerIndex { get; set; }

        public override bool CheckAnswer(int selectedIndex)
        {
            return selectedIndex == CorrectAnswerIndex;
        }
    }
}
