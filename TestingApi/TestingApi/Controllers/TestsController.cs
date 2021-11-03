using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingApi.Entities;
using TestingApi.Repositories;
using TestingApi.Services;

namespace TestingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        TestRepository testRepository;

        public TestsController(TestRepository testRepository)
        {
            TestData.CreateTestData();
            this.testRepository = testRepository;
        }

        /// <summary>
        /// Get all tests
        /// </summary>
        /// <returns>All tests</returns>
        [HttpGet()]
        public ActionResult<List<Test>> Get()
        {
            return Ok(testRepository.GetList());
        }

        /// <summary>
        /// Get test by ID
        /// </summary>
        /// <param name="id">Test id</param>
        /// <returns>Test by ID</returns>
        [HttpGet("id")]
        public ActionResult<Test> Get(int id)
        {
            Test test = testRepository.GetByID(id);
            if (test == null)
            {
                return NotFound();
            }

            return Ok(test);
        }

        /// <summary>
        /// Update Test
        /// </summary>
        /// <param name="test">Updated test</param>
        /// <returns>Updated test</returns>
        [HttpPut("Test")]
        public ActionResult<Test> Put(Test test)
        {
            if (test == null)
            {
                return BadRequest();
            }
            if (!testRepository.GetList().Any(t => t.ID == test.ID))
            {
                NotFound();
            }
            

            testRepository.Update(test);

            if (test.SaveToJson == true)
            {
                TestService.SaveTestToJson(test);
            }

            return Ok(test);
        }

        /// <summary>
        /// Add test to DB
        /// </summary>
        /// <param name="test">Added test</param>
        /// <returns>Created test</returns>
        [HttpPost("Test")]
        public ActionResult<Test> Post(Test test)
        {
            if (test == null)
            {
                return BadRequest();
            }

            testRepository.Create(test);

            if (test.SaveToJson == true)
            {
                TestService.SaveTestToJson(test);
            }

            return Ok(test);
        }

        /// <summary>
        /// Delete test from DB
        /// </summary>
        /// <param name="test">Added test</param>
        /// <returns>Deleted test</returns>
        [HttpDelete("id")]
        public ActionResult<Test> Delete(int id)
        {
            Test test = testRepository.GetList().FirstOrDefault(t => t.ID == id);
            if (test == null)
            {
                return NotFound();
            }

            testRepository.Delete(id);
            return Ok(test);
        }

        /// <summary>
        /// Calculate test results
        /// </summary>
        /// <param name="testResult">test result object</param>
        /// <returns>count of right answers</returns>
        [HttpGet("TestResult")]
        public ActionResult<int> FinishTest(TestResult testResult)
        {
            if (testResult == null)
            {
                return BadRequest();
            }

            Test test = testRepository.GetList().FirstOrDefault(t => t.ID == testResult.TestID);
            int countOfRightAnswers = 0;

            foreach (var result in testResult.QuestionResults)
            {
                var rightAnswers = test.Questions[result.QuestionID].Answers.Where(a => a.IsCorrect == true).Select(a => a.ID).ToArray();
                var answers = result.AnswersID.ToArray();

                if (Enumerable.SequenceEqual(answers, rightAnswers))
                {
                    countOfRightAnswers++;
                }
            }

            test.CountOfCorrectAnswers = countOfRightAnswers;
            test.ResultInPersent = (countOfRightAnswers / test.Questions.Count) * 100;


            return Ok(countOfRightAnswers);
        }
    }
}
