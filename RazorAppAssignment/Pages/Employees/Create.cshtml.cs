using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace RazorAppAssignment.Pages.Employees
{
    public class CreateModel : PageModel
    {
        public EmployeeInfo employeeInfo = new EmployeeInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost() {
            employeeInfo.name = Request.Form["empName"];
            employeeInfo.email = Request.Form["empEmail"];
            employeeInfo.phone = Request.Form["empPhone"];
            employeeInfo.address = Request.Form["empAddress"];

            if (employeeInfo.name.Length == 0 || employeeInfo.email.Length == 0 || 
                employeeInfo.phone.Length == 0 || employeeInfo.address.Length == 0 )
            {
                errorMessage = "All the fields are required";
                return;
            }

            // Save the employee info to the database

            try
            {
                string connString = "<Enter your connection string here>";
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    string sqlQuery = "INSERT INTO employees" +
                        "(name,email,phone,address) VALUES" +
                        "(@name,@email,@phone,@address);";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@name", employeeInfo.name);
                        command.Parameters.AddWithValue("@email", employeeInfo.email);
                        command.Parameters.AddWithValue("@phone", employeeInfo.phone);
                        command.Parameters.AddWithValue("@address", employeeInfo.address);

                        command.ExecuteNonQuery();
                    }
                }

            }
            catch( Exception ex )
            {
                errorMessage = ex.Message;
                return;
            }

            employeeInfo.name = "";
            employeeInfo.email = "";
            employeeInfo.address = "";
            employeeInfo.phone = "";

            successMessage = "New employee added successfully!";
            Response.Redirect("/Employees/Index");
        }
    }
}
