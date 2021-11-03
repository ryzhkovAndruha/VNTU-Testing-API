using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingApi.Entities
{
    public class Test
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Question> Questions { get; set; }
        public int CountOfCorrectAnswers { get; set; }
        public int ResultInPersent { get; set; }
        public int TimeForQuestionInSeconds { get; set; }
        public bool SaveToJson { get; set; }

        public Test()
        {
            Questions = new List<Question>();
        }
    }
}
