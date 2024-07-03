using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetAtlas_The_True_Project.Migrations
{
    public partial class final00 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    IdMembre = table.Column<int>(type: "int", nullable: false),
                    IdModerateur = table.Column<int>(type: "int", nullable: false),
                    IdReport = table.Column<int>(type: "int", nullable: false),
                    MessageAvertissement = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.IdReport);
                    table.ForeignKey(
                        name: "FK_Reports_Membres_IdMembre",
                        column: x => x.IdMembre,
                        principalTable: "Membres",
                        principalColumn: "IdMembre",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_Moderateurs_IdModerateur",
                        column: x => x.IdModerateur,
                        principalTable: "Moderateurs",
                        principalColumn: "IdModerateur",
                        onDelete: ReferentialAction.Cascade);
                });
        }



        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropTable(
                name: "Reports");

            
        }
    }
}
