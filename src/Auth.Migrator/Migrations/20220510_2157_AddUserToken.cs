using System.Data;
using FluentMigrator;

namespace Auth.Migrator.Migrations;

[TimestampedMigration(2022, 05, 10, 21, 57)]
public class AddUserToken : MigrationBase
{
    public override void Up()
    {
        CreateTableIfNotExists(
            "auth",
            "UserToken",
            table =>
            {
                table.WithColumn("UserId").AsString(256).NotNullable();
                table.WithColumn("LoginProvider").AsString(256).NotNullable();
                table.WithColumn("Name").AsString(256).NotNullable();
                table.WithColumn("Value").AsString(int.MaxValue).Nullable();
            });
        
        CreatePrimaryKeyIfNotExists(
            "auth",
            "UserToken",
            "PK_UserToken",
            constraint => constraint
                .OnTable("UserToken")
                .WithSchema("auth")
                .Columns("UserId", "LoginProvider", "Name"));
        
        CreateForeignKeyIfNotExists(
            "auth",
            "UserToken",
            "FK_UserToken_UserId",
            constraint => constraint
                .FromTable("UserToken").InSchema("auth").ForeignColumn("UserId")
                .ToTable("User").InSchema("auth").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade));
        
        CreateIndexIfNotExists(
            "auth",
            "UserToken",
            "IX_UserToken_UserId",
            index => index
                .OnColumn("UserId").Ascending()
                .WithOptions()
                .NonClustered());
    }
}