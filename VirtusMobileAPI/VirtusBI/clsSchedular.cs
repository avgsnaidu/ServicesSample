using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace VirtusBI
{
  public  class clsSchedular
    {


        public DataSet fnGetApplointments(string strPersons, string strOUs, string strObjects, DateTime dtFromDate, DateTime dtToDate, 
            int iLanguageID, bool bFromTimeline, bool bProjectViewRight, bool bTaskViewRight, bool bDecisionViewRight, bool bMeetingViewRight, 
            bool bMyAppointmentViewRight, string strLoginName, bool bIsShowOpenObejcts)
        {
            try
            {

                SqlParameter[] Params = new SqlParameter[15];
                int iIndex = 0;

                Params[iIndex] = new SqlParameter("@Persons", SqlDbType.VarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = strPersons;

                Params[++iIndex] = new SqlParameter("@OUs", SqlDbType.VarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = strOUs;

                Params[++iIndex] = new SqlParameter("@Objects", SqlDbType.VarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = strObjects;

                Params[++iIndex] = new SqlParameter("@FromDate", SqlDbType.DateTime);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = dtFromDate;

                Params[++iIndex] = new SqlParameter("@ToDate", SqlDbType.DateTime);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = dtToDate;

                Params[++iIndex] = new SqlParameter("@LanguageID", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iLanguageID;

                Params[++iIndex] = new SqlParameter("@FromTimeline", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bFromTimeline;

                Params[++iIndex] = new SqlParameter("@ProjectViewRight", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bProjectViewRight;

                Params[++iIndex] = new SqlParameter("@TaskViewRight", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bTaskViewRight;

                Params[++iIndex] = new SqlParameter("@DecisionViewRight", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bDecisionViewRight;

                Params[++iIndex] = new SqlParameter("@MeetingViewRight", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bMeetingViewRight;

                Params[++iIndex] = new SqlParameter("@MyAppointmentViewRight", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bMyAppointmentViewRight;

                Params[++iIndex] = new SqlParameter("@LoginName", SqlDbType.NVarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = strLoginName;

                Params[++iIndex] = new SqlParameter("@IsShowOpenObejcts", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bIsShowOpenObejcts;

                Params[++iIndex] = new SqlParameter("@IsShowSubtituteUsers", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = true;

                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetAppointments", Params);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet fnGetPersonNames(string strPersons)
        {
            try
            {
                SqlParameter[] sPrms = new SqlParameter[1];
                SqlParameter sPrm = new SqlParameter();

                sPrm.SqlDbType = System.Data.SqlDbType.VarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@PersonsIds";
                sPrm.Value = strPersons;
                sPrms[0] = sPrm;

                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetPersonNames", sPrms);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet fnGetCommitteesForNewMeetings()
        {
            try
            {
                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetCommitteesForNewMeetings");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        
    }
}
