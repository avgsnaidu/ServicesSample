using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VirtusBI;
using VirtusDataModel;
using VirtusMobileService.Models;

namespace VirtusMobileAPI.Controllers
{
    public class TenderController : ApiController
    {
        clsDesignTenders repository = new clsDesignTenders();

        [ActionName("GetTenderDetails")]
        [Route("VirtusApi/Tender/GetTenderDetails/{tenderId}")]
        public HttpResponseMessage GetTenderDetails(int tenderId)
        {
            var result = repository.fnGetDesignDetails(tenderId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }
        /// <summary>
        /// Get all the tender Consultants of the Tender by providing the Tender Id
        /// </summary>
        /// <param name="tenderId"></param>
        /// <returns></returns>

        [ActionName("GetDesignTenderConsultants")]
        [Route("VirtusApi/Tender/GetTenderConsultants/{tenderId}")]
        public HttpResponseMessage GetDesignTenderConsultants(int tenderId)
        {
            var result = repository.GetDesignConsultants(tenderId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

        }

        [ActionName("GetApprovingAuthority")]
        [Route("VirtusApi/Tender/GetApprovingAuthority")]
        public HttpResponseMessage GetApprovingAuthority()
        {
            var result = repository.fnGetUsersForApprovingAuthority();
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

        }

        [ActionName("GetNextObjectRecordId")]
        [Route("VirtusApi/Tender/GetNextObjectRecordId/{RecordId}/{objectEnumId}")]
        public HttpResponseMessage GetNextObjectRecordId(int RecordId, int objectEnumId)
        {
            var result = repository.fnGetObjectRecordId(RecordId, objectEnumId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }



        [ActionName("GetDesignTendersList")]
        [Route("VirtusApi/Tender/GetRecordsList/{LoginUserName}/{IsDesign}/{IsUnDone}/{WhereCondition}")]
        public HttpResponseMessage GetDesignTendersList(string WhereCondition, string LoginUserName, bool IsDesign, bool IsUnDone)
        {
            var result = repository.GetListViewDataSet(ConverterHelper.CheckSingleQuote(WhereCondition), LoginUserName, IsDesign, IsUnDone);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }


        [ActionName("GetDesignTendersListWithSearch")]
        [Route("VirtusApi/Tender/GetRecordsListBasedOnSearch/{LoginUserName}/{TotalCount}/{IsDesign}/{IsUnDone}/{NoOfRecords}/{OrderBy}/{SortOrder}/{SearchCondition}/{WhereCondition}")]
        public HttpResponseMessage GetDesignTendersListWithSearch(string WhereCondition, string SearchCondition, int NoOfRecords, string OrderBy, string SortOrder, string LoginUserName, ref int TotalCount, bool IsDesign, bool IsUnDone)
        {
            var result = repository.GetListViewDataSet(ConverterHelper.CheckSingleQuote(WhereCondition), ConverterHelper.CheckSingleQuote(SearchCondition), NoOfRecords, ConverterHelper.CheckSingleQuote(OrderBy), ConverterHelper.CheckSingleQuote(SortOrder), ConverterHelper.CheckSingleQuote(LoginUserName), ref TotalCount, IsDesign, IsUnDone);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        /// <summary>
        /// Saving the Tender Consultants of particular Tender 
        /// </summary>
        /// <param name="designId"></param>
        /// <param name="data">Here need to pass the parameter of Type TenderConsultantActionData with properties Containg like   (int)AddresseId  ,
        ///   (string)Comments  ,
        ///    (string) Component  ,
        ///   (bool )Flag  ,
        ///   (bool )IsSelected  ,
        ///   (int )RecordId   </param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("SaveTenderConsultants")]
        [Route("VirtusApi/Tender/SaveTenderConsultants/{designId}")]
        public HttpResponseMessage SaveTenderConsultants(int designId, [FromBody]TenderConsultantActionData data)
        {
            var listData = new List<TenderConsultantActionData>() { data };

            DataTable dt = ConverterHelper.ConvertToDataTable(listData);

            if (dt != null)
            {
                var result = repository.fnSaveConsultants(dt, designId);
                return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please enter valid data");
            }
        }

        [HttpPost]
        [ActionName("InsertBasicTenderContracts")]
        [Route("VirtusApi/Tender/InsertTenderContracts")]
        public HttpResponseMessage InsertBasicTenderContracts([FromBody]TenderContractActionData data)
        {
            var result = repository.fnInsertBasicTenderContracts(data);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [ActionName("InsertPMObjectRights")]
        [Route("VirtusApi/Tender/InsertPMObjectRights/{RecordId}/{objectEnumId}")]
        public HttpResponseMessage InsertPMObjectRights(int RecordId, int objectEnumId)
        {
            var result = repository.fnInsertPMObjectRight(RecordId, objectEnumId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }



        [HttpPost]
        [ActionName("UpdateContractVendors")]
        [Route("VirtusApi/Tender/UpdateContractVendors/{recordId}")]
        public HttpResponseMessage UpdateContractVendors(int recordId)
        {
            var result = repository.fnUpdateContractVendors(recordId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

        }




        [HttpPost]
        [ActionName("SaveTenderDetails")]
        [Route("VirtusApi/Tender/SaveTender/{recordId}/{IsDesign}")]
        public HttpResponseMessage SaveTenderDetails(int recordId, bool IsDesign, [FromBody]TenderActionData data)
        {
            try
            {
                repository.BeginTrans();
                bool success = default(bool);
                int insertRecord = repository.fnSave(recordId, IsDesign, ref success, data);
                string str = string.Format(" Record Id : {0} and Saving Sucess is : {1}", insertRecord, success);
                if (!success)
                {
                    repository.RollbackTrans();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, str, Configuration.Formatters.JsonFormatter);

                }
                repository.CommitTrans();
                return Request.CreateResponse(HttpStatusCode.OK, str, Configuration.Formatters.JsonFormatter);

            }
            catch (Exception ex)
            {
                repository.RollbackTrans();
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }



        [HttpPost]
        [ActionName("SetReadStatus")]
        [Route("VirtusApi/Tender/SetReadStatus/{loginName}/{recordId}/{objectEnumId}")]
        public HttpResponseMessage SetReadStatus(string loginName, int recordId, int objectEnumId)
        {
            int result = repository.fnSetReadStatusObject(recordId, objectEnumId, loginName);
            if (result > 0)
                return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
            else
                return Request.CreateResponse(HttpStatusCode.NotModified, result, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [ActionName("SetIsDone")]
        [Route("VirtusApi/Tender/SetIsDone/{recordId}/{IsDone}/{ModifiedBy}")]
        public HttpResponseMessage SetIsDone(string recordId, int IsDone, string ModifiedBy)
        {
            try
            {
                repository.BeginTrans();
                var result = repository.fnSetIsDone(recordId, IsDone, ModifiedBy);
                repository.CommitTrans();
                if (result)
                    return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
                else
                {
                    repository.RollbackTrans();
                    return Request.CreateResponse(HttpStatusCode.NotModified, result, Configuration.Formatters.JsonFormatter);
                }
            }
            catch (Exception ex)
            {
                repository.RollbackTrans();
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }





    }
}
