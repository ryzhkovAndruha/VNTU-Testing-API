using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingApi.Entities
{
    public class QuestionResult
    {
        public int QuestionID { get; set; }
        public List<int> AnswersID { get; set; }

        public QuestionResult()
        {
            AnswersID = new List<int>();
        }
    }
}
