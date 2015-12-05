using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sc2iqapi.Models
{
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [Required]
        [JsonProperty(PropertyName = "battleNetId")]
        public string BattleNetId { get; set; }

        [JsonProperty(PropertyName = "pointsEarned")]
        public int PointsEarned { get; set; }

        [JsonProperty(PropertyName = "pointsSpent")]
        public int PointsSpent { get; set; }

        [JsonProperty(PropertyName = "reputation")]
        public int Reputation { get; set; }

        [JsonProperty(PropertyName = "role")]
        public UserRole Role { get; set; }

        [JsonProperty(PropertyName = "created")]
        public DateTimeOffset Created { get; set; }
    }

    public enum UserRole
    {
        User,
        Admin,
        Moderator
    }
}
