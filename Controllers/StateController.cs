using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Admin3.Models;

namespace Admin3.Controllers
{
    public class StateController : Controller
    {
        private IConfiguration _configuration;
        public StateController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
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
            objCmd.CommandText = "loc_state_selectAll";

            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            conn.Close();
            return View("Index", dt);
        }

        public IActionResult Delete(int StateID)
        {
            string connectionstr = _configuration.GetConnectionString("myConnString");
            using (SqlConnection conn = new SqlConnection(connectionstr))
            {
                conn.Open();
                using (SqlCommand sqlCommand = conn.CreateCommand())
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_LOC_State_Delete";
                    sqlCommand.Parameters.AddWithValue("@StateID", StateID);
                    sqlCommand.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index");
        }

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

        public IActionResult Save(StateModel stateModel)
        {
            try
            {
                string str = this._configuration.GetConnectionString("myConnString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();

                cmd.Parameters.AddWithValue("CountryID", stateModel.CountryID);
                cmd.Parameters.AddWithValue("StateName", stateModel.StateName);
                cmd.Parameters.AddWithValue("StateCode", stateModel.StateCode);

                if (stateModel.StateID != null)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PR_LOC_State_Update";
                    cmd.Parameters.AddWithValue("StateID", stateModel.StateID);
                }
                else
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PR_LOC_Satet_Insert";
                }

                if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
                {
                    if (stateModel.StateID== null)
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
                return RedirectToAction("AddEdit", stateModel);
            }
        }
        public IActionResult AddEdit(int? stateID)
        {
            ViewBag.countryList = CountryDropdown();
            if (stateID.HasValue)
            {
                string connectionstr = _configuration.GetConnectionString("myConnString");
                DataTable dt = new DataTable();

                using (SqlConnection conn = new SqlConnection(connectionstr))
                {
                    conn.Open();
                    using (SqlCommand objCmd = conn.CreateCommand())
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.CommandText = "PR_LOC_State_SelectByPK";
                        objCmd.Parameters.Add("@StateID", SqlDbType.Int).Value = stateID;

                        using (SqlDataReader objSDR = objCmd.ExecuteReader())
                        {
                            dt.Load(objSDR);
                        }
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    StateModel model = new StateModel();
                    foreach (DataRow dr in dt.Rows)
                    {
                        model.StateName= dr["StateName"].ToString();
                        model.StateID = Convert.ToInt32(dr["StateID"]);
                        model.StateCode = dr["StateCode"].ToString();
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
