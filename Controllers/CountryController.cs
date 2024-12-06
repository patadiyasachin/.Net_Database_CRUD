using Admin3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace Admin3.Controllers
{
    public class CountryController : Controller
    {
        private IConfiguration _configuration;
        public CountryController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public IActionResult Index()
        {
            string connectionstr = this._configuration.GetConnectionString("myConnString");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();
            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "loc_country_selectAll";

            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            conn.Close();
            return View("Index", dt);
        }

        public IActionResult Delete(int CountryID)
        {
            string connectionstr = _configuration.GetConnectionString("myConnString");
            using (SqlConnection conn = new SqlConnection(connectionstr))
            {
                conn.Open();
                using (SqlCommand sqlCommand = conn.CreateCommand())
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_LOC_Country_Delete";
                    sqlCommand.Parameters.AddWithValue("@CountryID", CountryID);
                    sqlCommand.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Save(CountryModel countryModel)
        {
            try
            {
                string str = this._configuration.GetConnectionString("myConnString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();

                
                cmd.Parameters.AddWithValue("CountryName", countryModel.CountryName);
                cmd.Parameters.AddWithValue("CountryCode", countryModel.CountryCode);

                if (countryModel.CountryID!= null)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PR_LOC_Country_Update";
                    cmd.Parameters.AddWithValue("CountryID", countryModel.CountryID);
                }
                else
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PR_LOC_Country_Insert";
                }

                if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
                {
                    if (countryModel.CountryID==null)
                    {
                        TempData["insertMsg"] = "Reconrd Inserted Successfully .. ";
                    }
                    else
                    {
                        TempData["insertMsg"] = "Reconrd Updated Successfully .. ";
                    }
                }

                conn.Close();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return RedirectToAction("AddEdit", countryModel);
            }
        }
        public IActionResult AddEdit(int? countryID)
        {
            if (countryID.HasValue)
            {
                string connectionstr = _configuration.GetConnectionString("myConnString");
                DataTable dt = new DataTable();

                using (SqlConnection conn = new SqlConnection(connectionstr))
                {
                    conn.Open();
                    using (SqlCommand objCmd = conn.CreateCommand())
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.CommandText = "PR_LOC_Country_SelectByPK";
                        objCmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = countryID;

                        using (SqlDataReader objSDR = objCmd.ExecuteReader())
                        {
                            dt.Load(objSDR);
                        }
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    CountryModel model = new CountryModel();
                    foreach (DataRow dr in dt.Rows)
                    {
                        model.CountryName= dr["CountryName"].ToString();
                        model.CountryCode = dr["CountryCode"].ToString();
                        //ViewBag.StateList = GetStateByCountryID(model.CountryID); // Load states for selected country
                    }
                    //GetStatesByCountry(model.CountryID);
                    return View("AddEdit", model);
                }
            }
            return View("AddEdit");
        }
    }
}
