﻿using System;
using System.ComponentModel.DataAnnotations;

namespace MyConcordiaID.Models.Event
{
    public class NewEvent
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Room { get; set; }

        [Required]
        public DateTime TimeBegin { get; set; }

        [Required]
        public DateTime TimeEnd { get; set; }

        [Required]
        public EventType Type { get; set; }
    }
}
