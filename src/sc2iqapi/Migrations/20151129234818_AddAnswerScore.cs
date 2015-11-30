using System.Collections.Generic;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Operations;

namespace sc2iqapi.Migrations
{
    public partial class AddAnswerScore : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.CreateTable(
                name: "Score",
                columns: table => new
                {
                    Id = table.Column(type: "int", nullable: false),
                    Duration = table.Column(type: "int", nullable: false),
                    Points = table.Column(type: "int", nullable: false),
                    Submitted = table.Column(type: "datetimeoffset", nullable: false),
                    UserId = table.Column(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Score", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Score_User_UserId",
                        columns: x => x.UserId,
                        referencedTable: "User",
                        referencedColumn: "Id");
                });
            migration.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column(type: "int", nullable: false),
                    AnswerIndex = table.Column(type: "int", nullable: false),
                    Duration = table.Column(type: "int", nullable: false),
                    Points = table.Column(type: "int", nullable: false),
                    QuestionId = table.Column(type: "int", nullable: false),
                    ScoreId = table.Column(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answer_Question_QuestionId",
                        columns: x => x.QuestionId,
                        referencedTable: "Question",
                        referencedColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Answer_Score_ScoreId",
                        columns: x => x.ScoreId,
                        referencedTable: "Score",
                        referencedColumn: "Id");
                });
        }
        
        public override void Down(MigrationBuilder migration)
        {
            migration.DropTable("Answer");
            migration.DropTable("Score");
        }
    }
}
