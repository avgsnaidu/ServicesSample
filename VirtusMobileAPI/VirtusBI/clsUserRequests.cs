using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EPMEnums;
using Microsoft.VisualBasic;
using VirtusDataModel;

namespace VirtusBI
{
    public class clsUserRequests : clsBaseBI
    {
        public clsUserRequests()
        {
            if (Common.dbMgr == null)
                Common.SetConnectionString(ConfigurationManager.AppSettings["DBConnectionString"].ToString());
        }


        public DataSet fnGetUserRequestDetails(int iUserRequestId, string strLoginName)
        {
            try
            {

                if (Common.dbMgr == null)
                    Common.SetConnectionString("");

                DataSet ds = new DataSet();
                SqlParameter[] sPrms = new SqlParameter[2];
                SqlParameter sPrm = new SqlParameter();

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@UserRequestId";
                sPrm.Value = iUserRequestId;
                sPrms[0] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@LoginName";
                sPrm.Value = strLoginName;
                sPrms[1] = sPrm;

                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetUserRequestDetailsDS", sPrms);
                string strToCcUsers = fnGetCCUsers(iUserRequestId, strLoginName);

                if (!string.IsNullOrEmpty(strToCcUsers))
                {
                    DataColumn newCol = new DataColumn("ToCcUsers", typeof(string));
                    newCol.AllowDBNull = true;
                    ds.Tables[0].Columns.Add(newCol);

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        row["ToCcUsers"] = strToCcUsers;
                    }
                }
                return ds;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public UserRequestReceived fnGetUserRequestDetails(int iUserRequestId, string strLoginName)
        //{
        //    try
        //    {
        //        UserRequestReceived objReceivedUR = null;
        //        if (Common.dbMgr == null)
        //            Common.SetConnectionString("");

        //        DataSet ds = new DataSet();
        //        SqlParameter[] sPrms = new SqlParameter[2];
        //        SqlParameter sPrm = new SqlParameter();

        //        sPrm.SqlDbType = System.Data.SqlDbType.Int;
        //        sPrm.Direction = ParameterDirection.Input;
        //        sPrm.ParameterName = "@UserRequestId";
        //        sPrm.Value = iUserRequestId;
        //        sPrms[0] = sPrm;

        //        sPrm = new SqlParameter();
        //        sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
        //        sPrm.Direction = ParameterDirection.Input;
        //        sPrm.ParameterName = "@LoginName";
        //        sPrm.Value = strLoginName;
        //        sPrms[1] = sPrm;

        //        ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetUserRequestDetailsDS", sPrms);

        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //          objReceivedUR=  new UserRequestReceived();
        //            int iRowIndex = ds.Tables[0].Rows.Count - 1;
        //            objReceivedUR.Code = ds.Tables[0].Rows[iRowIndex]["Code"] + "";
        //            objReceivedUR.Subject = ds.Tables[0].Rows[iRowIndex]["Subject"] + "";

        //            if (ds.Tables[0].Rows[iRowIndex]["RequestType"] != DBNull.Value)
        //                objReceivedUR.RequestType = int.Parse(ds.Tables[0].Rows[iRowIndex]["RequestType"].ToString());
        //            else
        //                objReceivedUR.RequestType = 0;

        //            if (ds.Tables[0].Rows[iRowIndex]["PriorityId"] != DBNull.Value)
        //                objReceivedUR.PriorityId = int.Parse(ds.Tables[0].Rows[iRowIndex]["PriorityId"].ToString());
        //            else
        //                objReceivedUR.PriorityId = 0;

        //            objReceivedUR.Description = ds.Tables[0].Rows[iRowIndex]["Description"] + "";

        //            if (ds.Tables[0].Rows[iRowIndex]["DeadlineEndDate"] != DBNull.Value)
        //                objReceivedUR.DeadLineEndDate = DateTime.Parse(ds.Tables[0].Rows[iRowIndex]["DeadlineEndDate"].ToString());
        //            //objReceivedUR.DeadLineEndDate =  DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(ds.Tables[0].Rows[iRowIndex]["DeadlineEndDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);
        //            else
        //                objReceivedUR.DeadLineEndDate = (DateTime?)null;


        //            if (ds.Tables[0].Rows[iRowIndex]["InitialBudget"] != DBNull.Value)
        //                objReceivedUR.InitialBudget = Convert.ToDecimal(ds.Tables[0].Rows[iRowIndex]["InitialBudget"].ToString());
        //            else
        //                objReceivedUR.InitialBudget = 0;

        //            //objReceivedUR.DeadlineRemind = (ds.Tables[0].Rows[iRowIndex]["DeadlineRemind"] != DBNull.Value && (bool)ds.Tables[0].Rows[iRowIndex]["DeadlineRemind"]);

        //            //if (ds.Tables[0].Rows[iRowIndex]["DeadlineRemindDays"] != DBNull.Value)
        //            //    iDeadlineRemindDays = int.Parse(ds.Tables[0].Rows[iRowIndex]["DeadlineRemindDays"].ToString());
        //            //else
        //            //    iDeadlineRemindDays = -1;

        //            objReceivedUR.IsDone = (ds.Tables[0].Rows[iRowIndex]["IsDone"] != DBNull.Value && (bool)ds.Tables[0].Rows[iRowIndex]["IsDone"]);

        //            if (ds.Tables[0].Rows[iRowIndex]["DoneDate"] != DBNull.Value)
        //                objReceivedUR.DoneDate = DateTime.Parse(ds.Tables[0].Rows[iRowIndex]["DoneDate"].ToString());

        //                //objReceivedUR.DoneDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(ds.Tables[0].Rows[iRowIndex]["DoneDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);
        //            else
        //                objReceivedUR.DoneDate =(DateTime?)null;
        //                //objReceivedUR.DoneDate = (DateTime?)null;


        //            objReceivedUR.IsDesignRequired = (ds.Tables[0].Rows[iRowIndex]["IsDesignRequired"] != DBNull.Value && (bool)ds.Tables[0].Rows[iRowIndex]["IsDesignRequired"]);
        //            objReceivedUR.IsTenderRequired = (ds.Tables[0].Rows[iRowIndex]["IsTenderRequired"] != DBNull.Value && (bool)ds.Tables[0].Rows[iRowIndex]["IsTenderRequired"]);
        //            objReceivedUR.IsContractRequired = (ds.Tables[0].Rows[iRowIndex]["IsContractRequired"] != DBNull.Value && (bool)ds.Tables[0].Rows[iRowIndex]["IsContractRequired"]);

        //            objReceivedUR.CreatedBy = ds.Tables[0].Rows[iRowIndex]["CreatedBy"] + "";
        //            objReceivedUR.CreatedOn = string.IsNullOrEmpty(ds.Tables[0].Rows[iRowIndex]["CreatedOn"] + "") ? (DateTime?)null : Convert.ToDateTime(ds.Tables[0].Rows[iRowIndex]["CreatedOn"] + "");
        //            objReceivedUR.ModifiedBy = ds.Tables[0].Rows[iRowIndex]["ModifiedBy"] + "";
        //            objReceivedUR.ModifiedOn = string.IsNullOrEmpty(ds.Tables[0].Rows[iRowIndex]["ModifiedOn"] + "") ? (DateTime?)null : Convert.ToDateTime(ds.Tables[0].Rows[iRowIndex]["ModifiedOn"] + "");

        //            if (ds.Tables[0].Rows[iRowIndex]["Status"] != DBNull.Value)
        //                objReceivedUR.Status = int.Parse(ds.Tables[0].Rows[iRowIndex]["Status"].ToString());
        //            else
        //                objReceivedUR.Status = 0;

        //            if (ds.Tables[0].Rows[iRowIndex]["OriginalRequest"] != DBNull.Value && ds.Tables[0].Rows[iRowIndex]["OriginalStatus"] != DBNull.Value
        //                && ds.Tables[0].Rows[iRowIndex]["OrgSubject"] != DBNull.Value)
        //            {
        //                objReceivedUR.OriginalRequestId = int.Parse(ds.Tables[0].Rows[iRowIndex]["OriginalRequest"].ToString());
        //                objReceivedUR.OriginalStatus = int.Parse(ds.Tables[0].Rows[iRowIndex]["OriginalStatus"].ToString());
        //                objReceivedUR.OriginalSubject = ds.Tables[0].Rows[iRowIndex]["OrgSubject"].ToString();
        //            }
        //            else
        //            {
        //                objReceivedUR.OriginalRequestId = -1;
        //                objReceivedUR.OriginalSubject = "";
        //                objReceivedUR.OriginalStatus = -1;
        //            }
        //            if (ds.Tables[0].Rows[iRowIndex]["OriginalReqProject"] != DBNull.Value)
        //                objReceivedUR.OriginalReqProject = int.Parse(ds.Tables[0].Rows[iRowIndex]["OriginalReqProject"].ToString());
        //            else
        //                objReceivedUR.OriginalReqProject = -1;

        //            if (ds.Tables[0].Rows[iRowIndex]["Project"] != DBNull.Value)
        //                objReceivedUR.Project = int.Parse(ds.Tables[0].Rows[iRowIndex]["Project"].ToString());
        //            else
        //                objReceivedUR.Project = 0;
        //            //if (ds.Tables[0].Rows[iRowIndex]["Comments"] != DBNull.Value)
        //            //    Comments = ds.Tables[0].Rows[iRowIndex]["Comments"].ToString();
        //            //else
        //            //    Comments = "";
        //            if (ds.Tables[0].Rows[iRowIndex]["ForwardedTo"] != DBNull.Value)
        //                objReceivedUR.ForwardedTo = ds.Tables[0].Rows[iRowIndex]["ForwardedTo"].ToString();
        //            else
        //                objReceivedUR.ForwardedTo = "";
        //            if (ds.Tables[0].Rows[iRowIndex]["ToUser"] != DBNull.Value)
        //                objReceivedUR.ToUserIDs = ds.Tables[0].Rows[iRowIndex]["ToUser"].ToString();
        //            else
        //                objReceivedUR.ToUserIDs = "0";
        //            if (ds.Tables[0].Rows[iRowIndex]["DepartmentId"] != DBNull.Value)
        //                objReceivedUR.DepartmentId = int.Parse(ds.Tables[0].Rows[iRowIndex]["DepartmentId"].ToString());
        //            else
        //                objReceivedUR.DepartmentId = 0;
        //            string str;

        //            objReceivedUR.ToCCUsers = fnGetCCUsers(iUserRequestId, strLoginName);
        //            string strRequestComponent = string.Empty;
        //            str = fnGetRequestComponents(iUserRequestId, ref strRequestComponent);

        //            objReceivedUR.RequestComponents = strRequestComponent;
        //        }

        //        return objReceivedUR;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public UserRequestActionData fnGetUserRequestDetails(int iUserRequestId, string strLoginName)
        //{
        //    try
        //    {
        //        UserRequestActionData objReceivedUR = null;
        //        if (Common.dbMgr == null)
        //            Common.SetConnectionString("");

        //        DataSet ds = new DataSet();
        //        SqlParameter[] sPrms = new SqlParameter[2];
        //        SqlParameter sPrm = new SqlParameter();

        //        sPrm.SqlDbType = System.Data.SqlDbType.Int;
        //        sPrm.Direction = ParameterDirection.Input;
        //        sPrm.ParameterName = "@UserRequestId";
        //        sPrm.Value = iUserRequestId;
        //        sPrms[0] = sPrm;

        //        sPrm = new SqlParameter();
        //        sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
        //        sPrm.Direction = ParameterDirection.Input;
        //        sPrm.ParameterName = "@LoginName";
        //        sPrm.Value = strLoginName;
        //        sPrms[1] = sPrm;

        //        ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetUserRequestDetailsDS", sPrms);

        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            objReceivedUR = new UserRequestActionData();
        //            int iRowIndex = ds.Tables[0].Rows.Count - 1;
        //            objReceivedUR.Code = ds.Tables[0].Rows[iRowIndex]["Code"] + "";
        //            objReceivedUR.Subject = ds.Tables[0].Rows[iRowIndex]["Subject"] + "";

        //            if (ds.Tables[0].Rows[iRowIndex]["RequestType"] != DBNull.Value)
        //                objReceivedUR.RequestType = int.Parse(ds.Tables[0].Rows[iRowIndex]["RequestType"].ToString());
        //            else
        //                objReceivedUR.RequestType = 0;

        //            if (ds.Tables[0].Rows[iRowIndex]["PriorityId"] != DBNull.Value)
        //                objReceivedUR.PriorityId = int.Parse(ds.Tables[0].Rows[iRowIndex]["PriorityId"].ToString());
        //            else
        //                objReceivedUR.PriorityId = 0;

        //            objReceivedUR.Description = ds.Tables[0].Rows[iRowIndex]["Description"] + "";

        //            if (ds.Tables[0].Rows[iRowIndex]["DeadlineEndDate"] != DBNull.Value)
        //                objReceivedUR.DeadlineEndDate = DateTime.Parse(ds.Tables[0].Rows[iRowIndex]["DeadlineEndDate"].ToString());
        //            //objReceivedUR.DeadLineEndDate =  DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(ds.Tables[0].Rows[iRowIndex]["DeadlineEndDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);
        //            else
        //                objReceivedUR.DeadlineEndDate = (DateTime?)null;


        //            if (ds.Tables[0].Rows[iRowIndex]["InitialBudget"] != DBNull.Value)
        //                objReceivedUR.InitialBudget = Convert.ToDecimal(ds.Tables[0].Rows[iRowIndex]["InitialBudget"].ToString());
        //            else
        //                objReceivedUR.InitialBudget = 0;

        //            //objReceivedUR.DeadlineRemind = (ds.Tables[0].Rows[iRowIndex]["DeadlineRemind"] != DBNull.Value && (bool)ds.Tables[0].Rows[iRowIndex]["DeadlineRemind"]);

        //            //if (ds.Tables[0].Rows[iRowIndex]["DeadlineRemindDays"] != DBNull.Value)
        //            //    iDeadlineRemindDays = int.Parse(ds.Tables[0].Rows[iRowIndex]["DeadlineRemindDays"].ToString());
        //            //else
        //            //    iDeadlineRemindDays = -1;

        //            objReceivedUR.IsDone = (ds.Tables[0].Rows[iRowIndex]["IsDone"] != DBNull.Value && (bool)ds.Tables[0].Rows[iRowIndex]["IsDone"]);

        //            if (ds.Tables[0].Rows[iRowIndex]["DoneDate"] != DBNull.Value)
        //                objReceivedUR.DoneDate = DateTime.Parse(ds.Tables[0].Rows[iRowIndex]["DoneDate"].ToString());

        //                //objReceivedUR.DoneDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(ds.Tables[0].Rows[iRowIndex]["DoneDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);
        //            else
        //                objReceivedUR.DoneDate = (DateTime?)null;
        //            //objReceivedUR.DoneDate = (DateTime?)null;


        //            objReceivedUR.IsDesignRequired = (ds.Tables[0].Rows[iRowIndex]["IsDesignRequired"] != DBNull.Value && (bool)ds.Tables[0].Rows[iRowIndex]["IsDesignRequired"]);
        //            objReceivedUR.IsTenderRequired = (ds.Tables[0].Rows[iRowIndex]["IsTenderRequired"] != DBNull.Value && (bool)ds.Tables[0].Rows[iRowIndex]["IsTenderRequired"]);
        //            objReceivedUR.IsContractRequired = (ds.Tables[0].Rows[iRowIndex]["IsContractRequired"] != DBNull.Value && (bool)ds.Tables[0].Rows[iRowIndex]["IsContractRequired"]);

        //            objReceivedUR.CreatedBy = ds.Tables[0].Rows[iRowIndex]["CreatedBy"] + "";
        //            objReceivedUR.CreatedOn = string.IsNullOrEmpty(ds.Tables[0].Rows[iRowIndex]["CreatedOn"] + "") ? (DateTime?)null : Convert.ToDateTime(ds.Tables[0].Rows[iRowIndex]["CreatedOn"] + "");
        //            objReceivedUR.ModifiedBy = ds.Tables[0].Rows[iRowIndex]["ModifiedBy"] + "";
        //            objReceivedUR.ModifiedOn = string.IsNullOrEmpty(ds.Tables[0].Rows[iRowIndex]["ModifiedOn"] + "") ? (DateTime?)null : Convert.ToDateTime(ds.Tables[0].Rows[iRowIndex]["ModifiedOn"] + "");

        //            if (ds.Tables[0].Rows[iRowIndex]["Status"] != DBNull.Value)
        //                objReceivedUR.Status = int.Parse(ds.Tables[0].Rows[iRowIndex]["Status"].ToString());
        //            else
        //                objReceivedUR.Status = 0;

        //            if (ds.Tables[0].Rows[iRowIndex]["OriginalRequest"] != DBNull.Value && ds.Tables[0].Rows[iRowIndex]["OriginalStatus"] != DBNull.Value
        //                && ds.Tables[0].Rows[iRowIndex]["OrgSubject"] != DBNull.Value)
        //            {
        //                objReceivedUR.OriginalRequestId = int.Parse(ds.Tables[0].Rows[iRowIndex]["OriginalRequest"].ToString());
        //                objReceivedUR.OriginalStatus = int.Parse(ds.Tables[0].Rows[iRowIndex]["OriginalStatus"].ToString());
        //                objReceivedUR.OriginalSubject = ds.Tables[0].Rows[iRowIndex]["OrgSubject"].ToString();
        //            }
        //            else
        //            {
        //                objReceivedUR.OriginalRequestId = -1;
        //                objReceivedUR.OriginalSubject = "";
        //                objReceivedUR.OriginalStatus = -1;
        //            }
        //            if (ds.Tables[0].Rows[iRowIndex]["OriginalReqProject"] != DBNull.Value)
        //                objReceivedUR.OriginalReqProject = int.Parse(ds.Tables[0].Rows[iRowIndex]["OriginalReqProject"].ToString());
        //            else
        //                objReceivedUR.OriginalReqProject = -1;

        //            if (ds.Tables[0].Rows[iRowIndex]["Project"] != DBNull.Value)
        //                objReceivedUR.Project = int.Parse(ds.Tables[0].Rows[iRowIndex]["Project"].ToString());
        //            else
        //                objReceivedUR.Project = 0;
        //            //if (ds.Tables[0].Rows[iRowIndex]["Comments"] != DBNull.Value)
        //            //    Comments = ds.Tables[0].Rows[iRowIndex]["Comments"].ToString();
        //            //else
        //            //    Comments = "";
        //            if (ds.Tables[0].Rows[iRowIndex]["ForwardedTo"] != DBNull.Value)
        //                objReceivedUR.ForwardedTo = ds.Tables[0].Rows[iRowIndex]["ForwardedTo"].ToString();
        //            else
        //                objReceivedUR.ForwardedTo = "";
        //            if (ds.Tables[0].Rows[iRowIndex]["ToUser"] != DBNull.Value)
        //                objReceivedUR.ToUserids = ds.Tables[0].Rows[iRowIndex]["ToUser"].ToString();
        //            else
        //                objReceivedUR.ToUserids = "0";
        //            if (ds.Tables[0].Rows[iRowIndex]["DepartmentId"] != DBNull.Value)
        //                objReceivedUR.DepartmentId = int.Parse(ds.Tables[0].Rows[iRowIndex]["DepartmentId"].ToString());
        //            else
        //                objReceivedUR.DepartmentId = 0;
        //            string str;

        //            objReceivedUR.ToCCUsers = fnGetCCUsers(iUserRequestId, strLoginName);
        //            string strRequestComponent = string.Empty;
        //            str = fnGetRequestComponents(iUserRequestId, ref strRequestComponent);

        //            objReceivedUR.RequestComponents = strRequestComponent;
        //        }

        //        return objReceivedUR;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        private string fnGetCCUsers(int iUserRequestId, string LoginName)
        {
            string strToCcUsers = string.Empty;

            try
            {
                DataSet ds;
                SqlParameter[] sPrms = new SqlParameter[2];
                SqlParameter sPrm = new SqlParameter();

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@UserRequestId";
                sPrm.Value = iUserRequestId;
                sPrms[0] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@Sentby";
                sPrm.Value = LoginName;
                sPrms[1] = sPrm;

                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetUserRequestCCNames", sPrms);

                if (ds.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        if (strToCcUsers != "")
                            strToCcUsers += ",";
                        strToCcUsers += ds.Tables[0].Rows[i][1];
                    }
                }
                else
                    strToCcUsers = "-100";
                return strToCcUsers.Trim();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string fnGetRequestComponents(int iUserRequestId, ref string strRequestComponentIds)
        {
            string strName = "";
            strRequestComponentIds = "";

            try
            {
                DataSet ds;
                //SqlParameter[] sPrms = new SqlParameter[1];
                SqlParameter[] sPrms = new SqlParameter[2];
                SqlParameter sPrm = new SqlParameter();
                int iIndex = 0;

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@UserRequestId";
                sPrm.Value = iUserRequestId;
                sPrms[iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@UILanguageId";
                sPrm.Value = CommonVariable.iLanguageId;
                sPrms[++iIndex] = sPrm;

                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetUserRequestComponentNames", sPrms);

                if (ds.Tables[0].Rows.Count != 0)
                {
                    // iDepartmentId = int.Parse(ds.Tables[0].Rows[0]["DepartmentId"].ToString());
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (strName != "")
                            strName += ";";
                        strName += ds.Tables[0].Rows[i][0];

                        if (strRequestComponentIds != "")
                            strRequestComponentIds += ",";
                        strRequestComponentIds += ds.Tables[0].Rows[i][2];

                    }
                }

                return strName.Trim();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet fnGetMyRequestData(string strLoginUserName, bool bUnDone, bool bIsReject, bool bIsCC, bool bIsAllRequests)
        {
            int iUnDone = 0;
            if (bUnDone)
                iUnDone = 1;
            int iIsAllRequests = 0;
            if (bIsAllRequests)
                iIsAllRequests = 1;

            int iReject = 0;
            if (bIsReject)
                iReject = 1;

            DataSet ds = new DataSet();
            try
            {

                //SqlParameter[] sPrms = new SqlParameter[5];
                SqlParameter[] sPrms = new SqlParameter[6];
                SqlParameter sPrm = new SqlParameter();
                int iIndex = 0;

                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@UserName";
                sPrm.Value = strLoginUserName;
                sPrms[iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@IsDone";
                sPrm.Value = iUnDone;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ShowReject";
                sPrm.Value = iReject;
                sPrms[++iIndex] = sPrm;


                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ShowCC";
                sPrm.Value = bIsCC;
                sPrms[++iIndex] = sPrm;


                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@IsAllRequests";
                sPrm.Value = iIsAllRequests;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@UILanguageId";
                sPrm.Value = CommonVariable.iLanguageId;
                sPrms[++iIndex] = sPrm;

                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetMyUserRequestsDS", sPrms);
                fnUpdateDatasetwithRecepientNameAndComponents(ref  ds);

                return ds;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void fnUpdateDatasetwithRecepientNameAndComponents(ref DataSet ds)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string strToUsers = "";
                    string strToOUs = "";
                    string strRequestComponentIds = "";
                    if (ds.Tables[0].Rows[i]["RecepientName"].ToString() == "")
                        //   ds.Tables[0].Rows[i]["RecepientName"] = fnGetReceipentsName(int.Parse(ds.Tables[0].Rows[i]["UserRequestId"].ToString()), strLoginName, ref strToUsers, ref strToOUs);
                        ds.Tables[0].Rows[i]["ToUserIDs"] = strToUsers;
                    ds.Tables[0].Rows[i]["ToOUs"] = strToOUs;

                    if (ds.Tables[0].Rows[i]["RequestComponents"].ToString() == "")
                        ds.Tables[0].Rows[i]["RequestComponents"] = fnGetRequestComponents(int.Parse(ds.Tables[0].Rows[i]["UserRequestId"].ToString()), ref strRequestComponentIds);
                    ds.Tables[0].Rows[i]["RequestComponentIds"] = strRequestComponentIds;

                }
            }
        }

        public DataSet fnGetSentItemsData(string strLoginUserName, bool bUnDone, bool bIsReject)
        {
            DataSet ds = new DataSet();
            try
            {

                //SqlParameter[] sPrms = new SqlParameter[3];
                SqlParameter[] sPrms = new SqlParameter[4];
                SqlParameter sPrm = new SqlParameter();
                int iIndex = 0;

                int iUnDone = 0;
                if (bUnDone)
                    iUnDone = 1;
                int iReject = 0;
                if (bIsReject)
                    iReject = 1;

                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@UserName";
                sPrm.Value = strLoginUserName;
                sPrms[iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@IsDone";
                sPrm.Value = iUnDone;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@IsReject";
                sPrm.Value = iReject;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@UILanguageId";
                sPrm.Value = 3;
                sPrms[++iIndex] = sPrm;

                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetUserRequestsSentItemsDS", sPrms);
                fnUpdateDatasetwithRecepientNameAndComponents(ref  ds);

                return ds;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int fnSave(int iRecordID, ref bool bSuccess, EditUserRequestActionData uReqObj)
        {
            try
            {
                string strSql = "";

                if (iRecordID == 0)
                {

                    strSql = "Insert into UserRequests (Code,RequestType,Subject,PriorityId,InitialBudget,Description,DeadlineRemind,DeadlineRemindDays,DeadlineEndDate,";
                    strSql += "  IsDone,DoneDate,IsDesignRequired,IsTenderRequired,IsContractRequired,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,Status,IsArchived,DepartmentId,";
                    strSql += "  OriginalRequest,OriginalReqProject,Project) Values ( ";
                    strSql += Common.EncodeNString(uReqObj.Code) + "," + uReqObj.RequestType.ToString() + "," + Common.EncodeNString(uReqObj.Subject) + ",";
                    strSql += uReqObj.PriorityId.ToString() + "," + uReqObj.InitialBudget.ToString() + "," + Common.EncodeNString(uReqObj.Description) + ",";
                    if (uReqObj.DeadlineRemind)
                        strSql += "1," + uReqObj.DeadlineReminderDays.ToString() + ",";
                    else
                    {
                        strSql += "0,null,";
                        uReqObj.DeadlineReminderDays = -1;
                    }
                    if (uReqObj.DeadlineEndDate == null || uReqObj.DeadlineEndDate <= DateTime.MinValue)
                        strSql += "null,";
                    else
                        strSql += "cast(" + DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, (DateTime)uReqObj.DeadlineEndDate, Microsoft.VisualBasic.FirstDayOfWeek.System, FirstWeekOfYear.System) + " as datetime),";

                    if (uReqObj.IsDone)
                        strSql += "1,";
                    else
                        strSql += "0,";

                    if (uReqObj.DoneDate == null || uReqObj.DoneDate <= DateTime.MinValue)
                        strSql += "Null,";
                    else
                        strSql += "cast(" + DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, (DateTime)uReqObj.DoneDate, Microsoft.VisualBasic.FirstDayOfWeek.System, FirstWeekOfYear.System) + " as datetime),";

                    if (uReqObj.IsDesignRequired)
                        strSql += "1,";
                    else
                        strSql += "0,";

                    if (uReqObj.IsTenderRequired)
                        strSql += "1,";
                    else
                        strSql += "0,";

                    if (uReqObj.IsContractRequired)
                        strSql += "1,";
                    else
                        strSql += "0,";


                    strSql += Common.EncodeString(uReqObj.CreatedBy) + "," + "getdate(),";
                    strSql += "Null," + "Null,";
                    strSql += "0,0,";
                    if (uReqObj.DepartmentId != 0)
                        strSql += uReqObj.DepartmentId + ",";
                    else
                        strSql += "Null,";
                    if (uReqObj.OriginalRequestId != 0)
                        strSql += uReqObj.OriginalRequestId + "," + uReqObj.OriginalReqProject + ",NULL)";
                    else
                        strSql += "NULL,NULL,NULL)";
                }
                else
                {

                    strSql = "Update UserRequests Set ";
                    strSql += " Subject = " + Common.EncodeNString(uReqObj.Subject) + ",";
                    strSql += " PriorityId=" + uReqObj.PriorityId.ToString() + ",InitialBudget=" + uReqObj.InitialBudget.ToString() + ",Description=" + Common.EncodeNString(uReqObj.Description) + ",";
                    if (uReqObj.DeadlineRemind)
                        strSql += "DeadlineRemind=1,DeadlineRemindDays=" + uReqObj.DeadlineReminderDays.ToString() + ",";
                    else
                    {
                        strSql += "DeadlineRemind=0,DeadlineRemindDays=null,";
                        uReqObj.DeadlineReminderDays = -1;
                    }
                    if (uReqObj.DeadlineEndDate == null || uReqObj.DeadlineEndDate <= DateTime.MinValue)
                        strSql += "DeadlineEndDate=null,";
                    else
                        strSql += "DeadlineEndDate=cast(" + DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, (DateTime)uReqObj.DeadlineEndDate, Microsoft.VisualBasic.FirstDayOfWeek.System, FirstWeekOfYear.System) + " as datetime),";

                    if (uReqObj.IsDone)
                        strSql += "IsDone=1,";
                    else
                        strSql += "IsDone=0,";

                    if (uReqObj.DoneDate == null || uReqObj.DoneDate <= DateTime.MinValue)
                        strSql += "DoneDate=Null,";
                    else
                        strSql += "DoneDate=cast(" + DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, (DateTime)uReqObj.DoneDate, Microsoft.VisualBasic.FirstDayOfWeek.System, FirstWeekOfYear.System) + " as datetime),";

                    if (uReqObj.IsDesignRequired)
                        strSql += "IsDesignRequired=1,";
                    else
                        strSql += "IsDesignRequired=0,";

                    if (uReqObj.IsTenderRequired)
                        strSql += "IsTenderRequired=1,";
                    else
                        strSql += "IsTenderRequired=0,";

                    if (uReqObj.IsContractRequired)
                        strSql += "IsContractRequired=1,";
                    else
                        strSql += "IsContractRequired=0,";
                    if (uReqObj.OriginalRequestId != 0)
                        strSql += "OriginalRequest=" + uReqObj.OriginalRequestId + ",";
                    if (uReqObj.OriginalReqProject != 0)
                        strSql += "OriginalReqProject=" + uReqObj.OriginalReqProject + ",";
                    if (uReqObj.DepartmentId != 0)
                        strSql += "DepartmentId=" + uReqObj.DepartmentId + ",";
                    strSql += "ModifiedBy=" + Common.EncodeString(uReqObj.ModifiedBy) + ",ModifiedOn=" + "getdate(),";
                    strSql += "Status=" + uReqObj.Status;
                    strSql += " where UserRequestId=" + iRecordID;

                }

                Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                string strFromUser = string.Empty;
                if (iRecordID == 0)
                {
                    iRecordID = int.Parse(Common.dbMgr.ExecuteScalar(CommandType.Text, "Select max(UserRequestId) from UserRequests").ToString());
                    strFromUser = uReqObj.CreatedBy;
                }
                else
                    strFromUser = uReqObj.ModifiedBy;


                string sSql = string.Empty;
                sSql = "Delete From UserReminders Where ReminderType in ";
                sSql += "(" + ((int)Enums.ReminderType.Hold).ToString() + ")";
                sSql += " And ObjectTypeId in ";
                sSql += "(" + ((int)Enums.VTSObjects.UserRequests).ToString() + ")";
                sSql += " And ObjectId=" + iRecordID;

                Common.dbMgr.ExecuteNonQuery(CommandType.Text, sSql);



                if (uReqObj.IsKeptHold)
                {
                    if (fnSaveHoldData(iRecordID, strFromUser, uReqObj))
                    {
                        bSuccess = true;
                        return iRecordID;
                    }

                }
                if (uReqObj.IsSaveDraft)
                {
                    if (fnSaveDraft(iRecordID, strFromUser, uReqObj))
                    {
                        bSuccess = true;
                        return iRecordID;
                    }
                }

                if (!uReqObj.AlreadyApproved)
                {
                    SqlParameter[] sPrms = new SqlParameter[7];
                    SqlParameter sPrm = new SqlParameter();
                    int iIndex = 0;

                    sPrm.SqlDbType = System.Data.SqlDbType.Int;
                    sPrm.Direction = ParameterDirection.Input;
                    sPrm.ParameterName = "@UserRequestId";
                    sPrm.Value = iRecordID;
                    sPrms[iIndex] = sPrm;

                    sPrm = new SqlParameter();
                    sPrm.SqlDbType = System.Data.SqlDbType.Int;
                    sPrm.Direction = ParameterDirection.Input;
                    sPrm.ParameterName = "@ToPersons";
                    sPrm.Value = uReqObj.ToUserId;
                    sPrms[++iIndex] = sPrm;


                    sPrm = new SqlParameter();
                    sPrm.SqlDbType = System.Data.SqlDbType.VarChar;
                    sPrm.Direction = ParameterDirection.Input;
                    sPrm.ParameterName = "@ToCcUsers";
                    sPrm.Value = string.IsNullOrEmpty(uReqObj.ToCCUsers) ? "" : uReqObj.ToCCUsers;
                    sPrms[++iIndex] = sPrm;

                    sPrm = new SqlParameter();
                    sPrm.SqlDbType = System.Data.SqlDbType.VarChar;
                    sPrm.Direction = ParameterDirection.Input;
                    sPrm.ParameterName = "@Components";
                    sPrm.Value = string.IsNullOrEmpty(uReqObj.RequestComponents) ? " " : uReqObj.RequestComponents;
                    sPrms[++iIndex] = sPrm;

                    sPrm = new SqlParameter();
                    sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                    sPrm.Direction = ParameterDirection.Input;
                    sPrm.ParameterName = "@FromUser";
                    sPrm.Value = strFromUser;
                    sPrms[++iIndex] = sPrm;

                    sPrm = new SqlParameter();
                    sPrm.SqlDbType = System.Data.SqlDbType.Int;
                    sPrm.Direction = ParameterDirection.Input;
                    sPrm.ParameterName = "@Status";
                    sPrm.Value = uReqObj.Status;
                    sPrms[++iIndex] = sPrm;

                    sPrm = new SqlParameter();
                    sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                    sPrm.Direction = ParameterDirection.Input;
                    sPrm.ParameterName = "@Comments";
                    sPrm.Value = string.IsNullOrEmpty(uReqObj.Comments) ? " " : uReqObj.Comments;
                    sPrms[++iIndex] = sPrm;

                    Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spSendRequests", sPrms);
                }
                if (uReqObj.CreateNextStep)
                {
                    fnInsertBasicDesignTender(iRecordID, uReqObj);
                }

                bSuccess = true;

                return iRecordID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool fnSaveDraft(int iRecordId, string strFromUser, EditUserRequestActionData uReqObj)
        {
            try
            {
                SqlParameter[] sPrms = new SqlParameter[5];
                SqlParameter sPrm = new SqlParameter();
                int iIndex = 0;

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@UserRequestId";
                sPrm.Value = iRecordId;
                sPrms[iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ToPersons";
                sPrm.Value = uReqObj.ToUserId;
                sPrms[++iIndex] = sPrm;


                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.VarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ToCcUsers";
                sPrm.Value = string.IsNullOrEmpty(uReqObj.ToCCUsers) ? " " : uReqObj.ToCCUsers;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.VarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@Components";
                sPrm.Value = string.IsNullOrEmpty(uReqObj.RequestComponents) ? " " : uReqObj.RequestComponents;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@FromUser";
                sPrm.Value = strFromUser;
                sPrms[++iIndex] = sPrm;

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spSaveRequestData", sPrms);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool fnSaveHoldData(int iRecordId, string strFromUser, EditUserRequestActionData uReqObj)
        {
            try
            {
                SqlParameter[] sPrms = new SqlParameter[6];
                SqlParameter sPrm = new SqlParameter();
                int iIndex = 0;

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@UserRequestId";
                sPrm.Value = iRecordId;
                sPrms[iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@HoldUntil";
                sPrm.Value = uReqObj.HoldUntilDate;
                sPrms[++iIndex] = sPrm;


                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@IsRemind";
                sPrm.Value = uReqObj.IsHoldRemind;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@RemindDays";
                //sPrm.Value = uReqObj.HoldRemindDays;
                sPrm.Value = 0;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@FromUser";
                sPrm.Value = strFromUser;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@Comments";
                sPrm.Value = uReqObj.Comments;
                sPrms[++iIndex] = sPrm;

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spSaveRequestHoldData", sPrms);

                if (uReqObj.IsHoldRemind)
                {
                    string sSql = string.Empty;
                    //sSql = "Delete From UserReminders Where ReminderType in ";
                    //sSql += "(" + ((int)Enums.ReminderType.Hold).ToString() + ")";
                    //sSql += " And ObjectTypeId in ";
                    //sSql += "(" + ((int)Enums.VTSObjects.UserRequests).ToString() + ")";
                    //sSql += " And ObjectId=" + iRecordId;

                    //Common.dbMgr.ExecuteNonQuery(CommandType.Text, sSql);

                    //Enums.VTSObjects objType = Enums.VTSObjects.UserRequests;
                    //int iWorkStartHours = 8;
                    //int iWorkStartMinutes = 0;
                    //string sRemindPeriodType = "dd";
                    //int iRemindPeriodValue = 0;
                    //Common.GetRemindPeriod((Enums.Remind_Durations)iDeadlineRemindDays, ref sRemindPeriodType, ref iRemindPeriodValue);
                    //sSql = string.Empty;
                    //sSql = "Select isnull((Select datepart(hh, DateTimeValue) From VTSSettings Where SettingEnumId=" + ((int)Enums.SysSettingEntryTitle.WorkStartTime).ToString() + "),8) as H";
                    //iWorkStartHours = int.Parse(Common.dbMgr.ExecuteScalar(CommandType.Text, sSql));

                    //sSql = "Select isnull((Select datepart(mi, DateTimeValue) From VTSSettings Where SettingEnumId=" + ((int)Enums.SysSettingEntryTitle.WorkStartTime).ToString() + "),0) as H";
                    //iWorkStartMinutes = int.Parse(Common.dbMgr.ExecuteScalar(CommandType.Text, sSql));

                    //insert reminder for deadline end date.
                    //sSql = "Insert into UserReminders";
                    //sSql += " (LoginName,ReminderType,ActualTime,RemindTime,ObjectTypeId,ObjectId)";
                    //sSql += " Select ";
                    //sSql += Common.EncodeString(strFromUser) + ", " + ((int)Enums.ReminderType.Hold).ToString() + " as RemindType, ";
                    //sSql += " dateadd(mi, " + iWorkStartMinutes.ToString() + ", dateadd(hh," + iWorkStartHours.ToString() + ",A.HoldToDate)),";
                    //sSql += " dateadd(" + sRemindPeriodType + "," + iRemindPeriodValue.ToString() + ", dateadd(mi, " + iWorkStartMinutes.ToString() + ", dateadd(hh," + iWorkStartHours.ToString() + ",A.HoldToDate))),";
                    //sSql += ((int)objType).ToString() + " as ObjectTypeId, A.UserRequestId ";
                    //sSql += " From ";
                    //sSql += " UserRequestHoldDetails as A ";
                    //sSql += " Where A.HoldToDate is not null";
                    //sSql += " And A.UserRequestId=" + iRecordId.ToString();
                    //sSql += " And ";
                    //sSql += " dateadd(mi, " + iWorkStartMinutes.ToString() + ", dateadd(hh," + iWorkStartHours.ToString() + ",A.HoldToDate))";
                    //sSql += " > dateadd(mm, " + (-1 * iOldReminderMonths).ToString() + ", getdate())";

                    //int iRecCount = Common.dbMgr.ExecuteNonQuery(CommandType.Text, sSql);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int fnInsertBasicDesignTender(int iRecordID, EditUserRequestActionData uReqObj)
        {
            try
            {
                SqlParameter[] sPrms = new SqlParameter[5];
                SqlParameter sPrm = new SqlParameter();
                int iIndex = 0;

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@UserRequestId";
                sPrm.Value = iRecordID;
                sPrms[iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@Subject";
                sPrm.Value = uReqObj.Subject;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@CreatedBy";
                sPrm.Value = uReqObj.CreatedBy;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@Code";
                sPrm.Value = string.IsNullOrEmpty(uReqObj.DesignContractCode) ? "" : uReqObj.DesignContractCode;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@Comments";
                sPrm.Value = string.IsNullOrEmpty(uReqObj.Comments) ? "" : uReqObj.Comments;
                sPrms[++iIndex] = sPrm;
                return Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spInsertBasicDesignTenders", sPrms);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet fnGetUsersForUserRequest(string strLoginName, int UserRequestId, string strSearchCondition, int iNoOfRecords, string strOrderBy, string strSortOrder, int iTotCount, bool bisForward = false)
        {
            try
            {
                SqlParameter[] Params = new SqlParameter[8];
                SqlParameter param = null;
                int iIndex = 0;

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.NVarChar;
                param.Direction = ParameterDirection.Input;
                param.ParameterName = "@LoginName";
                param.Value = strLoginName;
                Params[iIndex] = param;

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.Int;
                param.Direction = ParameterDirection.Input;
                param.ParameterName = "@UserRequestId";
                param.Value = UserRequestId;
                Params[++iIndex] = param;

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.Bit;
                param.Direction = ParameterDirection.Input;
                param.ParameterName = "@IsForward";
                param.Value = bisForward;
                Params[++iIndex] = param;

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.NVarChar;
                param.Direction = ParameterDirection.Input;
                param.ParameterName = "@SearchCondition";
                param.Value = strSearchCondition;
                Params[++iIndex] = param;


                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.Int;
                param.Direction = ParameterDirection.Input;
                param.ParameterName = "@NoTopRecords";
                param.Value = iNoOfRecords;
                Params[++iIndex] = param;

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.ParameterName = "@OrderBy";
                param.Value = strOrderBy;
                Params[++iIndex] = param;

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.ParameterName = "@SortOrder";
                param.Value = strSortOrder;
                Params[++iIndex] = param;

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.Int;
                param.Direction = ParameterDirection.Output;
                param.ParameterName = "@TotCount";
                // param.Value = strSortOrder;
                Params[++iIndex] = param;

                DataSet ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetUsersForUserRequestDS", Params);
                iTotCount = int.Parse(Params[iIndex].Value.ToString());

                return ds;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public int fnGetUnReadObjectsCount(string strLoginName, int ObjectEnumId)
        {

            try
            {
                SqlParameter[] Params = new SqlParameter[2];
                SqlParameter param = null;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.NVarChar;
                param.ParameterName = "@LoginName";
                param.Value = strLoginName;
                Params[0] = param;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@ObjectEnumId";
                param.Value = ObjectEnumId;
                Params[1] = param;
                return int.Parse(Common.dbMgr.ExecuteScalar(CommandType.StoredProcedure, "spGetUnReadRequestsCount", Params));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int fnGetUserRequestToUser(int iRecordId)
        {
            try
            {
                SqlParameter[] sPrms = new SqlParameter[1];
                SqlParameter sPrm = new SqlParameter();
                int iIndex = 0;

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@UserRequestId";
                sPrm.Value = iRecordId;
                sPrms[iIndex] = sPrm;


                object Id = Common.dbMgr.ExecuteScalar(CommandType.StoredProcedure, "spGetUserRequestToUser", sPrms);
                if (Id.ToString() == "")
                    return 0;
                return int.Parse(Id.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // Get the old userRequests based on the Typeof the requests
        public DataSet fnGetRequestForUsers(string LoginName, int iRequestType)
        {
            try
            {
                SqlParameter[] Params = new SqlParameter[2];
                SqlParameter param = null;
                int iIndex = 0;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.NVarChar;
                param.ParameterName = "@LoginUserName";
                param.Value = LoginName;
                Params[iIndex] = param;



                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@RequestType";
                param.Value = iRequestType;
                Params[1] = param;

                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetUserRequestsForUser", Params);

            }
            catch (Exception ex)
            { throw ex; }
        }

        public DataSet fnGetProjectComponents(int OrginalRequestId, bool bIsMaintanaceRequest)
        {
            try
            {
                SqlParameter[] Params = new SqlParameter[2];
                SqlParameter param = null;
                int iIndex = 0;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@OriginalUserRequestId";
                param.Value = OrginalRequestId;
                Params[iIndex] = param;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Bit;
                param.ParameterName = "@IsMaintanaceRequest";
                param.Value = bIsMaintanaceRequest;
                Params[++iIndex] = param;


                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetProjectComponents", Params);

            }
            catch (Exception ex)
            { throw ex; }
        }

        public string fnGetRequestStatus(int iRequestId)
        {
            try
            {
                SqlParameter[] Params = new SqlParameter[2];
                //SqlParameter[] Params = new SqlParameter[1];
                SqlParameter param = null;
                int iIndex = 0;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@UserRequestId";
                param.Value = iRequestId;
                Params[iIndex] = param;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@UILanguageId";
                param.Value = CommonVariable.iLanguageId;
                Params[iIndex + 1] = param;

                return Convert.ToString(Common.dbMgr.ExecuteScalar(CommandType.StoredProcedure, "spGetUserRequestStatus", Params));

            }
            catch (Exception ex)
            { throw ex; }
        }

        public int fnGetReportingToId(int LoginUserId)
        {
            try
            {
                SqlParameter[] sPrms = new SqlParameter[2];
                SqlParameter sPrm = new SqlParameter();
                int iIndex = 0;

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@LoginUserId";
                sPrm.Value = LoginUserId;
                sPrms[iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Output;
                sPrm.ParameterName = "@ReportingTo";
                sPrms[++iIndex] = sPrm;



                Common.dbMgr.ExecuteScalar(CommandType.StoredProcedure, "spGetReportingTo", sPrms);
                return int.Parse(sPrms[iIndex].Value.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string fnGetReportingToNames(int id)
        {
            DataSet dsDepart = new DataSet();
            try
            {
                string strSql = string.Empty;
                strSql = "select FullName as Name  from Addresses Where AddresseId = " + id + "";
                string strValues = Common.dbMgr.ExecuteScalar(CommandType.Text, strSql);
                return strValues;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool fnLoginNameExists(string strLoginUserName, int RequestId)
        {
            SqlParameter[] sPrms = new SqlParameter[2];
            SqlParameter sPrm = new SqlParameter();
            int iIndex = 0;

            sPrm.SqlDbType = System.Data.SqlDbType.Int;
            sPrm.Direction = ParameterDirection.Input;
            sPrm.ParameterName = "@UserRequestId";
            sPrm.Value = RequestId;
            sPrms[iIndex] = sPrm;

            sPrm = new SqlParameter();
            sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
            sPrm.Direction = ParameterDirection.Input;
            sPrm.ParameterName = "@LoginName";
            sPrm.Value = strLoginUserName;
            sPrms[++iIndex] = sPrm;

            var iCount = int.Parse(Common.dbMgr.ExecuteScalar(CommandType.StoredProcedure, "spGetUserRecipientNameCount", sPrms));
            if (iCount > 0)
                return true;
            else
                return false;


        }

        public int fnSetReadStatusRequest(int iRequestId, string strLoginName)
        {

            try
            {
                SqlParameter[] Params = new SqlParameter[2];
                //SqlParameter[] Params = new SqlParameter[3];
                SqlParameter param = null;
                int iIndex = 0;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@UserRequestId";
                param.Value = iRequestId;
                Params[iIndex] = param;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.NVarChar;
                param.ParameterName = "@LoginName";
                param.Value = strLoginName;
                Params[1] = param;

                //param = new SqlParameter();
                //param.Direction = ParameterDirection.Input;
                //param.SqlDbType = SqlDbType.Int;
                //param.ParameterName = "@ToUser";
                //param.Value = iToUserId;
                //Params[2] = param;

                return Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spSetReadStatusforRequest", Params);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet fnGetRequestComponents(int iRequestTypeID, int iDepartmentId)
        {
            try
            {
                SqlParameter[] Params = new SqlParameter[2];
                SqlParameter param = null;
                int iIndex = 0;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@RequestTYpe";
                param.Value = iRequestTypeID;
                Params[iIndex] = param;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@DepartmentId";
                param.Value = iDepartmentId;
                Params[++iIndex] = param;

                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetRequestComponents", Params);

            }
            catch (Exception ex)
            { throw ex; }
        }

        public NextObjectRecordDetails fnGetObjectRecordId(int iRequestId, int iUserRequestEnumId)
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
                param.Value = iUserRequestEnumId;
                Params[iIndex] = param;

                param = new SqlParameter();
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                param.ParameterName = "@ObjectId";
                param.Value = iRequestId;
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

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spGetNextObjectRecordId", Params);
                NextObjectRecordDetails nextObj = new NextObjectRecordDetails();
                nextObj.RecordId = int.Parse(Params[iIndex].Value.ToString());
                nextObj.NextObjectEnumId = int.Parse(Params[iIndex - 1].Value.ToString());

                //NextObjEnumId = int.Parse(Params[iIndex - 1].Value.ToString());
                //return int.Parse(Params[iIndex].Value.ToString());
                return nextObj;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
