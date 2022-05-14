using System.Data;
using FluentMigrator;

namespace Auth.Migrator.Migrations;

[TimestampedMigration(2022, 05, 10, 21, 54)]
public class AddUserClaim : MigrationBase
{
    public override void Up()
    {
        CreateTableIfNotExists(
            "auth",
            "UserClaim",
            table =>
            {
                table.WithColumn("Id").AsInt32().NotNullable().PrimaryKey("PK_UserClaim").Identity();
                table.WithColumn("ClaimType").AsString(int.MaxValue).Nullable();
                table.WithColumn("ClaimValue").AsString(int.MaxValue).Nullable();
                table.WithColumn("UserId").AsString(256).NotNullable();
            });
        
        CreateForeignKeyIfNotExists(
            "auth",
            "UserClaim",
            "FK_UserClaim_UserId",
            constraint => constraint
                .FromTable("UserClaim").InSchema("auth").ForeignColumn("UserId")
                .ToTable("User").InSchema("auth").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade));
        
        CreateIndexIfNotExists(
            "auth",
            "UserClaim",
            "IX_UserClaim_UserId",
            index => index
                .OnColumn("UserId").Ascending()
                .WithOptions()
                .NonClustered());
    }
}