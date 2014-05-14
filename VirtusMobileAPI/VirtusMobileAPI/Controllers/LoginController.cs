using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VirtusBI;

namespace VirtusMobileAPI.Controllers
{
    [RoutePrefix("VirtusApi/Login")]
    public class LoginController : ApiController
    {
        clsLogin repository = new clsLogin();

        [HttpGet]
        [Route("GetUserAfterAuthentication/{UserName}/{password}")]
        public HttpResponseMessage GetUser(string UserName, string password)
        {
            //var result = repository.fnCheckUser(UserName);
            //if (result != null && result.Tables[0].Rows.Count > 0)
            //    return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
            //else
            //    return Request.CreateResponse(HttpStatusCode.NotFound);

            DataSet ds = repository.fnCheckUser(UserName);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (Common.GetDecryptedValue(ds.Tables[0].Rows[0]["loginpassword"].ToString()).Equals(password))
                {
                    ds.Tables[0].Columns.Remove("loginpassword");
                    return Request.CreateResponse(HttpStatusCode.OK, ds, Configuration.Formatters.JsonFormatter);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, false, Configuration.Formatters.JsonFormatter);
                }
            }
            else
                return Request.CreateResponse(HttpStatusCode.NotFound, "Enter Correct LoginDetails", Configuration.Formatters.JsonFormatter);


        }

        [HttpGet]
        [Route("IsUserAuthenticated/{UserName}/{password}")]
        public HttpResponseMessage IsUserAuthenticated(string UserName, string password)
        {
            DataSet ds = repository.fnCheckUser(UserName);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (Common.GetDecryptedValue(ds.Tables[0].Rows[0]["loginpassword"].ToString()).Equals(password))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, true, Configuration.Formatters.JsonFormatter);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, false, Configuration.Formatters.JsonFormatter);
                }
            }
            else
                return Request.CreateResponse(HttpStatusCode.NotFound, ds, Configuration.Formatters.JsonFormatter);

        }


        [HttpGet]
        [Route("CheckUserName/{UserName}")]
        public HttpResponseMessage CheckUserName(string UserName)
        {
            bool isWindowsPassword = default(bool);
            bool isUserExists = repository.fnCheckUserName(UserName, ref isWindowsPassword);
            //LoginUserNameExist obj = new LoginUserNameExist();
            //obj.IsUserDetailsExists = result;
            //obj.IsWindowsPassword = isWindowsPassword;

            var result = new { IsUserDetailsExists = isUserExists };
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

        }

        [HttpPost]
        [AcceptVerbs("GET", "POST")]
        [Route("SaveCurrentUserDetails/{userId}/{ipAddress}/{windowsUser}")]
        public HttpResponseMessage SaveCurrentLoginDetails(string userId, string ipAddress, string windowsUser)
        {
            try
            {
                repository.fnSaveCurrentLoginDeatils(Convert.ToInt32(userId), ipAddress, windowsUser);
                return Request.CreateResponse(HttpStatusCode.OK, "Inserted", Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error while inserting..");

            }
        }





    }
}
