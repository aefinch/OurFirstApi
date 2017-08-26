using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using OurFirstApi.Models;

namespace OurFirstApi.Controllers
{
	//api/employees
	public class EmployeesController : ApiController
	{
		//api/employees
		public HttpResponseMessage Get()
		{
			using (var connection =
				new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
			{
				try
				{
					connection.Open();

					var result = connection.Query<EmployeeListResult>("select * " +
																	  "from Employee");


					return Request.CreateResponse(HttpStatusCode.OK, result);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					Console.WriteLine(ex.StackTrace);
					return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Query blew up");
				}
			}
		}

		//api/employees/3000
		public HttpResponseMessage Get(int id)
		{
			using (var connection =
				new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
			{
				try
				{
					connection.Open();

					var result =
						connection.Query<EmployeeListResult>("Select * From Employee where EmployeeId = @id",
							new { id = id }).FirstOrDefault();

					if (result == null)
					{
						return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Employee with the Id {id} was not found");
					}

					return Request.CreateResponse(HttpStatusCode.OK, result);
				}
				catch (Exception ex)
				{
					return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
				}
			}
		}
		//api/employees/add/employee
		public HttpResponseMessage Post(EmployeeListResult employee)
		{
			using (var connection =
				new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
			{
				try
				{
					connection.Open();
					var result = connection.Execute("Insert into Employee(FirstName, LastName) " +
										  "Values(@FirstName, @LastName)", new { FirstName = employee.FirstName, LastName = employee.LastName });
					return Request.CreateResponse(HttpStatusCode.OK, result);
				}

				catch (Exception ex)
				{
					return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
				}
			}
		}
		public HttpResponseMessage Put(int id, EmployeeListResult employee)
		{
			using (var connection =
				new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
			{
				try
				{
					connection.Open();

					var result = connection.Execute("Update Employee " +
																"Set FirstName = @firstname, LastName = @lastname " +
																"Where EmployeeId = @employeeId", new { FirstName = employee.FirstName, LastName = employee.LastName, EmployeeId = id });
					return Request.CreateResponse(HttpStatusCode.OK, result);

				}
				catch (Exception ex)
				{
					return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

				}
			}
		}
		public HttpResponseMessage Delete(int id)
		{
			using (var connection =
				new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
			{
				try
				{
					connection.Open();
					var result = connection.Execute("Delete from Employee " +
														  "Where EmployeeId = @employeeId", new { employeeId = id });

					return Request.CreateResponse(HttpStatusCode.OK, result);

				}
				catch (Exception ex)
				{
					return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

				}
			}
		}
	}
}
