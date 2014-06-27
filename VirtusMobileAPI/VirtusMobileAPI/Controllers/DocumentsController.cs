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
    /// <summary>
    /// All the Document related action services for each object.
    /// </summary>
    [RoutePrefix("VirtusApi/Documents")]
    public class DocumentsController : ApiController
    {
        clsDocuments repository = new clsDocuments();

        /// <summary>
        /// Returns the List of Document Types (which relates to objects).
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDocumentTypes")]
        public HttpResponseMessage GetDocumentTypeList(int fileId)
        {
            var result = repository.GetDocumentTypeList();
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }
        /// <summary>
        /// Returns the just file Name of the File.
        /// </summary>
        /// <param name="fileId">specify the fileId</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFileName/{fileId}")]
        public HttpResponseMessage GetFileName(int fileId)
        {
            var result = repository.GetFileName(fileId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }


        /// <summary>
        /// Get the single document information
        /// </summary>
        /// <param name="objectRecordId"></param>
        /// <param name="fileId"></param>
        /// <param name="loginName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSingleFile/{objectRecordId}/{fileId}/{loginName}")]
        public HttpResponseMessage GetSingleFileDetails(int objectRecordId, string fileId, string loginName)
        {
            var result = repository.GetDocDatasetSingleFile(objectRecordId, fileId, 3, loginName);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }


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
        /// Retruns the list of documents/files corresponding to the object recordId.
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="recordId"></param>
        /// <param name="loginName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetObjectRecordDocumentsList/{objectType}/{recordId}/{loginName}")]
        public HttpResponseMessage GetObjectRecordDocumentsList(int objectType, int recordId, string loginName)
        {
            var result = repository.GetDocDataset(objectType, recordId, false, 3, true, true, true, true, true, loginName);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }




        /// <summary>
        /// For Saving the Genarated Tender letter.
        /// </summary>
        /// <param name="tenderId">tender Id for which the letter is genarated</param>
        /// <param name="loginUser">current login username</param>
        /// <param name="genaratedTenderLetterData">genarated tender letter saving content/information.</param>
        /// <returns></returns>

        [HttpPost]
        [Route("SaveGenaratedTenderLetter/{tenderId}/{loginUser}")]
        public HttpResponseMessage SaveGenaratedTenderLetter(int tenderId, string loginUser, [FromBody]TenderLetterActionData genaratedTenderLetterData)
        {
            try
            {
                if (genaratedTenderLetterData != null)
                {
                    var result = repository.fnSaveGeneratedTenderLetter(tenderId, loginUser, genaratedTenderLetterData.FileName, genaratedTenderLetterData.Extension, genaratedTenderLetterData.FileContent);
                    return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Provide valid document details,those should not be null.");
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error while saving the data" + ex.Message);
            }
        }




        /// <summary>
        /// Saves the File or document related to specific Object.
        /// </summary>
        /// <param name="objectTypeId">Objects Type Id </param>
        /// <param name="recordId">RecordId of the specific object in which the current documents relates to</param>
        /// <param name="loginUserId">login userId</param>
        /// <param name="documentDetails">Document details for saving.</param>
        /// <returns> file Id of saved File/Document</returns>
        [HttpPost]
        [Route("SaveFile/{objectTypeId}/{recordId}/{loginUserName}")]
        public HttpResponseMessage SaveFileOrDocument(int objectTypeId, int recordId, string loginUserName, [FromBody]DocumentActionData documentDetails)
        {
            if (documentDetails == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Provide valid document details,those should not be null.");

            var dt = ConverterHelper.ConvertToDataTable(new List<DocumentActionData>() { documentDetails });
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                string savedFileId = string.Empty;
                try
                {
                    var fileContent = documentDetails.FileContent;
                    var smallThumbContent = documentDetails.SmallThumb;
                    var largeThumbContent = documentDetails.LargeThumb;

                    repository.BeginTrans();
                    var result = repository.fnSaveAttachFile(objectTypeId, dr, recordId, loginUserName, fileContent, smallThumbContent, largeThumbContent, ref savedFileId);
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
