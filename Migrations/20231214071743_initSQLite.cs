using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cashbox.Migrations
{
    /// <inheritdoc />
    public partial class initSQLite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MoneyBox",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    money = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneyBox", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    method = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    category = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    role = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    login = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false),
                    pin = table.Column<int>(type: "INTEGER", nullable: false),
                    TFA = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    articul_code = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: true),
                    title = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false),
                    image = table.Column<byte[]>(type: "image", nullable: true),
                    brand = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false),
                    category_id = table.Column<int>(type: "INTEGER", nullable: false),
                    purchase_сost = table.Column<double>(type: "REAL", nullable: false),
                    sell_cost = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.id);
                    table.ForeignKey(
                        name: "FK_Product_ProductCategory",
                        column: x => x.category_id,
                        principalTable: "ProductCategory",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "AuthHistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    datetime = table.Column<DateTime>(type: "datetime", nullable: false),
                    user_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthHistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_AuthHistory_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DailyReport",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    data = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    open_time = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    close_time = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    proceeds = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyReport", x => x.id);
                    table.ForeignKey(
                        name: "FK_DailyReport_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    sell_datetime = table.Column<DateTime>(type: "datetime", nullable: false),
                    payment_method_id = table.Column<int>(type: "INTEGER", nullable: false),
                    user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    sell_cost = table.Column<double>(type: "REAL", nullable: false),
                    discount = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.id);
                    table.ForeignKey(
                        name: "FK_Orders_PaymentMethod",
                        column: x => x.payment_method_id,
                        principalTable: "PaymentMethod",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Orders_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "TFAData",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    code = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TFAData", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_TFAData_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false),
                    surname = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false),
                    patronymic = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false),
                    location = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false),
                    phone = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false),
                    role_id = table.Column<int>(type: "INTEGER", nullable: false),
                    isActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_UserInfo_Roles",
                        column: x => x.role_id,
                        principalTable: "Roles",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_UserInfo_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Refund",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    product_id = table.Column<int>(type: "INTEGER", nullable: false),
                    reason = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false),
                    buy_date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    isPurchased = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refund", x => x.id);
                    table.ForeignKey(
                        name: "FK_Refund_Product",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "INTEGER", nullable: false),
                    amount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.product_id);
                    table.ForeignKey(
                        name: "FK_Stock_Product",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "AutoDReport",
                columns: table => new
                {
                    daily_report_id = table.Column<int>(type: "INTEGER", nullable: false),
                    salary = table.Column<double>(type: "REAL", nullable: false),
                    auto_proceeds = table.Column<double>(type: "REAL", nullable: false),
                    forfeit = table.Column<double>(type: "REAL", nullable: false),
                    award = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoDReport", x => x.daily_report_id);
                    table.ForeignKey(
                        name: "FK_AutoDReport_DailyReport",
                        column: x => x.daily_report_id,
                        principalTable: "DailyReport",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "OrderProduct",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    order_id = table.Column<int>(type: "INTEGER", nullable: false),
                    product_id = table.Column<int>(type: "INTEGER", nullable: false),
                    amount = table.Column<int>(type: "INTEGER", nullable: false),
                    purchase_сost = table.Column<double>(type: "REAL", nullable: false),
                    sell_cost = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProduct", x => x.id);
                    table.ForeignKey(
                        name: "FK_OrderProduct_Orders",
                        column: x => x.order_id,
                        principalTable: "Orders",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_OrderProduct_Product",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthHistory_user_id",
                table: "AuthHistory",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_DailyReport_user_id",
                table: "DailyReport",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_order_id",
                table: "OrderProduct",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_product_id",
                table: "OrderProduct",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_payment_method_id",
                table: "Orders",
                column: "payment_method_id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_user_id",
                table: "Orders",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_category_id",
                table: "Product",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Refund_product_id",
                table: "Refund",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserInfo_role_id",
                table: "UserInfo",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthHistory");

            migrationBuilder.DropTable(
                name: "AutoDReport");

            migrationBuilder.DropTable(
                name: "MoneyBox");

            migrationBuilder.DropTable(
                name: "OrderProduct");

            migrationBuilder.DropTable(
                name: "Refund");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "TFAData");

            migrationBuilder.DropTable(
                name: "UserInfo");

            migrationBuilder.DropTable(
                name: "DailyReport");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ProductCategory");
        }
    }
}
