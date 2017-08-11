using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FunFacts.Models
{
    public class ChuckNorrisFunFact : IFunFact
    {
        [Range(0, long.MaxValue, ErrorMessage = "Enter valid positive number.")]
        public long Id { get; set; }
        [Required]
        public string Fact { get; set; }
        [Required]
        [Index]
        public int Rating { get; set; }
        [Required]
        public DateTime ModifiedWhen { get; set; }
        [Required]
        public string ModifiedBy { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}