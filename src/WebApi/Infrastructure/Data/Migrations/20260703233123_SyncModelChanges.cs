using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Infrastructure.Migrations;

/// <inheritdoc />
public partial class SyncModelChanges : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "IsActive", table: "categories");

        migrationBuilder.RenameColumn(
            name: "CreatedAt",
            table: "categories",
            newName: "created_at"
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "created_at",
            table: "categories",
            newName: "CreatedAt"
        );

        migrationBuilder.AddColumn<bool>(
            name: "IsActive",
            table: "categories",
            type: "boolean",
            nullable: false,
            defaultValue: false
        );
    }
}
