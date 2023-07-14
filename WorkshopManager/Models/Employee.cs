﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopManager.Models
{
    public class Employee
    {
        public long ID { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string TypeOfJob { get; set; }
        public Status Status { get; set; }
    }

    public enum Status
    {
        Working,
        OnVacation,
        Fired
    }
}