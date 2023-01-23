using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace RazorAppAssignment.Pages.Employees
{
    public class IndexModel : PageModel
    {
        public List<EmployeeInfo> ListEmployees = new List<EmployeeInfo>();
        public void OnGet()
        {
            try
            {
                string connString = "<Enter your connection string here>";
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    string sqlQuery = "SELECT * FROM employees";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EmployeeInfo employeeInfo = new EmployeeInfo();
                                employeeInfo.id = "" + reader.GetInt32(0);
                                employeeInfo.name = reader.GetString(1);
                                employeeInfo.email = reader.GetString(2);
                                employeeInfo.phone = reader.GetString(3);
                                employeeInfo.address= reader.GetString(4);
                                employeeInfo.created_at = reader.GetDateTime(5).ToString();

                                ListEmployees.Add(employeeInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex){ 
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public class EmployeeInfo
    {
        public string id;
        public string name;
        public string email;
        public string phone;
        public string address;
        public string created_at;
    }
}
