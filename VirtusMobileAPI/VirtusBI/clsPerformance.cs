using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EPMEnums;
using Microsoft.VisualBasic;

namespace VirtusBI
{
    public class clsPerformance : clsBaseBI
    {

        public DataSet fnGetPreformanceServiceDataSet(int iObjType, int iObjID, bool bSubObjects, bool bClearedTask, bool bViewRight)
        {
            try
            {

                SqlParameter[] sPrms = new SqlParameter[5];
                SqlParameter sPrm = new SqlParameter();

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ObjectTypeId";
                sPrm.Value = iObjType;
                sPrms[0] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ObjectId";
                sPrm.Value = iObjID;
                sPrms[1] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ShowSubObjects";
                sPrm.Value = bSubObjects;
                sPrms[2] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@IsClearedTask";
                sPrm.Value = bClearedTask;
                sPrms[3] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ViewRight";
                sPrm.Value = bViewRight;
                sPrms[4] = sPrm;

                DataSet ds = new DataSet();
                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetObjectPerformanceServiceDS", sPrms);
                return ds;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataSet fnGetPreformanceExpensesDataSet(int iObjType, int iObjID, bool bSubObjects, bool bClearedTask, bool bViewRight)
        {
            try
            {

                SqlParameter[] sPrms = new SqlParameter[5];
                SqlParameter sPrm = new SqlParameter();

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ObjectTypeId";
                sPrm.Value = iObjType;
                sPrms[0] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ObjectId";
                sPrm.Value = iObjID;
                sPrms[1] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ShowSubObjects";
                sPrm.Value = bSubObjects;
                sPrms[2] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@IsClearedTask";
                sPrm.Value = bClearedTask;
                sPrms[3] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ViewRight";
                sPrm.Value = bViewRight;
                sPrms[4] = sPrm;

                DataSet ds = new DataSet();
                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetObjectPerformanceExpensesDS", sPrms);
                return ds;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataSet fnGetTaskServiceTypesList(int iLanguageId)
        {
            try
            {
                SqlParameter[] sPrms = new SqlParameter[1];
                SqlParameter sPrm = new SqlParameter();

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@UILanguageId";
                sPrm.Value = iLanguageId;
                sPrms[0] = sPrm;

                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetTaskServiceTypesListDS", sPrms);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet fnGetProductsList(int iLanguageId)
        {
            try
            {
                SqlParameter[] sPrms = new SqlParameter[1];
                SqlParameter sPrm = new SqlParameter();

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@UILanguageId";
                sPrm.Value = iLanguageId;
                sPrms[0] = sPrm;

                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetProductsListDS", sPrms);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet fnGetObjectPerformedByDropdown(bool bIsExpenses, int iObjectTypeId, int iObjectId, string strCurrentUsers)
        {
            try
            {
                SqlParameter[] Params = new SqlParameter[4];

                int iIndex = 0;


                Params[iIndex] = new SqlParameter("@IsExpenses", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bIsExpenses;

                Params[++iIndex] = new SqlParameter("@ObjectTypeId", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iObjectTypeId;

                Params[++iIndex] = new SqlParameter("@ObjectId", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iObjectId;

                Params[++iIndex] = new SqlParameter("@CurrentUsers", SqlDbType.VarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = strCurrentUsers;


                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetObjectPerformancesUsers", Params);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet fnGetPerformedByDropdown(bool bIsExpenses, DateTime dtFromDate, DateTime dtToDate, int iUserId)
        {
            try
            {
                SqlParameter[] Params = new SqlParameter[4];
                int iIndex = 0;


                Params[iIndex] = new SqlParameter("@IsExpenses", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bIsExpenses;

                Params[++iIndex] = new SqlParameter("@FromDate", SqlDbType.DateTime);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = dtFromDate;

                Params[++iIndex] = new SqlParameter("@ToDate", SqlDbType.DateTime);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = dtToDate;

                Params[++iIndex] = new SqlParameter("@LoginUserId", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iUserId;



                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetPerformancesUsers", Params);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet fnGetProjectDropdowns(int iUserId, DateTime dtFromDate, DateTime dtToDate, bool bIsExpenses, string strExistingIds)
        {
            //@IsProject int, @LoginUserId int,@FromDate Datetime,@ToDate Datetime,@IsExpenses bit

            SqlParameter[] sPrms = new SqlParameter[5];
            SqlParameter sPrm = new SqlParameter();



            sPrm = new SqlParameter();
            sPrm.SqlDbType = System.Data.SqlDbType.Int;
            sPrm.Direction = ParameterDirection.Input;
            sPrm.ParameterName = "@LoginUserId";
            sPrm.Value = iUserId;
            sPrms[0] = sPrm;

            sPrm = new SqlParameter();
            sPrm.SqlDbType = System.Data.SqlDbType.DateTime;
            sPrm.Direction = ParameterDirection.Input;
            sPrm.ParameterName = "@FromDate";
            sPrm.Value = dtFromDate;
            sPrms[1] = sPrm;

            sPrm = new SqlParameter();
            sPrm.SqlDbType = System.Data.SqlDbType.DateTime;
            sPrm.Direction = ParameterDirection.Input;
            sPrm.ParameterName = "@ToDate";
            sPrm.Value = dtToDate;
            sPrms[2] = sPrm;

            sPrm = new SqlParameter();
            sPrm.SqlDbType = System.Data.SqlDbType.Bit;
            sPrm.Direction = ParameterDirection.Input;
            sPrm.ParameterName = "@IsExpenses";
            sPrm.Value = bIsExpenses;
            sPrms[3] = sPrm;

            sPrm = new SqlParameter();
            sPrm.SqlDbType = System.Data.SqlDbType.VarChar;
            sPrm.Direction = ParameterDirection.Input;
            sPrm.ParameterName = "@ExistingProjectIds";
            sPrm.Value = strExistingIds;
            sPrms[4] = sPrm;

            return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetPerformancesProjectDropDownDS", sPrms);
        }

        public DataSet fnGetTasksDropdowns(int iUserId, DateTime dtFromDate, DateTime dtToDate, bool bIsExpenses, int iParentProjectId, string strExistingIds, bool bProjectViewRight)
        {
            //@IsProject int, @LoginUserId int,@FromDate Datetime,@ToDate Datetime,@IsExpenses bit

            SqlParameter[] sPrms = new SqlParameter[7];
            SqlParameter sPrm = new SqlParameter();



            sPrm = new SqlParameter();
            sPrm.SqlDbType = System.Data.SqlDbType.Int;
            sPrm.Direction = ParameterDirection.Input;
            sPrm.ParameterName = "@LoginUserId";
            sPrm.Value = iUserId;
            sPrms[0] = sPrm;

            sPrm = new SqlParameter();
            sPrm.SqlDbType = System.Data.SqlDbType.DateTime;
            sPrm.Direction = ParameterDirection.Input;
            sPrm.ParameterName = "@FromDate";
            sPrm.Value = dtFromDate;
            sPrms[1] = sPrm;

            sPrm = new SqlParameter();
            sPrm.SqlDbType = System.Data.SqlDbType.DateTime;
            sPrm.Direction = ParameterDirection.Input;
            sPrm.ParameterName = "@ToDate";
            sPrm.Value = dtToDate;
            sPrms[2] = sPrm;

            sPrm = new SqlParameter();
            sPrm.SqlDbType = System.Data.SqlDbType.Bit;
            sPrm.Direction = ParameterDirection.Input;
            sPrm.ParameterName = "@IsExpenses";
            sPrm.Value = bIsExpenses;
            sPrms[3] = sPrm;

            sPrm = new SqlParameter();
            sPrm.SqlDbType = System.Data.SqlDbType.Int;
            sPrm.Direction = ParameterDirection.Input;
            sPrm.ParameterName = "@ProjectId";
            sPrm.Value = iParentProjectId;
            sPrms[4] = sPrm;

            sPrm = new SqlParameter();
            sPrm.SqlDbType = System.Data.SqlDbType.VarChar;
            sPrm.Direction = ParameterDirection.Input;
            sPrm.ParameterName = "@ExistingTaskIds";
            sPrm.Value = strExistingIds;
            sPrms[5] = sPrm;

            sPrm = new SqlParameter();
            sPrm.SqlDbType = System.Data.SqlDbType.Bit;
            sPrm.Direction = ParameterDirection.Input;
            sPrm.ParameterName = "@ProjectViewRight";
            sPrm.Value = bProjectViewRight;
            sPrms[6] = sPrm;


            return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetPerformancesTasksDropDownDS", sPrms);
        }

        public DataSet fnGetPreformanceServicesDataSet(DateTime dtFromDate, DateTime dtToDate, int iUserId, bool bProjectViewRight, bool bTaskViewRight)
        {
            try
            {
                SqlParameter[] sPrms = new SqlParameter[5];
                SqlParameter sPrm = new SqlParameter();
                int iIndex = 0;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@LoginUserId";
                sPrm.Value = iUserId;
                sPrms[iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.DateTime;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@FromDate";
                sPrm.Value = dtFromDate;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.DateTime;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ToDate";
                sPrm.Value = dtToDate;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ProjectViewRight";
                sPrm.Value = bProjectViewRight;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@TaskViewRight";
                sPrm.Value = bTaskViewRight;
                sPrms[++iIndex] = sPrm;


                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetPerformanceServiceDS", sPrms);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DataSet fnGetPreformanceExpensesDataSet(DateTime dtFromDate, DateTime dtToDate, int iUserId, bool bProjectViewRight, bool bTaskViewRight)
        {
            try
            {
                SqlParameter[] sPrms = new SqlParameter[5];
                SqlParameter sPrm = new SqlParameter();

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@LoginUserId";
                sPrm.Value = iUserId;
                sPrms[0] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.DateTime;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@FromDate";
                sPrm.Value = dtFromDate;
                sPrms[1] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.DateTime;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ToDate";
                sPrm.Value = dtToDate;
                sPrms[2] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ProjectViewRight";
                sPrm.Value = bProjectViewRight;
                sPrms[3] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@TaskViewRight";
                sPrm.Value = bTaskViewRight;
                sPrms[4] = sPrm;


                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetPerformanceExpensesDS", sPrms);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool fnSavePerformances(DataSet dsService, DataSet dsExpenses, bool bServiceModify, bool bExpensesModify, int iUserId)
        {
            int i = 0;

            long lngDate = 0;
            bool bTFlag = false;
            string strSql = "";
            try
            {

                // DataRow[] dr = dsService.Tables[0].Select("Flag = 'N'");

                if ((dsService.Tables[0].Rows.Count > 0) && (bServiceModify))
                {
                    for (i = 0; i <= dsService.Tables[0].Rows.Count - 1; i++)
                    {
                        lngDate = 0;
                        bTFlag = false;
                        strSql = "";
                        if ((dsService.Tables[0].Rows[i]["Flag"].ToString() == "N") && (dsService.Tables[0].Rows[i]["RecType"].ToString() != "0"))
                        {
                            strSql = "insert into ObjectServices (MeetingNo,ObjectType,ObjectId,ServiceDate,PerformedBy,";
                            strSql += "TaskServiceTypeId,TimeFrom,TimeUntil,Effort,ExternalEffort,HourlyRate,BillingAmount,RoundingDifference,Comment,Cleared,CreatedBy,CreatedOn) values";

                            strSql += "(null,";
                            if ((dsService.Tables[0].Rows[i]["ServiceTaskId"] != DBNull.Value) && (dsService.Tables[0].Rows[i]["ServiceTaskId"].ToString() != ""))
                                strSql += (int)Enums.VTSObjects.Task + "," + dsService.Tables[0].Rows[i]["ServiceTaskId"];
                            else
                                bTFlag = true;

                            if ((dsService.Tables[0].Rows[i]["ServiceProjectId"] != DBNull.Value) && (dsService.Tables[0].Rows[i]["ServiceProjectId"].ToString() != ""))
                            {
                                if (bTFlag)
                                    strSql += (int)Enums.VTSObjects.Project + "," + dsService.Tables[0].Rows[i]["ServiceProjectId"];
                            }
                            if (dsService.Tables[0].Rows[i]["ServiceDate"].ToString() != string.Empty)
                                lngDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(dsService.Tables[0].Rows[i]["ServiceDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);


                            strSql += ",cast(" + lngDate.ToString() + " as datetime)";

                            strSql += "," + dsService.Tables[0].Rows[i]["PerformedBy"].ToString();

                            if ((dsService.Tables[0].Rows[i]["TaskServiceTypeId"].ToString() == "") || (dsService.Tables[0].Rows[i]["TaskServiceTypeId"] == null) || (dsService.Tables[0].Rows[i]["TaskServiceTypeId"] == DBNull.Value))
                                strSql += ", null";
                            else
                                strSql += "," + dsService.Tables[0].Rows[i]["TaskServiceTypeId"];

                            strSql += ",'" + dsService.Tables[0].Rows[i]["TimeFrom"].ToString() + "'";
                            strSql += ",'" + dsService.Tables[0].Rows[i]["TimeUntil"].ToString() + "'";
                            strSql += ",'" + dsService.Tables[0].Rows[i]["Effort"].ToString() + "'";
                            strSql += ",'" + dsService.Tables[0].Rows[i]["ExternalEffort"].ToString() + "'";



                            if ((dsService.Tables[0].Rows[i]["HourlyRate"].ToString() == "") || (dsService.Tables[0].Rows[i]["HourlyRate"] == null) || (dsService.Tables[0].Rows[i]["HourlyRate"] == DBNull.Value))
                                strSql += ", null";
                            else
                                strSql += "," + dsService.Tables[0].Rows[i]["HourlyRate"];


                            if ((dsService.Tables[0].Rows[i]["BillingAmount"].ToString() == "") || (dsService.Tables[0].Rows[i]["BillingAmount"] == null) || (dsService.Tables[0].Rows[i]["BillingAmount"] == DBNull.Value))
                                strSql += ", null";
                            else
                                strSql += "," + dsService.Tables[0].Rows[i]["BillingAmount"];
                            if ((dsService.Tables[0].Rows[i]["RoundingDifference"].ToString() == "") || (dsService.Tables[0].Rows[i]["RoundingDifference"] == null) || (dsService.Tables[0].Rows[i]["RoundingDifference"] == DBNull.Value))
                                strSql += ", null";
                            else
                                strSql += "," + dsService.Tables[0].Rows[i]["RoundingDifference"];

                            if (dsService.Tables[0].Rows[i]["Comment"] == DBNull.Value)
                                strSql += ",null";
                            else
                                strSql += " ," + Common.EncodeNString(dsService.Tables[0].Rows[i]["Comment"].ToString());
                            strSql += ",0," + iUserId + ",getdate())";
                        }
                        else if ((dsService.Tables[0].Rows[i]["Flag"].ToString() == "M") && (dsService.Tables[0].Rows[i]["RecType"].ToString() != "0"))
                        {
                            strSql = "update ObjectServices set ";

                            if ((dsService.Tables[0].Rows[i]["ServiceTaskId"] != DBNull.Value) && (dsService.Tables[0].Rows[i]["ServiceTaskId"].ToString() != ""))
                                strSql += " ObjectType = " + (int)Enums.VTSObjects.Task + ",ObjectId=" + dsService.Tables[0].Rows[i]["ServiceTaskId"];
                            else
                                bTFlag = true;

                            if ((dsService.Tables[0].Rows[i]["ServiceProjectId"] != DBNull.Value) && (dsService.Tables[0].Rows[i]["ServiceProjectId"].ToString() != ""))
                            {
                                if (bTFlag)
                                    strSql += " ObjectType = " + (int)Enums.VTSObjects.Project + ",ObjectId=" + dsService.Tables[0].Rows[i]["ServiceProjectId"];
                            }

                            if (dsService.Tables[0].Rows[i]["ServiceDate"].ToString() != string.Empty)
                                lngDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(dsService.Tables[0].Rows[i]["ServiceDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);

                            strSql += ",ServiceDate=cast(" + lngDate.ToString() + " as datetime)";

                            strSql += ",PerformedBy=" + dsService.Tables[0].Rows[i]["PerformedBy"];
                            if ((dsService.Tables[0].Rows[i]["TaskServiceTypeId"].ToString() == "") || (dsService.Tables[0].Rows[i]["TaskServiceTypeId"] == null) || (dsService.Tables[0].Rows[i]["TaskServiceTypeId"] == DBNull.Value))
                                strSql += ",TaskServiceTypeId= null";
                            else
                                strSql += ",TaskServiceTypeId=" + dsService.Tables[0].Rows[i]["TaskServiceTypeId"];
                            strSql += ",TimeFrom='" + dsService.Tables[0].Rows[i]["TimeFrom"].ToString() + "'";
                            strSql += ",TimeUntil='" + dsService.Tables[0].Rows[i]["TimeUntil"].ToString() + "'";
                            strSql += ",Effort='" + dsService.Tables[0].Rows[i]["Effort"].ToString() + "'";
                            strSql += ",ExternalEffort='" + dsService.Tables[0].Rows[i]["ExternalEffort"].ToString() + "'";
                            if ((dsService.Tables[0].Rows[i]["HourlyRate"].ToString() == "") || (dsService.Tables[0].Rows[i]["HourlyRate"] == null) || (dsService.Tables[0].Rows[i]["HourlyRate"] == DBNull.Value))
                                strSql += ",HourlyRate= null";
                            else
                                strSql += ",HourlyRate=" + dsService.Tables[0].Rows[i]["HourlyRate"];
                            if ((dsService.Tables[0].Rows[i]["BillingAmount"].ToString() == "") || (dsService.Tables[0].Rows[i]["BillingAmount"] == null) || (dsService.Tables[0].Rows[i]["BillingAmount"] == DBNull.Value))
                                strSql += ",BillingAmount= null";
                            else
                                strSql += ",BillingAmount=" + dsService.Tables[0].Rows[i]["BillingAmount"];

                            if ((dsService.Tables[0].Rows[i]["RoundingDifference"].ToString() == "") || (dsService.Tables[0].Rows[i]["RoundingDifference"] == null) || (dsService.Tables[0].Rows[i]["RoundingDifference"] == DBNull.Value))
                                strSql += ",RoundingDifference= null";
                            else
                                strSql += ",RoundingDifference=" + dsService.Tables[0].Rows[i]["RoundingDifference"];



                            strSql += ",Comment=" + Common.EncodeNString(dsService.Tables[0].Rows[i]["Comment"].ToString());

                            strSql += " where recordid=" + dsService.Tables[0].Rows[i]["RecordId"];
                        }
                        else if ((dsService.Tables[0].Rows[i]["Flag"].ToString() == "D") && (dsService.Tables[0].Rows[i]["RecType"].ToString() != "0"))
                        {
                            if (dsService.Tables[0].Rows[i]["RecordId"].ToString() != "")
                            {
                                strSql = "delete from ObjectServices where recordid=" + int.Parse(dsService.Tables[0].Rows[i]["RecordId"].ToString());
                            }
                        }
                        if (strSql != "")
                            Common.dbMgr.ExecuteDataSet(CommandType.Text, strSql);
                    }
                }


                if ((dsExpenses.Tables[0].Rows.Count > 0) && (bExpensesModify))
                {
                    for (i = 0; i <= dsExpenses.Tables[0].Rows.Count - 1; i++)
                    {

                        bTFlag = false;
                        strSql = "";
                        if ((dsExpenses.Tables[0].Rows[i]["Flag"].ToString() == "N") && (dsExpenses.Tables[0].Rows[i]["RecType"].ToString() != "0"))
                        {
                            strSql = "insert into ObjectProducts (MeetingNo,ObjectType,ObjectId,ProductDate,PerformedBy,ProductId,";
                            strSql += " Quantity,ProductRate,PriceInternal,IsVATIncluded,VATPercent,IsBillable,AmountInternal,";
                            strSql += " AmountExternal,AmountExternalHidden,RoundingDifference,Comment,IsApproved,IsPayback,Cleared,CreatedBy,CreatedOn) values (null,";


                            if ((dsExpenses.Tables[0].Rows[i]["ProductTaskId"] != DBNull.Value) && (dsExpenses.Tables[0].Rows[i]["ProductTaskId"].ToString() != ""))
                                strSql += (int)Enums.VTSObjects.Task + "," + dsExpenses.Tables[0].Rows[i]["ProductTaskId"];
                            else
                                bTFlag = true;

                            if ((dsExpenses.Tables[0].Rows[i]["ProductProjectId"] != DBNull.Value) && (dsExpenses.Tables[0].Rows[i]["ProductProjectId"].ToString() != ""))
                            {
                                if (bTFlag)
                                    strSql += (int)Enums.VTSObjects.Project + "," + dsExpenses.Tables[0].Rows[i]["ProductProjectId"];
                            }
                            if (dsExpenses.Tables[0].Rows[i]["ProductDate"].ToString() != string.Empty)
                                lngDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(dsExpenses.Tables[0].Rows[i]["ProductDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);

                            strSql += ",cast(" + lngDate.ToString() + " as datetime)";

                            strSql += "," + dsExpenses.Tables[0].Rows[i]["PerformedBy"].ToString();

                            if ((dsExpenses.Tables[0].Rows[i]["ProductCode"].ToString() == "") || (dsExpenses.Tables[0].Rows[i]["ProductCode"] == null) || (dsExpenses.Tables[0].Rows[i]["ProductCode"] == DBNull.Value))
                                strSql += ", null";
                            else
                                strSql += ",'" + dsExpenses.Tables[0].Rows[i]["ProductCode"].ToString() + "'";



                            strSql += "," + dsExpenses.Tables[0].Rows[i]["Quantity"].ToString();

                            //for (j = 10; j < dsExpenses.Tables[0].Rows[i].Table.Columns.Count - 9; j++)
                            //{
                            //    strSql += " , " + dsExpenses.Tables[0].Rows[i][j];
                            //}

                            if ((dsExpenses.Tables[0].Rows[i]["ProductRate"].ToString() == "") || (dsExpenses.Tables[0].Rows[i]["ProductRate"] == null) || (dsExpenses.Tables[0].Rows[i]["ProductRate"] == DBNull.Value))
                                strSql += ", null";
                            else
                                strSql += "," + dsExpenses.Tables[0].Rows[i]["ProductRate"];

                            if ((dsExpenses.Tables[0].Rows[i]["PriceInternal"].ToString() == "") || (dsExpenses.Tables[0].Rows[i]["PriceInternal"] == null) || (dsExpenses.Tables[0].Rows[i]["ProductRate"] == DBNull.Value))
                                strSql += ", null";
                            else
                                strSql += "," + dsExpenses.Tables[0].Rows[i]["PriceInternal"];

                            if (Convert.ToBoolean(dsExpenses.Tables[0].Rows[i]["IsVATIncluded"].ToString()))
                                strSql += ",1";
                            else
                                strSql += " ,0 ";

                            if ((dsExpenses.Tables[0].Rows[i]["VATPercent"].ToString() == "") || (dsExpenses.Tables[0].Rows[i]["VATPercent"] == null) || (dsExpenses.Tables[0].Rows[i]["ProductRate"] == DBNull.Value))
                                strSql += ", null";
                            else
                                strSql += "," + dsExpenses.Tables[0].Rows[i]["VATPercent"];

                            if (Convert.ToBoolean(dsExpenses.Tables[0].Rows[i]["IsBillable"].ToString()))
                                strSql += ",1";
                            else
                                strSql += " ,0 ";

                            if ((dsExpenses.Tables[0].Rows[i]["AmountInternal"].ToString() == "") || (dsExpenses.Tables[0].Rows[i]["AmountInternal"] == null) || (dsExpenses.Tables[0].Rows[i]["AmountInternal"] == DBNull.Value))
                                strSql += ", null";
                            else
                                strSql += "," + dsExpenses.Tables[0].Rows[i]["AmountInternal"];
                            if ((dsExpenses.Tables[0].Rows[i]["AmountExternal"].ToString() == "") || (dsExpenses.Tables[0].Rows[i]["AmountExternal"] == null) || (dsExpenses.Tables[0].Rows[i]["AmountExternal"] == DBNull.Value))
                                strSql += ", null";
                            else
                                strSql += "," + dsExpenses.Tables[0].Rows[i]["AmountExternal"];

                            if ((dsExpenses.Tables[0].Rows[i]["AmountExternalHidden"].ToString() == "") || (dsExpenses.Tables[0].Rows[i]["AmountExternalHidden"] == null) || (dsExpenses.Tables[0].Rows[i]["AmountExternalHidden"] == DBNull.Value))
                                strSql += ", null";
                            else
                                strSql += "," + dsExpenses.Tables[0].Rows[i]["AmountExternalHidden"];

                            if ((dsExpenses.Tables[0].Rows[i]["RoundingDifference"].ToString() == "") || (dsExpenses.Tables[0].Rows[i]["RoundingDifference"] == null) || (dsExpenses.Tables[0].Rows[i]["RoundingDifference"] == DBNull.Value))
                                strSql += ", null";
                            else
                                strSql += "," + dsExpenses.Tables[0].Rows[i]["RoundingDifference"];



                            if (dsExpenses.Tables[0].Rows[i]["Comment"] == DBNull.Value)
                                strSql += ",null";
                            else
                                strSql += " ," + Common.EncodeNString(dsExpenses.Tables[0].Rows[i]["Comment"].ToString());

                            if (Convert.ToBoolean(dsExpenses.Tables[0].Rows[i]["IsApproved"].ToString()))
                                strSql += ",1";
                            else
                                strSql += " ,0 ";

                            if (Convert.ToBoolean(dsExpenses.Tables[0].Rows[i]["IsPayback"].ToString()))
                                strSql += ",1";
                            else
                                strSql += " ,0 ";


                            strSql += ",0," + iUserId + ",getdate())";
                        }
                        else if ((dsExpenses.Tables[0].Rows[i]["Flag"].ToString() == "M") && (dsExpenses.Tables[0].Rows[i]["RecType"].ToString() != "0"))
                        {
                            strSql = "update ObjectProducts set ";

                            if ((dsExpenses.Tables[0].Rows[i]["ProductTaskId"] != DBNull.Value) && (dsExpenses.Tables[0].Rows[i]["ProductTaskId"].ToString() != ""))
                                strSql += " ObjectType = " + (int)Enums.VTSObjects.Task + ",ObjectId=" + dsExpenses.Tables[0].Rows[i]["ProductTaskId"];
                            else
                                bTFlag = true;

                            if ((dsExpenses.Tables[0].Rows[i]["ProductProjectId"] != DBNull.Value) && (dsExpenses.Tables[0].Rows[i]["ProductProjectId"].ToString() != ""))
                            {
                                if (bTFlag)
                                    strSql += " ObjectType = " + (int)Enums.VTSObjects.Project + ",ObjectId=" + dsExpenses.Tables[0].Rows[i]["ProductProjectId"];
                            }

                            if (dsExpenses.Tables[0].Rows[i]["ProductDate"].ToString() != string.Empty)
                                lngDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(dsExpenses.Tables[0].Rows[i]["ProductDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);
                            strSql += ",ProductDate=cast(" + lngDate.ToString() + " as datetime)";

                            strSql += ",PerformedBy=" + dsExpenses.Tables[0].Rows[i]["PerformedBy"];
                            if ((dsExpenses.Tables[0].Rows[i]["ProductCode"].ToString() == "") || (dsExpenses.Tables[0].Rows[i]["ProductCode"] == null) || (dsExpenses.Tables[0].Rows[i]["ProductCode"] == DBNull.Value))
                                strSql += ",ProductId=null";
                            else
                                strSql += ",ProductId=" + dsExpenses.Tables[0].Rows[i]["ProductCode"];
                            strSql += ",Quantity=" + dsExpenses.Tables[0].Rows[i]["Quantity"];
                            if ((dsExpenses.Tables[0].Rows[i]["ProductRate"].ToString() == "") || (dsExpenses.Tables[0].Rows[i]["ProductRate"] == null) || (dsExpenses.Tables[0].Rows[i]["ProductRate"] == DBNull.Value))
                                strSql += ",ProductRate=null";
                            else
                                strSql += ",ProductRate=" + dsExpenses.Tables[0].Rows[i]["ProductRate"];

                            if ((dsExpenses.Tables[0].Rows[i]["PriceInternal"].ToString() == "") || (dsExpenses.Tables[0].Rows[i]["PriceInternal"] == null) || (dsExpenses.Tables[0].Rows[i]["ProductRate"] == DBNull.Value))
                                strSql += ",PriceInternal= null";
                            else
                                strSql += ",PriceInternal=" + dsExpenses.Tables[0].Rows[i]["PriceInternal"];

                            if (Convert.ToBoolean(dsExpenses.Tables[0].Rows[i]["IsVATIncluded"].ToString()))
                                strSql += ",IsVATIncluded=1";
                            else
                                strSql += " ,IsVATIncluded=0 ";

                            if ((dsExpenses.Tables[0].Rows[i]["VATPercent"].ToString() == "") || (dsExpenses.Tables[0].Rows[i]["VATPercent"] == null) || (dsExpenses.Tables[0].Rows[i]["VATPercent"] == DBNull.Value))
                                strSql += ",VATPercent=null";
                            else
                                strSql += ",VATPercent=" + dsExpenses.Tables[0].Rows[i]["VATPercent"];

                            if (Convert.ToBoolean(dsExpenses.Tables[0].Rows[i]["IsBillable"].ToString()))
                                strSql += ",IsBillable=1";
                            else
                                strSql += " ,IsBillable=0 ";

                            if ((dsExpenses.Tables[0].Rows[i]["AmountInternal"].ToString() == "") || (dsExpenses.Tables[0].Rows[i]["AmountInternal"] == null) || (dsExpenses.Tables[0].Rows[i]["AmountInternal"] == DBNull.Value))
                                strSql += ",AmountInternal=null";
                            else
                                strSql += ",AmountInternal=" + dsExpenses.Tables[0].Rows[i]["AmountInternal"];

                            if ((dsExpenses.Tables[0].Rows[i]["AmountExternal"].ToString() == "") || (dsExpenses.Tables[0].Rows[i]["AmountExternal"] == null) || (dsExpenses.Tables[0].Rows[i]["AmountExternal"] == DBNull.Value))
                                strSql += ",AmountExternal=null";
                            else
                                strSql += ",AmountExternal=" + dsExpenses.Tables[0].Rows[i]["AmountExternal"];

                            if ((dsExpenses.Tables[0].Rows[i]["AmountExternalHidden"].ToString() == "") || (dsExpenses.Tables[0].Rows[i]["AmountExternalHidden"] == null) || (dsExpenses.Tables[0].Rows[i]["AmountExternalHidden"] == DBNull.Value))
                                strSql += ",AmountExternalHidden=null";
                            else
                                strSql += ",AmountExternalHidden=" + dsExpenses.Tables[0].Rows[i]["AmountExternalHidden"];

                            if ((dsExpenses.Tables[0].Rows[i]["RoundingDifference"].ToString() == "") || (dsExpenses.Tables[0].Rows[i]["RoundingDifference"] == null) || (dsExpenses.Tables[0].Rows[i]["RoundingDifference"] == DBNull.Value))
                                strSql += ",RoundingDifference=null";
                            else
                                strSql += ",RoundingDifference=" + dsExpenses.Tables[0].Rows[i]["RoundingDifference"];


                            strSql += ",Comment=" + Common.EncodeNString(dsExpenses.Tables[0].Rows[i]["Comment"].ToString());

                            if (Convert.ToBoolean(dsExpenses.Tables[0].Rows[i]["IsApproved"].ToString()))
                                strSql += ",IsApproved=1";
                            else
                                strSql += " ,IsApproved=0 ";

                            if (Convert.ToBoolean(dsExpenses.Tables[0].Rows[i]["IsPayback"].ToString()))
                                strSql += ",IsPayback=1";
                            else
                                strSql += " ,IsPayback=0 ";


                            strSql += " where recordid=" + dsExpenses.Tables[0].Rows[i]["RecordId"];
                        }
                        else if ((dsExpenses.Tables[0].Rows[i]["Flag"].ToString() == "D") && (dsExpenses.Tables[0].Rows[i]["RecType"].ToString() != "0"))
                        {
                            if (dsExpenses.Tables[0].Rows[i]["RecordId"].ToString() != "")
                            {
                                strSql = "delete from ObjectProducts where recordid=" + int.Parse(dsExpenses.Tables[0].Rows[i]["RecordId"].ToString());
                            }
                        }
                        if (strSql != "")
                            Common.dbMgr.ExecuteDataSet(CommandType.Text, strSql);
                    }
                }


                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


    }
}
