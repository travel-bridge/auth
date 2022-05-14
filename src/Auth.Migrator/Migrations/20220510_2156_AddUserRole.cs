using FluentMigrator;

namespace Auth.Migrator.Migrations;

[TimestampedMigration(2022, 05, 10, 21, 56)]
public class AddUserRole : MigrationBase
{
    public override void Up()
    {
        CreateTableIfNotExists(
            "auth",
            "UserRole",
            table =>
            {
                table.WithColumn("UserId").AsString(256).NotNullable();
                table.WithColumn("RoleId").AsString(256).NotNullable();
            });
        
        CreatePrimaryKeyIfNotExists(
            "auth",
            "UserRole",
            "PK_UserRole",
            constraint => constraint
                .OnTable("UserRole")
                .WithSchema("auth")
                .Columns("UserId", "RoleId"));
        
        CreateForeignKeyIfNotExists(
            "auth",
            "UserRole",
            "FK_UserRole_UserId",
            constraint => constraint
                .FromTable("UserRole").InSchema("auth").ForeignColumn("UserId")
                .ToTable("User").InSchema("auth").PrimaryColumn("Id"));
        
        CreateForeignKeyIfNotExists(
            "auth",
            "UserRole",
            "FK_UserRole_RoleId",
            constraint => constraint
                .FromTable("UserRole").InSchema("auth").ForeignColumn("RoleId")
                .ToTable("Role").InSchema("auth").PrimaryColumn("Id"));
        
        CreateIndexIfNotExists(
            "auth",
            "UserRole",
            "IX_UserRole_UserId",
            index => index
                .OnColumn("UserId").Ascending()
                .WithOptions()
                .NonClustered());
        
        CreateIndexIfNotExists(
            "auth",
            "UserRole",
            "IX_UserRole_RoleId",
            index => index
                .OnColumn("RoleId").Ascending()
                .WithOptions()
                .NonClustered());
    }
}