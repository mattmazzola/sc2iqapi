using System.Collections.Generic;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Operations;

namespace sc2iqapi.Migrations
{
    public partial class AddPoll : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.CreateTable(
                name: "Poll",
                columns: table => new
                {
                    Id = table.Column(type: "int", nullable: false),
                    Created = table.Column(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column(type: "int", nullable: true),
                    Description = table.Column(type: "nvarchar(max)", nullable: true),
                    Title = table.Column(type: "nvarchar(max)", nullable: true),
                    Votes = table.Column(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poll", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Poll_User_CreatedById",
                        columns: x => x.CreatedById,
                        referencedTable: "User",
                        referencedColumn: "Id");
                });
        }
        
        public override void Down(MigrationBuilder migration)
        {
            migration.DropTable("Poll");
        }
    }
}
