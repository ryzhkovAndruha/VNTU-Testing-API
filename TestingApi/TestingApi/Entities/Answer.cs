using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestingApi.Entities
{
    public class Answer
    {
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
