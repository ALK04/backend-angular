using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomPalettes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // La table a déjà été créée via Supabase MCP — IF NOT EXISTS rend la migration idempotente.
            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS custom_palettes (
                    id            UUID        DEFAULT gen_random_uuid() PRIMARY KEY,
                    name          TEXT        NOT NULL DEFAULT 'Ma palette',
                    brand_default TEXT        NOT NULL DEFAULT '#5b3df5',
                    brand_dark    TEXT        NOT NULL DEFAULT '#3a1fd0',
                    surface       TEXT        NOT NULL DEFAULT '#ffffff',
                    page_bg       TEXT        NOT NULL DEFAULT '#eef0f5',
                    ink           TEXT        NOT NULL DEFAULT '#14121f',
                    created_at    TIMESTAMPTZ DEFAULT NOW()
                );
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE IF EXISTS custom_palettes;");
        }
    }
}
