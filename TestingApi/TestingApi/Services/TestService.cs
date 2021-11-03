using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TestingApi.Entities;

namespace TestingApi.Services
{
    public static class TestService
    {
        private const string TESTS_FOLDER = "tests";
        public static void SaveTestToJson(Test test)
        {
            Directory.CreateDirectory(TESTS_FOLDER);
            string fileName = $"{TESTS_FOLDER}\\test_{test.ID}_{test.Name}.json";
            string jsonObject = JsonSerializer.Serialize(test, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileName, jsonObject);
        }
    }
}
