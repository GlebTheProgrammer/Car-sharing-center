using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarSharingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedActionNotesTableStructureMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "ActionNotes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "ActionNotes");
        }
    }
}
