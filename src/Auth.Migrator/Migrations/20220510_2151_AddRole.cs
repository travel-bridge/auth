using FluentMigrator;
using FluentMigrator.SqlAnywhere;

namespace Auth.Migrator.Migrations;

[TimestampedMigration(2022, 05, 10, 21, 51)]
public class AddRole : MigrationBase
{
    public override void Up()
    {
        CreateTableIfNotExists(
            "auth",
            "Role",
            table =>
            {
                table.WithColumn("Id").AsString(256).NotNullable().PrimaryKey("PK_Role");
                table.WithColumn("ConcurrencyStamp").AsString(int.MaxValue).Nullable();
                table.WithColumn("Name").AsString(256).Nullable();
                table.WithColumn("NormalizedName").AsString(256).Nullable();
            });
        
        CreateUniqueConstraintIfNotExists(
            "auth",
            "Role",
            "UQ_Role_NormalizedName",
            index => index
                .Column("NormalizedName")
                .NonClustered());
    }
}