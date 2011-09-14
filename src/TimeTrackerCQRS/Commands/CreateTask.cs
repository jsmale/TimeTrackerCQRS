using System;
using System.ComponentModel.DataAnnotations;

namespace TimeTrackerCQRS.Commands
{
    public class CreateTask : ICommand
    {
        public Guid Id { get; set; }
        [Required]
        public string Task { get; set; }
        [Required]
        public string Project { get; set; }
    }
}