using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestingApi.Entities
{
    public class Question
    {
        public int ID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TestID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Answer> Answers { get; set; }

        public Question()
        {
            Answers = new List<Answer>();
        }
    }
}
