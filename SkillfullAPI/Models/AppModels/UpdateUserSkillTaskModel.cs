﻿namespace SkillfullAPI.Models.AppModels
{
    public class UpdateUserSkillTaskModel
    {
        public int UserSkillTaskId { get; set; }
        public string NewTaskName { get; set; }

        public string NewTaskDescription { get; set; }
        public int NewTaskStatusId { get; set; }
    }
}
