using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;
using Newtonsoft.Json.Linq;
using QuickCode.MyecommerceDemo.Common.Extensions;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Entities;

namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BaseData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var fileList = typeof(BaseData).GetMigrationDataFiles();
            migrationBuilder.ParseJsonAsInitialData<BaseData>(fileList);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
