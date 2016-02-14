using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwtTest.Models
{
    public class User
    {
        public int Id { get; set; }
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
