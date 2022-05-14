using FluentMigrator;

namespace Auth.Migrator.Migrations;

[TimestampedMigration(2022, 05, 10, 21, 50)]
public class AddSchema : MigrationBase
{
    public override void Up()
    {
        Create.Schema("auth");
    }
}