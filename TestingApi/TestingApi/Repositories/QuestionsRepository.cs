using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingApi.Context;
using TestingApi.Entities;

namespace TestingApi.Repositories
{
    public class QuestionsRepository
    {
        TestContext testContext;
        AnswerRepository answerRepository;

        public QuestionsRepository(TestContext testContext)
        {
            this.testContext = testContext;
            answerRepository = new AnswerRepository(testContext);
        }

        public List<Question> GetList() => testContext.Questions.ToList();

        public List<Question> GetQuestionsForSpecificTest(int testID)
        {
            var specificQuestions = testContext.Questions.Where(t => t.TestID == testID).ToList();
            
            if(specificQuestions == null)
            {
                return null;
            }

            foreach (var question in specificQuestions)
            {
                answerRepository.GetAnswersForSpecificQuestion(question.ID);
            }

            return specificQuestions;
        }
    }
}
