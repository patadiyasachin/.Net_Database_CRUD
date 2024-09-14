using Admin3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace Admin3.Controllers
{
    public class LoginController : Controller
    {
        public IConfiguration configuration;
        public LoginController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        //[Route("")]
        public IActionResult Login(LoginModel loginModel)
        {
            string connectionstr1 = this.configuration.GetConnectionString("myConnString");
            SqlConnection con = new SqlConnection(connectionstr1);
            con.Open();
            SqlCommand command = con.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "pr_user_login";
            command.Parameters.AddWithValue("UserName", loginModel.UserName);
            command.Parameters.AddWithValue("Password", loginModel.Password);
            DataTable table = new DataTable();
            SqlDataReader drr = command.ExecuteReader();
            table.Load(drr);
            con.Close();

            foreach (DataRow dr in table.Rows)
            {
                HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                HttpContext.Session.SetString("Password", dr["Password"].ToString());
            }

            if (table != null)
            {
                return View();
            }
            else
            {
                return View("GetBill_List");
            }
            
        }
    }
}
