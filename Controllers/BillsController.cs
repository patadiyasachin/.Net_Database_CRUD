using Admin3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace Admin3.Controllers
{
    [CheckAccess]
    public class BillsController : Controller
    {
        private IConfiguration configuration;
        public BillsController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        List<BillsModel> billModel = new List<BillsModel>(){
            new BillsModel{BillID=1,BillNumber="8293",BillDate=DateTime.Now,OrderID=1,TotalAmount=1000,Discount=500,NetAmount=1000,UserID=1},
            new BillsModel{BillID=2,BillNumber="37284",BillDate=DateTime.Now,OrderID=1,TotalAmount=2000,Discount=600,NetAmount=3000,UserID=1},
        };
        public IActionResult Index()
        {
            return View(billModel);
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

        public IActionResult Save(BillsModel billModel)
        {
            if (ModelState.IsValid)
            {
                string connectionStr = this.configuration.GetConnectionString("myConnString");
                SqlConnection conn = new SqlConnection(connectionStr);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                if (billModel.BillID == null)
                {
                    cmd.CommandText = "PR_Bills_Insert";
                    cmd.Parameters.AddWithValue("BillNumber", billModel.BillNumber);
                    cmd.Parameters.AddWithValue("BillDate", billModel.BillDate);
                    cmd.Parameters.AddWithValue("OrderID", billModel.OrderID);
                    cmd.Parameters.AddWithValue("TotalAmount", billModel.TotalAmount);
                    cmd.Parameters.AddWithValue("Discount", billModel.Discount);
                    cmd.Parameters.AddWithValue("NetAmount", billModel.NetAmount);
                    cmd.Parameters.AddWithValue("UserID", billModel.UserID);
                }
                else
                {
                    cmd.CommandText = "PR_Bills_Update";
                    cmd.Parameters.AddWithValue("BillID", billModel.BillID);
                    cmd.Parameters.AddWithValue("BillNumber", billModel.BillNumber);
                    cmd.Parameters.AddWithValue("BillDate", billModel.BillDate);
                    cmd.Parameters.AddWithValue("OrderID", billModel.OrderID);
                    cmd.Parameters.AddWithValue("TotalAmount", billModel.TotalAmount);
                    cmd.Parameters.AddWithValue("Discount", billModel.Discount);
                    cmd.Parameters.AddWithValue("NetAmount", billModel.NetAmount);
                    cmd.Parameters.AddWithValue("UserID", billModel.UserID);
                }

                if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
                {
                    if (billModel.BillID == null)
                    {
                        TempData["insertMsg"] = "Reconrd Inserted Successfully .. ";
                    }
                    else
                    {
                        TempData["insertMsg"] = "Reconrd Updated Successfully .. ";
                    }
                }

                conn.Close();
                return RedirectToAction("GetBill_List");
            }
            else
            {
                return View("Add_Edit", billModel);
            }
        }

        public IActionResult Add_Edit(int? billID)
        {
            ViewBag.orderlist = combo_order();
            ViewBag.userlist = combo_user();

            if (billID != null)
            {
                string connectionstr = this.configuration.GetConnectionString("myConnString");
                SqlConnection conn = new SqlConnection(connectionstr);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Bills_SelectByID";
                cmd.Parameters.AddWithValue("BillID", billID);
                DataTable dt = new DataTable();
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);

                BillsModel billModel = new BillsModel();
                foreach (DataRow d in dt.Rows)
                {
                    billModel.BillID = Convert.ToInt32(d["BillID"]);
                    billModel.BillNumber = d["BillNumber"].ToString();
                    billModel.BillDate = Convert.ToDateTime(d["BillDate"]);
                    billModel.OrderID = Convert.ToInt32(d["OrderID"]);
                    billModel.TotalAmount = Convert.ToDecimal(d["TotalAmount"]);
                    billModel.Discount = Convert.ToDecimal(d["Discount"]);
                    billModel.NetAmount= Convert.ToDecimal(d["NetAmount"]);
                    billModel.UserID = Convert.ToInt32(d["UserID"]);
                }
                cmd.ExecuteNonQuery();
                return View("Add_Edit", billModel);
            }
            return View("Add_Edit");
        }

        public IActionResult GetBill_List() {
            string connectionstr = this.configuration.GetConnectionString("myConnString");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Bills_SelectAll";
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            return View(dt);
        }

        public IActionResult DeleteBill(int billID)
        {
            try
            {
                string connectionstr = this.configuration.GetConnectionString("myConnString");
                SqlConnection conn = new SqlConnection(connectionstr);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Bills_Delete";
                cmd.Parameters.AddWithValue("@BillID", billID);
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
            return RedirectToAction("GetBill_List");
        }
    }
}
