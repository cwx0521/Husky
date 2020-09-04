﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Husky.TwoFactor.Data.Migrations
{
    public partial class Init_TwoFactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TwoFactorCodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserIdString = table.Column<string>(type: "varchar(36)", nullable: true),
                    SentTo = table.Column<string>(type: "varchar(50)", nullable: true),
                    Code = table.Column<string>(type: "varchar(8)", nullable: true),
                    ErrorTimes = table.Column<int>(nullable: false),
                    IsUsed = table.Column<bool>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwoFactorCodes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TwoFactorCodes");
        }
    }
}
