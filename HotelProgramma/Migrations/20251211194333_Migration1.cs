using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelProgramma.Migrations
{
    /// <inheritdoc />
    public partial class Migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblAccount",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAccount", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "tblGite",
                columns: table => new
                {
                    GiteNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GitePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    GiteAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CapacityMin = table.Column<int>(type: "int", nullable: false),
                    CapacityMax = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblGite", x => x.GiteNumber);
                });

            migrationBuilder.CreateTable(
                name: "tblHotelRoom",
                columns: table => new
                {
                    RoomNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelroomPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    CapacityMin = table.Column<int>(type: "int", nullable: false),
                    CapacityMax = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblHotelRoom", x => x.RoomNumber);
                });

            migrationBuilder.CreateTable(
                name: "tblReservation",
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
                    table.PrimaryKey("PK_tblReservation", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK_tblReservation_tblAccount_AccountId",
                        column: x => x.AccountId,
                        principalTable: "tblAccount",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblHotelRoomAmenity",
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
                    table.PrimaryKey("PK_tblHotelRoomAmenity", x => x.RoomNumber);
                    table.ForeignKey(
                        name: "FK_tblHotelRoomAmenity_tblHotelRoom_RoomNumber",
                        column: x => x.RoomNumber,
                        principalTable: "tblHotelRoom",
                        principalColumn: "RoomNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblHotelRoomBed",
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
                    table.PrimaryKey("PK_tblHotelRoomBed", x => x.RoomNumber);
                    table.ForeignKey(
                        name: "FK_tblHotelRoomBed_tblHotelRoom_RoomNumber",
                        column: x => x.RoomNumber,
                        principalTable: "tblHotelRoom",
                        principalColumn: "RoomNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblReservationClient",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblReservationClient", x => new { x.ReservationId, x.Firstname, x.Lastname });
                    table.ForeignKey(
                        name: "FK_tblReservationClient_tblReservation_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "tblReservation",
                        principalColumn: "ReservationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblReservationGite",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false),
                    GiteNumber = table.Column<int>(type: "int", nullable: false),
                    GiteDiscount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblReservationGite", x => new { x.ReservationId, x.GiteNumber });
                    table.ForeignKey(
                        name: "FK_tblReservationGite_tblGite_GiteNumber",
                        column: x => x.GiteNumber,
                        principalTable: "tblGite",
                        principalColumn: "GiteNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblReservationGite_tblReservation_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "tblReservation",
                        principalColumn: "ReservationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblReservationHotel",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false),
                    RoomNumber = table.Column<int>(type: "int", nullable: false),
                    HotelroomDiscount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblReservationHotel", x => new { x.ReservationId, x.RoomNumber });
                    table.ForeignKey(
                        name: "FK_tblReservationHotel_tblHotelRoom_RoomNumber",
                        column: x => x.RoomNumber,
                        principalTable: "tblHotelRoom",
                        principalColumn: "RoomNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblReservationHotel_tblReservation_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "tblReservation",
                        principalColumn: "ReservationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblReservation_AccountId",
                table: "tblReservation",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_tblReservationGite_GiteNumber",
                table: "tblReservationGite",
                column: "GiteNumber");

            migrationBuilder.CreateIndex(
                name: "IX_tblReservationHotel_RoomNumber",
                table: "tblReservationHotel",
                column: "RoomNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblHotelRoomAmenity");

            migrationBuilder.DropTable(
                name: "tblHotelRoomBed");

            migrationBuilder.DropTable(
                name: "tblReservationClient");

            migrationBuilder.DropTable(
                name: "tblReservationGite");

            migrationBuilder.DropTable(
                name: "tblReservationHotel");

            migrationBuilder.DropTable(
                name: "tblGite");

            migrationBuilder.DropTable(
                name: "tblHotelRoom");

            migrationBuilder.DropTable(
                name: "tblReservation");

            migrationBuilder.DropTable(
                name: "tblAccount");
        }
    }
}
