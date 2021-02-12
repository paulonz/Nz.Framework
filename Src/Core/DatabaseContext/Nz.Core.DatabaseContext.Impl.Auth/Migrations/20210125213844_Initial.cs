using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nz.Core.DatabaseContext.Impl.Auth.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    password = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    is_email_validated = table.Column<bool>(type: "boolean", nullable: false),
                    recovery_password_token = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    created_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<long>(type: "bigint", nullable: true),
                    excluded_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    excluded_by = table.Column<long>(type: "bigint", nullable: true),
                    updated_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_role",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    role_type = table.Column<int>(type: "integer", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<long>(type: "bigint", nullable: true),
                    excluded_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    excluded_by = table.Column<long>(type: "bigint", nullable: true),
                    updated_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_role", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_role_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_role_user_id",
                table: "user_role",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_role");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
