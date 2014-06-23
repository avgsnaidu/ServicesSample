using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VirtusBI;
using VirtusMobileAPI.Models;
//using VirtusDataModel;

namespace VirtusMobileAPI.Controllers
{
    [RoutePrefix("VirtusApi/Documents")]
    public class DocumentsController : ApiController
    {
        clsDocuments repository = new clsDocuments();

        /// <summary>
        /// Returns the Details of the File/Document requested.
        /// </summary>
        /// <param name="fileId">Id of the requested file </param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFileDetails/{fileId}")]
        public HttpResponseMessage GetFileDetails(int fileId)
        {
            var result = repository.fnGetFileDeatils(fileId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        /// <summary>
        /// Saves the File or document related to specific Object.
        /// </summary>
        /// <param name="objectTypeId">Objects Type Id </param>
        /// <param name="recordId">RecordId of the specific object in which the current documents relates to</param>
        /// <param name="loginUserId">login userId</param>
        /// <param name="documentDetails">Document details for saving.</param>
        /// <returns> file Id of saved File/Document</returns>
        [Route("SaveFile/{objectTypeId}/{recordId}/{loginUserId}")]
        public HttpResponseMessage SaveFileOrDocument(int objectTypeId, int recordId, string loginUserId, [FromBody]DocumentActionData documentDetails)
        {
            var dt = ConverterHelper.ConvertToDataTable(new List<DocumentActionData>() { documentDetails });
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                string savedFileId = string.Empty;
                try
                {
                    repository.BeginTrans();
                    var result = repository.fnSaveAttachFile(objectTypeId, dr, recordId, loginUserId, ref savedFileId);
                    repository.CommitTrans();
                    return Request.CreateResponse(HttpStatusCode.OK, new { SavedSuccessfully = result, SavedFileId = savedFileId }, Configuration.Formatters.JsonFormatter);

                }
                catch (Exception ex)
                {
                    repository.RollbackTrans();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Some error while saving data. " + ex.Message);
                }
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Provide Valid Document Details");
        }




    }
}
