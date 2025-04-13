namespace QuizApplication.Models
{
    public class MultiAnswerQuestion : Question
    {
        public override string QuestionType => "MultiAnswer";

        public List<int> CorrectAnswerIndexes { get; set; } // CorrectAnswerIndexes is defined here

        public override bool CheckAnswer(List<int> selectedIndexes)
        {
            return selectedIndexes.OrderBy(x => x).SequenceEqual(CorrectAnswerIndexes.OrderBy(x => x));
        }
    }
}