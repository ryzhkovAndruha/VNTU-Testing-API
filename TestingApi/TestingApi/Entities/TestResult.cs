using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingApi.Entities
{
    public class TestResult
    {
        public int TestId { get; set; }
        public List<QuestionResult> QuestionResults { get; set; }

        public TestResult()
        {
            QuestionResults = new List<QuestionResult>();
        }
    }
}
