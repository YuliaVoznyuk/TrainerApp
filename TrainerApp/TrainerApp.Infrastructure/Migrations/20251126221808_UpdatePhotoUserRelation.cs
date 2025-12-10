using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainerApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePhotoUserRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_AspNetUsers_ClientId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_AspNetUsers_TrainerId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_ClientId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_TrainerId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "TrainerId",
                table: "Photos");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Photos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Photos_UserId",
                table: "Photos",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_AspNetUsers_UserId",
                table: "Photos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_AspNetUsers_UserId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_UserId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Photos");

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "Photos",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TrainerId",
                table: "Photos",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_ClientId",
                table: "Photos",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_TrainerId",
                table: "Photos",
                column: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_AspNetUsers_ClientId",
                table: "Photos",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_AspNetUsers_TrainerId",
                table: "Photos",
                column: "TrainerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
