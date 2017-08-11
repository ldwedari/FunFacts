using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunFacts.Models
{
    public interface IFunFact
    {
        [Range(0, long.MaxValue, ErrorMessage = "Please enter valid positive number.")]
        long Id { get; set; }
        [Required]
        string Fact { get; set; }
        [Required]
        int Rating { get; set; }

        [Required]
        DateTime ModifiedWhen { get; set; }
        [Required]
        string ModifiedBy { get; set; }
        [Timestamp]
        byte[] RowVersion { get; set; }
    }
}
