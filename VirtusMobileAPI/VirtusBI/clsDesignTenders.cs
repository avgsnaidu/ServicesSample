using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EPMEnums;
using VirtusDataModel;
 

namespace VirtusBI
{
    public class clsDesignTenders : clsBaseBI
    {


        public DataSet fnGetDesignDetails(int designId)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlParameter[] sPrms = new SqlParameter[1];
                SqlParameter sPrm = new SqlParameter();

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@DesignId";
                sPrm.Value = designId;
                sPrms[0] = sPrm;

                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetDesignDetailsDS", sPrms);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDesignConsultants(int designId)
        {
            try
            {

                SqlParameter[] Params = new SqlParameter[1];
                SqlParameter param = null;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@DesignId";
                param.Value = designId;
                Params[0] = param;

                DataSet ds = null;
                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetDesignConsultantsDS", Params);
                return ds;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataSet fnGetUsersForApprovingAuthority()
        {
            //SqlParameter sPrm = new SqlParameter();

            return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetApprovingAuthorityDS");
        }

        public int fnSetReadStatusObject(int recordId, int objectEnumId, string loginName)
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
                param.Value = recordId;
                Params[iIndex] = param;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@ObjectEnumId";
                param.Value = objectEnumId;
                Params[++iIndex] = param;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.NVarChar;
                param.ParameterName = "@LoginName";
                param.Value = loginName;
                Params[++iIndex] = param;

                return Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spSetReadStatusforObject", Params);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool fnSaveConsultants(DataTable dtTab, int iDesignId)
        {
            try
            {
                int i;
                string strSQL = "";

                if (dtTab.Rows.Count > 0)
                {
                    for (i = 0; i < dtTab.Rows.Count; i++)
                    {
                        if (dtTab.Rows[i]["Flag"].ToString() == "N")
                        {
                            strSQL = "Insert into DesignConsultantList(DesignId,AddresseId,IsSelected,Component,Comments)";
                            strSQL += " values(" + iDesignId + ",";
                            strSQL += Common.EncodeString(dtTab.Rows[i]["AddresseId"].ToString()) + ",";

                            if (Convert.ToBoolean(dtTab.Rows[i]["IsSelected"]))
                                strSQL += "1,";
                            else
                                strSQL += "0,";

                            strSQL += Common.EncodeNString(dtTab.Rows[i]["Component"].ToString()) + ",";
                            strSQL += Common.EncodeNString(dtTab.Rows[i]["Comments"].ToString()) + ")";

                            Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSQL);
                        }
                        else if (dtTab.Rows[i]["Flag"].ToString() == "M")
                        {
                            strSQL = "Update DesignConsultantList Set DesignId=" + iDesignId + ",";
                            strSQL += "AddresseId=" + Common.EncodeString(dtTab.Rows[i]["AddresseId"].ToString()) + ",";

                            if ((bool)dtTab.Rows[i]["IsSelected"])
                                strSQL += "IsSelected=1,";
                            else
                                strSQL += "IsSelected=0,";

                            strSQL += "Component=";
                            strSQL += Common.EncodeNString(dtTab.Rows[i]["Component"].ToString()) + ",";

                            strSQL += "Comments=";
                            strSQL += Common.EncodeNString(dtTab.Rows[i]["Comments"].ToString());

                            strSQL += " Where RecordId=" + dtTab.Rows[i]["RecordId"].ToString();
                            Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSQL);
                        }
                        else if (dtTab.Rows[i]["Flag"].ToString() == "D")
                        {

                            strSQL = "Delete from DesignConsultantList where ";
                            strSQL += "RecordId=" + Common.EncodeString(dtTab.Rows[i]["RecordId"].ToString());

                            Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSQL);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int fnSave(int iRecordID, bool bIsDesign, ref bool bSuccess, TenderActionData data)
        {
            try
            {
                string strSql = "";
                if (iRecordID == 0)
                {
                    strSql = "insert into Designs(Code,Subject,ProjectManager,Description,SelectedConsultant,ApporivingAuthority,";
                    strSql += "DeadlineRemind,DeadlineRemindDays,DeadlineEndDate,IsDone,DoneDate,CreatedBy,CreatedOn,";
                    strSql += " Status,IsArchived,ApprovedBudget,IsDesign,UserRequestId,TenderDesignID )";
                    strSql += Common.EncodeNString(data.Code) + "," + Common.EncodeNString(data.Subject) + ",";
                    if (data.ProjectManagerID != 0)
                        strSql += data.ProjectManagerID.ToString() + ",";
                    else
                        strSql += "Null,";

                    strSql += Common.EncodeNString(data.Description) + ",";
                    if (data.SelectedConsultant != 0)
                        strSql += data.SelectedConsultant.ToString() + ",";
                    else
                        strSql += "Null,";

                    if (data.ApprovingAuthority != 0)
                        strSql += data.ApprovingAuthority.ToString() + ",";
                    else
                        strSql += "Null,";

                    if (data.DeadlineRemind)
                    {
                        strSql += "1,";
                        strSql += Common.EncodeValue(data.DeadlineRemindDays) + ",";
                    }
                    else
                    {
                        strSql += "0,";
                        strSql += "null,";
                        data.DeadlineRemindDays = -1;
                    }

                    if (data.DeadlineEndDate <= DateTime.MinValue || data.DeadlineEndDate == null)
                        strSql += "Null,";
                    else
                        strSql += "cast(" + data.DeadlineEndDate.ToString() + " as datetime),";

                    if (data.IsDone)
                    {
                        strSql += "1,";
                        strSql += "cast(" + data.DoneDate.ToString() + " as datetime),";
                    }
                    else
                    {
                        strSql += "0,";
                        strSql += "null,";
                    }

                    strSql += Common.EncodeNString(data.CreatedBy) + "," + "getdate(),";
                    strSql += " 0,0,";
                    if (data.ApprovedBudget != 0)
                        strSql += data.ApprovedBudget.ToString() + ",";
                    else
                        strSql += "null,";

                    if (bIsDesign)
                        strSql += "1,";
                    else
                        strSql += "0,";
                    if (data.UserRequestId == 0)
                        strSql += "null,";
                    else
                        strSql += data.UserRequestId.ToString() + ",";

                    if (data.TenderDesignID == 0)
                        strSql += "null,";
                    else
                        strSql += data.TenderDesignID.ToString() + ",";
                }
                else
                {
                    strSql = "Update  Designs Set Code=" + Common.EncodeNString(data.Code) + ",Subject=" + Common.EncodeNString(data.Subject) + ",";
                    if (data.ProjectManagerID != 0)
                        strSql += "ProjectManager=" + data.ProjectManagerID.ToString() + ",";
                    else
                        strSql += "ProjectManager=Null,";
                    strSql += "Description=" + Common.EncodeNString(data.Description) + ",";

                    if (data.SelectedConsultant != 0)
                        strSql += "SelectedConsultant=" + data.SelectedConsultant.ToString() + ",";
                    else
                        strSql += "SelectedConsultant=Null,";
                    if (data.ApprovingAuthority != 0)
                        strSql += "ApporivingAuthority=" + data.ApprovingAuthority.ToString() + ",";
                    else
                        strSql += "ApporivingAuthority=Null,";

                    if (data.DeadlineRemind)
                    {
                        strSql += "DeadlineRemind=1,";
                        strSql += "DeadlineRemindDays=" + data.DeadlineRemindDays.ToString() + ",";
                    }
                    else
                    {
                        strSql += "DeadlineRemind=0,";
                        strSql += "DeadlineRemindDays=null,";
                        data.DeadlineRemindDays = -1;
                    }

                    if (data.DeadlineEndDate <= DateTime.MinValue || data.DeadlineEndDate == null)
                        strSql += "DeadlineEndDate=Null,";
                    else
                        strSql += "DeadlineEndDate=cast(" + data.DeadlineEndDate.ToString() + " as datetime),";


                    if ((data.Status == (int)Enums.ObjectStatus.Reject) || (data.Status == (int)Enums.ObjectStatus.Approved) || (data.Status == (int)Enums.ObjectStatus.ContractCreated))
                    {
                        strSql += "IsDone=1,";
                        strSql += "DoneDate=getdate(),";
                    }
                    else
                    {
                        strSql += "IsDone=0,";
                        strSql += "DoneDate=null,";
                    }

                    if (data.UseSpecialRights)
                        strSql += " UseSpecialRights=1,";
                    else
                        strSql += " UseSpecialRights=0,";

                    if (data.OwnerHasFullRights)
                        strSql += " IsOwnerFullRights=1,";
                    else
                        strSql += " IsOwnerFullRights=0,";

                    strSql += "Status = " + data.Status + ",";

                    strSql += " ModifiedBy=" + Common.EncodeNString(data.ModifiedBy) + ",ModifiedOn=getdate(),";

                    if (data.ApprovedBudget != 0)
                        strSql += "ApprovedBudget=" + data.ApprovedBudget.ToString() + ",";
                    else
                        strSql += "ApprovedBudget=Null,";

                    if (data.UserRequestId != 0)
                        strSql += "UserRequestId=" + data.UserRequestId.ToString() + ",";
                    else
                        strSql += "UserRequestId=Null,";

                    if (data.TenderDesignID != 0)
                        strSql += "TenderDesignID=" + data.TenderDesignID.ToString();
                    else
                        strSql += "TenderDesignID=Null";

                    //if(ProjectIsStumbled)
                    //  strSql += "Project=Null ";


                    strSql += " Where DesignId=" + iRecordID;

                    if (data.Status == (int)Enums.ObjectStatus.Approved || data.Status == (int)Enums.ObjectStatus.Reject)
                    {
                        int iObjectTypId = (int)Enums.VTSObjects.Tenders;
                        if (bIsDesign)
                            iObjectTypId = (int)Enums.VTSObjects.Designs;

                        fnInsertWorkFlowPath(iRecordID, iObjectTypId, data.ModifiedBy, data.Status);
                    }


                }

                Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                bSuccess = true;
                if (iRecordID == 0)
                    return int.Parse(Common.dbMgr.ExecuteScalar(CommandType.Text, "Select max(DesignId) from Designs"));
                else
                {
                    return iRecordID;
                }
            }
            catch (Exception ex)
            {
                bSuccess = false;
                throw ex;
            }
        }

        public bool fnInsertWorkFlowPath(int iRecordID, int iObjectTypId, string strModifiedby, int iStatusId)
        {
            try
            {

                int iIndex = 0;
                SqlParameter[] sPrms = new SqlParameter[5];
                SqlParameter sPrm = new SqlParameter();

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ObjectType";
                sPrm.Value = iObjectTypId;
                sPrms[iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ObjectId";
                sPrm.Value = iRecordID;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@FromUser";
                sPrm.Value = strModifiedby;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ToUser";
                sPrm.Value = "";
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@Status";
                sPrm.Value = iStatusId;
                sPrms[++iIndex] = sPrm;

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "SpInsertWorkFlowDeatils", sPrms);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool fnUpdateContractVendors(int iRecordId)
        {
            try
            {
                SqlParameter[] sPrms = new SqlParameter[1];
                SqlParameter sPrm = new SqlParameter();
                int iIndex = 0;
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@DesignId";
                sPrm.Value = iRecordId;
                sPrms[iIndex] = sPrm;

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spUpdateContractVendors", sPrms);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool fnInsertBasicTenderContracts(TenderContractActionData data)
        {
            try
            {
                SqlParameter[] sPrms = new SqlParameter[7];
                SqlParameter sPrm = new SqlParameter();
                int iIndex = 0;

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@UserRequestId";
                sPrm.Value = data.UserReqeustId;
                sPrms[iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@DesignId";
                sPrm.Value = data.TenderRecordId;
                sPrms[++iIndex] = sPrm;


                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@Subject";
                sPrm.Value = data.Subject;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@Code";
                sPrm.Value = data.NewCodeToContract;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@CreatedBy";
                sPrm.Value = data.CreatedBy;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@IsToCreateTender";
                sPrm.Value = data.IsNeedToCreateTender;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@DesignContractCode";
                sPrm.Value = data.NewDesignContractCode;
                sPrms[++iIndex] = sPrm;

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spInsertBasicTenderContracts", sPrms);


                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public NextObjectRecordDetails fnGetObjectRecordId(int iRecordId, int iObjectEnumId)
        {
            try
            {
                SqlParameter[] Params = new SqlParameter[4];
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

                param = new SqlParameter();
                param.Direction = ParameterDirection.Output;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@iNextObjectEnumId";
                Params[++iIndex] = param;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Output;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@RecordId";
                Params[++iIndex] = param;

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spGetObjectRecordId", Params);
                NextObjectRecordDetails obj = new NextObjectRecordDetails();
                obj.NextObjectEnumId = int.Parse(Params[iIndex - 1].Value.ToString());
                obj.RecordId = int.Parse(Params[iIndex].Value.ToString());

                return obj;

                //NextObjEnumId = int.Parse(Params[iIndex - 1].Value.ToString());
                //return int.Parse(Params[iIndex].Value.ToString());

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

        public string GetWorkPlaceWhereCondition(string sUserLoginId)
        {
            string sMyWorkPlaceCondition = "";

            // sMyWorkPlaceCondition += " A.IsDone=0 And A.IsActive=1 And A.CreatedBy =" + Common.EncodeNString(sUserLoginId);
            sMyWorkPlaceCondition += " A.IsActive=1 And A.CreatedBy =" + Common.EncodeNString(sUserLoginId);

            return sMyWorkPlaceCondition;
        }

        public DataSet GetListViewDataSet(string sWhereCondition, string sLoginUserName, bool bIsDesign, bool bUnDone)
        {
            try
            {
                int iTotCount = 0;
                return GetListViewDataSet(sWhereCondition, "", 1000, "19", "DESC", sLoginUserName, ref iTotCount, bIsDesign, bUnDone);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public DataSet GetListViewDataSet(string sWhereCondition, string strSearchCondition, int iNoOfRecords, string strOrderBy, string strSortOrder, string sLoginUserName, ref int iTotCount, bool bIsDesign, bool bUnDone)
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

                Params[++iIndex] = new SqlParameter("@IsDesign", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bIsDesign;


                Params[++iIndex] = new SqlParameter("@IsDone", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bUnDone;

                Params[++iIndex] = new SqlParameter("@TotCount", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Output;


                DataSet ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetDesignListViewDS", Params);
                iTotCount = int.Parse(Params[iIndex].Value.ToString());

                return ds;
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
                strSql = "Update Designs Set IsDone=" + iIsDone + ",";
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

    }
}
