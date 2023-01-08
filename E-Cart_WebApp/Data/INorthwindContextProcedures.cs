﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using E_Cart_WebApp.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace E_Cart_WebApp.Data
{
    public partial interface INorthwindContextProcedures
    {
        Task<List<CalculateCumulativeTotalResult>> CalculateCumulativeTotalAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<CustomerDetailsCursorOptimisticExampleResult>> CustomerDetailsCursorOptimisticExampleAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<CustomerDetailsCursorScrollExampleResult>> CustomerDetailsCursorScrollExampleAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<CustOrderHistResult>> CustOrderHistAsync(string CustomerID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<CustOrdersDetailResult>> CustOrdersDetailAsync(int? OrderID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<CustOrdersOrdersResult>> CustOrdersOrdersAsync(string CustomerID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<EmployeeSalesbyCountryResult>> EmployeeSalesbyCountryAsync(DateTime? Beginning_Date, DateTime? Ending_Date, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<int> GetProductsWithReorderYNAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<int> GetTotalOrdersAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<int> IsReorderNeededAsync(int? ProductId, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<ProductsWithReOrderYNColResult>> ProductsWithReOrderYNColAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<ProductsWithReOrderYNCol_WithCursorResult>> ProductsWithReOrderYNCol_WithCursorAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SalesbyYearResult>> SalesbyYearAsync(DateTime? Beginning_Date, DateTime? Ending_Date, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SalesByCategoryResult>> SalesByCategoryAsync(string CategoryName, string OrdYear, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SelectProductByProductIDResult>> SelectProductByProductIDAsync(int? ProductID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SelectProductByProductIDAndProductNameResult>> SelectProductByProductIDAndProductNameAsync(int? ProductID, string ProductName, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SelectProductByProductIDAndProductNameAndOutputParamResult>> SelectProductByProductIDAndProductNameAndOutputParamAsync(string ProductName, OutputParameter<int?> TotalPrice, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<TenMostExpensiveProductsResult>> TenMostExpensiveProductsAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
    }
}
