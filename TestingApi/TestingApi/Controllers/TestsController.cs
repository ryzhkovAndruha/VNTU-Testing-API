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
using TestingApi.MyLogger;

namespace TestingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        TestRepository testRepository;

        public TestsController(TestRepository testRepository)
        {
            Logger.Init();
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
            try
            {
                var allTests = testRepository.GetList();
                Logger.WriteMessage("Get", $"Get all tests sucsesfull. Tests count {allTests.Count}", LogLevel.Info);

                return Ok(allTests);
            }
            catch(Exception ex)
            {
                Logger.WriteMessage("Get", $"Can't get all tests. Exception {ex}", LogLevel.Error);
                return NoContent();
            }
            
        }

        /// <summary>
        /// Get test by ID
        /// </summary>
        /// <param name="id">Test id</param>
        /// <returns>Test by ID</returns>
        [HttpGet("id")]
        public ActionResult<Test> Get(int id)
        {
            Test test = null;
            try
            {
                test = testRepository.GetByID(id);

                foreach (var question in test.Questions)
                {
                    question.Answers.Shuffle();
                }

                test.Questions.Shuffle();
            }
            catch(Exception ex)
            {
                Logger.WriteMessage("GetById", $"Can't get test number {id}. Exception {ex}", LogLevel.Error);
                return NoContent();
            }
            
            if (test == null)
            {
                Logger.WriteMessage("GetById", $"Get test number {id} returns null", LogLevel.Warn);
                return NotFound();
            }

            Logger.WriteMessage("GetById", $"Get test number {id} sucsesfull. Test name: {test.Name}", LogLevel.Info);
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
                Logger.WriteMessage("Put", $"Updated test equals null", LogLevel.Warn);
                return BadRequest();
            }
            if (!testRepository.GetList().Any(t => t.ID == test.ID))
            {
                Logger.WriteMessage("Put", $"Updated test not found in DB", LogLevel.Warn);
                return NotFound();
            }

            try
            {
                testRepository.Update(test);
            }
            catch(Exception ex)
            {
                Logger.WriteMessage("Put", $"Can't update test number {test.ID}. Exception {ex}", LogLevel.Error);
                return Problem();
            }

            if (test.SaveToJson)
            {
                try
                {
                    TestService.SaveTestToJson(test);
                    Logger.WriteMessage("Put", $"Test number {test.ID} saved to JSON sucsesfully", LogLevel.Info);
                }
                catch (Exception ex)
                {
                    Logger.WriteMessage("Put", $"Can't save test number {test.ID} to JSON. Exception {ex}", LogLevel.Error);
                }
            }

            Logger.WriteMessage("Put", $"Test number {test.ID} updated sucsesfully", LogLevel.Info);
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
                Logger.WriteMessage("Post", $"Added test equals null", LogLevel.Warn);
                return BadRequest();
            }

            try
            {
                testRepository.Create(test);
            }
            catch(Exception ex)
            {
                Logger.WriteMessage("Post", $"Can't create new test. Exception {ex}", LogLevel.Error);
                return Problem();
            }

            if (test.SaveToJson == true)
            {
                try
                {
                    TestService.SaveTestToJson(test);
                    Logger.WriteMessage("Post", $"Test number {test.ID} saved to JSON sucsesfully", LogLevel.Info);
                }
                catch (Exception ex)
                {
                    Logger.WriteMessage("Post", $"Can't save test number {test.ID} to JSON. Exception {ex}", LogLevel.Error);
                }
            }

            Logger.WriteMessage("Post", $"Test number {test.ID} created sucsesfully", LogLevel.Info);
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
            Test test = null;
            try
            {
                test = testRepository.GetByID(id);
            }
            catch(Exception ex)
            {
                Logger.WriteMessage("Delete", $"Can't get test number {id}. Exception {ex}", LogLevel.Error);
                return Problem();
            }

            if (test == null)
            {
                Logger.WriteMessage("Delete", $"Can't find deleted test number {id}", LogLevel.Warn);
                return NotFound();
            }

            try
            {
                testRepository.Delete(id);
            }
            catch(Exception ex)
            {
                Logger.WriteMessage("Delete", $"Can't delete test number {id}. Exception {ex}", LogLevel.Error);
                return Problem();
            }

            Logger.WriteMessage("Delete", $"Test number {test.ID} deleted sucsesfully", LogLevel.Info);
            return Ok(test);
        }

        /// <summary>
        /// Add to data base test that will be loaded from json-file
        /// </summary>
        /// <param name="fileName">path to file</param>
        /// <returns>Added test</returns>
        [HttpPost("fileName")]
        public ActionResult<Test> ImportTestFromFile(string fileName)
        {
            Test testFromFile;
            try
            {
                testFromFile = TestService.LoadTestFromJson(fileName);
            }
            catch(Exception ex)
            {
                Logger.WriteMessage("ImportTestFromFile", $"Can't parse test from file {fileName}. Exception {ex}", LogLevel.Error);
                return BadRequest();
            }

            if(testFromFile == null)
            {
                Logger.WriteMessage("ImportTestFromFile", $"Test from file returned null", LogLevel.Warn);
                return BadRequest();
            }

            try
            {
                testRepository.Create(testFromFile);
            }
            catch(Exception ex)
            {
                Logger.WriteMessage("ImportTestFromFile", $"Can't add parsed test to DB. Exception {ex}", LogLevel.Error);
                return BadRequest();
            }

            Logger.WriteMessage("ImportTestFromFile", $"Test number {testFromFile.ID} import from JSON sucsesfully", LogLevel.Info);
            return Ok(testFromFile);
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
                Logger.WriteMessage("FinishTest", $"Test result get null", LogLevel.Warn);
                return BadRequest();
            }

            Test test;

            try
            {
                test = testRepository.GetByID(testResult.TestID);
            }
            catch(Exception ex)
            {
                Logger.WriteMessage("FinishTest", $"Can't get test {testResult.TestID} from DB. Exception {ex}", LogLevel.Error);
                return BadRequest();
            }
            
            int countOfRightAnswers = 0;

            foreach (var result in testResult.QuestionResults)
            {
                int[] rightAnswers, answers;
                try
                {
                    rightAnswers = test.Questions[result.QuestionID].Answers.Where(a => a.IsCorrect == true).Select(a => a.ID).ToArray();
                }
                catch (Exception ex)
                {
                    Logger.WriteMessage("FinishTest", $"Can't get right answers for question {result.QuestionID}. Exception {ex}", LogLevel.Error);
                    return BadRequest();
                }
                try
                {
                    answers = result.AnswersID.ToArray();
                }
                catch(Exception ex)
                {
                    Logger.WriteMessage("FinishTest", $"Can't get student answers for question {result.QuestionID}. Exception {ex}", LogLevel.Error);
                    return BadRequest();
                }

                if (Enumerable.SequenceEqual(answers, rightAnswers))
                {
                    countOfRightAnswers++;
                }
            }

            test.CountOfCorrectAnswers = countOfRightAnswers;
            test.ResultInPersent = (countOfRightAnswers / test.Questions.Count) * 100;

            Logger.WriteMessage("FinishTest", $"Test result calculation for test {test.ID} finish sucsesfully", LogLevel.Info);
            return Ok(countOfRightAnswers);
        }
    }
}
