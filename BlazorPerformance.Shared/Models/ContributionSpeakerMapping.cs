using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPerformance.Shared.Models
{
    public class ContributionSpeakerMapping
    {
        [Key]
        public Guid Id { get; set; }
        public int SpeakerId { get; set; }
        public int ContributionId { get; set; }
    }
}
