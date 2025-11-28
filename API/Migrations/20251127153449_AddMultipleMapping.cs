using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSADProjectBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddMultipleMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "JoinedAt",
                table: "UserProblemSolverMappings",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<DateTime>(
                name: "InvitedAt",
                table: "UserProblemSolverMappings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "UserProblemSolverMappings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProblemSolvers",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvitedAt",
                table: "UserProblemSolverMappings");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "UserProblemSolverMappings");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProblemSolvers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "JoinedAt",
                table: "UserProblemSolverMappings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
