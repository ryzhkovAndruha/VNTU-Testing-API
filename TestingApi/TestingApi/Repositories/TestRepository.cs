using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingApi.Context;
using TestingApi.Entities;

namespace TestingApi.Repositories
{
    public class TestRepository
    {
        TestContext testContext;
        QuestionsRepository questionsRepository;

        public TestRepository(TestContext testContext)
        {
            this.testContext = testContext;
            questionsRepository = new QuestionsRepository(testContext);
        }

        public List<Test> GetList()
        {
            try
            {
                return testContext.Tests.ToList();
            }
            catch
            {
                throw;
            }
        }
        public Test GetByID(int testID)
        {
            try
            {
                var test = testContext.Tests.FirstOrDefault(t => t.ID == testID);
                if (test == null)
                {
                    return null;
                }

                questionsRepository.GetQuestionsForSpecificTest(testID);

                return test;
            }
            catch
            {
                throw;
            }
        }
        public void Update(Test item)
        {
            try
            {
                var test = testContext.Tests.Find(item.ID);
                if (test == null)
                {
                    return;
                }

                test.Name = item.Name;
                test.Questions = item.Questions;
                test.TimeForQuestionInSeconds = item.TimeForQuestionInSeconds;
                testContext.SaveChanges();
            }
            catch
            {
                throw;
            }
            
        }
        public void Create(Test item)
        {

            if(testContext.Tests.Any(t => t.ID == item.ID))
            {
                throw new Exception($"Test with such id {item.ID} already exist");
            }

            try
            {
                testContext.Tests.Add(item);
                testContext.SaveChanges();

                foreach (var question in item.Questions)
                {
                    question.TestID = item.ID;
                    foreach (var answer in question.Answers)
                    {
                        answer.QuestionID = question.ID;
                    }
                }

                Update(item);
            }
            catch
            {
                throw;
            }
            
        }
        public void Delete(int id)
        {
            try
            {
                testContext.Tests.Remove(testContext.Tests.Find(id));
                testContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void AddTestDataToDb()
        {
            TestData.CreateTestData();
            testContext.Tests.Add(TestData.Tests[0]);
            testContext.SaveChanges();
        }
    }
}
