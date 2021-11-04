using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace TestingApi.Context
{
    public class TestContext: DbContext
    {
        public TestContext(DbContextOptions<TestContext> options)
        : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
    }
}
