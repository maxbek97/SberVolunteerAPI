using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SberVolunteerAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id_user = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_login = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_role = table.Column<string>(type: "enum('organiser','volunteer')", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_surname = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_middlename = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    volunteers_hours = table.Column<uint>(type: "int unsigned", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id_user);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    Id_event = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    event_title = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    event_description = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    datetime_start = table.Column<DateTime>(type: "datetime", nullable: false),
                    datetime_end = table.Column<DateTime>(type: "datetime", nullable: false),
                    creationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    creator_id = table.Column<uint>(type: "int unsigned", nullable: false),
                    event_state = table.Column<string>(type: "enum('active','closed','cancelled')", nullable: false, defaultValueSql: "'active'", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id_event);
                    table.ForeignKey(
                        name: "events_ibfk_1",
                        column: x => x.creator_id,
                        principalTable: "users",
                        principalColumn: "Id_user");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "events_to_volunteers",
                columns: table => new
                {
                    Id_record = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_event = table.Column<uint>(type: "int unsigned", nullable: false),
                    Id_volunteer = table.Column<uint>(type: "int unsigned", nullable: false),
                    request_status = table.Column<string>(type: "enum('pending','approved','rejected')", nullable: true, defaultValueSql: "'pending'", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    visit_status = table.Column<string>(type: "enum('unknown','came','absent')", nullable: true, defaultValueSql: "'unknown'", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id_record);
                    table.ForeignKey(
                        name: "events_to_volunteers_ibfk_1",
                        column: x => x.Id_event,
                        principalTable: "events",
                        principalColumn: "Id_event");
                    table.ForeignKey(
                        name: "events_to_volunteers_ibfk_2",
                        column: x => x.Id_volunteer,
                        principalTable: "users",
                        principalColumn: "Id_user");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "creator_id",
                table: "events",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "event_title",
                table: "events",
                column: "event_title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Id_event",
                table: "events_to_volunteers",
                columns: new[] { "Id_event", "Id_volunteer" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Id_volunteer",
                table: "events_to_volunteers",
                column: "Id_volunteer");

            migrationBuilder.CreateIndex(
                name: "user_login",
                table: "users",
                column: "user_login",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "events_to_volunteers");

            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
