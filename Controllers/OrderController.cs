using Admin3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
namespace Admin3.Controllers
{
    [CheckAccess]
    public class OrderController : Controller
    {
        private IConfiguration configuration;
        public OrderController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        List<OrderModel> orderModel = new List<OrderModel>()
        {
            new OrderModel{OrderID=1,OrderDate=new DateTime(2024,12,16),CustomerID=1,PaymentMode="cash",TotalAmount=10000,ShippingAddress="Rajkot",UserID=1},
            new OrderModel{OrderID=2,OrderDate=DateTime.Now,CustomerID=1,PaymentMode="upi",TotalAmount=12000,ShippingAddress="Rajkot",UserID=1},
        };
        public IActionResult Index()
        {
            return View(orderModel);
        }

        public IActionResult Save(OrderModel orderModel)
        {
            if (ModelState.IsValid)
            {
                string connectionStr = this.configuration.GetConnectionString("myConnString");
                SqlConnection conn = new SqlConnection(connectionStr);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                if (orderModel.OrderID == null)
                {
                    cmd.CommandText = "PR_Order_Insert";
                    cmd.Parameters.AddWithValue("OrderDate", orderModel.OrderDate);
                    cmd.Parameters.AddWithValue("CustomerID", orderModel.CustomerID);
                    cmd.Parameters.AddWithValue("PaymentMode", orderModel.PaymentMode);
                    cmd.Parameters.AddWithValue("TotalAmount", orderModel.TotalAmount);
                    cmd.Parameters.AddWithValue("ShippingAddress", orderModel.ShippingAddress);
                    cmd.Parameters.AddWithValue("UserID", orderModel.UserID);
                }
                else
                {
                    cmd.CommandText = "PR_Order_Update";
                    cmd.Parameters.AddWithValue("OrderID", orderModel.OrderID);
                    cmd.Parameters.AddWithValue("OrderDate", orderModel.OrderDate);
                    cmd.Parameters.AddWithValue("CustomerID", orderModel.CustomerID);
                    cmd.Parameters.AddWithValue("PaymentMode", orderModel.PaymentMode);
                    cmd.Parameters.AddWithValue("TotalAmount", orderModel.TotalAmount);
                    cmd.Parameters.AddWithValue("ShippingAddress", orderModel.ShippingAddress);
                    cmd.Parameters.AddWithValue("UserID", orderModel.UserID);
                }

                if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
                {
                    if (orderModel.OrderID == null)
                    {
                        TempData["insertMsg"] = "Reconrd Inserted Successfully .. ";
                    }
                    else
                    {
                        TempData["insertMsg"] = "Reconrd Updated Successfully .. ";
                    }
                }

                conn.Close();
                return RedirectToAction("GetAll_Order");
            }
            else
            {
                return View("Add_Edit", orderModel);
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

        public List<Customer_DropDown> combo_customer()
        {
            string connectionstr1 = this.configuration.GetConnectionString("myConnString");
            SqlConnection con = new SqlConnection(connectionstr1);
            con.Open();
            SqlCommand command = con.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Customer_ComboBox";
            DataTable table = new DataTable();
            SqlDataReader drr = command.ExecuteReader();
            table.Load(drr);
            con.Close();


            List<Customer_DropDown> list = new List<Customer_DropDown>();
            foreach (DataRow dr in table.Rows)
            {
                Customer_DropDown customer_DropDown = new Customer_DropDown();
                customer_DropDown.CustomerID = Convert.ToInt32(dr["CustomerID"]);
                customer_DropDown.CustomerName = dr["CustomerName"].ToString();
                list.Add(customer_DropDown);
            }

            return list;
        }

        public IActionResult Add_Edit(int? orderID)
        {
            ViewBag.customerList = combo_customer();
            ViewBag.userList= combo_user();

            if (orderID != null)
            {
                string connectionStr = this.configuration.GetConnectionString("myConnString");
                SqlConnection conn = new SqlConnection(connectionStr);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Order_SelectByID";
                cmd.Parameters.AddWithValue("OrderID", orderID);
                DataTable dt = new DataTable();
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);

                OrderModel orderModel = new OrderModel();
                foreach (DataRow d1 in dt.Rows)
                {
                    orderModel.OrderID = Convert.ToInt32(d1["OrderID"]);
                    orderModel.OrderDate = Convert.ToDateTime(d1["OrderDate"]);
                    orderModel.CustomerID = Convert.ToInt32(d1["CustomerID"]);
                    orderModel.PaymentMode = d1["PaymentMode"].ToString();
                    orderModel.ShippingAddress = d1["ShippingAddress"].ToString();
                    orderModel.TotalAmount = Convert.ToDecimal(d1["TotalAmount"]);
                    orderModel.UserID = Convert.ToInt32(d1["UserID"]);
                }

                cmd.ExecuteNonQuery();
                return View("Add_Edit", orderModel);
            }
            return View("Add_Edit");
        }

        public IActionResult GetAll_Order()
        {
            string connectonString = this.configuration.GetConnectionString("myConnString");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectonString);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Order_SelectAll";
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            return View(dt);
        }

        public IActionResult Delete_Order(int orderID)
        {
            try
            {
                string connectionStr = this.configuration.GetConnectionString("myConnString");
                SqlConnection conn = new SqlConnection(connectionStr);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Order_Delete";
                cmd.Parameters.AddWithValue("OrderID",orderID);
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
            return RedirectToAction("GetAll_Order");
        }
    }
}
