using System.Collections.Generic;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Operations;

namespace sc2iqapi.Migrations
{
    public partial class CreatedByUsersAndManyToMany : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.DropColumn(name: "CreatedBy", table: "Question");
            migration.DropColumn(name: "CreatedBy", table: "Tag");
            migration.CreateTable(
                name: "QuestionTag",
                columns: table => new
                {
                    QuestionId = table.Column(type: "int", nullable: false),
                    TagId = table.Column(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTag", x => new { x.QuestionId, x.TagId });
                    table.ForeignKey(
                        name: "FK_QuestionTag_Question_QuestionId",
                        columns: x => x.QuestionId,
                        referencedTable: "Question",
                        referencedColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuestionTag_Tag_TagId",
                        columns: x => x.TagId,
                        referencedTable: "Tag",
                        referencedColumn: "Id");
                });
            migration.AddColumn(
                name: "CreatedById",
                table: "Question",
                type: "int",
                nullable: true);
            migration.AddForeignKey(
                name: "FK_Question_User_CreatedById",
                table: "Question",
                column: "CreatedById",
                referencedTable: "User",
                referencedColumn: "Id");
            migration.AddColumn(
                name: "CreatedById",
                table: "Tag",
                type: "int",
                nullable: true);
            migration.AddForeignKey(
                name: "FK_Tag_User_CreatedById",
                table: "Tag",
                column: "CreatedById",
                referencedTable: "User",
                referencedColumn: "Id");
        }
        
        public override void Down(MigrationBuilder migration)
        {
            migration.DropForeignKey(name: "FK_Question_User_CreatedById", table: "Question");
            migration.DropForeignKey(name: "FK_Tag_User_CreatedById", table: "Tag");
            migration.DropColumn(name: "CreatedById", table: "Question");
            migration.DropColumn(name: "CreatedById", table: "Tag");
            migration.DropTable("QuestionTag");
            migration.AddColumn(
                name: "CreatedBy",
                table: "Question",
                type: "int",
                nullable: false);
            migration.AddColumn(
                name: "CreatedBy",
                table: "Tag",
                type: "int",
                nullable: false);
        }
    }
}
