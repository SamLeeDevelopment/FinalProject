using Final_Project.StringComparison;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Models
{
    public class QuestionInfo
    {
        public String Question { get; private set; }
        public String Answer { get; private set; }

        private String UserEnterQuestion { get; set; }

        public QuestionInfo(String question, String answer, String userEnterQuestion)
        {
            Question = question;
            Answer = answer;
            UserEnterQuestion = userEnterQuestion;
        }

        /// <summary>
        /// Checks if the user question matches the current question pulled from quizlet
        /// </summary>
        /// <returns></returns>
        public FoundType Matches()
        {

            JaccardDistance jaccardDistance = new JaccardDistance();

            decimal d = jaccardDistance.Distance(Question, UserEnterQuestion);


            if (d == 100)
                return FoundType.CORRECT;

            if (d >= 50)
            {
                return FoundType.POSSIBLE;
            }

            return FoundType.NONE;


        }

    }

    public enum FoundType
    {
        CORRECT, POSSIBLE, NONE,
    }
}
