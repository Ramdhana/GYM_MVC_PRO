using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using MY_GYM.Models;
using Newtonsoft.Json;
using System.Collections;
using System.Data;
using System.Globalization;

namespace MY_GYM.Controllers
{
    public class Admin : Controller
    {
        public IActionResult AdminPage()
        {
             string loginId = HttpContext.Session.GetString("LOGIN_ID");
            if (string.IsNullOrEmpty(loginId))
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public IActionResult MemberCreation()
        {
            string loginId = HttpContext.Session.GetString("LOGIN_ID");
            if (string.IsNullOrEmpty(loginId))
            {
                return RedirectToAction("Login", "Home");  
            }
            return View();
        }

        public IActionResult ViewMember()
        {
            string loginId = HttpContext.Session.GetString("LOGIN_ID");
            if (string.IsNullOrEmpty(loginId))
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        [HttpPost]
        public IActionResult SubmitCreation([FromBody] MemberCreation member)
        {
            string loginId = HttpContext.Session.GetString("LOGIN_ID");
            if (string.IsNullOrEmpty(loginId))
            {
                return RedirectToAction("Login", "Home");
            }
            string strResult = string.Empty;
            string strError = string.Empty;
            string strStatus = "00";
            dbConnect _dbConnect = new dbConnect();
            DataSet _ds = new DataSet();
            try
            {
                if (member.FLAG == "I" || member.FLAG == "U")
                {
                    member.START = !string.IsNullOrEmpty(member.START)
                    ? DateTime.ParseExact(member.START, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyyMMdd")
                    : "";
                    member.END = !string.IsNullOrEmpty(member.END)
                    ? DateTime.ParseExact(member.END, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyyMMdd")
                    : "";
                    member.JOINING = !string.IsNullOrEmpty(member.JOINING)
                    ? DateTime.ParseExact(member.JOINING, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyyMMdd")
                    : "";
                    member.BIRTH = !string.IsNullOrEmpty(member.BIRTH)
                    ? DateTime.ParseExact(member.BIRTH, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyyMMdd")
                    : "";
                    string strErrorMsg = string.Empty;
                    Hashtable _hs = new Hashtable();
                    _hs.Add("MEM_NAME", Convert.ToString(member.NAME));
                    _hs.Add("MEM_GENDER", Convert.ToString(member.GENDER));
                    _hs.Add("MEM_IMAGE", Convert.ToString(member.IMAGE));
                    _hs.Add("MEM_TYPE", Convert.ToString(member.TYPE));
                    _hs.Add("MEM_EMAIL", Convert.ToString(member.EMAIL));
                    _hs.Add("MEM_MOBILE", Convert.ToString(member.MOBILE));
                    _hs.Add("MEM_ADDRESS", Convert.ToString(member.ADDRESS));
                    _hs.Add("MEM_JOININGDATE", Convert.ToString(member.JOINING));
                    _hs.Add("MEM_PACKAGE_TYPE", Convert.ToString(member.PACKAGE));
                    _hs.Add("MEM_PACKAGE_DAYS", Convert.ToString(member.DAYS));
                    _hs.Add("MEM_PACKAGE_START_DATE", Convert.ToString(member.START));
                    _hs.Add("MEM_PACKAGE_END_DATE", Convert.ToString(member.END));
                    _hs.Add("MEM_PAYMENT", Convert.ToString(member.PAYMENT));
                    _hs.Add("FLAG", Convert.ToString(member.FLAG));
                    _hs.Add("MEM_STATUS", Convert.ToString(member.STATUS));
                    _hs.Add("MEM_ID", Convert.ToString(member.ID));
                    _hs.Add("MEM_BIRTH", Convert.ToString(member.BIRTH));
                    _ds = _dbConnect.ExecSelectProcedure("P_MEMBER_PROFILE", _hs, ref strErrorMsg);
                    strError = strErrorMsg;
                    if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                    {
                        strStatus = _ds.Tables[0].Rows[0]["ResultCode"].ToString();
                    }
                }
                else
                {
                    string strErrorMsg = string.Empty;
                    Hashtable _hs = new Hashtable();
                    if (member.SEARCH_TYPE == "DB")
                    {
                        member.SEARCH_VALUE = !string.IsNullOrEmpty(member.SEARCH_VALUE)
                                       ? DateTime.ParseExact(member.SEARCH_VALUE, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd")
                                       : "";

                    }
                    _hs.Add("MEM_SEARCH_TYPE", member.SEARCH_TYPE);
                    _hs.Add("MEM_SEARCH_VALUE", member.SEARCH_VALUE);
                    
                    _hs.Add("FLAG", member.FLAG);
                    _ds = _dbConnect.ExecSelectProcedure("P_MEMBER_PROFILE", _hs, ref strErrorMsg);
                    strError = strErrorMsg;
                    if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                    {
                        strStatus = "01";
                        strResult = JsonConvert.SerializeObject(_ds).ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
            }

            return Json(new { result = strResult, status = strStatus, error = strError });
        }


        public IActionResult Fetch_Reports()
        {
            string loginId = HttpContext.Session.GetString("LOGIN_ID");
            if (string.IsNullOrEmpty(loginId))
            {
                return RedirectToAction("Login", "Home");
            }
            string strResult = string.Empty;

            string strError = string.Empty;
            string strStatus = "00";
            dbConnect _dbConnect = new dbConnect();
            DataSet _ds = new DataSet();
            try
            {
                string strErrorMsg = string.Empty;
                Hashtable _hs = new Hashtable();
                _ds = _dbConnect.ExecSelectProcedure("P_FETCH_ADMIN_REPORT", _hs, ref strErrorMsg);
                strError = strErrorMsg;
                if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                {
                    strStatus = "01";
                    strResult = JsonConvert.SerializeObject(_ds).ToString();
                }

            }
            catch (Exception ex)
            {
                strError = ex.Message;
            }

            return Json(new { result = strResult, status = strStatus, error = strError });
        }
    }
}
