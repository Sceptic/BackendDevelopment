using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblReservation",
                columns: table => new
                {
                    reservationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    accountId = table.Column<int>(type: "int", nullable: false),
                    reservationStatus = table.Column<string>(type: "text", nullable: false),
                    paymentStatus = table.Column<string>(type: "text", nullable: false),
                    reservationPrice = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    discount = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false),
                    touristTarif = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false),
                    reservationStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    reservationEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblReservation", x => x.reservationId);
                });

            migrationBuilder.CreateTable(
                name: "tblReservationCamping",
                columns: table => new
                {
                    reservationId = table.Column<int>(type: "int", nullable: false),
                    campingId = table.Column<int>(type: "int", nullable: false),
                    campingDiscount = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblReservationCamping", x => new { x.reservationId, x.campingId });
                    table.ForeignKey(
                        name: "FK_tblReservationCamping_tblReservation_reservationId",
                        column: x => x.reservationId,
                        principalTable: "tblReservation",
                        principalColumn: "reservationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblReservationClient",
                columns: table => new
                {
                    reservationId = table.Column<int>(type: "int", nullable: false),
                    firstName = table.Column<string>(type: "char(50)", fixedLength: true, nullable: false),
                    lastName = table.Column<string>(type: "char(50)", fixedLength: true, nullable: false),
                    birthDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblReservationClient", x => new { x.reservationId, x.firstName, x.lastName });
                    table.ForeignKey(
                        name: "FK_tblReservationClient_tblReservation_reservationId",
                        column: x => x.reservationId,
                        principalTable: "tblReservation",
                        principalColumn: "reservationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblReservationFacility",
                columns: table => new
                {
                    reservationId = table.Column<int>(type: "int", nullable: false),
                    facility = table.Column<string>(type: "char(50)", fixedLength: true, nullable: false),
                    facilityDiscount = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblReservationFacility", x => new { x.reservationId, x.facility });
                    table.ForeignKey(
                        name: "FK_tblReservationFacility_tblReservation_reservationId",
                        column: x => x.reservationId,
                        principalTable: "tblReservation",
                        principalColumn: "reservationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblReservationGite",
                columns: table => new
                {
                    reservationId = table.Column<int>(type: "int", nullable: false),
                    giteId = table.Column<int>(type: "int", nullable: false),
                    giteDiscount = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblReservationGite", x => new { x.reservationId, x.giteId });
                    table.ForeignKey(
                        name: "FK_tblReservationGite_tblReservation_reservationId",
                        column: x => x.reservationId,
                        principalTable: "tblReservation",
                        principalColumn: "reservationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblReservationHotelroom",
                columns: table => new
                {
                    reservationId = table.Column<int>(type: "int", nullable: false),
                    roomId = table.Column<int>(type: "int", nullable: false),
                    hotelroomDiscount = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblReservationHotelroom", x => new { x.reservationId, x.roomId });
                    table.ForeignKey(
                        name: "FK_tblReservationHotelroom_tblReservation_reservationId",
                        column: x => x.reservationId,
                        principalTable: "tblReservation",
                        principalColumn: "reservationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblReservationRestaurant",
                columns: table => new
                {
                    reservationRestaurantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reservationId = table.Column<int>(type: "int", nullable: false),
                    tableId = table.Column<int>(type: "int", nullable: false),
                    tableReservationStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    tableReservationEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    tableBill = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    tableDiscount = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblReservationRestaurant", x => x.reservationRestaurantId);
                    table.ForeignKey(
                        name: "FK_tblReservationRestaurant_tblReservation_reservationId",
                        column: x => x.reservationId,
                        principalTable: "tblReservation",
                        principalColumn: "reservationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblVehicle",
                columns: table => new
                {
                    reservationId = table.Column<int>(type: "int", nullable: false),
                    registrationPlate = table.Column<string>(type: "char(50)", fixedLength: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblVehicle", x => new { x.reservationId, x.registrationPlate });
                    table.ForeignKey(
                        name: "FK_tblVehicle_tblReservation_reservationId",
                        column: x => x.reservationId,
                        principalTable: "tblReservation",
                        principalColumn: "reservationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblReservationRestaurant_reservationId",
                table: "tblReservationRestaurant",
                column: "reservationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblReservationCamping");

            migrationBuilder.DropTable(
                name: "tblReservationClient");

            migrationBuilder.DropTable(
                name: "tblReservationFacility");

            migrationBuilder.DropTable(
                name: "tblReservationGite");

            migrationBuilder.DropTable(
                name: "tblReservationHotelroom");

            migrationBuilder.DropTable(
                name: "tblReservationRestaurant");

            migrationBuilder.DropTable(
                name: "tblVehicle");

            migrationBuilder.DropTable(
                name: "tblReservation");
        }
    }
}
