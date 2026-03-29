using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingAspNetUsersColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('AspNetUsers', 'Email') IS NULL
BEGIN
    ALTER TABLE [AspNetUsers] ADD [Email] nvarchar(256) NULL;
END
ELSE
BEGIN
    ALTER TABLE [AspNetUsers] ALTER COLUMN [Email] nvarchar(256) NULL;
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('AspNetUsers', 'IsActive') IS NULL
BEGIN
    ALTER TABLE [AspNetUsers] ADD [IsActive] bit NOT NULL CONSTRAINT [DF_AspNetUsers_IsActive] DEFAULT(1);
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('AspNetUsers', 'Position') IS NULL
BEGIN
    ALTER TABLE [AspNetUsers] ADD [Position] nvarchar(max) NULL;
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('AspNetUsers', 'Position') IS NOT NULL
BEGIN
    ALTER TABLE [AspNetUsers] DROP COLUMN [Position];
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('AspNetUsers', 'IsActive') IS NOT NULL
BEGIN
    DECLARE @constraintName NVARCHAR(200);
    SELECT @constraintName = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    WHERE dc.parent_object_id = OBJECT_ID('AspNetUsers') AND c.name = 'IsActive';

    IF @constraintName IS NOT NULL
        EXEC('ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @constraintName + ']');

    ALTER TABLE [AspNetUsers] DROP COLUMN [IsActive];
END
");
        }
    }
}
