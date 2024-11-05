using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobBoard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobListings",
                columns: table => new
                {
                    JobID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CityAndState = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    PayRange = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    JobType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobPostedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FullDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobListings", x => x.JobID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "JobListings",
                columns: new[] { "JobID", "CityAndState", "CompanyName", "Email", "FullDescription", "JobPostedDate", "JobTitle", "JobType", "PayRange" },
                values: new object[,]
                {
                    { new Guid("0e1f6488-acd8-44da-8496-8f7690fe711d"), "Phoenix, AZ", "Supportify", "apply@supportify.com", "We need a customer support specialist to assist clients with product-related inquiries. Previous support experience is preferred.", new DateTime(2024, 11, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Customer Support Specialist", "Remote", "$40,000 - $60,000" },
                    { new Guid("0e865692-297b-42d3-9518-cfa48cdf4e47"), "Miami, FL", "DesignPro", "talent@designpro.com", "Looking for a creative graphic designer to create visuals for our brand. Experience with Adobe Suite is required.", new DateTime(2024, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Graphic Designer", "FullTime", "$55,000 - $80,000" },
                    { new Guid("2ca7dd51-69a2-44b9-aa5a-b9932eab315c"), "Los Angeles, CA", "BrandMakers", "recruitment@brandmakers.com", "We are looking for a marketing specialist to handle our social media campaigns and drive customer engagement.", new DateTime(2024, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), "Marketing Specialist", "PartTime", "$60,000 - $85,000" },
                    { new Guid("44386e74-2cef-405f-816c-5a7ae480ab95"), "Chicago, IL", "SalesHub", "join@saleshub.com", "We are hiring sales representatives to expand our client base. Experience in B2B sales is a plus.", new DateTime(2024, 10, 29, 0, 0, 0, 0, DateTimeKind.Utc), "Sales Representative", "Internship", "$50,000 - $75,000" },
                    { new Guid("7bb130c5-4492-43a0-bf26-b79cefb9c395"), "Seattle, WA", "WriteWorks", "hr@writeworks.com", "Seeking a content writer with experience in creating SEO-friendly articles and blog posts.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Content Writer", "Remote", "$45,000 - $65,000" },
                    { new Guid("aabb5fcb-35f7-4cd1-b97e-ff3e6e737a4d"), "Denver, CO", "BuildRight", "careers@buildright.com", "Experienced project manager needed for overseeing construction projects. PMP certification preferred.", new DateTime(2024, 10, 28, 0, 0, 0, 0, DateTimeKind.Utc), "Project Manager", "PartTime", "$75,000 - $110,000" },
                    { new Guid("bfbc1319-b732-4d1c-a23b-afb473014f1f"), "Sacramento, CA", "Tech Innovations", "hr@techinnovations.com", "Seeking a software engineer with experience in web and mobile application development. Must be proficient in C# and JavaScript.", new DateTime(2024, 10, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Software Engineer", "FullTime", "$80,000 - $120,000" },
                    { new Guid("d5b88e15-0303-4382-9f27-a1c6f0a97b66"), "Austin, TX", "Creative Solutions", "contact@creativesolutions.com", "Seeking a UX/UI designer to create user-centered designs for our web and mobile products.", new DateTime(2024, 10, 25, 0, 0, 0, 0, DateTimeKind.Utc), "UX/UI Designer", "PartTime", "$70,000 - $95,000" },
                    { new Guid("dcc51691-e4de-4571-9969-b4b0a56fa689"), "Boston, MA", "FinServe", "jobs@finserve.com", "We are looking for a financial analyst with experience in forecasting and budgeting. CFA certification is a plus.", new DateTime(2024, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Financial Analyst", "FullTime", "$85,000 - $110,000" },
                    { new Guid("f358f000-46d2-4d56-95bc-05375645a6fc"), "New York, NY", "Big Data Analytics", "jobs@bigdata.com", "Looking for a skilled data scientist to analyze large datasets and help drive data-driven decisions.", new DateTime(2024, 10, 22, 0, 0, 0, 0, DateTimeKind.Utc), "Data Scientist", "FullTime", "$90,000 - $130,000" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "JobListings");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
