using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ID3.Finance.Binance.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KlineResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Open = table.Column<string>(type: "text", nullable: false),
                    High = table.Column<string>(type: "text", nullable: false),
                    Low = table.Column<string>(type: "text", nullable: false),
                    Close = table.Column<string>(type: "text", nullable: false),
                    WeightedAveragePrice = table.Column<string>(type: "text", nullable: false),
                    BaseAssetVolume = table.Column<string>(type: "text", nullable: false),
                    QuoteAssetVolume = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KlineResults", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KlineResults");
        }
    }
}
