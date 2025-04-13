namespace QuizApplication.Models
{
    public class UserScore : IComparable<UserScore>
    {
        public string Username { get; set; }
        public double Score { get; set; }
        public DateTime AnswerTime { get; set; } = DateTime.Now;

        public int CompareTo(UserScore other)
        {
            if (other == null) return -1;

            int scoreComparison = other.Score.CompareTo(this.Score);
            // if score is different, sort by score descending
            if (scoreComparison != 0)
            {
                return scoreComparison;
            }
            // otherwise, sort by username ascending
            return string.Compare(this.Username, other.Username, StringComparison.Ordinal);
        }
    }
}
