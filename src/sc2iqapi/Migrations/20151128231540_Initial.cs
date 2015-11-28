using System.Collections.Generic;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Operations;

namespace sc2iqapi.Migrations
{
    public partial class Initial : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.CreateSequence(
                name: "DefaultSequence",
                type: "bigint",
                startWith: 1L,
                incrementBy: 10);
            migration.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column(type: "int", nullable: false),
                    A1 = table.Column(type: "nvarchar(max)", nullable: true),
                    A2 = table.Column(type: "nvarchar(max)", nullable: true),
                    A3 = table.Column(type: "nvarchar(max)", nullable: true),
                    A4 = table.Column(type: "nvarchar(max)", nullable: true),
                    CorrectAnswerIndex = table.Column(type: "int", nullable: false),
                    CreatedBy = table.Column(type: "int", nullable: false),
                    Difficulty = table.Column(type: "int", nullable: false),
                    Q = table.Column(type: "nvarchar(max)", nullable: true),
                    State = table.Column(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                });
        }
        
        public override void Down(MigrationBuilder migration)
        {
            migration.DropSequence("DefaultSequence");
            migration.DropTable("Question");
        }
    }
}
