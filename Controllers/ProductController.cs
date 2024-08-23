using Admin3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace Admin3.Controllers
{
    public class ProductController : Controller
    {
        private IConfiguration configuration;

        public ProductController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        List<ProductModel> proModel=new List<ProductModel>
        {
            new ProductModel{ProductID = 1,ProductName="Iphone12",ProductPrice=100000,ProductCode="IOS",Description="Nice Product",UserID=1},
            new ProductModel{ProductID = 2,ProductName="Iphone13",ProductPrice=110000,ProductCode="IOS",Description="Nice Product",UserID=2},
            new ProductModel{ProductID = 3,ProductName="Iphone12",ProductPrice=100000,ProductCode="IOS",Description="Nice Product",UserID=1},
            new ProductModel{ProductID = 4,ProductName="Iphone13",ProductPrice=110000,ProductCode="IOS",Description="Nice Product",UserID=2},
            new ProductModel{ProductID = 5,ProductName="Iphone15",ProductPrice=150000,ProductCode="IOS",Description="Nice Product",UserID=2},
        };

        public IActionResult Index()
        {
            return View(proModel);
        }

        public IActionResult Save(ProductModel proModel)
        {
            if (ModelState.IsValid)
            {
                string connectionstr = this.configuration.GetConnectionString("myConnString");
                DataTable dt = new DataTable();
                SqlConnection conn = new SqlConnection(connectionstr);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                if (proModel.ProductID == null)
                {
                    cmd.CommandText = "Proc_Insert";
                    cmd.Parameters.AddWithValue("ProductName", proModel.ProductName);
                    cmd.Parameters.AddWithValue("ProductPrice", proModel.ProductPrice);
                    cmd.Parameters.AddWithValue("ProductCode", proModel.ProductCode);
                    cmd.Parameters.AddWithValue("Description", proModel.Description);
                    cmd.Parameters.AddWithValue("UserID", proModel.UserID);
                }
                else
                {
                    cmd.CommandText = "Product_Update";
                    cmd.Parameters.AddWithValue("ProductId", SqlDbType.Int).Value = proModel.ProductID;
                    cmd.Parameters.AddWithValue("ProductName", SqlDbType.VarChar).Value = proModel.ProductName;
                    cmd.Parameters.AddWithValue("ProductPrice", SqlDbType.Decimal).Value = proModel.ProductPrice;
                    cmd.Parameters.AddWithValue("ProductCode", SqlDbType.VarChar).Value = proModel.ProductCode;
                    cmd.Parameters.AddWithValue("Description", SqlDbType.VarChar).Value = proModel.Description;
                    cmd.Parameters.AddWithValue("UserID", SqlDbType.Int).Value = proModel.UserID;
                }

                if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
                {
                    if (proModel.ProductID == null)
                    {
                        TempData["insertMsg"] = "Reconrd Inserted Successfully .. ";
                    }
                    else
                    {
                        TempData["insertMsg"] = "Reconrd Updated Successfully .. ";
                    }
                }

                conn.Close();
                return RedirectToAction("GetAllProduct");
            }
            else
            {
                return View("Add_Edit",proModel);
            } 
        }

        public IActionResult Add_Edit(int? productId)
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
            ViewBag.userList = list;


            if (productId != null)
            {
                string connectionstr = this.configuration.GetConnectionString("myConnString");
                SqlConnection conn = new SqlConnection(connectionstr);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Product_SelectByPk";
                cmd.Parameters.AddWithValue("ProductId", productId);
                DataTable dt = new DataTable();
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);

                ProductModel proModel=new ProductModel();
                foreach (DataRow d in dt.Rows)
                {
                    proModel.ProductID = Convert.ToInt32(d["ProductId"]);
                    proModel.ProductName = d["ProductName"].ToString();
                    proModel.ProductPrice = Convert.ToDecimal(d["ProductPrice"]);
                    proModel.ProductCode= d["ProductCode"].ToString();
                    proModel.Description = d["Description"].ToString();
                    proModel.UserID = Convert.ToInt32(d["UserID"]);
                }
                cmd.ExecuteNonQuery();
                return View("Add_Edit", proModel);
            }
            return View("Add_Edit");
        }

        public IActionResult GetAllProduct()
        {
            string connectionstr = this.configuration.GetConnectionString("myConnString");
            DataTable dt=new DataTable();
            SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Product_SelectAll";
            SqlDataReader dr=cmd.ExecuteReader();
            dt.Load(dr);
            return View(dt);
        }

        public IActionResult DeleteProduct(int productId)
        {
            try
            {
                string connectionstr = this.configuration.GetConnectionString("myConnString");
                SqlConnection conn = new SqlConnection(connectionstr);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Product_Delete";
                cmd.Parameters.AddWithValue("ProductId", productId);
                cmd.ExecuteNonQuery();
                return RedirectToAction("GetAllProduct");
            }
            catch (SqlException ex) when (ex.Number == 547) // Foreign key conflict
            {
                ViewBag.ErrorMessage = "Cannot delete this product due to related records.";
                return RedirectToAction("GetAllProduct");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong while deleting the product.";
                return RedirectToAction("GetAllProduct");
            }
        }
    }
}
