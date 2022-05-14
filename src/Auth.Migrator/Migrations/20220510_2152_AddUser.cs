using FluentMigrator;
using FluentMigrator.SqlAnywhere;

namespace Auth.Migrator.Migrations;

[TimestampedMigration(2022, 05, 10, 21, 52)]
public class AddUser : MigrationBase
{
    public override void Up()
    {
        CreateTableIfNotExists(
            "auth",
            "User",
            table =>
            {
                table.WithColumn("Id").AsString(256).NotNullable().PrimaryKey("PK_User");
                table.WithColumn("AccessFailedCount").AsInt32().NotNullable();
                table.WithColumn("ConcurrencyStamp").AsString(int.MaxValue).Nullable();
                table.WithColumn("Email").AsString(256).Nullable();
                table.WithColumn("EmailConfirmed").AsBoolean().NotNullable();
                table.WithColumn("LockoutEnabled").AsBoolean().NotNullable();
                table.WithColumn("LockoutEnd").AsDateTimeOffset().Nullable();
                table.WithColumn("NormalizedEmail").AsString(256).Nullable();
                table.WithColumn("NormalizedUserName").AsString(256).Nullable();
                table.WithColumn("PasswordHash").AsString(int.MaxValue).Nullable();
                table.WithColumn("PhoneNumber").AsString(int.MaxValue).Nullable();
                table.WithColumn("PhoneNumberConfirmed").AsBoolean().NotNullable();
                table.WithColumn("SecurityStamp").AsString(int.MaxValue).Nullable();
                table.WithColumn("TwoFactorEnabled").AsBoolean().NotNullable();
                table.WithColumn("UserName").AsString(256).Nullable();
            });
        
        CreateUniqueConstraintIfNotExists(
            "auth",
            "User",
            "UQ_User_NormalizedUserName",
            index => index
                .Column("NormalizedUserName")
                .NonClustered());
        
        CreateIndexIfNotExists(
            "auth",
            "User",
            "IX_User_NormalizedEmail",
            index => index
                .OnColumn("NormalizedEmail").Ascending()
                .WithOptions()
                .NonClustered());
    }
}