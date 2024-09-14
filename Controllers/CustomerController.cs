using Admin3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace Admin3.Controllers
{
    public class CustomerController : Controller
    {

        private IConfiguration configuration;
        public CustomerController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        List<CustomerModel> customerModel = new List<CustomerModel>()
        {
            new CustomerModel{CustomerID=1,CustomerName="sachin patadiya",HomeAddress="rajkot",Email="sachinpatadiya@gmail.com",MobileNo="2378943927",GSTNO="38792379",CityName="Rajkot",PinCode="360002",NetAmount=20000,UserID=1},
            new CustomerModel{CustomerID=2,CustomerName="mehul parmar",HomeAddress="rajkot",Email="mehulparmar@gmail.com",MobileNo="2378943927",GSTNO="38792379",CityName="Rajkot",PinCode="360002",NetAmount=40000,UserID=1},
        };
        public IActionResult Index()
        {
            return View(customerModel);
        }

        public IActionResult Save(CustomerModel customerModel)
        {
            if (ModelState.IsValid)
            {
                string connectionStr = this.configuration.GetConnectionString("myConnString");
                SqlConnection conn = new SqlConnection(connectionStr);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                if (customerModel.CustomerID == null)
                {
                    cmd.CommandText = "PR_Customer_Insert";
                    cmd.Parameters.AddWithValue("CustomerName", customerModel.CustomerName);
                    cmd.Parameters.AddWithValue("HomeAddress", customerModel.HomeAddress);
                    cmd.Parameters.AddWithValue("Email", customerModel.Email);
                    cmd.Parameters.AddWithValue("MobileNo", customerModel.MobileNo);
                    cmd.Parameters.AddWithValue("GSTNO", customerModel.GSTNO);
                    cmd.Parameters.AddWithValue("CityName", customerModel.CityName);
                    cmd.Parameters.AddWithValue("PinCode", customerModel.PinCode);
                    cmd.Parameters.AddWithValue("NetAmount", customerModel.NetAmount);
                    cmd.Parameters.AddWithValue("UserID", customerModel.UserID);
                }
                else
                {
                    cmd.CommandText = "PR_Customer_Update";
                    cmd.Parameters.AddWithValue("CustomerID", customerModel.CustomerID);
                    cmd.Parameters.AddWithValue("CustomerName", customerModel.CustomerName);
                    cmd.Parameters.AddWithValue("HomeAddress", customerModel.HomeAddress);
                    cmd.Parameters.AddWithValue("Email", customerModel.Email);
                    cmd.Parameters.AddWithValue("MobileNo", customerModel.MobileNo);
                    cmd.Parameters.AddWithValue("GSTNO", customerModel.GSTNO);
                    cmd.Parameters.AddWithValue("CityName", customerModel.CityName);
                    cmd.Parameters.AddWithValue("PinCode", customerModel.PinCode);
                    cmd.Parameters.AddWithValue("NetAmount", customerModel.NetAmount);
                    cmd.Parameters.AddWithValue("UserID", customerModel.UserID);
                }

                if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
                {
                    if (customerModel.CustomerID == null)
                    {
                        TempData["insertMsg"] = "Reconrd Inserted Successfully .. ";
                    }
                    else
                    {
                        TempData["insertMsg"] = "Reconrd Updated Successfully .. ";
                    }
                }

                conn.Close();
                return RedirectToAction("GetCustomer_list");
            }
            else
            {
                return View("Add_Edit", customerModel);
            }
        }

        public List<User_dropDown> combo_user()
        {
            string connectionstr1 = this.configuration.GetConnectionString("myConnString");
            SqlConnection con = new SqlConnection(connectionstr1);
            con.Open();
            SqlCommand command = con.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "select_combo_userName";
            DataTable table = new DataTable();
            SqlDataReader drr = command.ExecuteReader();
            table.Load(drr);
            con.Close();


            List<User_dropDown> list = new List<User_dropDown>();
            foreach (DataRow dr in table.Rows)
            {
                User_dropDown user_DropDown = new User_dropDown();
                user_DropDown.UserID = Convert.ToInt32(dr["UserID"]);
                user_DropDown.UserName = dr["UserName"].ToString();
                list.Add(user_DropDown);
            }
            return list;
        }
        public IActionResult Add_Edit(int? customerID)
        {
            ViewBag.userList = combo_user();
            if (customerID != null)
            {
                string connectionStr = this.configuration.GetConnectionString("myConnString");
                SqlConnection conn = new SqlConnection(connectionStr);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Customer_SelectByID";
                cmd.Parameters.AddWithValue("CustomerID", customerID);
                DataTable dt = new DataTable();
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);

                CustomerModel customerModel = new CustomerModel();
                foreach (DataRow d1 in dt.Rows)
                {
                    customerModel.CustomerID = Convert.ToInt32(d1["CustomerID"]);
                    customerModel.CustomerName = d1["CustomerName"].ToString();
                    customerModel.HomeAddress = d1["HomeAddress"].ToString();
                    customerModel.Email = d1["Email"].ToString();
                    customerModel.MobileNo= d1["MobileNo"].ToString();
                    customerModel.GSTNO= d1["GSTNO"].ToString();
                    customerModel.CityName = d1["CityName"].ToString();
                    customerModel.PinCode = d1["PinCode"].ToString();
                    customerModel.NetAmount = Convert.ToDecimal(d1["NetAmount"]);
                    customerModel.UserID = Convert.ToInt32(d1["UserID"]);
                }

                cmd.ExecuteNonQuery();
                return View("Add_Edit", customerModel);
            }
            return View("Add_Edit");
        }
        public IActionResult GetCustomer_list()
        {
            string connectonString = this.configuration.GetConnectionString("myConnString");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectonString);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Customer_SelectAll";
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            return View(dt);
        }

        public IActionResult Delete_Customer(int customerID)
        {
            try
            {
                string connectionStr = this.configuration.GetConnectionString("myConnString");
                SqlConnection conn = new SqlConnection(connectionStr);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Customer_Delete";
                cmd.Parameters.AddWithValue("CustomerID", customerID);
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
            return RedirectToAction("GetCustomer_list");
        }
    }
}
