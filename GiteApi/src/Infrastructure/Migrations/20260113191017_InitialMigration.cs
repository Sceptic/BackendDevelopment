using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblGite",
                columns: table => new
                {
                    giteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    giteNumber = table.Column<int>(type: "int", nullable: false),
                    gitePrice = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    isAvailable = table.Column<bool>(type: "bit", nullable: false),
                    giteAddress = table.Column<string>(type: "char(100)", fixedLength: true, nullable: false),
                    capacityMin = table.Column<int>(type: "int", nullable: false),
                    capacityMax = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblGite", x => x.giteId);
                });

            migrationBuilder.CreateTable(
                name: "tblGiteAmenities",
                columns: table => new
                {
                    giteId = table.Column<int>(type: "int", nullable: false),
                    wifi = table.Column<bool>(type: "bit", nullable: true),
                    bath = table.Column<bool>(type: "bit", nullable: true),
                    shower = table.Column<bool>(type: "bit", nullable: true),
                    hairDryer = table.Column<bool>(type: "bit", nullable: true),
                    smallChild = table.Column<bool>(type: "bit", nullable: true),
                    toiletries = table.Column<bool>(type: "bit", nullable: true),
                    desk = table.Column<bool>(type: "bit", nullable: true),
                    chair = table.Column<bool>(type: "bit", nullable: true),
                    balcony = table.Column<bool>(type: "bit", nullable: true),
                    sofa = table.Column<bool>(type: "bit", nullable: true),
                    sofaBed = table.Column<bool>(type: "bit", nullable: true),
                    miniFridge = table.Column<bool>(type: "bit", nullable: true),
                    kettle = table.Column<bool>(type: "bit", nullable: true),
                    cuttlery = table.Column<bool>(type: "bit", nullable: true),
                    eatingArea = table.Column<bool>(type: "bit", nullable: true),
                    roomService = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblGiteAmenities", x => x.giteId);
                    table.ForeignKey(
                        name: "FK_tblGiteAmenities_tblGite_giteId",
                        column: x => x.giteId,
                        principalTable: "tblGite",
                        principalColumn: "giteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblGiteBed",
                columns: table => new
                {
                    giteBedId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    giteId = table.Column<int>(type: "int", nullable: false),
                    amount1PrBed = table.Column<int>(type: "int", nullable: false),
                    amount2PrBed = table.Column<int>(type: "int", nullable: false),
                    amount3PrBed = table.Column<int>(type: "int", nullable: false),
                    bedSort = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblGiteBed", x => x.giteBedId);
                    table.ForeignKey(
                        name: "FK_tblGiteBed_tblGite_giteId",
                        column: x => x.giteId,
                        principalTable: "tblGite",
                        principalColumn: "giteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblGite_giteNumber",
                table: "tblGite",
                column: "giteNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblGiteBed_giteId",
                table: "tblGiteBed",
                column: "giteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblGiteAmenities");

            migrationBuilder.DropTable(
                name: "tblGiteBed");

            migrationBuilder.DropTable(
                name: "tblGite");
        }
    }
}
