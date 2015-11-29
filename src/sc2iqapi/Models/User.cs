using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sc2iqapi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string BattleNetId { get; set; }

        public int PointsEarned { get; set; }

        public int PointsSpent { get; set; }

        public int Reputation { get; set; }

        public UserRole Role { get; set; }

        public DateTimeOffset Created { get; set; }
    }

    public enum UserRole
    {
        User,
        Admin,
        Moderator
    }
}
