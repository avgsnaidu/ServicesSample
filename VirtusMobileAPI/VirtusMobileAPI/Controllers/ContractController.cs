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

        /// <summary>
        /// Returns records related/specific to loginUser with done or processed filter.
        /// </summary>
        /// <param name="LoginUserName">login user name</param>
        /// <param name="IsProcessed">Is contract is processed or not (it depends on project status)</param>
        /// <param name="IsUnDone">Is contract is done or not.</param>
        /// <returns></returns>
        [Route("VirtusApi/Contract/GetMyContractsList/{LoginUserName}/{IsProcessed}/{IsUnDone}")]
        public HttpResponseMessage GetMyContractsList(string LoginUserName, bool IsProcessed, bool IsUnDone)
        {
            string whereCondition = repository.GetWorkPlaceWhereCondition(LoginUserName);
            var result = repository.GetListViewDataSet(ConverterHelper.CheckSingleQuote(whereCondition), LoginUserName, IsUnDone, IsProcessed);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }


        /// <summary>
        /// It gets the all the details of contracts which is accessible to login User with done or processed filter.
        /// </summary>
        /// <param name="LoginUserName"> login user name</param>
        /// <param name="IsProcessed">Is contract is processed or not (it depends on project status)</param>
        /// <param name="IsUnDone">Is contract is done or not.</param>
        /// <returns></returns>
        [ActionName("GetContractsList")]
        [Route("VirtusApi/Contract/GetContractsList/{LoginUserName}/{IsProcessed}/{IsUnDone}")]
        public HttpResponseMessage GetContractsList(string LoginUserName, bool IsProcessed, bool IsUnDone)
        {
            var result = repository.GetListViewDataSet("", LoginUserName, IsUnDone, IsProcessed);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

       

        [ActionName("GetContractsListWithSearch")]
        [Route("VirtusApi/Contract/GetContractsList/{LoginUserName}/{TotalCount}/{IsProcessed}/{IsUnDone}/{NoOfRecords}/{OrderBy}/{SortOrder}/{SearchCondition}/{WhereCondition}")]
        public HttpResponseMessage GetContractsListWithSearch(string WhereCondition, string SearchCondition, int NoOfRecords, string OrderBy, string SortOrder, string LoginUserName, ref int TotalCount, bool IsProcessed, bool IsUnDone)
        {
            var result = repository.GetListViewDataSet(ConverterHelper.CheckSingleQuote(WhereCondition), ConverterHelper.CheckSingleQuote(SearchCondition), NoOfRecords, ConverterHelper.CheckSingleQuote(OrderBy), ConverterHelper.CheckSingleQuote(SortOrder), LoginUserName, ref TotalCount, IsUnDone, IsProcessed);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }
        /// <summary>
        /// Returns Contract Details
        /// </summary>
        /// <param name="contractId"> Contractid of record.</param>
        /// <returns></returns>
        [ActionName("GetContractDetails")]
        [Route("VirtusApi/Contract/GetContract/{contractId}")]
        public HttpResponseMessage GetContractDetails(int contractId)
        {
            var result = repository.fnGetContractDetails(contractId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        /// <summary>
        /// Returns the Contract Milestones details if it has any already save milestones.
        /// </summary>
        /// <param name="contractId">contract id </param>
        /// <param name="objectType">Object Type for contractId like - 27 </param>
        /// <returns></returns>
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



        /// <summary>
        /// Returns the list Contractors details associated with the Tender and Contract.
        /// </summary>
        /// <param name="designId"></param>
        /// <returns></returns>
        [ActionName("GetContractorsList")]
        [Route("VirtusApi/Contract/GetContractorsList/{tenderId}")]
        public HttpResponseMessage GetContractorsList(string tenderId)
        {
            var result = repository.fnGetContractorsList(tenderId);
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



        //[ActionName("GetWorkPlaceWhereCondition")]
        //[Route("VirtusApi/Contract/GetWorkPlaceWhereCondition/{userLoginId}")]
        public HttpResponseMessage GetWorkPlaceWhereCondition(string userLoginId)
        {
            var result = repository.GetWorkPlaceWhereCondition(userLoginId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }


        [ActionName("GetIsAccessDenied")]
        [Route("VirtusApi/Contract/IsAccessDenied/{LoginUserName}/{RecordId}/{IsForModify}")]
        public HttpResponseMessage GetIsAccessDenied(string LoginUserName, string RecordId, bool IsForModify)
        {
            var result = repository.IsAccessDenied(LoginUserName, RecordId, IsForModify);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [ActionName("GetCurrencyDetails")]
        [Route("VirtusApi/Contract/GetCurrencyDetails")]
        public HttpResponseMessage GetCurrencyDetails()
        {
            var result = repository.fnGetCurrencyDetails();
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }


        /// <summary>
        /// For Saving the Contract Details and Creating the Project Call this service.
        /// </summary>
        /// <param name="RecordId"></param>
        /// <param name="createProject"> If you need to create the Project the - True otherwise false</param>
        /// <param name="userInitials"></param>
        /// <param name="parentId"></param>
        /// <param name="data"> Penality Type values is like : PerDay = 1,PerWeek = 2,PerMonth = 3,Percentage = 4,Amount = 5</param>
        /// <returns></returns>

        [HttpPost]
        [ActionName("SaveContractDetails")]
        [Route("VirtusApi/Contract/SaveContract/{RecordId}/{createProject}/{userInitials}/{parentId}")]
        public HttpResponseMessage SaveContractDetails(int RecordId, bool createProject, string userInitials, int parentId, [FromBody]ContractSaveActionData data)
        {
            try
            {
                if (data.MileStoneActionData == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Milestones data should not be null");
                }
                else if (data.ContractVendorsData == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Contract vendors data should not be null");

                DataTable dtMileStone = ConverterHelper.ConvertToDataTable<MileStoneActionData>(data.MileStoneActionData);
                DataTable dtContractVendors = ConverterHelper.ConvertToDataTable<ContractVendorsData>(data.ContractVendorsData);

                repository.BeginTrans();

                if (!repository.fnSaveData(RecordId, data.ContractActionData, createProject, dtMileStone, dtContractVendors, userInitials, parentId))
                {
                    repository.RollbackTrans();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Request Not Saved");

                }
                else
                {
                    repository.CommitTrans();
                    return Request.CreateResponse(HttpStatusCode.OK, true, Configuration.Formatters.JsonFormatter);

                }
            }
            catch (Exception ex)
            {
                repository.RollbackTrans();
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }


        [HttpPost]
        [ActionName("InsertPMObjectRight")]
        [Route("VirtusApi/Contract/InsertPMObjectRight/{RecordId}/{ObjectEnumId}")]
        public HttpResponseMessage InsertPMObjectRight(int RecordId, int ObjectEnumId)
        {
            var result = repository.fnInsertPMObjectRight(RecordId, ObjectEnumId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }



        [HttpPost]
        [ActionName("ChangeContractStatus")]
        [Route("VirtusApi/Contract/ChangeContractStatus/{RecordId}")]
        public HttpResponseMessage ChangeContractStatus(int RecordId)
        {
            bool FavAffected = default(bool);
            var result = repository.fn_ChangeStatus(RecordId, ref FavAffected);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [ActionName("SetIsContractDone")]
        [Route("VirtusApi/Contract/SetIsDone/{RecordId}/{IsDone}/{ModifiedBy}")]
        public HttpResponseMessage SetIsContractDone(string RecordId, int IsDone, string ModifiedBy)
        {
            var result = repository.fnSetIsDone(RecordId, IsDone, ModifiedBy);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }


        [HttpPost]
        [ActionName("SetReadStatusObject")]
        [Route("VirtusApi/Contract/SetReadStatus/{RecordId}/{ObjectEnumId}/{LoginName}")]
        public HttpResponseMessage SetReadStatusObject(int RecordId, int ObjectEnumId, string LoginName)
        {
            var result = repository.fnSetReadStatusObject(RecordId, ObjectEnumId, LoginName);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }



    }
}
