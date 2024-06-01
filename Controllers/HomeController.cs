using Microsoft.AspNetCore.Mvc;
using MY_GYM.Models;
using Newtonsoft.Json;
using System.Collections;
using System.Data;
using System.Diagnostics;

namespace MY_GYM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Verify_Login([FromBody] MemberCreation member)
        {
            string strResult = string.Empty;
            HttpContext.Session.Clear();
            string strError = string.Empty;
            string strStatus = "00";
            dbConnect _dbConnect = new dbConnect();
            DataSet _ds = new DataSet();
            HttpContext.Session.Remove("LOGIN_ID");
            try
            {
                string strErrorMsg = string.Empty;
                Hashtable _hs = new Hashtable();
                _hs.Add("MEM_ID", member.ID);
                _hs.Add("FLAG", member.FLAG);
                _ds = _dbConnect.ExecSelectProcedure("P_FETCH_LOGIN_DETAILS", _hs, ref strErrorMsg);
                strError = strErrorMsg;
                if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                {
                    strStatus = "01";
                    string strLoginType = _ds.Tables[0].Rows[0]["MEM_LOGIN_TYPE"].ToString();
                    strResult = JsonConvert.SerializeObject(_ds).ToString();
                    HttpContext.Session.SetString("LOGIN_ID", member.ID);
                }
                strError = strErrorMsg;

            }
            catch (Exception ex)
            {
                strError = ex.Message;
            }

            return Json(new { result = strResult, status = strStatus, error = strError });
        }

        public IActionResult Sign_Out_Login([FromBody] MemberCreation member)
        {
            string strResult = string.Empty;
            string strError = string.Empty;
            string strStatus = "00";
            dbConnect _dbConnect = new dbConnect();
            DataSet _ds = new DataSet();
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("LOGIN_ID");
            try
            {
                string strErrorMsg = string.Empty;
                Hashtable _hs = new Hashtable();
                _hs.Add("MEM_ID", member.ID);
                _hs.Add("FLAG", member.FLAG);
                _ds = _dbConnect.ExecSelectProcedure("P_FETCH_LOGIN_DETAILS", _hs, ref strErrorMsg);
                strError = strErrorMsg;
                if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                {
                    strStatus = "01";
                    string strLoginType = _ds.Tables[0].Rows[0]["MEM_LOGIN_TYPE"].ToString();
                    if (strLoginType == "A")
                    {
                        RedirectToAction("Admin", "AdminPage");
                        // strResult = JsonConvert.SerializeObject(_ds).ToString();
                    }
                    else
                    {
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
    }
}
