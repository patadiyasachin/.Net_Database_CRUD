using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Admin3.Models;


namespace CoffeeShop.Controllers
{
    public class CityController : Controller
    {
        private readonly IConfiguration _configuration;

        #region configuration
        public CityController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            string connectionstr = this._configuration.GetConnectionString("myConnString");
            //PrePare a connection
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();

            //Prepare a Command
            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_LOC_City_SelectAll";

            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            conn.Close();
            return View("Index", dt);
        }
        #endregion

        public List<CountryDropDown> CountryDropdown()
        {
            string connstr = this._configuration.GetConnectionString("myConnString");
            SqlConnection conn = new SqlConnection(connstr);
            SqlCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "loc_CountryDropdown";
            DataTable dt = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            conn.Close();
            List<CountryDropDown> list = new List<CountryDropDown>();

            foreach (DataRow d in dt.Rows)
            {
                CountryDropDown countryDropDown = new CountryDropDown();

                countryDropDown.CountryID = Convert.ToInt32(d["CountryID"]);
                countryDropDown.CountryName = d["CountryName"].ToString();
                list.Add(countryDropDown);
            }
            return list;
        }

        public List<StateDropDown> StateDropdown()
        {
            string connstr = this._configuration.GetConnectionString("myConnString");
            SqlConnection conn = new SqlConnection(connstr);
            SqlCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "loc_StateDropdown";
            DataTable dt = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            conn.Close();
            List<StateDropDown> list = new List<StateDropDown>();

            foreach (DataRow d in dt.Rows)
            {
                StateDropDown stateDropDown = new StateDropDown();

                stateDropDown.StateID= Convert.ToInt32(d["StateID"]);
                stateDropDown.StateName = d["StateName"].ToString();
                list.Add(stateDropDown);
            }
            return list;
        }

        //public IActionResult AddEdit(int? cityId)
        //{
        //    ViewBag.cityList = cityDropdown();
        //    if (cityId != null)
        //    {
        //        string str = this._configuration.GetConnectionString("myConnString");
        //        SqlConnection conn = new SqlConnection(str);
        //        conn.Open();
        //        SqlCommand cmd = conn.CreateCommand();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "PR_LOC_City_SelectByPK";
        //        cmd.Parameters.AddWithValue("CityID", cityId);
        //        DataTable dt = new DataTable();
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        dt.Load(dr);
        //        CityModel cityModel = new CityModel();

        //        foreach (DataRow dr2 in dt.Rows)
        //        {
        //            cityModel.CityID = Convert.ToInt32(dr2["CityID"]);
        //            cityModel.StateID = Convert.ToInt32(dr2["StateID"]);
        //            cityModel.CountryID = Convert.ToInt32(dr2["CountryID"]);
        //            cityModel.CityName = dr2["CityName"].ToString();
        //            cityModel.CityCode = dr2["CityCode"].ToString();
        //        }
        //        cmd.ExecuteNonQuery();
        //        return View("AddEdit", cityModel);
        //    }
        //    return View("AddEdit");
        //}

        public IActionResult Delete(int CityID)
        {
            string connectionstr = _configuration.GetConnectionString("myConnString");
            using (SqlConnection conn = new SqlConnection(connectionstr))
            {
                conn.Open();
                using (SqlCommand sqlCommand = conn.CreateCommand())
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_LOC_City_Delete";
                    sqlCommand.Parameters.AddWithValue("@CityID", CityID);
                    sqlCommand.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index");
        }

        // This action displays the City Add/Edit form
        public IActionResult Save(CityModel cityModel)
        {
            try
            {
                string str = this._configuration.GetConnectionString("myConnString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();

                cmd.Parameters.AddWithValue("StateID", cityModel.StateID);
                cmd.Parameters.AddWithValue("CountryID", cityModel.CountryID);
                cmd.Parameters.AddWithValue("CityName", cityModel.CityName);
                cmd.Parameters.AddWithValue("CityCode", cityModel.CityCode);

                if (cityModel.CityID != null)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PR_LOC_City_Update";
                    cmd.Parameters.AddWithValue("CityID", cityModel.CityID);
                }
                else
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PR_LOC_City_Insert";  
                }
                
                if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
                {
                    if (cityModel.CityID == null)
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
                return RedirectToAction("AddEdit", cityModel);
            }
        }
        public IActionResult AddEdit(int? cityId)
        {
            ViewBag.countryList=CountryDropdown();
            ViewBag.stateList=StateDropdown();
            if (cityId.HasValue)
            {
                string connectionstr = _configuration.GetConnectionString("myConnString");
                DataTable dt = new DataTable();

                using (SqlConnection conn = new SqlConnection(connectionstr))
                {
                    conn.Open();
                    using (SqlCommand objCmd = conn.CreateCommand())
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.CommandText = "PR_LOC_City_SelectByPK";
                        objCmd.Parameters.Add("@CityID", SqlDbType.Int).Value = cityId;

                        using (SqlDataReader objSDR = objCmd.ExecuteReader())
                        {
                            dt.Load(objSDR);
                        }
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    CityModel model = new CityModel();
                    foreach (DataRow dr in dt.Rows)
                    {
                        model.CityID = Convert.ToInt32(dr["CityID"]);
                        model.CityName = dr["CityName"].ToString();
                        model.StateID = Convert.ToInt32(dr["StateID"]);
                        model.CountryID = Convert.ToInt32(dr["CountryID"]);
                        model.CityCode = dr["CityCode"].ToString();
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

