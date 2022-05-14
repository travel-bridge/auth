using System.Data;
using FluentMigrator;

namespace Auth.Migrator.Migrations;

[TimestampedMigration(2022, 05, 10, 21, 55)]
public class AddUserLogin : MigrationBase
{
    public override void Up()
    {
        CreateTableIfNotExists(
            "auth",
            "UserLogin",
            table =>
            {
                table.WithColumn("LoginProvider").AsString(256).NotNullable();
                table.WithColumn("ProviderKey").AsString(256).NotNullable();
                table.WithColumn("ProviderDisplayName").AsString(int.MaxValue).Nullable();
                table.WithColumn("UserId").AsString(256).NotNullable();
            });
        
        CreatePrimaryKeyIfNotExists(
            "auth",
            "UserLogin",
            "PK_UserLogin",
            constraint => constraint
                .OnTable("UserLogin")
                .WithSchema("auth")
                .Columns("LoginProvider", "ProviderKey"));
        
        CreateForeignKeyIfNotExists(
            "auth",
            "UserLogin",
            "FK_UserLogin_UserId",
            constraint => constraint
                .FromTable("UserLogin").InSchema("auth").ForeignColumn("UserId")
                .ToTable("User").InSchema("auth").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade));
        
        CreateIndexIfNotExists(
            "auth",
            "UserLogin",
            "IX_UserLogin_UserId",
            index => index
                .OnColumn("UserId").Ascending()
                .WithOptions()
                .NonClustered());
    }
}