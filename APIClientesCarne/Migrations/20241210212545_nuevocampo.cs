﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIClientesCarne.Migrations
{
    /// <inheritdoc />
    public partial class nuevocampo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanUpdateRead",
                table: "RolePermisos",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanUpdateRead",
                table: "RolePermisos");
        }
    }
}
