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
                return RedirectToAction("GetAll_User");
            }
            catch (SqlException ex) when (ex.Number == 547) // Foreign key conflict
            {
                ViewBag.ErrorMessage = "Cannot delete this product due to related records.";
                return RedirectToAction("GetAll_User");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong while deleting the product.";
                return RedirectToAction("GetAll_User");
            }
        }
    }
}
