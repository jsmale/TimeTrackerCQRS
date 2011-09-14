using System;
using System.ComponentModel.DataAnnotations;

namespace TimeTrackerCQRS.Commands
{
    public class StopTask : ICommand
    {
        public Guid CommandId { get; set; }
        [Display(Name = "Stop Time")]
        public DateTime? StopTime { get; set; }
        public string Comment { get; set; }
        public int OriginalVersion { get; set; }
    }
}