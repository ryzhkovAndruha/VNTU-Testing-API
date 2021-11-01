using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingApi.Context;
using TestingApi.Entities;

namespace TestingApi.Repositories
{
    public class AnswerRepository
    {
        TestContext testContext;

        public AnswerRepository(TestContext testContext)
        {
            this.testContext = testContext;
        }

        public List<Answer> GetList() => testContext.Answers.ToList();

        public List<Answer> GetAnswersForSpecificQuestion(int questionId)
        {
            return testContext.Answers.Where(a => a.QuestionId == questionId).ToList();
        }
    }
}
