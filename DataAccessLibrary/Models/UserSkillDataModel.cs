using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class UserSkillDataModel
    {
        public int? Id { get; set; } = null;
        public string SkillId { get; set; }
        public string SkillName { get; set; }
        public int SkillAssessmentId { get; set; }
        
    }
}
