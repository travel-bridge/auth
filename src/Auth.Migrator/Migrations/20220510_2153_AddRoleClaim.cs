using System.Data;
using FluentMigrator;

namespace Auth.Migrator.Migrations;

[TimestampedMigration(2022, 05, 10, 21, 53)]
public class AddRoleClaim : MigrationBase
{
    public override void Up()
    {
        CreateTableIfNotExists(
            "auth",
            "RoleClaim",
            table =>
            {
                table.WithColumn("Id").AsInt32().NotNullable().PrimaryKey("PK_RoleClaim").Identity();
                table.WithColumn("ClaimType").AsString(int.MaxValue).Nullable();
                table.WithColumn("ClaimValue").AsString(int.MaxValue).Nullable();
                table.WithColumn("RoleId").AsString(256).NotNullable();
            });
        
        CreateForeignKeyIfNotExists(
            "auth",
            "RoleClaim",
            "FK_RoleClaim_RoleId",
            constraint => constraint
                .FromTable("RoleClaim").InSchema("auth").ForeignColumn("RoleId")
                .ToTable("Role").InSchema("auth").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade));
        
        CreateIndexIfNotExists(
            "auth",
            "RoleClaim",
            "IX_RoleClaim_RoleId",
            index => index
                .OnColumn("RoleId").Ascending()
                .WithOptions()
                .NonClustered());
    }
}