﻿using System;
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

        public List<Test> GetList() => testContext.Tests.ToList();
        public Test GetById(int testId)
        {
            var test = testContext.Tests.FirstOrDefault(t => t.Id == testId);
            if (test==null)
            {
                return null;
            }

            questionsRepository.GetQuestionsForSpecificTest(testId);

            return test;
        }
        public void Update(Test item)
        {
            var test = testContext.Tests.Find(item.Id);
            if (test == null)
            {
                return;
            }

            test.Name = item.Name;
            test.Questions = item.Questions;
            test.TimeForQuestionInSeconds = item.TimeForQuestionInSeconds;
            testContext.SaveChanges();
        }
        public void Create(Test item)
        {
            testContext.Tests.Add(item);
            testContext.SaveChanges();

            foreach (var question in item.Questions)
            {
                question.TestId = item.Id;
                foreach (var answer in question.Answers)
                {
                    answer.QuestionId = question.Id;
                }
            }

            Update(item);
        }
        public void Delete(int id)
        {
            testContext.Tests.Remove(testContext.Tests.Find(id));
            testContext.SaveChanges();
        }

        public void AddTestDataToDb()
        {
            TestData.CreateTestData();
            testContext.Tests.Add(TestData.Tests[0]);
            testContext.SaveChanges();
        }
    }
}
