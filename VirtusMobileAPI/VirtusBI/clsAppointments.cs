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
    public class clsAppointments:clsBaseBI
    {
        public DataSet fnGetAppointments(string strLoginUserName, bool bUnDone, bool bIsAllAppointments, int iPeriod)
        {
            int iUnDone = 0;
            if (bUnDone)
                iUnDone = 1;

            DataSet ds = new DataSet();
            try
            {

                SqlParameter[] sPrms = new SqlParameter[5];
                SqlParameter sPrm = new SqlParameter();
                int iIndex = 0;

                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Size = 50;
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
                sPrm.ParameterName = "@UILanguageId";
                sPrm.Value = CommonVariable.iLanguageId;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@IsAllAppointment";
                sPrm.Value = bIsAllAppointments;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@Period";
                sPrm.Value = iPeriod;
                sPrms[++iIndex] = sPrm;


                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetMyAppointmentsDS", sPrms);
                fnUpdateDatasetwithRecepientName(ref  ds);

                return ds;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        private void fnUpdateDatasetwithRecepientName(ref DataSet ds)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string strToUsers = "";
                    string strToOUs = "";
                    if (ds.Tables[0].Rows[i]["Participants"].ToString() == "")
                        ds.Tables[0].Rows[i]["Participants"] = fnGetParticipentsName(int.Parse(ds.Tables[0].Rows[i]["AppointmentId"].ToString()), ref strToUsers, ref strToOUs);
                    ds.Tables[0].Rows[i]["ToUserIDs"] = strToUsers;
                    ds.Tables[0].Rows[i]["ToOUs"] = strToOUs;

                }
            }
        }
        
        public string fnGetParticipentsName(int iAppointmentId, ref string strToUserIds, ref string strToOUs)
        {
            string strName = "";
            strToUserIds = "";
            strToOUs = "";
            try
            {
                DataSet ds;
                SqlParameter[] sPrms = new SqlParameter[1];
                SqlParameter sPrm = new SqlParameter();

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@AppointmentId";
                sPrm.Value = iAppointmentId;
                sPrms[0] = sPrm;

                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "[spGetMyAppointmentReceipentNames]", sPrms);

                if (ds.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (strName != "")
                            strName += ";";
                        strName += ds.Tables[0].Rows[i][0];
                        if (ds.Tables[0].Rows[i]["AddressType"].ToString() == "P")
                        {
                            if (strToUserIds != "")
                                strToUserIds += ",";
                            strToUserIds += ds.Tables[0].Rows[i][1];
                        }
                        else
                        {
                            if (strToOUs != "")
                                strToOUs += ",";
                            strToOUs += ds.Tables[0].Rows[i][1];
                        }
                    }
                }
                else
                {
                    strName = "epmadmin";
                    strToUserIds = "-100";
                }
                return strName.Trim();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet fnGetUsers(string strSearchCondition, int iNoOfRecords, string strOrderBy, string strSortOrder, int iTotCount)
        {
            SqlParameter[] sPrms = new SqlParameter[5];
            SqlParameter sPrm = new SqlParameter();
            try
            {
                int iIndex = 0;

                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@SearchCondition";
                sPrm.Value = strSearchCondition;
                sPrms[iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@NoTopRecords";
                sPrm.Value = iNoOfRecords;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.VarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@OrderBy";
                sPrm.Value = strOrderBy;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.VarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@SortOrder";
                sPrm.Value = strSortOrder;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Output;
                sPrm.ParameterName = "@TotCount";
                sPrms[++iIndex] = sPrm;

                DataSet ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetUsersForAppointmentsDS", sPrms);
                iTotCount = int.Parse(sPrms[iIndex].Value.ToString());

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }


            //return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetUsersForAppointmentsDS");
        }
                
        public DataSet fnGetAppointmentDetails(int iAppointmentId, string strUserName)
        {
            try
            {
                SqlParameter[] sPrms = new SqlParameter[2];
                SqlParameter sPrm = new SqlParameter();

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@AppointmentId";
                sPrm.Value = iAppointmentId;
                sPrms[0] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@LoginName";
                sPrm.Value = strUserName;
                sPrms[1] = sPrm;


                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetMyAppointmentDetailsDS", sPrms);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
         
        public DataSet fnGetAppOccurrenceDetails(int iOccurrSeriesId)
        {
            try
            {
                SqlParameter[] sPrms = new SqlParameter[1];
                SqlParameter sPrm = new SqlParameter();

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@OccurrSeriesId";
                sPrm.Value = iOccurrSeriesId;
                sPrms[0] = sPrm;


                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetAppOccurrenceDetails", sPrms);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool fnSaveAppointment(int IPriorityId, string strUserName, string strToPersons, string strToOU, string strSubject, DateTime dtAppDate, DateTime dtAppFrom, DateTime dtAppUntill, string strLocation,
        string strMessage, int iCanotModify, int iPrivateAppointment, int iAppointmentId, int iReminder, int iReminderDuration, int iIsDone, bool bIsReOccurr, int iOccurrSeriesId, bool bIsOpenSeries,
        string strCreatedBy, DateTime dtCreatedOn, bool bIsAllDay, string strEntryId, DateTime dtLastModificationTime, ref bool bRefresh)
        {

            try
            {
                DateTime SQLServerZeroDate = new DateTime(1900, 1, 1);

                string sRemindPeriodType = "dd";
                int iRemindPeriodValue = 0;
                Common.GetRemindPeriod((Enums.Remind_Durations)iReminderDuration, ref sRemindPeriodType, ref iRemindPeriodValue);

                long lngAppEndDate = 0;
                if ((bIsReOccurr) || (iOccurrSeriesId != 0))
                    lngAppEndDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, dtAppDate, FirstDayOfWeek.System, FirstWeekOfYear.System);
                else
                    lngAppEndDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, dtAppUntill, FirstDayOfWeek.System, FirstWeekOfYear.System);

                long lngAppDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, dtAppDate, FirstDayOfWeek.System, FirstWeekOfYear.System);
                long lngCreatedOn = 0;
                if (dtCreatedOn != SQLServerZeroDate)
                    lngCreatedOn = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, dtCreatedOn, FirstDayOfWeek.System, FirstWeekOfYear.System);

                long lngLastModificationTime = 0;
                if (dtLastModificationTime != SQLServerZeroDate)
                    lngLastModificationTime = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, dtLastModificationTime, FirstDayOfWeek.System, FirstWeekOfYear.System);


                if (bIsAllDay)
                {
                    dtAppFrom = new DateTime(dtAppDate.Year, dtAppDate.Month, dtAppDate.Day);
                    dtAppUntill = new DateTime(dtAppDate.Year, dtAppDate.Month, dtAppDate.Day);
                }

                SqlParameter[] Params = new SqlParameter[28];
                int iIndex = 0;

                Params[iIndex] = new SqlParameter("@AppointmentId", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iAppointmentId;

                Params[++iIndex] = new SqlParameter("@PriorityId", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = IPriorityId;

                Params[++iIndex] = new SqlParameter("@UserName", SqlDbType.NVarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = strUserName;

                Params[++iIndex] = new SqlParameter("@ToPersons", SqlDbType.VarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = strToPersons.Replace(';', ',').ToString();

                Params[++iIndex] = new SqlParameter("@ToOU", SqlDbType.VarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = strToOU.Replace(';', ',').ToString();

                Params[++iIndex] = new SqlParameter("@Subject", SqlDbType.NVarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = strSubject;

                Params[++iIndex] = new SqlParameter("@Location", SqlDbType.NVarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = strLocation;

                Params[++iIndex] = new SqlParameter("@Message", SqlDbType.NVarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = strMessage;

                Params[++iIndex] = new SqlParameter("@CanotModify", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iCanotModify;

                Params[++iIndex] = new SqlParameter("@PrivateAppointment", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iPrivateAppointment;

                Params[++iIndex] = new SqlParameter("@AppDate", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = lngAppDate;

                Params[++iIndex] = new SqlParameter("@AppEndDate", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = lngAppEndDate;

                Params[++iIndex] = new SqlParameter("@AppFrom", SqlDbType.DateTime);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = dtAppFrom;

                Params[++iIndex] = new SqlParameter("@AppUntill", SqlDbType.DateTime);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = dtAppUntill;

                Params[++iIndex] = new SqlParameter("@Reminder", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iReminder;

                Params[++iIndex] = new SqlParameter("@ReminderDuration", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iReminderDuration;

                Params[++iIndex] = new SqlParameter("@RemindPeriodType", SqlDbType.NVarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = sRemindPeriodType;

                Params[++iIndex] = new SqlParameter("@RemindPeriodValue", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iRemindPeriodValue;

                Params[++iIndex] = new SqlParameter("@Done", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iIsDone;

                Params[++iIndex] = new SqlParameter("@IsReOccurr", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bIsReOccurr;

                Params[++iIndex] = new SqlParameter("@OccurrSeriesId", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iOccurrSeriesId;

                Params[++iIndex] = new SqlParameter("@IsOpenSeries", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bIsOpenSeries;

                Params[++iIndex] = new SqlParameter("@CreatedBy", SqlDbType.NVarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = strCreatedBy;

                Params[++iIndex] = new SqlParameter("@CreatedOn", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = lngCreatedOn;

                Params[++iIndex] = new SqlParameter("@IsAllDay", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bIsAllDay;

                Params[++iIndex] = new SqlParameter("@OutlookEntryId", SqlDbType.NVarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = strEntryId;

                Params[++iIndex] = new SqlParameter("@LastModiTime", SqlDbType.DateTime);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = dtLastModificationTime;

                Params[++iIndex] = new SqlParameter("@IsRefresh", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Output;

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spSaveAppointment", Params);
                bRefresh = Convert.ToBoolean(Params[iIndex].Value);

                return true;
            }
            catch (Exception ex)
            {

                throw (ex);
            }

        }



    }
}
