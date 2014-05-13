using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VirtusBI;

namespace VirtusMobileAPI.Controllers
{
     [RoutePrefix("VirtusApi/Address")]
    public class AddressController : ApiController
    {
        clsAddresses repository = new clsAddresses();
       

        [ActionName("GetContactPositionsList")]
        [Route("GetContactPositionsList")]
        public HttpResponseMessage GetContactPositionsList()
        {
            var result = repository.GetContactPositionsList();
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }



    }
}
