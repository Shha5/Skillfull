﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class UserSkillTaskDataModel
    {
        public int? Id { get; set; } = null;
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Created { get; set; } = null;
        public DateTime? Modified { get; set; } = null;
        public TaskStatusDataModel Status { get; set; }
    }
}
