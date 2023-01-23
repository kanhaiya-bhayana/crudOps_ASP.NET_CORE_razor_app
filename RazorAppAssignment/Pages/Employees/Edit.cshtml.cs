using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace RazorAppAssignment.Pages.Employees
{
    public class EditModel : PageModel
    {
        public EmployeeInfo employeeInfo = new EmployeeInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
             string id = Request.Query["id"];
            
            try
            {
                string connString = "<Enter your connection string here>";
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    string sqlQuery = "SELECT * FROM employees WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                //EmployeeInfo employeeInfo = new EmployeeInfo();
                                employeeInfo.id = "" + reader.GetInt32(0);
                                employeeInfo.name = reader.GetString(1);
                                employeeInfo.email = reader.GetString(2);
                                employeeInfo.phone = reader.GetString(3);
                                employeeInfo.address = reader.GetString(4);
                                //employeeInfo.created_at = reader.GetDateTime(5).ToString();

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            employeeInfo.id = Request.Form["id"];
            employeeInfo.name = Request.Form["empName"];
            employeeInfo.email = Request.Form["empEmail"];
            employeeInfo.phone = Request.Form["empPhone"];
            employeeInfo.address = Request.Form["empAddress"];

            if (employeeInfo.name.Length == 0 || employeeInfo.email.Length == 0 ||
                employeeInfo.phone.Length == 0 || employeeInfo.address.Length == 0)
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
                    string sqlQuery = "UPDATE employees " +
                        "SET name=@name, email=@email, phone=@phone, address=@address "+
                        "WHERE id=@id;";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@name", employeeInfo.name);
                        command.Parameters.AddWithValue("@email", employeeInfo.email);
                        command.Parameters.AddWithValue("@phone", employeeInfo.phone);
                        command.Parameters.AddWithValue("@address", employeeInfo.address);
                        command.Parameters.AddWithValue("@id", employeeInfo.id);

                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            employeeInfo.name = "";
            employeeInfo.email = "";
            employeeInfo.address = "";
            employeeInfo.phone = "";

            successMessage = "New employee updated successfully!";
            Response.Redirect("/Employees/Index");
        }

    }
}
