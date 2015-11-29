using System.Collections.Generic;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Operations;

namespace sc2iqapi.Migrations
{
    public partial class AddUser : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column(type: "int", nullable: false),
                    BattleNetId = table.Column(type: "nvarchar(max)", nullable: true),
                    Created = table.Column(type: "datetimeoffset", nullable: false),
                    PointsEarned = table.Column(type: "int", nullable: false),
                    PointsSpent = table.Column(type: "int", nullable: false),
                    Reputation = table.Column(type: "int", nullable: false),
                    Role = table.Column(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });
        }
        
        public override void Down(MigrationBuilder migration)
        {
            migration.DropTable("User");
        }
    }
}
