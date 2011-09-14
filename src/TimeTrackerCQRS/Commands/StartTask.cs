using System;
using System.ComponentModel.DataAnnotations;

namespace TimeTrackerCQRS.Commands
{
    public class StartTask : ICommand
    {
        public Guid CommandId { get; set; }
        [Display(Name="Start Time")]
        public DateTime? StartTime { get; set; }
        public string Comment { get; set; }
        public int OriginalVersion { get; set; }
    }
}