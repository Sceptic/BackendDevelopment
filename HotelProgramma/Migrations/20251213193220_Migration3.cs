using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelProgramma.Migrations
{
    /// <inheritdoc />
    public partial class Migration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblReservationGite_tblGite_GiteNumber",
                table: "tblReservationGite");

            migrationBuilder.DropForeignKey(
                name: "FK_tblReservationHotel_tblHotelRoom_RoomNumber",
                table: "tblReservationHotel");

            migrationBuilder.AddForeignKey(
                name: "FK_tblReservationGite_tblGite_GiteNumber",
                table: "tblReservationGite",
                column: "GiteNumber",
                principalTable: "tblGite",
                principalColumn: "GiteNumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblReservationHotel_tblHotelRoom_RoomNumber",
                table: "tblReservationHotel",
                column: "RoomNumber",
                principalTable: "tblHotelRoom",
                principalColumn: "RoomNumber",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblReservationGite_tblGite_GiteNumber",
                table: "tblReservationGite");

            migrationBuilder.DropForeignKey(
                name: "FK_tblReservationHotel_tblHotelRoom_RoomNumber",
                table: "tblReservationHotel");

            migrationBuilder.AddForeignKey(
                name: "FK_tblReservationGite_tblGite_GiteNumber",
                table: "tblReservationGite",
                column: "GiteNumber",
                principalTable: "tblGite",
                principalColumn: "GiteNumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tblReservationHotel_tblHotelRoom_RoomNumber",
                table: "tblReservationHotel",
                column: "RoomNumber",
                principalTable: "tblHotelRoom",
                principalColumn: "RoomNumber",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
