using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestingApi.Entities
{
    public class Answer
    {
        public int ID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int QuestionID { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
