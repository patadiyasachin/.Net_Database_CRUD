using Admin3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Admin3.Controllers
{
    public class UserController : Controller
    {
        List<UserModel> userModel=new List<UserModel>()
        {
            new UserModel{UserID=1,UserName="sachin",Email="sachinpatadiya@gmail.com",Password="123",MobileNumber="7572819370",Address="Rajkot",IsActive=true},
            new UserModel{UserID=2,UserName="mehul",Email="mehulparmar@gmail.com",Password="1234",MobileNumber="7572819370",Address="Rajkot",IsActive=true}
        };

        private IConfiguration configuration;
        public UserController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IActionResult Index()
        {
            return View(userModel);
        }

        public IActionResult Save(UserModel userModel)
        {
            if (ModelState.IsValid) {
                string connectionStr = this.configuration.GetConnectionString("myConnString");
                SqlConnection conn=new SqlConnection(connectionStr);
                conn.Open();
                SqlCommand cmd=conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                if (userModel.UserID==null)
                {
                    cmd.CommandText = "PR_User_Insert";
                    cmd.Parameters.AddWithValue("UserName", userModel.UserName);
                    cmd.Parameters.AddWithValue("Email", userModel.Email);
                    cmd.Parameters.AddWithValue("Address", userModel.Address);
                    cmd.Parameters.AddWithValue("Password", userModel.Password);
                    cmd.Parameters.AddWithValue("MobileNo", userModel.MobileNumber);
                    cmd.Parameters.AddWithValue("IsActive", userModel.IsActive);
                }
                else
                {
                    cmd.CommandText = "PR_User_Update";
                    cmd.Parameters.AddWithValue("UserID", userModel.UserID);
                    cmd.Parameters.AddWithValue("UserName", userModel.UserName);
                    cmd.Parameters.AddWithValue("Email", userModel.Email);
                    cmd.Parameters.AddWithValue("Address", userModel.Address);
                    cmd.Parameters.AddWithValue("Password", userModel.Password);
                    cmd.Parameters.AddWithValue("MobileNo", userModel.MobileNumber);
                    cmd.Parameters.AddWithValue("IsActive", userModel.IsActive);
                }

                if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
                {
                    if (userModel.UserID == null)
                    {
                        TempData["insertMsg"] = "Reconrd Inserted Successfully .. ";
                    }
                    else
                    {
                        TempData["insertMsg"] = "Reconrd Updated Successfully .. ";
                    }
                }

                conn.Close();
                return RedirectToAction("GetAll_User");
            }
            else
            {
                return View("Add_Edit", userModel);
            }
        }

        public IActionResult Add_Edit(int userID)
        {
            if (userID != null)
            {
                string connectionStr = this.configuration.GetConnectionString("myConnString");
                SqlConnection conn = new SqlConnection(connectionStr);
                conn.Open();
                SqlCommand cmd=conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_User_SelectByID";
                cmd.Parameters.AddWithValue("UserID", userID);
                DataTable dt = new DataTable();
                SqlDataReader dr=cmd.ExecuteReader();
                dt.Load(dr);

                UserModel userModel = new UserModel();
                foreach (DataRow d in dt.Rows) {
                    userModel.UserID = Convert.ToInt32(d["UserID"]);
                    userModel.UserName =d["UserName"].ToString();
                    userModel.Email = d["Email"].ToString();
                    userModel.Password = d["Password"].ToString();
                    userModel.MobileNumber = d["MobileNo"].ToString();
                    userModel.Address = d["Address"].ToString();
                    userModel.IsActive = Convert.ToBoolean(d["IsActive"]);
                }

                cmd.ExecuteNonQuery();
                return View("Add_Edit", userModel);
            }
            return View("Add_Edit");
        }

        public IActionResult GetAll_User()
        {
            string connectonString = this.configuration.GetConnectionString("myConnString");
            DataTable dt=new DataTable();
            SqlConnection conn = new SqlConnection(connectonString);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_User_SelectAll";
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            return View(dt);
        }

        public IActionResult Delete_User(int userID)
        {
            try
            {
                string connectionStr = this.configuration.GetConnectionString("myConnString");
                SqlConnection conn = new SqlConnection(connectionStr);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType= CommandType.StoredProcedure;
                cmd.CommandText = "PR_User_Delete";
                cmd.Parameters.AddWithValue("UserID", userID);
                cmd.ExecuteNonQuery();
                TempData["SuccessMessage"] = "Record Deleted Successfully !";
            }
            catch (SqlException ex) when (ex.Number == 547) // Foreign key conflict
            {
                TempData["ErrorMessage"] = "You can not delete this record !";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "You can not delete this record !";
            }
            return RedirectToAction("GetAll_User");
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult UserLogin(LoginModel userLoginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this.configuration.GetConnectionString("myConnString");
                    SqlConnection sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = "pr_user_login";
                    sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userLoginModel.UserName;
                    sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userLoginModel.Password;
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(sqlDataReader);
                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                            HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                        }
                        return RedirectToAction("GetAllProduct", "Product");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Enter Valid UserName and Password";
                        return RedirectToAction("Login", "User");
                    }

                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }
    }
}
