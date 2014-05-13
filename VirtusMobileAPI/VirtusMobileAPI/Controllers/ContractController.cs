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
    public class ContractController : ApiController
    {
        clsContracts repository = new clsContracts();
        [ActionName("GetContractsList")] 
        [Route("VirtusApi/Contract/GetContractsList/{LoginUserName}/{IsProcessed}/{IsUnDone}/{WhereCondition}")]
        public HttpResponseMessage GetContractsList(string WhereCondition, string LoginUserName, bool IsProcessed, bool IsUnDone)
        {
            var result = repository.GetListViewDataSet(ConverterHelper.CheckSingleQuote(WhereCondition), LoginUserName, IsUnDone, IsProcessed);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }


        [ActionName("GetContractsListWithSearch")]
        [Route("VirtusApi/Contract/GetContractsList/{LoginUserName}/{TotalCount}/{IsProcessed}/{IsUnDone}/{NoOfRecords}/{OrderBy}/{SortOrder}/{SearchCondition}/{WhereCondition}")]
        public HttpResponseMessage GetContractsListWithSearch(string WhereCondition, string SearchCondition, int NoOfRecords, string OrderBy, string SortOrder, string LoginUserName, ref int TotalCount, bool IsProcessed, bool IsUnDone)
        {
            var result = repository.GetListViewDataSet(ConverterHelper.CheckSingleQuote(WhereCondition), ConverterHelper.CheckSingleQuote(SearchCondition), NoOfRecords, ConverterHelper.CheckSingleQuote(OrderBy), ConverterHelper.CheckSingleQuote(SortOrder), LoginUserName, ref TotalCount, IsUnDone, IsProcessed);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [ActionName("GetContractDetails")]
        [Route("VirtusApi/Contract/GetContract/{contractId}")]
        public HttpResponseMessage GetContractDetails(int contractId)
        {
            var result = repository.fnGetContractDetails(contractId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [ActionName("GetContractMilestoneDetails")]
        [Route("VirtusApi/Contract/GetContractMileStone/{contractId}/{objectType}")]
        public HttpResponseMessage GetContractMilestoneDetails(string contractId, int objectType)
        {
            var result = repository.GetContractMilestones(contractId, objectType);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }


        [ActionName("GetContractVendors")]
        [Route("VirtusApi/Contract/GetContractVendors/{contractId}")]
        public HttpResponseMessage GetContractVendors(string contractId)
        {
            var result = repository.GetContractVendors(contractId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }




        [ActionName("GetContractorsList")]
        [Route("VirtusApi/Contract/GetContractorsList/{designId}")]
        public HttpResponseMessage GetContractorsList(string designId)
        {
            var result = repository.fnGetContractorsList(designId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [ActionName("GetIsProjectExist")]
        [Route("VirtusApi/Contract/GetIsProjectExist/{userReqeustId}")]
        public HttpResponseMessage GetIsProjectExistsForUserRequest(int userReqeustId)
        {
            var result = repository.fnProjectExistsForthisRequest(userReqeustId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }


        [ActionName("GetContainerId")]
        [Route("VirtusApi/Contract/GetContainerId/{recordId}/{userReqeustId}")]
        public HttpResponseMessage GetContainerId(string recordId, int userReqeustId)
        {
            var result = repository.fnGetContainerId(ConverterHelper.CheckSingleQuote(recordId), userReqeustId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }



        [ActionName("GetWorkPlaceWhereCondition")]
        [Route("VirtusApi/Contract/GetWorkPlaceWhereCondition/{userLoginId}")]
        public HttpResponseMessage GetWorkPlaceWhereCondition(string userLoginId)
        {
            var result = repository.GetWorkPlaceWhereCondition(userLoginId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }


        [ActionName("GetIsAccessDenied")]
        [Route("VirtusApi/Contract/IsAccessDenied/{LoginUserName}/{RecordId}/{ForModify}")]
        public HttpResponseMessage GetIsAccessDenied(string LoginUserName, string RecordId, bool ForModify)
        {
            var result = repository.IsAccessDenied(LoginUserName, RecordId, ForModify);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [ActionName("GetCurrencyDetails")]
        [Route("VirtusApi/Contract/GetCurrencyDetails")]
        public HttpResponseMessage GetCurrencyDetails()
        {
            var result = repository.fnGetCurrencyDetails();
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }
         


        [HttpPost]
        [ActionName("SaveContractDetails")]
        [Route("VirtusApi/Contract/SaveContract/{RecordId}/{createProject}/{userInitials}/{parentId}")]
        public HttpResponseMessage SaveContractDetails(int RecordId, bool createProject, string userInitials, int parentId, [FromBody]ContractSaveActionData data)
        {
            DataTable dtMileStone = ConverterHelper.ConvertToDataTable<MileStoneActionData>(new List<MileStoneActionData>() { data.MileStoneActionData });
            DataTable dtContractVendors = ConverterHelper.ConvertToDataTable<ContractVendorsData>(new List<ContractVendorsData> { data.ContractVendorsData });

            var result = repository.fnSaveData(RecordId, data.ContractActionData, createProject, dtMileStone, dtContractVendors, userInitials, parentId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }


        [HttpPost]
        [ActionName("InsertPMObjectRight")]
        [Route("VirtusApi/Contract/InsertPMObjectRight/{RecordId}/{ObjectEnumId}")]
        public HttpResponseMessage InsertPMObjectRight(int RecordId, int ObjectEnumId)
        {
            var result = repository.fnInsertPMObjectRight(RecordId, ObjectEnumId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }



        [HttpPut]
        [ActionName("ChangeContractStatus")]
        [Route("VirtusApi/Contract/ChangeContractStatus/{RecordId}")]
        public HttpResponseMessage ChangeContractStatus(int RecordId)
        {
            bool FavAffected = default(bool);
            var result = repository.fn_ChangeStatus(RecordId, ref FavAffected);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [HttpPut]
        [ActionName("SetIsContractDone")]
        [Route("VirtusApi/Contract/SetIsDone/{RecordId}/{IsDone}/{ModifiedBy}")]
        public HttpResponseMessage SetIsContractDone(string RecordId, int IsDone, string ModifiedBy)
        {
            var result = repository.fnSetIsDone(RecordId, IsDone, ModifiedBy);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }


        [HttpPut]
        [ActionName("SetReadStatusObject")]
        [Route("VirtusApi/Contract/SetReadStatus/{RecordId}/{ObjectEnumId}/{LoginName}")]
        public HttpResponseMessage SetReadStatusObject(int RecordId, int ObjectEnumId, string LoginName)
        {
            var result = repository.fnSetReadStatusObject(RecordId, ObjectEnumId, LoginName);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }



    }
}
