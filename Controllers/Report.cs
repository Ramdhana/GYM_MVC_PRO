using Microsoft.AspNetCore.Mvc;
using MY_GYM.Models;
using Newtonsoft.Json;
using System.Collections;
using System.Data;
using System.Globalization;

namespace MY_GYM.Controllers
{
    public class Report : Controller
    {
        public IActionResult MemberActivity()
        {
            string loginId = HttpContext.Session.GetString("LOGIN_ID");
            if (string.IsNullOrEmpty(loginId))
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public IActionResult SalesPage()
        {
            string loginId = HttpContext.Session.GetString("LOGIN_ID");
            if (string.IsNullOrEmpty(loginId))
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public IActionResult MemberActivity_Report([FromBody] MemberCreation member)
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
                member.START = !string.IsNullOrEmpty(member.START)
                ? DateTime.ParseExact(member.START, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyyMMdd")
                : "";
                member.END = !string.IsNullOrEmpty(member.END)
                ? DateTime.ParseExact(member.END, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyyMMdd")
                : "";
                string strErrorMsg = string.Empty;
                Hashtable _hs = new Hashtable();
                _hs.Add("MEM_ID", member.ID);
                _hs.Add("MEM_START", member.START);
                _hs.Add("MEM_END", member.END);

                _ds = _dbConnect.ExecSelectProcedure("P_FETCH_MEMBER_ACTIVITY_REPORT", _hs, ref strErrorMsg);
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


        public IActionResult Sales_Reports([FromBody] MemberCreation member)
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
                member.START = !string.IsNullOrEmpty(member.START)
                ? DateTime.ParseExact(member.START, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyyMMdd")
                : "";
                member.END = !string.IsNullOrEmpty(member.END)
                ? DateTime.ParseExact(member.END, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyyMMdd")
                : "";
                string strErrorMsg = string.Empty;
                Hashtable _hs = new Hashtable();
                _hs.Add("MEM_START", member.START);
                _hs.Add("MEM_END", member.END);
                _ds = _dbConnect.ExecSelectProcedure("P_FETCH_SALES_REPORT", _hs, ref strErrorMsg);
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
