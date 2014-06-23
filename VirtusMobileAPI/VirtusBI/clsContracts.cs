using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EPMEnums;
using Microsoft.VisualBasic;
using VirtusDataModel;

namespace VirtusBI
{
    public class clsContracts : clsBaseBI
    {
        public DataSet GetListViewDataSet(string sWhereCondition, string sLoginUserName, bool bUnDone, bool bIsProcessed)
        {
            try
            {
                int iTotCount = 0;
                return GetListViewDataSet(sWhereCondition, "", 0, "18", "DESC", sLoginUserName, ref iTotCount, bUnDone, bIsProcessed);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetListViewDataSet(string sWhereCondition, string strSearchCondition, int iNoOfRecords, string strOrderBy, string strSortOrder, string sLoginUserName, ref int iTotCount, bool bUnDone, bool bIsProcessed)
        {
            try
            {

                SqlParameter[] Params = new SqlParameter[10];
                int iIndex = 0;


                Params[iIndex] = new SqlParameter("@WhereCondition", SqlDbType.NVarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = sWhereCondition;

                Params[++iIndex] = new SqlParameter("@SearchCondition", SqlDbType.NVarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = strSearchCondition;

                Params[++iIndex] = new SqlParameter("@NoTopRecords", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iNoOfRecords;

                Params[++iIndex] = new SqlParameter("@OrderBy", SqlDbType.VarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = strOrderBy;

                Params[++iIndex] = new SqlParameter("@SortOrder", SqlDbType.VarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = strSortOrder;

                Params[++iIndex] = new SqlParameter("@LoginName", SqlDbType.NVarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = sLoginUserName;

                Params[++iIndex] = new SqlParameter("@UILanguageId", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = CommonVariable.iLanguageId;

                Params[++iIndex] = new SqlParameter("@IsDone", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bUnDone;

                Params[++iIndex] = new SqlParameter("@IsProcessed", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bIsProcessed;


                Params[++iIndex] = new SqlParameter("@TotCount", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Output;


                DataSet ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetContractListViewDS", Params);
                iTotCount = int.Parse(Params[iIndex].Value.ToString());

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet fnGetContractDetails(int iContractId)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlParameter[] sPrms = new SqlParameter[1];
                SqlParameter sPrm = new SqlParameter();

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ContractId";
                sPrm.Value = iContractId;
                sPrms[0] = sPrm;

                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetContractDetailsDS", sPrms);
                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool fn_ChangeStatus(int iRecordID, ref bool bFavAffected)
        {
            try
            {

                SqlParameter[] Params = new SqlParameter[2];
                int iIndex = 0;


                Params[iIndex] = new SqlParameter("@RecordID", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iRecordID;

                Params[++iIndex] = new SqlParameter("@Status", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Output;

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spChangeContractStatus", Params);

                bFavAffected = Convert.ToBoolean(Params[1].Value.ToString());
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool fnSetIsDone(string strRecordID, int iIsDone, string strModifiedBy)
        {
            string strSql = "";

            try
            {
                strSql = "Update Contracts Set IsDone=" + iIsDone + ",";
                strSql += " ModifiedBy=" + Common.EncodeNString(strModifiedBy) + ",ModifiedOn=getdate()";
                strSql += " Where DesignId=" + strRecordID;

                Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);


                return true;
            }
            catch
            {
                return false;
            }
        }

        public string fnGetContractorsList(string strDesignId)
        {
            string sIds = "";

            string sSql = "Select AddresseId from DesignConsultantList where DesignId=" + strDesignId;
            DataSet ds = Common.dbMgr.ExecuteDataSet(CommandType.Text, sSql);

            if (ds != null)
            {
                if (ds.Tables[0] != null)
                {
                    for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                    {
                        if (sIds != "")
                            sIds = sIds + "," + ds.Tables[0].Rows[i]["AddresseId"].ToString();
                        else
                            sIds = ds.Tables[0].Rows[i]["AddresseId"].ToString();
                    }
                }
            }
            return sIds;
        }

        public bool fnProjectExistsForthisRequest(int iUserRequestId)
        {
            //    int iRecordId = 0;
            //    if (sRecordId != "")
            //        iRecordId = int.Parse(sRecordId);

            string strSQL = "";
            strSQL = "Select Case When count(1) > 0 Then 1 Else 0 End as ProjectExists from Contracts where UserRequestId=" + iUserRequestId;
            strSQL += " and Project is not null";
            if (int.Parse(Common.dbMgr.ExecuteScalar(CommandType.Text, strSQL)) > 0)
                return true;
            else
                return false;
        }

        public int fnGetContainerId(string sRecordId, int iUserRequestId)
        {
            try
            {
                int iRecordId = 0;
                if (sRecordId != "")
                    iRecordId = int.Parse(sRecordId);

                SqlParameter[] Params = new SqlParameter[3];
                int iIndex = 0;

                Params[iIndex] = new SqlParameter("@UserRequestID", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iUserRequestId;

                Params[++iIndex] = new SqlParameter("@ContractId", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iRecordId;

                Params[++iIndex] = new SqlParameter("@ContainerId", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Output;

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spGetContainerId", Params);
                return int.Parse(Params[iIndex].Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetContractMilestones(string sRecordID, int iObjType)
        {

            try
            {
                int iRecordId = 0;
                if (sRecordID != "")
                    iRecordId = int.Parse(sRecordID);
                //else
                //    sRecordID = 0;

                SqlParameter[] sPrms = new SqlParameter[3];
                SqlParameter sPrm = new SqlParameter();


                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ObjectId";
                sPrm.Value = iRecordId;
                sPrms[0] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ObjTypeID";
                sPrm.Value = iObjType;
                sPrms[1] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@UILanguageId";
                sPrm.Value = CommonVariable.iLanguageId;
                sPrms[2] = sPrm;


                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetContractMilestonesDs", sPrms);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetContractVendors(string sRecordID)
        {

            try
            {
                int iRecordId = 0;
                if (sRecordID != "")
                    iRecordId = int.Parse(sRecordID);
                //else
                //    sRecordID = 0;

                SqlParameter[] sPrms = new SqlParameter[1];
                SqlParameter sPrm = new SqlParameter();


                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ContractId";
                sPrm.Value = iRecordId;
                sPrms[0] = sPrm;


                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetContractVendorsDs", sPrms);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool fnSaveData(int iRecordID, ContractActionData data, bool bCreateProject, DataTable dt, DataTable dtContractVendors, string sUserInitials, int iParentId)
        {
            try
            {
                string strSql = "";
                strSql = "Update Contracts Set ";
                strSql += " Subject = " + Common.EncodeNString(data.Subject) + ",";
                if (data.ProjectMangaer != 0)
                    strSql += " ProjectManager=" + data.ProjectMangaer.ToString() + ",";
                else
                    strSql += " ProjectManager=NULL,";
                strSql += "Remarks=" + Common.EncodeNString(data.Remarks) + ",";

                if (data.ApprovingAuthority != 0)
                    strSql += " ApporivingAuthority=" + data.ApprovingAuthority.ToString() + ",";
                else
                    strSql += " ApporivingAuthority=NULL,";

                strSql += " ApprovedBudget=" + data.ApprovedBudget.ToString() + ",";

                if (data.DeadlineRemind)
                    strSql += "DeadlineRemind=1,DeadlineRemindDays=" + data.DeadlineRemindDays.ToString() + ",";
                else
                {
                    strSql += "DeadlineRemind=0,DeadlineRemindDays=null,";
                    data.DeadlineRemindDays = -1;
                }
                if (data.DeadLineEndDate == null || data.DeadLineEndDate == DateTime.MinValue)
                    strSql += "DeadlineEndDate=null,";
                else
                    strSql += "DeadlineEndDate=cast(" + DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, (DateTime)data.DeadLineEndDate, Microsoft.VisualBasic.FirstDayOfWeek.System, FirstWeekOfYear.System) + " as datetime),";

                if (data.IsDone)
                    strSql += "IsDone=1,";
                else
                    strSql += "IsDone=0,";

                if (data.UseSplRights)
                    strSql += " UseSpecialRights=1,";
                else
                    strSql += " UseSpecialRights=0,";

                if (data.OwnerHasFullRights)
                    strSql += " IsOwnerFullRights=1,";
                else
                    strSql += " IsOwnerFullRights=0,";

                if (data.DoneDate == null || data.DoneDate == DateTime.MinValue)
                    strSql += "DoneDate=Null,";
                else
                    strSql += "DoneDate=cast(" + DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, (DateTime)data.DoneDate, Microsoft.VisualBasic.FirstDayOfWeek.System, FirstWeekOfYear.System) + " as datetime),";

                strSql += "ModifiedBy=" + Common.EncodeNString(data.ModifiedBy) + ",ModifiedOn=" + "getdate(),";
                strSql += "Status=" + data.Status.ToString();
                strSql += " where ContractId=" + iRecordID;
                Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                strSql = "";

                //Updating Project Manager and Total budget in Tasks if it is modified
                if (data.Project > 0)
                {
                    if (data.IsProjectManagerChanged)
                    {
                        strSql = "Update Tasks set ProjectManager=" + data.ProjectMangaer;
                        strSql += " Where TaskId=" + data.Project;
                        Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                    }

                    //Here we have write the query to change the child projects budget.
                    decimal dDiffBudget = 0;
                    for (int l = 0; l < dtContractVendors.Rows.Count; l++)
                    {
                        dDiffBudget = 0;
                        if (dtContractVendors.Rows[l]["Flag"].ToString().ToUpper() == "M" && dtContractVendors.Rows[l]["Project"].ToString() != string.Empty)
                        {
                            if ((bool)dtContractVendors.Rows[l]["IsBudgetChanged"] == true)
                            {
                                strSql = "Update Tasks set Budget=" + Common.EncodeValue(Convert.ToDecimal(dtContractVendors.Rows[l]["Budget"]));
                                strSql += " where TaskId=" + Convert.ToInt32(dtContractVendors.Rows[l]["Project"]);
                                Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                                if (dtContractVendors.Rows[l]["PrevBudget"] != DBNull.Value && dtContractVendors.Rows[l]["PrevBudget"].ToString() != "")
                                    dDiffBudget = Convert.ToDecimal(dtContractVendors.Rows[l]["Budget"]) - Convert.ToDecimal(dtContractVendors.Rows[l]["PrevBudget"]);
                                else
                                    dDiffBudget = Convert.ToDecimal(dtContractVendors.Rows[l]["Budget"]);
                                UpdateBudgetForParentProjects(dtContractVendors.Rows[l]["Project"].ToString(), dDiffBudget);
                            }
                        }

                        if (data.IsProjectManagerChanged)
                        {
                            if (dtContractVendors.Rows[l]["Project"].ToString() != string.Empty)
                            {
                                strSql = "Update Tasks set ProjectManager=" + data.ProjectMangaer;
                                strSql += " Where TaskId=" + dtContractVendors.Rows[l]["Project"].ToString();
                                Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                            }
                        }
                    }
                    //END code to change the child projects budget.
                }

                if (data.IsProjectManagerChanged)
                    data.IsProjectManagerChanged = false;


                string strCustomerId = string.Empty;
                string strVendorSubject = string.Empty;
                bool bMorethanOneVendor = false;
                DataRow[] dr = dtContractVendors.Select("Flag<>'D' and Flag<>'E'");

                if (dr.Length == 1)
                {
                    strCustomerId = dr[0]["VendorId"].ToString();
                    strVendorSubject = dr[0]["Component"].ToString();
                }
                else if (dr.Length > 1)
                    bMorethanOneVendor = true;


                long dMilestoneMaxDate = 0;
                long dCurrentMaxDate = 0;

                if (data.Project > 0 && data.IsToDateChanged)
                {
                    strSql = "Select isnull(max(ToDate),0) from  Milestones";
                    strSql += " Where ObjectId=" + iRecordID + " And ObjectType=" + (int)Enums.VTSObjects.Contracts;
                    dMilestoneMaxDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(Common.dbMgr.ExecuteScalar(CommandType.Text, strSql)), FirstDayOfWeek.System, FirstWeekOfYear.System);
                }

                //Saving Contract Vendors
                SaveContractVendors(dtContractVendors, iRecordID);

                //Saving Milestones for contract
                SaveContractMilestones(dt, (int)(Enums.VTSObjects.Contracts), iRecordID);

                //Updating Task deadlines if ToDate is changed
                if (data.Project > 0 && data.IsToDateChanged)
                {
                    //Updating Individual Project deadlines
                    if (bMorethanOneVendor)
                    {
                        for (int iIndex = 0; iIndex <= dtContractVendors.Rows.Count - 1; iIndex++)
                        {
                            dCurrentMaxDate = 0;
                            if (dtContractVendors.Rows[iIndex]["Project"].ToString() != string.Empty)
                            {

                                strSql = "Select isnull(max(ToDate),0) from  Milestones";
                                strSql += " Where ObjectId=" + iRecordID + " And ObjectType=" + (int)Enums.VTSObjects.Contracts + " And VendorId=" + dtContractVendors.Rows[iIndex]["VendorId"];
                                dCurrentMaxDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(Common.dbMgr.ExecuteScalar(CommandType.Text, strSql)), FirstDayOfWeek.System, FirstWeekOfYear.System);

                                if (dCurrentMaxDate > 0 && (dtContractVendors.Rows[iIndex]["VendorMaxDate"] == DBNull.Value && dtContractVendors.Rows[iIndex]["VendorMaxDate"].ToString() == ""))
                                {
                                    strSql = "Insert into TaskDeadlines(TaskId,DateFrom,DateTill,CreatedBy,Reason,CreatedOn)";
                                    strSql += " Select " + dtContractVendors.Rows[iIndex]["Project"].ToString() + ", Min(FromDate),Max(ToDate)," + Common.EncodeNString(data.CreatedBy) + ",Null,getdate() from Milestones";
                                    strSql += " Where ObjectId=" + iRecordID + " And ObjectType=" + (int)Enums.VTSObjects.Contracts + " And VendorId=" + dtContractVendors.Rows[iIndex]["VendorId"];
                                    Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                                }
                                else if (dtContractVendors.Rows[iIndex]["VendorMaxDate"] != DBNull.Value && dtContractVendors.Rows[iIndex]["VendorMaxDate"].ToString() != "")
                                {
                                    if (dCurrentMaxDate != DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(dtContractVendors.Rows[iIndex]["VendorMaxDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System))
                                    {
                                        strSql = "Insert into TaskDeadlines(TaskId,DateFrom,DateTill,CreatedBy,Reason,CreatedOn)";
                                        strSql += " Select " + dtContractVendors.Rows[iIndex]["Project"].ToString() + ", Min(FromDate),Max(ToDate)," + Common.EncodeNString(data.CreatedBy) + ",Null,getdate() from Milestones";
                                        strSql += " Where ObjectId=" + iRecordID + " And ObjectType=" + (int)Enums.VTSObjects.Contracts + " And VendorId=" + dtContractVendors.Rows[iIndex]["VendorId"];
                                        Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                                    }
                                }
                            }

                        }
                    }

                    //Updating Main Project deadlines
                    dCurrentMaxDate = 0;

                    dr = dtContractVendors.Select("Flag<>'D' and Flag<>'E' and IsStumbled<>1");

                    if (dr != null && dr.Length > 1)
                    {
                        strSql = "Select isnull(max(ToDate),0) from  Milestones";
                        strSql += " Where ObjectId=" + iRecordID + " And ObjectType=" + (int)Enums.VTSObjects.Contracts;
                        dCurrentMaxDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(Common.dbMgr.ExecuteScalar(CommandType.Text, strSql)), FirstDayOfWeek.System, FirstWeekOfYear.System);

                        if (dCurrentMaxDate != dMilestoneMaxDate)
                        {
                            strSql = "Insert into TaskDeadlines(TaskId,DateFrom,DateTill,CreatedBy,Reason,CreatedOn)";
                            strSql += " Select " + data.Project + ", Min(FromDate),Max(ToDate)," + Common.EncodeNString(data.CreatedBy) + ",Null,getdate() from Milestones";
                            strSql += " Where ObjectId=" + iRecordID + " And ObjectType=" + (int)Enums.VTSObjects.Contracts;
                            Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                        }
                    }

                }

                if (data.IsToDateChanged)
                    data.IsToDateChanged = false;


                //Creating project
                if (bCreateProject)
                {
                    bool bProjIsManualCode = false;
                    string strProjectCode = "";
                    string strProjectId = "";
                    string strStumbleProjId = string.Empty;
                    string strStumbleParent = string.Empty;
                    bool bStumbleSingleVendor = false;
                    bool bCanCreateGroup = false;

                    if (!data.StumbleExists)
                    {
                        bProjIsManualCode = clsCodeGeneration.GetId((int)Enums.VTSObjects.Project);

                        if (bProjIsManualCode)
                            strProjectCode = "";
                        else
                            strProjectCode = clsCodeGeneration.GetObjectCode((int)Enums.VTSObjects.Project, sUserInitials, data.UserRequestId);

                        strSql = "";
                        strSql = "Insert into Tasks(IsProject,TaskCode,Subject,IsPartOf,CustomerId,Remarks,IsActive,IsManualCode,CreatedBy,CreatedOn,IsInternalProject,ProjectManager,Budget)";
                        strSql += " values(1," + Common.EncodeNString(data.ProjectCode) + ",";

                        if (!bMorethanOneVendor && strVendorSubject != string.Empty)
                            strSql += Common.EncodeNString(strVendorSubject) + ",";
                        else
                            strSql += Common.EncodeNString(data.Subject) + ",";

                        if (iParentId != 0)
                            strSql += iParentId.ToString() + ",";
                        else
                            strSql += "null,";

                        if (!bMorethanOneVendor && strCustomerId != string.Empty)
                            strSql += strCustomerId + ",";
                        else
                            strSql += "null,";

                        strSql += Common.EncodeNString(data.Remarks) + ",1,";

                        if (bProjIsManualCode)
                            strSql += "1,";
                        else
                            strSql += "0,";
                        strSql += Common.EncodeNString(data.CreatedBy) + ",getdate(),0,";

                        if (data.ProjectMangaer != 0)
                            strSql += data.ProjectMangaer.ToString() + ",";
                        else
                            strSql += "null,";

                        strSql += Common.EncodeValue((decimal)data.ApprovedBudget) + ")";

                        Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                        strProjectId = Common.dbMgr.ExecuteScalar(CommandType.Text, "Select max(TaskId) from Tasks");
                        if (strProjectId != "")
                        {
                            data.Project = int.Parse(strProjectId);


                            //Updating Project deadlines
                            strSql = "Insert into TaskDeadlines(TaskId,DateFrom,DateTill,CreatedBy,Reason,CreatedOn)";
                            strSql += " Select " + strProjectId + ", Min(FromDate),Max(ToDate)," + Common.EncodeNString(data.CreatedBy) + ",Null,getdate() from Milestones";
                            strSql += " Where ObjectId=" + iRecordID + " And ObjectType=" + (int)Enums.VTSObjects.Contracts;

                            Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                            if (bMorethanOneVendor)
                                InsertSubProjectComponents(data, strProjectId, sUserInitials, dtContractVendors, iRecordID, data.IsManualCode, dt);
                            else
                            {
                                //Update ProjectId in ContractVendors and Milestones table --> For single vendor or change request contracts
                                strSql = "Update ContractVendors Set Project=" + strProjectId;
                                strSql += " Where ContractId=" + iRecordID + " And VendorId=" + strCustomerId;
                                Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                                dr = dtContractVendors.Select("VendorId=" + strCustomerId);
                                if (dr != null && dr.Length > 0)
                                    dr[0]["Project"] = strProjectId;


                                strSql = "Update Milestones Set Project=" + strProjectId;
                                strSql += " Where ObjectId=" + iRecordID + " And ObjectType=" + (int)Enums.VTSObjects.Contracts + " And VendorId=" + strCustomerId;
                                Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                                dr = dt.Select("VendorId=" + strCustomerId);
                                if (dr != null && dr.Length > 0)
                                {
                                    for (int j = 0; j <= dr.Length - 1; j++)
                                    {
                                        dr[j]["Project"] = strProjectId;
                                    }
                                }

                            }
                            //fnForwardDocuments(int.Parse(strProjectId), iRecordID, strCreatedBy);
                        }

                    }
                    else
                    {


                        dr = dtContractVendors.Select("Flag<>'D' and Flag<>'E' and IsStumbled=1");
                        DataRow[] drNew = dtContractVendors.Select("Flag <> 'D' and Flag <> 'E' and IsStumbled=0");

                        //if ((dr != null && dr.Length == 1) && (drNew != null && drNew.Length == 1)) //If Single vendor exists
                        //{
                        if (IsSingleVendorForStumble(dtContractVendors, data.Project))
                        {
                            strStumbleProjId = dr[0]["Project"].ToString();
                            bStumbleSingleVendor = true;
                            if ((dr != null && dr.Length == 1) && (drNew != null && drNew.Length == 1))
                            {
                                bCanCreateGroup = true;
                            }
                            strStumbleParent = Common.dbMgr.ExecuteScalar(CommandType.Text, "Select isnull(IsPartOf,0) from tasks where TaskId=" + strStumbleProjId);

                            //If there is one stumble and added multiple vendors
                            if ((dr != null && dr.Length == 1) && (drNew != null && drNew.Length > 1))
                            {
                                bProjIsManualCode = clsCodeGeneration.GetId((int)Enums.VTSObjects.Project);

                                if (bProjIsManualCode)
                                    strProjectCode = "";
                                else
                                    strProjectCode = clsCodeGeneration.GetObjectCode((int)Enums.VTSObjects.Project, sUserInitials, data.UserRequestId);

                                strSql = "";
                                strSql = "Insert into Tasks(IsProject,TaskCode,Subject,IsPartOf,CustomerId,Remarks,IsActive,IsManualCode,CreatedBy,CreatedOn,IsInternalProject,ProjectManager,Budget)";
                                strSql += " values(1," + Common.EncodeNString(data.ProjectCode) + ",";

                                //if (!bMorethanOneVendor && strVendorSubject != string.Empty)
                                //    strSql += Common.EncodeNString(strVendorSubject) + ",";
                                //else
                                strSql += Common.EncodeNString(data.Subject) + ",";

                                if (strStumbleParent != string.Empty && strStumbleParent != "0")
                                    strSql += strStumbleParent.ToString() + ",";
                                else
                                    strSql += "null,";

                                strSql += "null,";

                                strSql += Common.EncodeNString(data.Remarks) + ",1,";

                                if (bProjIsManualCode)
                                    strSql += "1,";
                                else
                                    strSql += "0,";
                                strSql += Common.EncodeNString(data.CreatedBy) + ",getdate(),0,";

                                if (data.ProjectMangaer != 0)
                                    strSql += data.ProjectMangaer.ToString() + ",";
                                else
                                    strSql += "null,";

                                strSql += Common.EncodeValue((decimal)data.ApprovedBudget) + ")";

                                Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                                strProjectId = Common.dbMgr.ExecuteScalar(CommandType.Text, "Select max(TaskId) from Tasks");

                                if (strProjectId != string.Empty)
                                {
                                    strStumbleParent = strProjectId;
                                    strSql = "Update Tasks set IsPartOf=" + strProjectId + " Where TaskId=" + strStumbleProjId;
                                    Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                                }
                                // fnForwardDocuments(int.Parse(strProjectId), iRecordID, strCreatedBy);
                            }

                        }
                        //}
                        else //If multiple vendors exists
                        {
                            strStumbleParent = data.Project.ToString();
                        }

                        InsertSubProjectComponents(data, strStumbleParent, sUserInitials, dtContractVendors, iRecordID, data.IsManualCode, dt);

                        if (bStumbleSingleVendor)
                        {
                            drNew = dtContractVendors.Select("Flag <> 'D' and Flag <> 'E' and IsStumbled=0");
                            strProjectId = drNew[0]["Project"].ToString();
                        }
                        else
                        {
                            strProjectId = data.Project.ToString();
                        }

                    }

                    if (!data.StumbleExists || (data.StumbleExists && bStumbleSingleVendor && bCanCreateGroup))
                    {
                        strProjectCode = "";

                        if (bProjIsManualCode)
                            strProjectCode = "";
                        else
                            strProjectCode = clsCodeGeneration.GetObjectCode((int)Enums.VTSObjects.Project, sUserInitials, data.UserRequestId);

                        SqlParameter[] sPrms = new SqlParameter[9];
                        SqlParameter sPrm = new SqlParameter();


                        sPrm = new SqlParameter();
                        sPrm.SqlDbType = System.Data.SqlDbType.Int;
                        sPrm.Direction = ParameterDirection.Input;
                        sPrm.ParameterName = "@ContractID ";
                        sPrm.Value = iRecordID;
                        sPrms[0] = sPrm;

                        sPrm = new SqlParameter();
                        sPrm.SqlDbType = System.Data.SqlDbType.Int;
                        sPrm.Direction = ParameterDirection.Input;
                        sPrm.ParameterName = "@ProjectID";
                        sPrm.Value = strProjectId;
                        sPrms[1] = sPrm;

                        sPrm = new SqlParameter();
                        sPrm.SqlDbType = System.Data.SqlDbType.Int;
                        sPrm.Direction = ParameterDirection.Input;
                        sPrm.ParameterName = "@UserRequestID";
                        sPrm.Value = data.UserRequestId;
                        sPrms[2] = sPrm;

                        sPrm = new SqlParameter();
                        sPrm.SqlDbType = System.Data.SqlDbType.Int;
                        sPrm.Direction = ParameterDirection.Input;
                        sPrm.ParameterName = "@DesignID";
                        sPrm.Value = data.DesignId;
                        sPrms[3] = sPrm;

                        sPrm = new SqlParameter();
                        sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                        sPrm.Direction = ParameterDirection.Input;
                        sPrm.ParameterName = "@CreatedBy";
                        sPrm.Value = data.CreatedBy;
                        sPrms[4] = sPrm;

                        sPrm = new SqlParameter();
                        sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                        sPrm.Direction = ParameterDirection.Input;
                        sPrm.ParameterName = "@TaskCode";
                        sPrm.Value = strProjectCode;
                        sPrms[5] = sPrm;

                        sPrm = new SqlParameter();
                        sPrm.SqlDbType = System.Data.SqlDbType.Decimal;
                        sPrm.Direction = ParameterDirection.Input;
                        sPrm.ParameterName = "@CurrentBudget";
                        sPrm.Value = data.ApprovedBudget;
                        sPrms[6] = sPrm;

                        sPrm = new SqlParameter();
                        sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                        sPrm.Direction = ParameterDirection.Input;
                        sPrm.ParameterName = "@IsStumbled";
                        sPrm.Value = data.StumbleExists;
                        sPrms[7] = sPrm;

                        sPrm = new SqlParameter();
                        sPrm.SqlDbType = System.Data.SqlDbType.Int;
                        sPrm.Direction = ParameterDirection.Input;
                        sPrm.ParameterName = "@StumbleProjectID";
                        if (data.StumbleExists && bStumbleSingleVendor)
                            sPrm.Value = int.Parse(strStumbleProjId);
                        else
                            sPrm.Value = 0;
                        sPrms[8] = sPrm;

                        Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spUpdateObjectsFromContract", sPrms);
                    }

                }

                //For workflow path in Request object
                if (data.Status == (int)(Enums.ObjectStatus.ProjectCreated) || data.Status == (int)(Enums.ObjectStatus.Reject))
                {
                    clsDesignTenders clsDesign = new clsDesignTenders();
                    clsDesign.fnInsertWorkFlowPath(iRecordID, (int)Enums.VTSObjects.Contracts, data.ModifiedBy, data.Status);
                    clsDesign = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveContractMilestones(DataTable dtMilestones, int ObjType, int iRecordID)
        {
            try
            {

                int i;
                string strSQL = "";
                string strRemind = "";
                long lngDate = 0;
                string strMilestoneId = string.Empty;
                if (dtMilestones.Rows.Count > 0)
                {
                    for (i = 0; i < dtMilestones.Rows.Count; i++)
                    {
                       ////// not using reminder now
                        ////if (dtMilestones.Rows[i]["Remind"] == DBNull.Value)
                        ////    strRemind = "0";
                        ////else if ((bool)dtMilestones.Rows[i]["Remind"] == true)
                        ////    strRemind = "1";
                        ////else
                            strRemind = "0";


                        if (dtMilestones.Rows[i]["Flag"].ToString() == "N")
                        {
                            strMilestoneId = string.Empty;

                            strSQL = "Insert into Milestones(ObjectType,ObjectId,VendorId,Subject,Description,FromDate,ToDate,AllotedBudgett,[Contribution%],DeadlineRemind,DeadlineRemindDays,Project)";
                            strSQL += " values(" + ObjType.ToString() + "," + iRecordID + ",";

                            if (dtMilestones.Rows[i]["VendorId"] != DBNull.Value && dtMilestones.Rows[i]["VendorId"].ToString() != "")
                                strSQL += dtMilestones.Rows[i]["VendorId"].ToString() + ",";
                            else
                                strSQL += "Null,";

                            strSQL += Common.EncodeNString(dtMilestones.Rows[i]["Name"].ToString()) + ",";
                            strSQL += Common.EncodeNString(dtMilestones.Rows[i]["Description"].ToString()) + ",";
                            if (dtMilestones.Rows[i]["FromDate"] != DBNull.Value)
                                lngDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(dtMilestones.Rows[i]["FromDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);
                            else
                                lngDate = 0;

                            if (lngDate == 0)
                                strSQL += "Null,";
                            else
                                strSQL += "cast(" + lngDate.ToString() + " as datetime),";

                            if (dtMilestones.Rows[i]["ToDate"] != DBNull.Value)
                                lngDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(dtMilestones.Rows[i]["ToDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);
                            else
                                lngDate = 0;

                            if (lngDate == 0)
                                strSQL += "Null,";
                            else
                                strSQL += "cast(" + lngDate.ToString() + " as datetime),";

                            if (dtMilestones.Rows[i]["AllotedBudget"] != DBNull.Value)
                                strSQL += Common.EncodeValue((decimal)dtMilestones.Rows[i]["AllotedBudget"]) + ",";
                            else
                                strSQL += "Null,";

                            if (dtMilestones.Rows[i]["Contribution"] != DBNull.Value)
                                strSQL += Common.EncodeValue((decimal)dtMilestones.Rows[i]["Contribution"]) + ",";
                            else
                                strSQL += "Null,";

                            strSQL += strRemind + ",";

                            //if (dtMilestones.Rows[i]["RemindBefore"] != DBNull.Value && dtMilestones.Rows[i]["RemindBefore"].ToString() != "")
                            //    strSQL += dtMilestones.Rows[i]["RemindBefore"].ToString() + ",";
                            //else
                            strSQL += "Null,";

                            if (dtMilestones.Rows[i]["Project"].ToString() != "")
                                strSQL += Convert.ToInt32(dtMilestones.Rows[i]["Project"]) + " )";
                            else
                                strSQL += "Null)";

                            Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSQL);

                            strMilestoneId = Common.dbMgr.ExecuteScalar(CommandType.Text, "Select max(MilestoneId) from Milestones");
                            dtMilestones.Rows[i]["MilestoneID"] = strMilestoneId;

                        }
                        else if (dtMilestones.Rows[i]["Flag"].ToString() == "M")
                        {
                            strSQL = "Update Milestones Set ObjectType=" + ObjType.ToString() + ",";
                            strSQL += "ObjectId=" + iRecordID + ",";

                            if (dtMilestones.Rows[i]["VendorId"] != DBNull.Value && dtMilestones.Rows[i]["VendorId"].ToString() != "")
                                strSQL += "VendorId=" + dtMilestones.Rows[i]["VendorId"].ToString() + ",";
                            else
                                strSQL += "VendorId=Null,";

                            strSQL += "Subject=" + Common.EncodeNString(dtMilestones.Rows[i]["Name"].ToString()) + ",";
                            strSQL += "Description=" + Common.EncodeNString(dtMilestones.Rows[i]["Description"].ToString()) + ",";
                            if (dtMilestones.Rows[i]["FromDate"] != DBNull.Value)
                                lngDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(dtMilestones.Rows[i]["FromDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);
                            else
                                lngDate = 0;

                            if (lngDate == 0)
                                strSQL += "FromDate=Null,";
                            else
                                strSQL += "FromDate=cast(" + lngDate.ToString() + " as datetime),";

                            if (dtMilestones.Rows[i]["ToDate"] != DBNull.Value)
                                lngDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(dtMilestones.Rows[i]["ToDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);
                            else
                                lngDate = 0;

                            if (lngDate == 0)
                                strSQL += "ToDate=Null,";
                            else
                                strSQL += "ToDate=cast(" + lngDate.ToString() + " as datetime),";

                            if (dtMilestones.Rows[i]["AllotedBudget"] != DBNull.Value)
                                strSQL += " AllotedBudgett=" + Common.EncodeValue((decimal)dtMilestones.Rows[i]["AllotedBudget"]) + ",";
                            else
                                strSQL += " AllotedBudgett=Null,";

                            if (dtMilestones.Rows[i]["Contribution"] != DBNull.Value)
                                strSQL += " [Contribution%]=" + Common.EncodeValue((decimal)dtMilestones.Rows[i]["Contribution"]) + ",";
                            else
                                strSQL += " [Contribution%]=Null,";

                            strSQL += "DeadlineRemind=" + strRemind + ",";

                            ////if (dtMilestones.Rows[i]["RemindBefore"] != DBNull.Value && dtMilestones.Rows[i]["RemindBefore"].ToString() != "")
                            ////    strSQL += " DeadlineRemindDays=" + dtMilestones.Rows[i]["RemindBefore"].ToString();
                            ////else
                            strSQL += " DeadlineRemindDays=Null ";

                            strSQL += " Where MilestoneId=" + dtMilestones.Rows[i]["MilestoneID"].ToString();
                            Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSQL);
                        }
                        else if (dtMilestones.Rows[i]["Flag"].ToString() == "D")
                        {
                            if (dtMilestones.Rows[i]["MilestoneID"].ToString() != "")
                            {
                                strSQL = "Delete from Milestones where ";
                                strSQL += "MilestoneId=" + dtMilestones.Rows[i]["MilestoneID"].ToString();

                                Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSQL);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateBudgetForParentProjects(string sTaskId, decimal dBudget)
        {
            try
            {
                string strSQL = "";
                clsProjectTask clsObj = new clsProjectTask();
                string strParentIds = string.Empty;
                strParentIds = clsObj.GetParentTaskIds(sTaskId);

                if (strParentIds != string.Empty)
                {
                    strSQL = "Update Tasks Set Budget=Budget + " + dBudget + " Where TaskId in(" + strParentIds + ")";
                    Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSQL);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InsertSubProjectComponents(ContractActionData data, string sParentId, string sUserInitials, DataTable dt, int iContractId, bool bProjIsManualCode, DataTable dtMilestones)
        {
            string strSql = "";
            string strProjectId = "";
            string strProjectCode = "";
            string strPerformedBy = "";
            DataRow[] dr = null;

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                if (dt.Rows[i]["Flag"] != "D" && dt.Rows[i]["Flag"] != "E" && dt.Rows[i]["Project"].ToString() == string.Empty)
                {
                    if (bProjIsManualCode)
                        strProjectCode = "";
                    else
                        strProjectCode = clsCodeGeneration.GetObjectCode((int)Enums.VTSObjects.Project, sUserInitials, data.UserRequestId);

                    strSql = "Insert into Tasks(IsProject,TaskCode,Subject,IsPartOf,CustomerId,IsActive,IsManualCode,CreatedBy,CreatedOn,IsInternalProject,ProjectManager,Budget)";
                    strSql += " values(1," + Common.EncodeNString(data.ProjectCode) + "," + Common.EncodeNString(dt.Rows[i]["Component"].ToString()) + ",";

                    if (sParentId != string.Empty && sParentId != "0")
                        strSql += sParentId.ToString() + ",";
                    else
                        strSql += "null,";

                    strSql += dt.Rows[i]["VendorId"].ToString() + ",1,";

                    if (bProjIsManualCode)
                        strSql += "1,";
                    else
                        strSql += "0,";

                    strSql += Common.EncodeNString(data.CreatedBy) + ",getdate(),0,";
                    if (data.ProjectMangaer != 0)
                        strSql += data.ProjectMangaer.ToString() + ",";
                    else
                        strSql += "null,";

                    if (dt.Rows[i]["Budget"] != DBNull.Value)
                        strSql += Common.EncodeValue((decimal)dt.Rows[i]["Budget"]) + ")";
                    else
                        strSql += "null)";

                    Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                    strProjectId = Common.dbMgr.ExecuteScalar(CommandType.Text, "Select max(TaskId) from Tasks");
                    dt.Rows[i]["Project"] = strProjectId;

                    if (strProjectId != "")
                    {
                        //Update ProjectId in ContractVendors and Milestones table
                        strSql = "Update ContractVendors Set Project=" + strProjectId;
                        strSql += " Where ContractId=" + iContractId + " And VendorId=" + dt.Rows[i]["VendorId"].ToString();
                        Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                        strSql = "Update Milestones Set Project=" + strProjectId;
                        strSql += " Where ObjectId=" + iContractId + " And ObjectType=" + (int)Enums.VTSObjects.Contracts + " And VendorId=" + dt.Rows[i]["VendorId"].ToString();
                        Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                        dr = dtMilestones.Select("VendorId=" + dt.Rows[i]["VendorId"].ToString());
                        if (dr != null && dr.Length > 0)
                        {
                            for (int j = 0; j <= dr.Length - 1; j++)
                            {
                                dr[j]["Project"] = strProjectId;
                            }
                        }

                        //iProject = int.Parse(strProjectId);

                        //Updating Project deadlines
                        strSql = "If  exists (Select 1 from Milestones Where ObjectId=" + iContractId + " And ObjectType=" + (int)Enums.VTSObjects.Contracts + " And VendorId=" + dt.Rows[i]["VendorId"].ToString() + ")";
                        strSql += " Insert into TaskDeadlines(TaskId,DateFrom,DateTill,CreatedBy,Reason,CreatedOn)";
                        strSql += " Select " + strProjectId + ", Min(FromDate),Max(ToDate)," + Common.EncodeNString(data.CreatedBy) + ",Null,getdate() from Milestones";
                        strSql += " Where ObjectId=" + iContractId + " And ObjectType=" + (int)Enums.VTSObjects.Contracts + " And VendorId=" + dt.Rows[i]["VendorId"].ToString();
                        Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);


                        //Inserting Processed By
                        strSql = "Select AddresseId from Addresses where LoginName=(Select CreatedBy from UserRequests where UserRequestId=" + data.UserRequestId + ")";
                        strPerformedBy = Common.dbMgr.ExecuteScalar(CommandType.Text, strSql);

                        strSql = "Insert into ProcessedBy(ObjectType,ObjectId,PerformedBy,Allow,Remind)";
                        strSql += " values(" + (int)Enums.VTSObjects.Project + "," + strProjectId + "," + strPerformedBy + ",1,1)";
                        Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                        if (strPerformedBy != data.ProjectMangaer.ToString())
                        {
                            strSql = "Insert into ProcessedBy(ObjectType,ObjectId,PerformedBy,Allow,Remind)";
                            strSql += " values(" + (int)Enums.VTSObjects.Project + "," + strProjectId + "," + data.ProjectMangaer.ToString() + ",1,1)";
                            Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                        }

                    }
                    //   fnForwardDocuments(int.Parse(strProjectId), iContractId, strCreatedBy);

                }

            }
        }

        private bool IsSingleVendorForStumble(DataTable dt, int Project)
        {
            bool bFlag = false;
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                if (dt.Rows[i]["Project"] != DBNull.Value && dt.Rows[i]["Project"].ToString() != string.Empty)
                {
                    if (Project.ToString() == dt.Rows[i]["Project"].ToString())
                    {
                        bFlag = true;
                        break;
                    }
                }
            }
            return bFlag;
        }

        private void SaveContractVendors(DataTable dtContractVendors, int iRecordID)
        {
            try
            {

                int i;
                string strSQL = "";
                string strPenalty = "";
                long lngDate = 0;

                if (dtContractVendors.Rows.Count > 0)
                {
                    for (i = 0; i < dtContractVendors.Rows.Count; i++)
                    {

                        if (dtContractVendors.Rows[i]["IsPenalty"] == DBNull.Value)
                            strPenalty = "0";
                        else if ((bool)dtContractVendors.Rows[i]["IsPenalty"] == true)
                            strPenalty = "1";
                        else
                            strPenalty = "0";

                        if (dtContractVendors.Rows[i]["Flag"].ToString() == "N")
                        {
                            strSQL = "Insert into ContractVendors(ContractId,VendorId,Component,Budget,CurrencyId,ExchangeRate,IsPenalty,PenaltyType,";
                            strSQL += "PenaltyAmount,PenaltyMaxAmount,SuretyRemarks,SuretyAmount,SuretyDate,SuretyBankName)";
                            strSQL += " values(" + iRecordID + ",";

                            if (dtContractVendors.Rows[i]["VendorId"] != DBNull.Value && dtContractVendors.Rows[i]["VendorId"].ToString() != "")
                                strSQL += dtContractVendors.Rows[i]["VendorId"].ToString() + ",";
                            else
                                strSQL += "Null,";

                            strSQL += Common.EncodeNString(dtContractVendors.Rows[i]["Component"].ToString()) + ",";

                            if (dtContractVendors.Rows[i]["Budget"] != DBNull.Value)
                                strSQL += Common.EncodeValue((decimal)dtContractVendors.Rows[i]["Budget"]) + ",";
                            else
                                strSQL += "Null,";

                            if (dtContractVendors.Rows[i]["CurrencyId"] != DBNull.Value && dtContractVendors.Rows[i]["CurrencyId"].ToString() != "")
                                strSQL += dtContractVendors.Rows[i]["CurrencyId"].ToString() + ",";
                            else
                                strSQL += "Null,";
                            // Commented for now no useful
                            ////if (dtContractVendors.Rows[i]["ExchangeRate"] != DBNull.Value)
                            ////    strSQL += Common.EncodeValue((decimal)dtContractVendors.Rows[i]["ExchangeRate"]) + ",";
                            ////else
                            strSQL += "Null,";

                            strSQL += strPenalty + ",";

                            if (strPenalty == "1")
                            {
                                if (dtContractVendors.Rows[i]["PenaltyType"] != DBNull.Value && dtContractVendors.Rows[i]["PenaltyType"].ToString() != "")
                                    strSQL += dtContractVendors.Rows[i]["PenaltyType"].ToString() + ",";
                                else
                                    strSQL += "Null,";

                                if (dtContractVendors.Rows[i]["PenaltyAmount"] != DBNull.Value)
                                    strSQL += Common.EncodeValue((decimal)dtContractVendors.Rows[i]["PenaltyAmount"]) + ",";
                                else
                                    strSQL += "Null,";

                                if (dtContractVendors.Rows[i]["PenaltyMaxAmount"] != DBNull.Value)
                                    strSQL += Common.EncodeValue((decimal)dtContractVendors.Rows[i]["PenaltyMaxAmount"]) + ",";
                                else
                                    strSQL += "Null,";
                            }
                            else
                                strSQL += "Null,Null,Null,";


                            strSQL += Common.EncodeNString(dtContractVendors.Rows[i]["SuretyRemarks"].ToString()) + ",";

                            if (dtContractVendors.Rows[i]["SuretyAmount"] != DBNull.Value)
                                strSQL += Common.EncodeValue((decimal)dtContractVendors.Rows[i]["SuretyAmount"]) + ",";
                            else
                                strSQL += "Null,";

                            if (dtContractVendors.Rows[i]["SuretyDate"] != DBNull.Value)
                                lngDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(dtContractVendors.Rows[i]["SuretyDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);
                            else
                                lngDate = 0;

                            if (lngDate == 0)
                                strSQL += "Null,";
                            else
                                strSQL += "cast(" + lngDate.ToString() + " as datetime),";

                            strSQL += Common.EncodeNString(dtContractVendors.Rows[i]["SuretyBankName"].ToString()) + ")";
                            Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSQL);

                        }
                        else if (dtContractVendors.Rows[i]["Flag"].ToString() == "M")
                        {
                            strSQL = "Update ContractVendors Set ContractId=" + iRecordID + ",";

                            if (dtContractVendors.Rows[i]["VendorId"] != DBNull.Value && dtContractVendors.Rows[i]["VendorId"].ToString() != "")
                                strSQL += "VendorId=" + dtContractVendors.Rows[i]["VendorId"].ToString() + ",";
                            else
                                strSQL += "VendorId=Null,";

                            strSQL += "Component=" + Common.EncodeNString(dtContractVendors.Rows[i]["Component"].ToString()) + ",";

                            if (dtContractVendors.Rows[i]["Budget"] != DBNull.Value)
                                strSQL += " Budget=" + Common.EncodeValue((decimal)dtContractVendors.Rows[i]["Budget"]) + ",";
                            else
                                strSQL += " Budget=Null,";

                            if (dtContractVendors.Rows[i]["CurrencyId"] != DBNull.Value && dtContractVendors.Rows[i]["CurrencyId"].ToString() != "")
                                strSQL += "CurrencyId=" + dtContractVendors.Rows[i]["CurrencyId"].ToString() + ",";
                            else
                                strSQL += "CurrencyId=Null,";

                           //// // commented for now
                            ////if (dtContractVendors.Rows[i]["ExchangeRate"] != DBNull.Value)
                            ////    strSQL += " ExchangeRate=" + Common.EncodeValue((decimal)dtContractVendors.Rows[i]["ExchangeRate"]) + ",";
                            ////else
                            strSQL += " ExchangeRate=Null,";

                            strSQL += "IsPenalty=" + strPenalty + ",";

                            if (strPenalty == "1")
                            {
                                if (dtContractVendors.Rows[i]["PenaltyType"] != DBNull.Value && dtContractVendors.Rows[i]["PenaltyType"].ToString() != "")
                                    strSQL += "PenaltyType=" + dtContractVendors.Rows[i]["PenaltyType"].ToString() + ",";
                                else
                                    strSQL += "PenaltyType=Null,";

                                if (dtContractVendors.Rows[i]["PenaltyAmount"] != DBNull.Value)
                                    strSQL += " PenaltyAmount=" + Common.EncodeValue((decimal)dtContractVendors.Rows[i]["PenaltyAmount"]) + ",";
                                else
                                    strSQL += " PenaltyAmount=Null,";

                                if (dtContractVendors.Rows[i]["PenaltyMaxAmount"] != DBNull.Value)
                                    strSQL += " PenaltyMaxAmount=" + Common.EncodeValue((decimal)dtContractVendors.Rows[i]["PenaltyMaxAmount"]) + ",";
                                else
                                    strSQL += " PenaltyMaxAmount=Null,";
                            }
                            else
                                strSQL += "PenaltyType=Null,PenaltyAmount=Null,PenaltyMaxAmount=Null,";


                            strSQL += "SuretyRemarks=" + Common.EncodeNString(dtContractVendors.Rows[i]["SuretyRemarks"].ToString()) + ",";

                            if (dtContractVendors.Rows[i]["SuretyDate"] != DBNull.Value)
                                lngDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(dtContractVendors.Rows[i]["SuretyDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);
                            else
                                lngDate = 0;

                            if (lngDate == 0)
                                strSQL += "SuretyDate=Null,";
                            else
                                strSQL += "SuretyDate=cast(" + lngDate.ToString() + " as datetime),";

                            if (dtContractVendors.Rows[i]["SuretyAmount"] != DBNull.Value)
                                strSQL += " SuretyAmount=" + Common.EncodeValue((decimal)dtContractVendors.Rows[i]["SuretyAmount"]) + ",";
                            else
                                strSQL += " SuretyAmount=Null,";

                            strSQL += "SuretyBankName=" + Common.EncodeNString(dtContractVendors.Rows[i]["SuretyBankName"].ToString());

                            strSQL += " Where RecordId=" + dtContractVendors.Rows[i]["ContractVendorId"].ToString();
                            Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSQL);
                        }
                        else if (dtContractVendors.Rows[i]["Flag"].ToString() == "D")
                        {
                            if (dtContractVendors.Rows[i]["ContractVendorId"].ToString() != "")
                            {
                                strSQL = "Delete from ContractVendors where ";
                                strSQL += "RecordId=" + dtContractVendors.Rows[i]["ContractVendorId"].ToString();

                                Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSQL);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetWorkPlaceWhereCondition(string sUserLoginId)
        {
            string sMyWorkPlaceCondition = "";

            sMyWorkPlaceCondition += " A.IsDone=0 And A.IsActive=1 And A.CreatedBy =" + Common.EncodeNString(sUserLoginId);


            return sMyWorkPlaceCondition;

        }

        public bool IsAccessDenied(string sLoginUserName, string sRecordId, bool bForModify)
        {
            if (sRecordId != "" && sRecordId != "-100" && sRecordId != "0")
            {
                int iModifyValue = 0;
                if (bForModify)
                    iModifyValue = 1;

                string sSql = "Select A.OSR_ContractId From ";
                sSql += " fnContract_OSR(" + Common.EncodeString(sLoginUserName) + "," + iModifyValue.ToString() + ") as A ";
                sSql += " Where A.OSR_ContractId=" + sRecordId;
                DataTable dt = Common.dbMgr.ExecuteDataTable(CommandType.Text, sSql);
                return dt.Rows.Count == 0;
            }
            else
            {
                return false;
            }
        }

        public DataSet fnGetCurrencyDetails()
        {
            DataSet ds = new DataSet();
            try
            {
                string sSql = string.Empty;
                sSql = "select CurrencyId,CurrencyTitle as Name from Currency";
                ds = Common.dbMgr.ExecuteDataSet(CommandType.Text, sSql);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int fnSetReadStatusObject(int iRecordId, int iObjectEnumId, string strLoginName)
        {

            try
            {
                SqlParameter[] Params = new SqlParameter[3];
                SqlParameter param = null;
                int iIndex = 0;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@RecordId";
                param.Value = iRecordId;
                Params[iIndex] = param;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@ObjectEnumId";
                param.Value = iObjectEnumId;
                Params[++iIndex] = param;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.NVarChar;
                param.ParameterName = "@LoginName";
                param.Value = strLoginName;
                Params[++iIndex] = param;

                return Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spSetReadStatusforObject", Params);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int fnInsertPMObjectRight(int iRecordId, int iObjectEnumId)
        {
            try
            {
                int iResultId = 0;
                SqlParameter[] Params = new SqlParameter[2];
                SqlParameter param = null;
                int iIndex = 0;



                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@ObjectEnumId";
                param.Value = iObjectEnumId;
                Params[iIndex] = param;


                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@ObjectId";
                param.Value = iRecordId;
                Params[++iIndex] = param;

                iResultId = Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spSetPMObjectRight", Params);
                return iResultId;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
