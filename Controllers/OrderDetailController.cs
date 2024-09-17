using Admin3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace Admin3.Controllers
{
    [CheckAccess]
    public class OrderDetailController : Controller
    {
        private IConfiguration configuration;
        public OrderDetailController(IConfiguration configuration)
        {
            this.configuration = configuration; 
        }

        List<OrderDetailModel> orderDetilModel = new List<OrderDetailModel>(){
            new OrderDetailModel{OrderDetailID=1,OrderID=1,ProductID=1,Quantity=5,Amount=2000,TotalAmount=20000,UserID=1},
            new OrderDetailModel{OrderDetailID=2,OrderID=1,ProductID=1,Quantity=6,Amount=3000,TotalAmount=30000,UserID=1}
        };
        public IActionResult Index()
        {
            return View(orderDetilModel);
        }

        public IActionResult Save(OrderDetailModel orderDetailModel)
        {
            if (ModelState.IsValid)
            {
                string connectionStr = this.configuration.GetConnectionString("myConnString");
                SqlConnection conn = new SqlConnection(connectionStr);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                if (orderDetailModel.OrderDetailID == null)
                {
                    cmd.CommandText = "PR_OrderDetail_Insert";
                    cmd.Parameters.AddWithValue("OrderID", orderDetailModel.OrderID);
                    cmd.Parameters.AddWithValue("ProductID", orderDetailModel.ProductID);
                    cmd.Parameters.AddWithValue("Quantity", orderDetailModel.Quantity);
                    cmd.Parameters.AddWithValue("Amount", orderDetailModel.Amount);
                    cmd.Parameters.AddWithValue("TotalAmount", orderDetailModel.TotalAmount);
                    cmd.Parameters.AddWithValue("UserID", orderDetailModel.UserID);
                }
                else
                {
                    cmd.CommandText = "PR_OrderDetail_Update";
                    cmd.Parameters.AddWithValue("OrderDetailID", orderDetailModel.OrderDetailID);
                    cmd.Parameters.AddWithValue("OrderID", orderDetailModel.OrderID);
                    cmd.Parameters.AddWithValue("ProductID", orderDetailModel.ProductID);
                    cmd.Parameters.AddWithValue("Quantity", orderDetailModel.Quantity);
                    cmd.Parameters.AddWithValue("Amount", orderDetailModel.Amount);
                    cmd.Parameters.AddWithValue("TotalAmount", orderDetailModel.TotalAmount);
                    cmd.Parameters.AddWithValue("UserID", orderDetailModel.UserID);
                }

                if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
                {
                    if (orderDetailModel.OrderDetailID == null)
                    {
                        TempData["insertMsg"] = "Reconrd Inserted Successfully .. ";
                    }
                    else
                    {
                        TempData["insertMsg"] = "Reconrd Updated Successfully .. ";
                    }
                }

                conn.Close();
                return RedirectToAction("GetAll_OrderDetail");
            }
            else
            {
                return View("Add_Edit", orderDetailModel);
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

        public List<order_dropdown> combo_order()
        {
            string connectionstr1 = this.configuration.GetConnectionString("myConnString");
            SqlConnection con = new SqlConnection(connectionstr1);
            con.Open();
            SqlCommand command = con.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "combo_ordertable";
            DataTable table = new DataTable();
            SqlDataReader drr = command.ExecuteReader();
            table.Load(drr);
            con.Close();


            List<order_dropdown> list = new List<order_dropdown>();
            foreach (DataRow dr in table.Rows)
            {
                order_dropdown order_DropDown = new order_dropdown();
                order_DropDown.OrderID = Convert.ToInt32(dr["OrderID"]);
                order_DropDown.ShippingAddress = dr["ShippingAddress"].ToString();
                list.Add(order_DropDown);
            }
            return list;
        }

        public List<product_dropdown> combo_product()
        {
            string connectionstr1 = this.configuration.GetConnectionString("myConnString");
            SqlConnection con = new SqlConnection(connectionstr1);
            con.Open();
            SqlCommand command = con.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "combo_producttable";
            DataTable table = new DataTable();
            SqlDataReader drr = command.ExecuteReader();
            table.Load(drr);
            con.Close();


            List<product_dropdown> list = new List<product_dropdown>();
            foreach (DataRow dr in table.Rows)
            {
                product_dropdown product_DropDown = new product_dropdown();
                product_DropDown.ProductID = Convert.ToInt32(dr["ProductID"]);
                product_DropDown.ProductName = dr["ProductName"].ToString();
                list.Add(product_DropDown);
            }
            return list;
        }

        public IActionResult Add_Edit(int? orderDetailID)
        {
            ViewBag.orderList = combo_order();
            ViewBag.productList = combo_product();
            ViewBag.userList = combo_user();

            if (orderDetailID != null)
            {
                string connectionStr = this.configuration.GetConnectionString("myConnString");
                SqlConnection conn = new SqlConnection(connectionStr);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_OrderDetail_SelectByID";
                cmd.Parameters.AddWithValue("OrderDetailID", orderDetailID);
                DataTable dt = new DataTable();
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);

                OrderDetailModel orderdetailModel = new OrderDetailModel();
                foreach (DataRow d1 in dt.Rows)
                {
                    orderdetailModel.OrderDetailID = Convert.ToInt32(d1["OrderDetailID"]);
                    orderdetailModel.OrderID = Convert.ToInt32(d1["OrderID"]);
                    orderdetailModel.ProductID = Convert.ToInt32(d1["ProductID"]);
                    orderdetailModel.Quantity = Convert.ToInt32(d1["Quantity"]);
                    orderdetailModel.Amount = Convert.ToDecimal(d1["Amount"]);
                    orderdetailModel.TotalAmount = Convert.ToDecimal(d1["TotalAmount"]);
                    orderdetailModel.UserID = Convert.ToInt32(d1["UserID"]);
                }

                cmd.ExecuteNonQuery();
                return View("Add_Edit", orderdetailModel);
            }
            return View("Add_Edit");
        }

        public IActionResult GetAll_OrderDetail()
        {
            string connectonString = this.configuration.GetConnectionString("myConnString");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectonString);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_OrderDetail_SelectAll";
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            return View("getAllDetail",dt);
        }

        public IActionResult DeleteOrderDetail(int orderDetailID)
        {
            try
            {
                string connectionStr = this.configuration.GetConnectionString("myConnString");
                SqlConnection conn = new SqlConnection(connectionStr);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_OrderDetail_Delete";
                cmd.Parameters.AddWithValue("OrderDetailID", orderDetailID);
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
            return RedirectToAction("GetAll_OrderDetail");
        }
    }
}
