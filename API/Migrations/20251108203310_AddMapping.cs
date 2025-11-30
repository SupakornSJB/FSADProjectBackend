using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSADProjectBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProblemSolvers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ProfilePic = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemSolvers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCommentVoteMappings",
                columns: table => new
                {
                    UserSubject = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ProblemId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CommentId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    IsUpvote = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCommentVoteMappings", x => new { x.UserSubject, x.ProblemId, x.CommentId });
                });

            migrationBuilder.CreateTable(
                name: "UserProblemVoteMappings",
                columns: table => new
                {
                    UserSubject = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ProblemId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    IsUpvote = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProblemVoteMappings", x => new { x.UserSubject, x.ProblemId });
                });

            migrationBuilder.CreateTable(
                name: "UserProblemSolverMappings",
                columns: table => new
                {
                    UserSubject = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ProblemSolverId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProblemSolverMappings", x => new { x.ProblemSolverId, x.UserSubject });
                    table.ForeignKey(
                        name: "FK_UserProblemSolverMappings_ProblemSolvers_ProblemSolverId",
                        column: x => x.ProblemSolverId,
                        principalTable: "ProblemSolvers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProblemTagMappings",
                columns: table => new
                {
                    TagId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ProblemId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemTagMappings", x => new { x.TagId, x.ProblemId });
                    table.ForeignKey(
                        name: "FK_ProblemTagMappings_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProblemTagMappings");

            migrationBuilder.DropTable(
                name: "UserCommentVoteMappings");

            migrationBuilder.DropTable(
                name: "UserProblemSolverMappings");

            migrationBuilder.DropTable(
                name: "UserProblemVoteMappings");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "ProblemSolvers");
        }
    }
}
