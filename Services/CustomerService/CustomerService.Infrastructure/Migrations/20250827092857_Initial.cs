using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CUSTOMERS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CUSTOMER_NUMBER = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    FIRST_NAME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    LAST_NAME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(320)", maxLength: 320, nullable: false),
                    PHONE_NUMBER = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    DATE_OF_BIRTH = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    STREET = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    CITY = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    COUNTRY = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    POSTAL_CODE = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    STATE = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    CUSTOMER_TYPE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ACCOUNT_STATUS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    KYC_STATUS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    RISK_LEVEL = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    BRANCH_CODE = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: true),
                    RELATIONSHIP_MANAGER_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PHOTO_FILE_NAME = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: true),
                    PHOTO_FILE_PATH = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    PHOTO_FILE_SIZE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PHOTO_CONTENT_TYPE = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    PHOTO_WIDTH = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PHOTO_HEIGHT = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PHOTO_INTERNAL_URL = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    PHOTO_UPLOADED_AT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUSTOMERS", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CUSTOMERS_ACCOUNT_STATUS",
                table: "CUSTOMERS",
                column: "ACCOUNT_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_CUSTOMERS_BRANCH_CODE",
                table: "CUSTOMERS",
                column: "BRANCH_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_CUSTOMERS_CREATED_DATE",
                table: "CUSTOMERS",
                column: "CREATED_DATE");

            migrationBuilder.CreateIndex(
                name: "IX_CUSTOMERS_CUSTOMER_NUMBER",
                table: "CUSTOMERS",
                column: "CUSTOMER_NUMBER",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CUSTOMERS_EMAIL",
                table: "CUSTOMERS",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CUSTOMERS_KYC_STATUS",
                table: "CUSTOMERS",
                column: "KYC_STATUS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CUSTOMERS");
        }
    }
}
