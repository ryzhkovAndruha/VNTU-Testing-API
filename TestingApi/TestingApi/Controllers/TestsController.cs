using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingApi.Entities;
using TestingApi.Repositories;

namespace TestingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        TestRepository testRepository;
        public TestsController()
        {
            TestData.CreateTestData();
            testRepository = new TestRepository();
        }

        /// <summary>
        /// Get all tests
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult<List<Test>> Get()
        {
            return Ok(testRepository.GetList());
        }

        /// <summary>
        /// Get test by ID
        /// </summary>
        /// <param name="id">Test id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Test> Get(int id)
        {
            Test test = testRepository.GetById(id);
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
        /// <returns></returns>
        [HttpPut("{Test}")]
        public ActionResult<Test> Put(Test test)
        {
            if (test == null)
            {
                return BadRequest();
            }
            if (!testRepository.GetList().Any(t => t.Id == test.Id))
            {
                NotFound();
            }

            testRepository.Update(test);

            return Ok(test);
        }

        /// <summary>
        /// Add test to DB
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        [HttpPost("{Test}")]
        public ActionResult<Test> Post(Test test)
        {
            if (test == null)
            {
                return BadRequest();
            }

            testRepository.Create(test);
            return Ok(test);
        }

        [HttpDelete("{id}")]
        public ActionResult<Test> Delete(int id)
        {
            Test test = testRepository.GetList().FirstOrDefault(t => t.Id == id);
            if (test == null)
            {
                return NotFound();
            }

            testRepository.Delete(id);
            return Ok(test);
        }

        [HttpGet("{TestResult}")]
        public ActionResult<int> FinishTest(TestResult testResult)
        {
            if (testResult == null)
            {
                return BadRequest();
            }

            Test test = testRepository.GetList().FirstOrDefault(t => t.Id == testResult.Id);
            int countOfRightAnswers = 0;

            foreach (var result in testResult.QuestionResults)
            {
                var rightAnswers = test.Questions[result.QuestionId].Answers.Where(a => a.IsCorrect == true).Select(a => a.Id).ToArray();
                var answers = result.AnswersId.ToArray();

                if (Enumerable.SequenceEqual(answers, rightAnswers))
                {
                    countOfRightAnswers++;
                }
            }

            return Ok(countOfRightAnswers);
        }
    }
}
