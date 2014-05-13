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
    public class clsProjectTask
    {
        public string GetParentTaskIds(string sTaskId)
        {
            string sIds = "";

            string sSql = "Select B.TaskId From Tasks A Inner Join Tasks B on A.IsPartOf=B.TaskId and A.TaskId=" + sTaskId + " And B.SpecialProjectType<>1";
            DataSet ds = Common.dbMgr.ExecuteDataSet(CommandType.Text, sSql);

            while (ds.Tables[0].Rows.Count > 0)
            {
                sTaskId = ds.Tables[0].Rows[0]["TaskId"].ToString();

                if (sIds != "")
                    sIds += ",";

                sIds += ds.Tables[0].Rows[0]["TaskId"].ToString();

                sSql = "Select B.TaskId From Tasks A Inner Join Tasks B on A.IsPartOf=B.TaskId and A.TaskId=" + sTaskId + " And B.SpecialProjectType<>1";
                ds = Common.dbMgr.ExecuteDataSet(CommandType.Text, sSql);
            }
            return sIds;
        }

        public DataSet fnGetProjectTreeForContracts(int StartSerialNum, int iParentNodeId, string strLogInName)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sPrms = new SqlParameter[3];
                SqlParameter sPrm = new SqlParameter();

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@StartSerialNum";
                sPrm.Value = StartSerialNum;
                sPrms[0] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ParentNodeId";
                sPrm.Value = iParentNodeId;
                sPrms[1] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@LoginName";
                sPrm.Value = strLogInName;
                sPrms[2] = sPrm;

                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetProjectTreeForContracts", sPrms);
                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet fnGetData(string strRecordID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlParameter[] Params = new SqlParameter[1];
                SqlParameter param = null;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@TaskId";
                param.Value = Convert.ToInt32(strRecordID);
                Params[0] = param;

                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetTasksDs", Params);
                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int fnGetUserRequestId(int iTaskId)
        {
            try
            {

                string sSql = "Select dbo.fnGetParentProjectUserRequestId(" + iTaskId + ") as URId ";
                DataSet ds = Common.dbMgr.ExecuteDataSet(CommandType.Text, sSql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return int.Parse(ds.Tables[0].Rows[0]["URId"].ToString());
                }
                return 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public DataSet fnGetMilestonesOrProjectManagers(bool IsProject, int iProjectId, bool bIsDone)
        {
            SqlParameter[] sPrms = new SqlParameter[3];
            SqlParameter sPrm = new SqlParameter();
            sPrm.SqlDbType = System.Data.SqlDbType.Bit;
            sPrm.Direction = ParameterDirection.Input;
            sPrm.ParameterName = "@Isproject";
            sPrm.Value = IsProject;
            sPrms[0] = sPrm;

            sPrm = new SqlParameter();
            sPrm.SqlDbType = System.Data.SqlDbType.Int;
            sPrm.Direction = ParameterDirection.Input;
            sPrm.ParameterName = "@ObjectId";
            sPrm.Value = iProjectId;
            sPrms[1] = sPrm;

            sPrm = new SqlParameter();
            sPrm.SqlDbType = System.Data.SqlDbType.Bit;
            sPrm.Direction = ParameterDirection.Input;
            sPrm.ParameterName = "@IsDone";
            sPrm.Value = bIsDone;
            sPrms[2] = sPrm;

            return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetMilestonesOrProjectManagers", sPrms);

        }

        public DataSet GetKindBindingDS(bool bIsProject, int iDepartment = 0)
        {
            try
            {
                SqlParameter[] Params = new SqlParameter[3];
                SqlParameter param = null;
                int iIndex = 0;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Bit;
                param.ParameterName = "@IsProject";
                param.Value = bIsProject;
                Params[iIndex] = param;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@UILanguageId";
                param.Value = CommonVariable.iLanguageId;
                Params[++iIndex] = param;


                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@DepartmentId";
                param.Value = iDepartment;
                Params[++iIndex] = param;

                DataSet ds = null;
                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetTaskKindsBindingDS", Params);
                return ds;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public DataSet GetProjectStatusBindingDS()
        {
            try
            {
                SqlParameter[] Params = new SqlParameter[1];
                SqlParameter param = null;


                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@UILanguageId";
                param.Value = CommonVariable.iLanguageId;
                Params[0] = param;

                DataSet ds = null;
                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetProjectStatusListDS", Params);
                return ds;
            }
            catch (Exception ex)
            { throw ex; }
        }


        public string GetListViewDataSetForCount(int objType, int iRecord_Id, string strLoginName, bool bIsArchived)
        {
            try
            {
                string strSql = "";
                string sWhereCondition = "";

                ////bool bIsShowInActive = fnShowIsActive(strLoginName, objType.ToString());

                int iIsActive = 0;
                ////if (bIsShowInActive)
                iIsActive = 1;

                if (objType == (int)Enums.VTSObjects.Project || objType == (int)Enums.VTSObjects.Task)
                {
                    //all tasks in which this project is choosen as parent (is part of field).
                    sWhereCondition = " A.IsPartOf=" + iRecord_Id.ToString();
                    sWhereCondition += " And (" + iIsActive + " =1 OR (" + iIsActive + "=0 And A.IsActive= 1))";
                }
                else
                {
                    string sAddresseIds = "";
                    sAddresseIds += " Select cast(" + iRecord_Id.ToString() + " as int) as AddresseId ";
                    sAddresseIds += " Union Select AddresseId from Addresses Where PersonId =" + iRecord_Id.ToString();
                    sAddresseIds += " Union Select OrganizationId From Addresses Where PersonId = " + iRecord_Id.ToString() + " OR AddresseId = " + iRecord_Id.ToString();
                    sAddresseIds += " Union Select AddresseId From Addresses Where OrganizationId = " + iRecord_Id.ToString();

                    sWhereCondition = " (" + iIsActive + " =1 OR (" + iIsActive + "=0 And A.IsActive= 1)) And ";
                    sWhereCondition += " ((A.CustomerId in (" + sAddresseIds + ") OR A.OnbehalfOf in (" + sAddresseIds + ")) ";
                    sWhereCondition += " OR (A.TaskId in (";
                    sWhereCondition += " (Select ObjectId From ProcessedBy Where ";
                    sWhereCondition += " ObjectType=" + ((int)Enums.VTSObjects.Task).ToString();
                    sWhereCondition += " And PerformedBy in (" + sAddresseIds + ")))))";
                }


                strSql = " Select ";
                strSql += " A.TaskId ";
                strSql += " From Tasks A";
                strSql += " Where A.IsProject=0 ";

                if (sWhereCondition != null && sWhereCondition != "")
                    strSql += " And " + sWhereCondition;


                strSql += " Order By A.TaskId";

                DataSet ds = new DataSet();
                ds = Common.dbMgr.ExecuteDataSet(CommandType.Text, strSql);
                if (ds != null)
                {
                    return " (" + ds.Tables[0].Rows.Count.ToString() + ")";
                }
                return " (0)";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void fnGetParentObjectCustomerAndKind(int iTaskId, ref int iCustomerId, ref string strCustomerName, ref int iKindId, ref int iMilestoneId, ref int iUserRequestId)
        {
            try
            {
                SqlParameter[] sPrms = new SqlParameter[6];
                SqlParameter sPrm = new SqlParameter();
                int iIndex = 0;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@TaskID";
                sPrm.Value = iTaskId;
                sPrms[iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Output;
                sPrm.ParameterName = "@CustomerId";
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Size = 800;
                sPrm.Direction = ParameterDirection.Output;
                sPrm.ParameterName = "@CustomerName";
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Output;
                sPrm.ParameterName = "@KindId";
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Output;
                sPrm.ParameterName = "@Milestone";
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Output;
                sPrm.ParameterName = "@UserRequestId";
                sPrms[++iIndex] = sPrm;

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spGetTaskParentObjectCustomerAndKindDetails", sPrms);

                iCustomerId = int.Parse(sPrms[iIndex - 4].Value.ToString());
                strCustomerName = sPrms[iIndex - 3].Value.ToString();
                iKindId = int.Parse(sPrms[iIndex - 2].Value.ToString());
                iMilestoneId = int.Parse(sPrms[iIndex - 1].Value.ToString());
                iUserRequestId = int.Parse(sPrms[iIndex].Value.ToString());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataSet GetTaskFunctions()
        {
            try
            {
                SqlParameter[] Params = new SqlParameter[1];
                SqlParameter param = null;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@UILanguageId";
                param.Value = CommonVariable.iLanguageId;
                Params[0] = param;

                DataSet ds = null;
                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetTaskFunctionsListDS", Params);
                return ds;
            }
            catch (Exception ex)
            { throw ex; }

        }

        public DataSet fnGetProcessedByContact(string strAddresseId)
        {
            string strSql = "Select AddresseId  ,isnull(HourlyRate,0) as HourlyRate from addresses where ";
            strSql += " Addresseid in(" + strAddresseId + ")";
            return Common.dbMgr.ExecuteDataSet(CommandType.Text, strSql);
        }

        public bool fnIsContributionExceeds(string sContribution, int iMilestoneId, string sRecordId)
        {
            try
            {
                int iRecordId = 0;
                if (sRecordId != "")
                    iRecordId = int.Parse(sRecordId);

                int iContribution = 0;
                if (sContribution != "")
                    iContribution = int.Parse(sContribution);
                string strSQL = "";
                strSQL = "Select isnull(Sum(Contribution),0) +" + iContribution + " from tasks where Milestone=" + iMilestoneId + " and TaskId<>" + iRecordId;
                if (int.Parse(Common.dbMgr.ExecuteScalar(CommandType.Text, strSQL)) > 100)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool fnIsInvalidBudget(decimal dCurrentBudget, decimal dBudget, string recordId, int iIsPartOf, ref string strMessage)
        {
            try
            {
                string strSQL = "";

                int iRecordId = 0;
                if (recordId != "")
                    iRecordId = int.Parse(recordId);

                strSQL = "Select isnull(Sum(Budget),0) from Tasks Where IsPartOf=" + iRecordId;

                if ((iRecordId > 0) && (dCurrentBudget < decimal.Parse(Common.dbMgr.ExecuteScalar(CommandType.Text, strSQL))))
                {
                    strMessage = "The budget is invalid as it is less than the total budget of its sub projects.";
                    return true;
                }
                else
                {

                    if (dCurrentBudget > dBudget)
                    {
                        if (fnIsExceedsContainerBudget(iIsPartOf, (dCurrentBudget - dBudget)))
                        {
                            strMessage = "The budget is invalid as it is more than its parent project.";
                            return true;
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool fnIsExceedsContainerBudget(int iContainerId, decimal dChangedBudget, string sRecordID = "")
        {
            try
            {
                int iRecordId = 0;
                if (sRecordID != "")
                    iRecordId = int.Parse(sRecordID);

                SqlParameter[] Params = new SqlParameter[4];
                int iIndex = 0;

                Params[iIndex] = new SqlParameter("@ContainerProjectID", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iContainerId;

                Params[++iIndex] = new SqlParameter("@ChangedBudget", SqlDbType.Decimal);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = dChangedBudget;

                Params[++iIndex] = new SqlParameter("@CurrentProjectID", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iRecordId;

                Params[++iIndex] = new SqlParameter("@Status", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Output;

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spCheckForContainerBudget", Params);
                return Convert.ToBoolean(Params[iIndex].Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool fnIsContainer(int recordId)
        {
            try
            {


                SqlParameter[] sPrms = new SqlParameter[2];
                SqlParameter sPrm = new SqlParameter();
                int iIndex = 0;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@TaskId";
                sPrm.Value = recordId;
                sPrms[iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Output;
                sPrm.ParameterName = "@IsContainer";
                sPrms[++iIndex] = sPrm;

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spIsContainer", sPrms);
                return Convert.ToBoolean(sPrms[iIndex].Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool fnProjectCanbeDone(string TaskId, ref string sMessage)
        {
            try
            {

                SqlParameter[] Params = new SqlParameter[3];
                int iIndex = 0;

                Params[iIndex] = new SqlParameter("@TaskId", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = int.Parse(TaskId);

                Params[++iIndex] = new SqlParameter("@Message", SqlDbType.VarChar);
                Params[iIndex].Size = 100;
                Params[iIndex].Direction = ParameterDirection.Output;

                Params[++iIndex] = new SqlParameter("@Status", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Output;

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spCheckProjectCanbeDone", Params);
                sMessage = Params[iIndex - 1].Value.ToString();
                return Convert.ToBoolean(Params[iIndex].Value.ToString());

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public string GetChildProjectIds(string sRecId)
        {
            string sSql = "Select TaskId from tasks where Isproject=1  and IsPartOf=" + sRecId;
            DataSet ds = Common.dbMgr.ExecuteDataSet(CommandType.Text, sSql);
            string sTaskIds = "";
            string sTempIds = "0";
            while (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sTempIds += "," + ds.Tables[0].Rows[i]["TaskId"].ToString();

                    if (sTaskIds != "")
                        sTaskIds += ",";
                    sTaskIds += ds.Tables[0].Rows[i]["TaskId"].ToString();

                }
                sSql = "Select TaskId from tasks where Isproject=1  and IsPartOf in (" + sTempIds + ")";

                ds = Common.dbMgr.ExecuteDataSet(CommandType.Text, sSql);
                sTempIds = "0";
            }

            return sTaskIds;
        }


        public DataSet GetDependentProjectTable(string sTaskids)
        {
            try
            {
                string sSql = "Select TaskId,Subject from tasks where taskid in(" + sTaskids + ") and customerid is not null";
                return Common.dbMgr.ExecuteDataSet(CommandType.Text, sSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int GetCountChildsNotDone(string sRecId)
        {
            int iRecCount = 0;
            string sSql = "Select TaskId,IsDone, cast(1 as bit) as CanBeParent From Tasks Where IsPartOf=" + sRecId;
            sSql += " Union Select DecisionId,IsDecided, cast(0 as bit) as CanBeParent  from Decisions Where IsPartOf=" + sRecId;
            DataSet ds = Common.dbMgr.ExecuteDataSet(CommandType.Text, sSql);
            string sTempIds = "0";

            while (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if ((bool)ds.Tables[0].Rows[i]["CanBeParent"])
                        sTempIds += "," + ds.Tables[0].Rows[i]["TaskId"].ToString();

                    if ((bool)ds.Tables[0].Rows[i]["IsDone"] == false)
                        iRecCount += 1;
                }

                sSql = "Select TaskId,IsDone, cast(1 as bit) as CanBeParent From Tasks Where IsPartOf in (" + sTempIds + ")";
                sSql += " Union Select DecisionId,IsDecided, cast(0 as bit) as CanBeParent  from Decisions Where IsPartOf in (" + sTempIds + ")";

                ds = Common.dbMgr.ExecuteDataSet(CommandType.Text, sSql);

                sTempIds = "0";
            }
            return iRecCount;
        }

        public void GetChildsNotDone(string sRecId, ref string sTaskIds, ref string sDecisionIds)
        {
            string sSql = "Select TaskId,IsDone, cast(1 as bit) as IsTask From Tasks Where IsPartOf=" + sRecId;
            sSql += " Union Select DecisionId,IsDecided, cast(0 as bit) as IsTask from Decisions Where IsPartOf=" + sRecId;
            DataSet ds = Common.dbMgr.ExecuteDataSet(CommandType.Text, sSql);
            string sTempIds = "0";

            while (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if ((bool)ds.Tables[0].Rows[i]["IsTask"])
                        sTempIds += "," + ds.Tables[0].Rows[i]["TaskId"].ToString();

                    if ((bool)ds.Tables[0].Rows[i]["IsDone"] == false)
                    {
                        if ((bool)ds.Tables[0].Rows[i]["IsTask"])
                        {
                            if (sTaskIds != "")
                                sTaskIds += ",";

                            sTaskIds += ds.Tables[0].Rows[i]["TaskId"].ToString();
                        }
                        else
                        {
                            if (sDecisionIds != "")
                                sDecisionIds += ",";

                            sDecisionIds += ds.Tables[0].Rows[i]["TaskId"].ToString();
                        }
                    }
                }

                sSql = "Select TaskId,IsDone, cast(1 as bit) as IsTask From Tasks Where IsPartOf in (" + sTempIds + ")";
                sSql += " Union Select DecisionId,IsDecided, cast(0 as bit) as IsTask  from Decisions Where IsPartOf in (" + sTempIds + ")";

                ds = Common.dbMgr.ExecuteDataSet(CommandType.Text, sSql);

                sTempIds = "0";
            }
        }

        public int GetCountParentsDone(string sRecId, ref string MilestoneIsDone)
        {

            string sId = "";
            sId = sRecId;
            int iRecCount = 0;
            string sSql = "Select B.TaskId, B.IsDone From Tasks A Inner Join Tasks B on A.IsPartOf=B.TaskId and A.TaskId=" + sRecId;
            DataSet ds = Common.dbMgr.ExecuteDataSet(CommandType.Text, sSql);

            while (ds.Tables[0].Rows.Count > 0)
            {
                sRecId = ds.Tables[0].Rows[0]["TaskId"].ToString();

                if ((bool)ds.Tables[0].Rows[0]["IsDone"])
                    iRecCount += 1;

                sSql = "Select B.TaskId, B.IsDone From Tasks A Inner Join Tasks B on A.IsPartOf=B.TaskId and A.TaskId=" + sRecId;
                ds = Common.dbMgr.ExecuteDataSet(CommandType.Text, sSql);
            }

            sSql = "";
            sSql = "Select cast(IsDone as varchar) from Milestones where MilestoneId=(Select Milestone from Tasks where TaskId=" + sId + ")";
            MilestoneIsDone = Common.dbMgr.ExecuteScalar(CommandType.Text, sSql);
            return iRecCount;

        }


        public string GetParentsDone(string sRecId, ref string strAllMilestoneIds)
        {
            string sIds = "";

            string sSql = "Select B.TaskId, B.IsDone,A.Milestone From Tasks A Inner Join Tasks B on A.IsPartOf=B.TaskId and A.TaskId=" + sRecId;
            DataSet ds = Common.dbMgr.ExecuteDataSet(CommandType.Text, sSql);

            while (ds.Tables[0].Rows.Count > 0)
            {
                sRecId = ds.Tables[0].Rows[0]["TaskId"].ToString();

                if ((bool)ds.Tables[0].Rows[0]["IsDone"])
                {
                    if (sIds != "")
                        sIds += ",";

                    sIds += ds.Tables[0].Rows[0]["TaskId"].ToString();
                }

                if (strAllMilestoneIds != "")
                {
                    if (ds.Tables[0].Rows[0]["Milestone"].ToString() != "")
                        strAllMilestoneIds = strAllMilestoneIds + "," + ds.Tables[0].Rows[0]["Milestone"].ToString();
                }
                else
                {
                    if (ds.Tables[0].Rows[0]["Milestone"].ToString() != "")
                        strAllMilestoneIds = ds.Tables[0].Rows[0]["Milestone"].ToString();
                }

                sSql = "Select B.TaskId, B.IsDone,A.Milestone From Tasks A Inner Join Tasks B on A.IsPartOf=B.TaskId and A.TaskId=" + sRecId;
                ds = Common.dbMgr.ExecuteDataSet(CommandType.Text, sSql);
            }

            return sIds;
        }



        public void fnGetIsChildObjectsExists(string sRecId, ref bool bTasks, ref bool bDecisions)
        {
            try
            {
                string sSql = "Select TaskId,cast(1 as bit) as CanBeParent From Tasks Where IsPartOf=" + sRecId + " and isActive=1";
                sSql += " Union Select DecisionId,cast(0 as bit) as CanBeParent  from Decisions Where IsPartOf=" + sRecId + " and isActive=1";
                DataSet ds = Common.dbMgr.ExecuteDataSet(CommandType.Text, sSql);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    bTasks = false;
                    bDecisions = false;
                    return;
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if ((bool)ds.Tables[0].Rows[i]["CanBeParent"])
                    {
                        bTasks = true;
                        if (bDecisions)
                        {
                            break;
                        }
                    }
                    else
                    {
                        bDecisions = true;
                        if (bTasks)
                        {
                            break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string fnSave(string strRecordID, ProjectTaskActionData data, ref bool bsuccess, ref string sParentIdsToBeOpened, ref string sChildTaskIdsToBeClosed, ref string sChildDecisionIdsToBeClosed, string sChildTasksForUseSplRights, string sChildDecisionsForUseSplRights)
        {
            try
            {
                string strSql = "";

                if ((strRecordID == "") || (strRecordID == "0"))
                {
                    //int iObjectSortIndex = 0;
                    //iObjectSortIndex = int.Parse(Common.dbMgr.ExecuteScalar(CommandType.Text, "Select isnull(Max(ObjectSortIndex) + 1,1) as NewIndex From Tasks"));

                    strSql = "Insert into Tasks(IsProject,IsDone,DoneDate,IsInternalProject,IsSpecialProject,SpecialProjectType,TaskCode,Subject,PriorityId,";
                    strSql += "CustomerId,ProjectKindId,TaskKindId,ProjectStatus,IsPartOf,Remarks,";
                    strSql += "InboxDate,InitiationSourceId,OnbehalfOf,InboxRemarks,";
                    strSql += "DeadlineRemind,DeadlineRemindDays,RemarksDetail,IsActive,";
                    strSql += "IsManualCode,ProjectIcon,Keywords,UseSpecialRights,IsOwnerFullRights,ProjectManager,MileStone,Budget,Contribution,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn)";

                    strSql += " Values(";

                    if (data.IsProject)
                        strSql += "1,";
                    else
                        strSql += "0,";

                    if (data.IsDone)
                        strSql += "1,";
                    else
                        strSql += "0,";

                    if (data.DoneDate == 0)
                        strSql += "Null,";
                    else
                        strSql += "cast(" + data.DoneDate.ToString() + " as datetime),";


                    if (data.IsInternalProject)
                        strSql += "1,";
                    else
                        strSql += "0,";

                    if (data.IsSpecialProject)
                        strSql += "1,";
                    else
                        strSql += "0,";

                    strSql += data.SpecialProjectType + ",";
                    strSql += Common.EncodeNString(data.TaskCode) + ",";
                    strSql += Common.EncodeNString(data.Subject) + ",";
                    strSql += Common.EncodeValue(data.Priority) + "," + Common.EncodeValue(data.Customer) + ",";
                    strSql += Common.EncodeValue(data.ProjectKind) + "," + Common.EncodeValue(data.TaskKind) + ",";
                    strSql += Common.EncodeValue(data.ProjectStatus) + ",";
                    strSql += Common.EncodeValue(data.IsPartOf) + ",";
                    strSql += Common.EncodeNString(data.Remarks) + ",";

                    if (data.InboxDate == 0)
                        strSql += "Null,";
                    else
                        strSql += "cast(" + data.InboxDate.ToString() + " as datetime),";

                    strSql += Common.EncodeValue(data.InitiationSource) + "," + Common.EncodeValue(data.OnBehalfOf) + ",";
                    strSql += Common.EncodeNString(data.InboxRemarks) + ",";

                    //if (dtDeadlineFrom == 0)
                    //    strSql += "Null,";
                    //else
                    //    strSql += "cast(" + dtDeadlineFrom.ToString() + " as datetime),";

                    //if (dtDeadlineTo == 0)
                    //    strSql += "Null,";
                    //else
                    //    strSql += "cast(" + dtDeadlineTo.ToString() + " as datetime),";

                    if (data.IsDeadlineRemind)
                    {
                        strSql += "1,";
                        strSql += data.DeadlineRemindDays + ",";
                    }
                    else
                    {
                        strSql += "0,";
                        strSql += "null,";
                        data.DeadlineRemindDays = -1;
                    }

                    if (data.IsSpecialProject)
                        strSql += "null,";
                    else
                        strSql += Common.EncodeNString(data.RemarksDetail) + ",";

                    if (data.IsActive)
                        strSql += "1,";
                    else
                        strSql += "0,";

                    if (data.IsManualCode)
                        strSql += "1,";
                    else
                        strSql += "0,";

                    if (data.ProjectIcon != null)
                        strSql += "@Pic,";
                    else
                        strSql += "Null,";


                    strSql += Common.EncodeNString(data.Keywords) + ",";
                    if (data.IsUseSplRights)
                        strSql += "1,";
                    else
                        strSql += "0,";

                    if (data.IsOwnerHasFullRights)
                        strSql += "1,";
                    else
                        strSql += "0,";

                    strSql += Common.EncodeValue(data.ProjectMangaer) + ",";
                    strSql += Common.EncodeValue(data.Milestone) + ",";
                    strSql += Common.EncodeValue(data.Budget) + ",";
                    strSql += Common.EncodeValue(data.Contribution) + ",";

                    strSql += Common.EncodeNString(data.CreatedBy) + "," + "getdate(),";
                    strSql += "Null," + "Null)";

                }
                else
                {

                    strSql = "Update Tasks ";
                    strSql += " Set TaskCode=";
                    strSql += Common.EncodeNString(data.TaskCode) + ",";

                    if (data.IsDone)
                        strSql += "IsDone=1,";
                    else
                        strSql += "IsDone=0,";

                    if (data.DoneDate == 0)
                        strSql += " DoneDate=Null,";
                    else
                        strSql += " DoneDate=cast(" + data.DoneDate.ToString() + " as datetime),";


                    if (data.IsInternalProject)
                        strSql += "IsInternalProject=1,";
                    else
                        strSql += "IsInternalProject=0,";

                    if (data.IsSpecialProject)
                        strSql += "IsSpecialProject=1,";
                    else
                        strSql += "IsSpecialProject=0,";
                    strSql += "SpecialProjectType=" + data.SpecialProjectType + ",";

                    strSql += " Subject=";
                    strSql += Common.EncodeNString(data.Subject) + ",PriorityId=" + Common.EncodeValue(data.Priority) + ",";
                    strSql += " CustomerId=" + Common.EncodeValue(data.Customer) + ",ProjectKindId=" + Common.EncodeValue(data.ProjectKind) + ",";
                    strSql += " TaskKindId=" + Common.EncodeValue(data.TaskKind) + ",IsPartOf=" + Common.EncodeValue(data.IsPartOf) + ",";
                    strSql += " Remarks=" + Common.EncodeNString(data.Remarks) + ",";
                    strSql += " ProjectStatus=" + Common.EncodeValue(data.ProjectStatus) + ",";

                    if (data.InboxDate == 0)
                        strSql += " InboxDate=Null,";
                    else
                        strSql += " InboxDate=cast(" + data.InboxDate.ToString() + " as datetime),";

                    strSql += " InitiationSourceId=" + Common.EncodeValue(data.InitiationSource) + ",OnbehalfOf=" + Common.EncodeValue(data.OnBehalfOf) + ",";

                    strSql += " InboxRemarks=" + Common.EncodeNString(data.InboxRemarks) + ",";

                    //if (dtDeadlineFrom == 0)
                    //    strSql += " DeadlineFrom=Null,";
                    //else
                    //    strSql += " DeadlineFrom=cast(" + dtDeadlineFrom.ToString() + " as datetime),";

                    //if (dtDeadlineTo == 0)
                    //    strSql += " DeadlineTo=Null,";
                    //else
                    //    strSql += " DeadlineTo=cast(" + dtDeadlineTo.ToString() + " as datetime),";

                    if (data.IsDeadlineRemind)
                    {
                        strSql += "DeadlineRemind=1,";
                        strSql += " DeadlineRemindDays=" + data.DeadlineRemindDays + ",";
                    }
                    else
                    {
                        strSql += "DeadlineRemind=0,";
                        strSql += " DeadlineRemindDays=null,";
                        data.DeadlineRemindDays = -1;
                    }
                    if (data.IsSpecialProject)
                        strSql += " RemarksDetail=null,";
                    else
                        strSql += " RemarksDetail=" + Common.EncodeNString(data.RemarksDetail) + ",";

                    if (data.IsActive)
                        strSql += "IsActive=1,";
                    else
                        strSql += "IsActive=0,";

                    if (data.IsManualCode)
                        strSql += "IsManualCode=1,";
                    else
                        strSql += "IsManualCode=0,";

                    if (data.ProjectIcon != null)
                        strSql += " ProjectIcon=@Pic,";
                    else
                        strSql += " ProjectIcon=NULL,";

                    strSql += " Keywords=" + Common.EncodeNString(data.Keywords) + ",";

                    if (data.IsUseSplRights)
                        strSql += " UseSpecialRights=1,";
                    else
                        strSql += " UseSpecialRights=0,";


                    if (data.IsOwnerHasFullRights)
                        strSql += "IsOwnerFullRights =1,";
                    else
                        strSql += "IsOwnerFullRights =0,";


                    strSql += " ProjectManager=" + Common.EncodeValue(data.ProjectMangaer) + ",";
                    strSql += " MileStone=" + Common.EncodeValue(data.Milestone) + ",";
                    strSql += " Budget=" + Common.EncodeValue(data.Budget) + ",";
                    strSql += " Contribution=" + Common.EncodeValue(data.Contribution) + ",";
                    if (data.IsAlternateContractor)
                        strSql += " IsAlteranteContractor=1 ,";
                    else
                        strSql += " IsAlteranteContractor=0 ,";
                    strSql += " ModifiedBy=" + Common.EncodeNString(data.ModifiedBy) + ",ModifiedOn=getdate()";
                    strSql += " Where TaskId=" + strRecordID;
                }

                if (data.ProjectIcon != null)
                {
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@Pic", SqlDbType.Image);
                    cmd.Parameters["@Pic"].Value = data.ProjectIcon;
                    cmd.CommandText = strSql;
                    Common.dbMgr.ExecuteNonQuery(cmd);
                }
                else
                {
                    Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                }
                //Record is saved.


                //handle changes to isDone Flag.  This applies only for existing records.
                if ((strRecordID != "") && (strRecordID != "0"))
                {
                    string strAllIds = "";
                    if (data.IsDone)
                    {
                        GetChildsNotDone(strRecordID, ref sChildTaskIdsToBeClosed, ref sChildDecisionIdsToBeClosed);

                        if (sChildTaskIdsToBeClosed != "")
                            strAllIds = strRecordID + "," + sChildTaskIdsToBeClosed;
                        else
                            strAllIds = strRecordID;

                        //Set all the milestones done status to 1 for a project
                        if (strAllIds != "")
                        {

                            strSql = " Update Milestones Set IsDone=1,";
                            if (data.DoneDate == 0)
                                strSql += " DoneDate=getdate() ";
                            else
                                strSql += " DoneDate=cast(" + data.DoneDate.ToString() + " as datetime) ";
                            strSql += " Where ObjectId in ( Select ContractId from Contracts where Project in(" + strAllIds + "))";

                            Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                            strSql = " Update Contracts Set ";
                            strSql += " ModifiedBy=" + Common.EncodeNString(data.ModifiedBy) + ",ModifiedOn=getdate(),";
                            strSql += " IsDone=1,Status= " + ((int)Enums.ObjectStatus.ContractDone).ToString();
                            if (data.DoneDate == 0)
                                strSql += ", DoneDate=getdate()";
                            else
                                strSql += ", DoneDate=cast(" + data.DoneDate.ToString() + " as datetime) ";
                            strSql += " Where Project in( " + strAllIds + ")";
                            Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                        }
                    }
                    else
                    {
                        sParentIdsToBeOpened = GetParentsDone(strRecordID, ref strAllIds);

                        if (strAllIds != "")
                        {
                            strSql = " Update Milestones Set ";
                            strSql += " IsDone=0,DoneDate=null Where MilestoneId in (" + strAllIds + ")";
                            Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                            strSql = " Update Contracts Set ";
                            strSql += " ModifiedBy=" + Common.EncodeNString(data.ModifiedBy) + ",ModifiedOn=getdate(),";
                            strSql += " IsDone=0, DoneDate=null,Status= " + ((int)Enums.ObjectStatus.ProjectCreated).ToString();
                            strSql += " Where ContractId in( Select ObjectId from Milestones where MilestoneId in( " + strAllIds + ")) and IsDone=1";
                            Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                        }

                        if (data.IsProject && strAllIds == "")
                        {
                            strSql = " Update Contracts Set ";
                            strSql += " ModifiedBy=" + Common.EncodeNString(data.ModifiedBy) + ",ModifiedOn=getdate(),";
                            strSql += " IsDone=0, DoneDate=null,Status= " + ((int)Enums.ObjectStatus.ProjectCreated).ToString();
                            strSql += " Where Project = " + strRecordID + " and IsDone=1";
                            Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                        }
                    }

                    if (sChildDecisionIdsToBeClosed != "")
                    {
                        strSql = " Update Decisions Set ";
                        strSql += " ModifiedBy=" + Common.EncodeNString(data.ModifiedBy) + ",ModifiedOn=getdate(),";
                        strSql += " IsDecided=1,DecidedDate=getDate() Where DecisionId in (" + sChildDecisionIdsToBeClosed + ")";
                        Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                    }
                    if (sChildTaskIdsToBeClosed != "")
                    {
                        strSql = " Update Tasks Set ";
                        strSql += " ModifiedBy=" + Common.EncodeNString(data.ModifiedBy) + ",ModifiedOn=getdate(),";
                        strSql += " IsDone=1, DoneDate=getDate() Where TaskId in (" + sChildTaskIdsToBeClosed + ")";
                        Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                    }
                    if (sParentIdsToBeOpened != "")
                    {
                        strSql = " Update Tasks Set ";
                        strSql += " ModifiedBy=" + Common.EncodeNString(data.ModifiedBy) + ",ModifiedOn=getdate(),";
                        strSql += " IsDone=0, DoneDate=null Where TaskId in (" + sParentIdsToBeOpened + ")";
                        Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                    }




                    if (sChildTasksForUseSplRights != "")
                    {
                        strSql = "Update Tasks Set UseSpecialRights=";
                        if (data.IsUseSplRights)
                            strSql += "1,";
                        else
                            strSql += "0,";

                        if (data.IsOwnerHasFullRights)
                            strSql += "IsOwnerFullRights =1,";
                        else
                            strSql += "IsOwnerFullRights =0,";

                        strSql += " ModifiedBy=" + Common.EncodeNString(data.ModifiedBy) + ",ModifiedOn=getdate()";
                        strSql += " where taskid in(" + sChildTasksForUseSplRights + ")";
                        Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                    }
                    if (sChildDecisionsForUseSplRights != "")
                    {
                        strSql = "Update Decisions Set UseSpecialRights=";
                        if (data.IsUseSplRights)
                            strSql += "1,";
                        else
                            strSql += "0,";

                        if (data.IsOwnerHasFullRights)
                            strSql += "IsOwnerFullRights =1,";
                        else
                            strSql += "IsOwnerFullRights =0,";
                        strSql += " ModifiedBy=" + Common.EncodeNString(data.ModifiedBy) + ",ModifiedOn=getdate()";

                        strSql += " where DecisionId in(" + sChildDecisionsForUseSplRights + ")";
                        Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                    }


                    if (data.IsProject && data.ProjectStatus == -1000)
                    {

                        // get the total amount which has sent finbase
                        strSql = " Select isnull(Sum(amount),0) from MilestoneInvoices MI Inner Join Milestones M On M.MilestoneId=MI.MilestoneID Where M.Project =" + strRecordID;
                        decimal dStumbleProjectBudget = Convert.ToDecimal(Common.dbMgr.ExecuteScalar(CommandType.Text, strSql).ToString());

                        // update the budget of the task
                        strSql = " Update Tasks Set Budget=" + dStumbleProjectBudget.ToString() + " Where TaskId=" + strRecordID;
                        Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                        // update the budget of the milestones
                        strSql = " Update ContractVendors Set Budget=" + dStumbleProjectBudget.ToString() + " Where Project=" + strRecordID;
                        Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                        //when parent project is group project then we need to reduce the extra budget
                        if (data.IsParentGroup)
                        {
                            strSql = " Update Tasks Set Budget=(Budget - " + data.Budget + ") +cast(" + dStumbleProjectBudget.ToString() + "  as decimal) Where TaskId=" + data.IsPartOf;
                            Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                        }

                        // REMOVE project Reminders
                        strSql = "Delete From UserReminders Where ReminderType in (";
                        strSql += ((int)Enums.ReminderType.DeadlineEnd).ToString() + ", " + ((int)Enums.ReminderType.DeadlineBegin).ToString() + ")";
                        strSql += " And ObjectTypeId = " + ((int)Enums.VTSObjects.Project).ToString();
                        strSql += " And ObjectId =" + strRecordID;
                        Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);



                        //remove deadline begin & end reminders for this object Milestones if any.
                        strSql = "Delete From UserReminders Where ReminderType = ";
                        strSql += ((int)Enums.ReminderType.MileStoneDone).ToString();
                        strSql += " And ObjectTypeId = " + ((int)Enums.VTSObjects.Contracts).ToString();
                        strSql += " And ObjectId in (Select MilestoneID From Milestones Where project=" + strRecordID + ")";
                        Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                        //remove deadline begin & end reminders for this object Milestones if any.
                        strSql = "Delete From UserReminders Where ReminderType = ";
                        strSql += ((int)Enums.ReminderType.Payments).ToString();
                        strSql += " And ObjectTypeId = " + ((int)Enums.VTSObjects.Project).ToString();
                        strSql += " And ObjectId in (select InvoiceID from Milestones M Inner Join MilestoneInvoices MI On M.MilestoneId=MI.MilestoneID ";
                        strSql += " Where M.Project=" + strRecordID + ")";
                        Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);




                    }

                }

                bsuccess = true;
                if ((strRecordID == "") || (strRecordID == "0"))
                    return Common.dbMgr.ExecuteScalar(CommandType.Text, "Select max(TaskId) from Tasks");
                else
                {
                    return strRecordID;
                }

            }
            catch (Exception ex)
            {
                bsuccess = false;
                throw ex;
            }
        }


        public bool fnDeleteDetialsData(string strTaskId)
        {
            try
            {
                SqlParameter[] Params = new SqlParameter[1];
                SqlParameter param = null;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@RecordId";
                param.Value = Convert.ToInt32(strTaskId);
                Params[0] = param;

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spDeleteGroupProjectDeatilsData", Params);

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        //public DataSet GetTaskInitiationSourcesList()
        //{
        //    try
        //    {
        //        SqlParameter[] Params = new SqlParameter[1];
        //        int iIndex = 0;

        //        Params[iIndex] = new SqlParameter("@UILanguageId", SqlDbType.Int);
        //        Params[iIndex].Direction = ParameterDirection.Input;
        //        Params[iIndex].Value = CommonVariable.iLanguageId;

        //        DataSet ds = null;
        //        ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetTaskInitiationSourcesListDS", Params);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    { throw ex; }

        //}



    }

    public class clsTasksProcessedBy
    {
        public DataSet GetTaskProcessedBy(string sRecordID, int iObjType)
        {
            return GetTaskProcessedBy(sRecordID, iObjType, false);
        }
        public DataSet GetTaskProcessedBy(string sRecordID, int iObjType, bool bIsArchivedData)
        {

            try
            {
                DataSet ds = new DataSet();
                int iRecordId = 0;
                if (sRecordID != "")
                    iRecordId = int.Parse(sRecordID);

                SqlParameter[] sPrms = new SqlParameter[2];
                SqlParameter sPrm = new SqlParameter();

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

                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetProcessedByDS", sPrms);
                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetProcessedByAddresses(string sRecordID, int iObjType)
        {

            try
            {
                int iRecordId = 0;
                if (sRecordID != "")
                    iRecordId = int.Parse(sRecordID);

                SqlParameter[] sPrms = new SqlParameter[2];
                SqlParameter sPrm = new SqlParameter();

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

                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetProcessedByAddressesDS", sPrms);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }

    public class clsTaskMilestones
    {

        public bool fnCheckDependency(string MilestoneId, bool bForDelete, ref string sMessage)
        {
            try
            {
                if (MilestoneId != "")
                {
                    SqlParameter[] Params = new SqlParameter[4];
                    int iIndex = 0;

                    Params[iIndex] = new SqlParameter("@RecordID", SqlDbType.Int);
                    Params[iIndex].Direction = ParameterDirection.Input;
                    Params[iIndex].Value = int.Parse(MilestoneId);

                    Params[++iIndex] = new SqlParameter("@bForDelete", SqlDbType.Bit);
                    Params[iIndex].Direction = ParameterDirection.Input;
                    Params[iIndex].Value = bForDelete;

                    Params[++iIndex] = new SqlParameter("@Message", SqlDbType.VarChar);
                    Params[iIndex].Size = 100;
                    Params[iIndex].Direction = ParameterDirection.Output;

                    Params[++iIndex] = new SqlParameter("@Status", SqlDbType.Bit);
                    Params[iIndex].Direction = ParameterDirection.Output;

                    Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spCheckMilestoneDependency", Params);

                    sMessage = Params[iIndex - 1].Value.ToString();
                    return Convert.ToBoolean(Params[iIndex].Value.ToString());
                }
                else
                    return false;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


    }

    public class clsDeadlineExtension
    {
        public DateTime GetProjectStartDate(string strTaskId)
        {

            string strSQL = "";
            try
            {
                strSQL = "Select isnull(Min(DateFrom),getdate()) as ProjStartDate from TaskDeadlines where TaskId=" + strTaskId;
                return Convert.ToDateTime(Common.dbMgr.ExecuteScalar(CommandType.Text, strSQL));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDeadlineExtn(string sRecordID)
        {
            string strSQL;
            try
            {
                if (sRecordID == "")
                    strSQL = "Select RecordId as ID,TaskId,DateFrom,DateTill,CreatedBy,CreatedOn,Reason,cast(Null as int) as Duration,'' as Flag from TaskDeadlines where TaskId=0 Order By CreatedOn Desc,RecordId Desc ";
                else
                    strSQL = "Select RecordId as ID,TaskId,DateFrom,DateTill,CreatedBy,CreatedOn, Reason,DateDiff(d,DateFrom,DateTill) as Duration,'' as Flag from TaskDeadlines where TaskId=" + sRecordID + " Order By CreatedOn Desc,RecordId Desc ";

                return Common.dbMgr.ExecuteDataSet(CommandType.Text, strSQL);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveDeadlines(DataTable dtDeadlines, string sRecordID, string sUserName)
        {
            try
            {

                int i;
                string strSQL = "";
                long lngDate = 0;
                if (dtDeadlines.Rows.Count > 0)
                {
                    for (i = 0; i < dtDeadlines.Rows.Count; i++)
                    {

                        if (dtDeadlines.Rows[i]["Flag"].ToString() == "N")
                        {
                            strSQL = "Insert into TaskDeadlines(TaskId,DateFrom,DateTill,CreatedBy,CreatedOn,Reason)";
                            strSQL += " values(" + sRecordID + ",";

                            if (dtDeadlines.Rows[i]["DateFrom"] != DBNull.Value)
                                lngDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(dtDeadlines.Rows[i]["DateFrom"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);
                            else
                                lngDate = 0;

                            if (lngDate == 0)
                                strSQL += "Null,";
                            else
                                strSQL += "cast(" + lngDate.ToString() + " as datetime),";

                            if (dtDeadlines.Rows[i]["DateTill"] != DBNull.Value)
                                lngDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(dtDeadlines.Rows[i]["DateTill"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);
                            else
                                lngDate = 0;

                            if (lngDate == 0)
                                strSQL += "Null,";
                            else
                                strSQL += "cast(" + lngDate.ToString() + " as datetime),";

                            strSQL += Common.EncodeNString(sUserName) + ",";
                            strSQL += "getdate(),";
                            strSQL += Common.EncodeNString(dtDeadlines.Rows[i]["Reason"].ToString()) + ")";

                            Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSQL);
                        }
                        else if (dtDeadlines.Rows[i]["Flag"].ToString() == "M")
                        {
                            strSQL = "Update TaskDeadlines Set TaskId=" + sRecordID + ",";

                            if (dtDeadlines.Rows[i]["DateFrom"] != DBNull.Value)
                                lngDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(dtDeadlines.Rows[i]["DateFrom"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);
                            else
                                lngDate = 0;

                            if (lngDate == 0)
                                strSQL += "DateFrom=Null,";
                            else
                                strSQL += "DateFrom=cast(" + lngDate.ToString() + " as datetime),";

                            if (dtDeadlines.Rows[i]["DateTill"] != DBNull.Value)
                                lngDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(dtDeadlines.Rows[i]["DateTill"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);
                            else
                                lngDate = 0;

                            if (lngDate == 0)
                                strSQL += "DateTill=Null, ";
                            else
                                strSQL += "DateTill=cast(" + lngDate.ToString() + " as datetime), ";

                            strSQL += " Reason=" + Common.EncodeNString(dtDeadlines.Rows[i]["Reason"].ToString());
                            strSQL += " Where RecordId=" + dtDeadlines.Rows[i]["ID"].ToString();
                            Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSQL);
                        }
                        else if (dtDeadlines.Rows[i]["Flag"].ToString() == "D")
                        {
                            strSQL = "Delete from TaskDeadlines where ";
                            strSQL += "RecordId=" + Common.EncodeString(dtDeadlines.Rows[i]["ID"].ToString());

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
        
    }





}
