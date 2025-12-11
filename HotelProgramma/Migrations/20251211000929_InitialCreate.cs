using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelProgramma.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "Gites",
                columns: table => new
                {
                    GiteNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GitePrice = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    GiteAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gites", x => x.GiteNumber);
                });

            migrationBuilder.CreateTable(
                name: "HotelRooms",
                columns: table => new
                {
                    RoomNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelroomPrice = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelRooms", x => x.RoomNumber);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    ReservationStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false),
                    ReservationStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReservationEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK_Reservations_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HotelRoomAmenities",
                columns: table => new
                {
                    RoomNumber = table.Column<int>(type: "int", nullable: false),
                    Wifi = table.Column<bool>(type: "bit", nullable: true),
                    Bath = table.Column<bool>(type: "bit", nullable: true),
                    Shower = table.Column<bool>(type: "bit", nullable: true),
                    Hairdryer = table.Column<bool>(type: "bit", nullable: true),
                    Smallchild = table.Column<bool>(type: "bit", nullable: true),
                    Toiletries = table.Column<bool>(type: "bit", nullable: true),
                    Desk = table.Column<bool>(type: "bit", nullable: true),
                    Chair = table.Column<bool>(type: "bit", nullable: true),
                    Balcony = table.Column<bool>(type: "bit", nullable: true),
                    Sofa = table.Column<bool>(type: "bit", nullable: true),
                    Sofabed = table.Column<bool>(type: "bit", nullable: true),
                    Minifridge = table.Column<bool>(type: "bit", nullable: true),
                    Kettle = table.Column<bool>(type: "bit", nullable: true),
                    Cuttlery = table.Column<bool>(type: "bit", nullable: true),
                    Eatingarea = table.Column<bool>(type: "bit", nullable: true),
                    Roomservice = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelRoomAmenities", x => x.RoomNumber);
                    table.ForeignKey(
                        name: "FK_HotelRoomAmenities_HotelRooms_RoomNumber",
                        column: x => x.RoomNumber,
                        principalTable: "HotelRooms",
                        principalColumn: "RoomNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HotelRoomBeds",
                columns: table => new
                {
                    RoomNumber = table.Column<int>(type: "int", nullable: false),
                    Amount1PrBed = table.Column<int>(type: "int", nullable: false),
                    Amount2PrBed = table.Column<int>(type: "int", nullable: false),
                    Amount3PrBed = table.Column<int>(type: "int", nullable: false),
                    BedSort = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelRoomBeds", x => x.RoomNumber);
                    table.ForeignKey(
                        name: "FK_HotelRoomBeds_HotelRooms_RoomNumber",
                        column: x => x.RoomNumber,
                        principalTable: "HotelRooms",
                        principalColumn: "RoomNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservationClients",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationClients", x => new { x.ReservationId, x.Firstname, x.Lastname });
                    table.ForeignKey(
                        name: "FK_ReservationClients_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "ReservationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservationGites",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false),
                    GiteNumber = table.Column<int>(type: "int", nullable: false),
                    GiteDiscount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationGites", x => new { x.ReservationId, x.GiteNumber });
                    table.ForeignKey(
                        name: "FK_ReservationGites_Gites_GiteNumber",
                        column: x => x.GiteNumber,
                        principalTable: "Gites",
                        principalColumn: "GiteNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationGites_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "ReservationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservationHotels",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false),
                    RoomNumber = table.Column<int>(type: "int", nullable: false),
                    HotelroomDiscount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationHotels", x => new { x.ReservationId, x.RoomNumber });
                    table.ForeignKey(
                        name: "FK_ReservationHotels_HotelRooms_RoomNumber",
                        column: x => x.RoomNumber,
                        principalTable: "HotelRooms",
                        principalColumn: "RoomNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationHotels_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "ReservationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationGites_GiteNumber",
                table: "ReservationGites",
                column: "GiteNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationHotels_RoomNumber",
                table: "ReservationHotels",
                column: "RoomNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_AccountId",
                table: "Reservations",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelRoomAmenities");

            migrationBuilder.DropTable(
                name: "HotelRoomBeds");

            migrationBuilder.DropTable(
                name: "ReservationClients");

            migrationBuilder.DropTable(
                name: "ReservationGites");

            migrationBuilder.DropTable(
                name: "ReservationHotels");

            migrationBuilder.DropTable(
                name: "Gites");

            migrationBuilder.DropTable(
                name: "HotelRooms");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
