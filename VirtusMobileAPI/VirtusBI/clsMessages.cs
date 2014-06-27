using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace VirtusBI
{
    public class clsMessages : clsBaseBI
    {
        public bool fnSetReadMessage(int iMessageId, string strUserName, int iRead)
        {
            try
            {

                SqlParameter[] sPrms = new SqlParameter[3];
                SqlParameter sPrm = new SqlParameter();

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@MessageId";
                sPrm.Value = iMessageId;
                sPrms[0] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@LoginName";
                sPrm.Value = strUserName;
                sPrms[1] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@IsRead";
                sPrm.Value = iRead;
                sPrms[2] = sPrm;


                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spSetMessageReadStatus", sPrms);

                //string strSql = "update MessageRecipients set IsRead=" + iRead + " where Messageid=" + iMessageId + " and LoginName= " + Common.EncodeString(strUserName);
                //Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                CheckMessageReminderStatus(iMessageId, strUserName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public DataSet fnGetUsers(string strLoginName, bool bIsForScanedDocs, string strSearchCondition, int iNoOfRecords, string strOrderBy, string strSortOrder, int iTotCount)
        {
            SqlParameter[] sPrms = new SqlParameter[7];
            SqlParameter sPrm = new SqlParameter();
            try
            {
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@LoginName";
                sPrm.Value = strLoginName;
                sPrms[0] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@IsForScanDocs";
                sPrm.Value = bIsForScanedDocs;
                sPrms[1] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@SearchCondition";
                sPrm.Value = strSearchCondition;
                sPrms[2] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@NoTopRecords";
                sPrm.Value = iNoOfRecords;
                sPrms[3] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.VarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@OrderBy";
                sPrm.Value = strOrderBy;
                sPrms[4] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.VarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@SortOrder";
                sPrm.Value = strSortOrder;
                sPrms[5] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Output;
                sPrm.ParameterName = "@TotCount";
                sPrms[6] = sPrm;

                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetUsersForMessagesDS", sPrms);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void fnGetReplyAllNames(int iMessageId, ref string strPersons, ref string strOUs, bool bAddFromUser, int iFromUserID)
        {
            strPersons = "";
            strOUs = "";
            DataSet ds;

            try
            {
                SqlParameter[] sPrms = new SqlParameter[1];
                SqlParameter sPrm = new SqlParameter();

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@MessageId";
                sPrm.Value = iMessageId;
                sPrms[0] = sPrm;

                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetMessageReceipentNames", sPrms);

                if (ds.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        if (ds.Tables[0].Rows[i]["AddressType"].ToString() == "P")
                        {
                            if (bAddFromUser)
                            {
                                if (iFromUserID != int.Parse(ds.Tables[0].Rows[i][1].ToString()))
                                {
                                    if (strPersons != "")
                                        strPersons += ",";

                                    strPersons += ds.Tables[0].Rows[i][1];
                                }
                            }
                            else
                            {
                                if (strPersons != "")
                                    strPersons += ",";

                                strPersons += ds.Tables[0].Rows[i][1];
                            }
                        }
                        else
                        {
                            if (strOUs != "")
                                strOUs += ",";
                            strOUs += ds.Tables[0].Rows[i][1];
                        }
                    }
                }


                //spCheckEPMADMINMessageStatus
                sPrms = new SqlParameter[2];
                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@MessageId";
                sPrm.Value = iMessageId;
                sPrms[0] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Output;
                sPrm.ParameterName = "@Status";
                sPrms[1] = sPrm;

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spCheckEPMADMINMessageStatus", sPrms);
                if (Convert.ToBoolean(sPrms[1].Value))
                {
                    if (strPersons != "")
                        strPersons += ",";
                    strPersons += "0";
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public DataSet fnGetReplyAllIds(int iMessageId, bool bAddFromUser, int iFromUserID)
        {

            DataSet ds;

            try
            {
                SqlParameter[] sPrms = new SqlParameter[1];
                SqlParameter sPrm = new SqlParameter();

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@MessageId";
                sPrm.Value = iMessageId;
                sPrms[0] = sPrm;

                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetMessageReceipentNames", sPrms);


                return ds;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataSet fnGetInternalOrganisations(bool bIsParentRequired)
        {
            try
            {
                SqlParameter[] sPrms = new SqlParameter[1];
                SqlParameter sPrm = new SqlParameter();

                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@IsParentRequired";
                sPrm.Value = bIsParentRequired;
                sPrms[0] = sPrm;

                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetInternalOrganizations", sPrms);
            }
            catch (Exception ex)
            {

                throw ex;
            }


            //string strSql = "";


            //if (bUseCommittee)
            //{
            //    strSql = " Select AddresseId,Name";
            //    strSql += " from Addresses where UseCommitteeMP=1 and AddressType='O' and IsActive=1";
            //}
            //else
            //{
            //    strSql = " Select A.AddresseId,A.Name,C.AddresseId As ParentId";
            //    strSql += " From Addresses A ";
            //    strSql += " Left Join Addresses C on A.ParentOrganizationId=C.AddresseId ";
            //    strSql += " Where A.IsInternalOrganization=1 and A.IsActive=1";
            //}

            //strSql += " Order By Name";

            //return Common.dbMgr.ExecuteDataSet(CommandType.Text, strSql);
        }

        private void CheckMessageReminderStatus(int iMessageId, string strUserName)
        {
            try
            {
                SqlParameter[] sPrms = new SqlParameter[2];
                SqlParameter sPrm = new SqlParameter();

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@MessageId";
                sPrm.Value = iMessageId;
                sPrms[0] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@LoginName";
                sPrm.Value = strUserName;
                sPrms[1] = sPrm;

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spCheckMessageReminderStatus", sPrms);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet fnGetInboxData(string strLoginUserName, bool bUnDone, int iLanguageID, bool bProjectViewRight, bool bTaskViewRight, bool bDecisionViewRight, bool bAgendaItemViewRight, bool bMeetingViewRight, bool bLinkFolderViewRight, bool bQueryFolderViewRight, bool bIsAllMessages)
        {
            int iUnDone = 0;
            if (bUnDone)
                iUnDone = 1;

            DataSet ds = new DataSet();
            try
            {

                SqlParameter[] sPrms = new SqlParameter[11];
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
                sPrm.ParameterName = "@UILanguageId";
                sPrm.Value = iLanguageID;
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

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@DecisionViewRight";
                sPrm.Value = bDecisionViewRight;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@AgendaItemViewRight";
                sPrm.Value = bAgendaItemViewRight;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@MeetingViewRight";
                sPrm.Value = bMeetingViewRight;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@LinkFolderViewRight";
                sPrm.Value = bLinkFolderViewRight;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@QueryFolderViewRight ";
                sPrm.Value = bQueryFolderViewRight;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@IsAllMessages";
                sPrm.Value = bIsAllMessages;
                sPrms[++iIndex] = sPrm;


                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetMessagesInboxDS", sPrms);
                fnUpdateDatasetwithRecepientName(ref  ds);

                return ds;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataSet fnGetSentItemsData(string strLoginUserName, int iLanguageID, bool bProjectViewRight, bool bTaskViewRight, bool bDecisionViewRight, bool bAgendaItemViewRight, bool bMeetingViewRight, bool bLinkFolderViewRight, bool bQueryFolderViewRight)
        {
            DataSet ds = new DataSet();
            try
            {

                SqlParameter[] sPrms = new SqlParameter[9];
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
                sPrm.ParameterName = "@UILanguageId";
                sPrm.Value = iLanguageID;
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

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@DecisionViewRight";
                sPrm.Value = bDecisionViewRight;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@AgendaItemViewRight";
                sPrm.Value = bAgendaItemViewRight;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@MeetingViewRight";
                sPrm.Value = bMeetingViewRight;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@LinkFolderViewRight";
                sPrm.Value = bLinkFolderViewRight;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@QueryFolderViewRight ";
                sPrm.Value = bQueryFolderViewRight;
                sPrms[++iIndex] = sPrm;


                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetMessagesSentItemsDS", sPrms);
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
                    if (ds.Tables[0].Rows[i]["RecepientName"].ToString() == "")
                        ds.Tables[0].Rows[i]["RecepientName"] = fnGetReceipentsName(int.Parse(ds.Tables[0].Rows[i]["MessageId"].ToString()), ref strToUsers, ref strToOUs);
                    ds.Tables[0].Rows[i]["ToUserIDs"] = strToUsers;
                    ds.Tables[0].Rows[i]["ToOUs"] = strToOUs;

                }
            }
        }

        public string fnGetReceipentsName(int iMessageId, ref string strToUserIds, ref string strToOUs)
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
                sPrm.ParameterName = "@MessageId";
                sPrm.Value = iMessageId;
                sPrms[0] = sPrm;

                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetMessageReceipentNames", sPrms);

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

        public int fnGetUnReadMessagesCount(string strLoginUserName, int iLanguageID, bool bProjectViewRight, bool bTaskViewRight, bool bDecisionViewRight, bool bAgendaItemViewRight, bool bMeetingViewRight, bool bLinkFolderViewRight, bool bQueryFolderViewRight)
        {
            try
            {
                SqlParameter[] sPrms = new SqlParameter[11];
                SqlParameter sPrm = new SqlParameter();
                int iIndex = 0;


                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@LoginName";
                sPrm.Value = strLoginUserName;
                sPrms[iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@UILanguageID";
                //sPrm.Value = iLanguageID;
                sPrm.Value = CommonVariable.iLanguageId;
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

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@DecisionViewRight";
                sPrm.Value = bDecisionViewRight;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@AgendaItemViewRight";
                sPrm.Value = bAgendaItemViewRight;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@MeetingViewRight";
                sPrm.Value = bMeetingViewRight;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@LinkFolderViewRight";
                sPrm.Value = bLinkFolderViewRight;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@QueryFolderViewRight ";
                sPrm.Value = bQueryFolderViewRight;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@DataArchivesViewRight";
                sPrm.Value = true;
                sPrms[++iIndex] = sPrm;


                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Output;
                sPrm.ParameterName = "@MessageCount";
                sPrms[++iIndex] = sPrm;

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spGetUnReadMessagesCount", sPrms);
                return int.Parse(sPrms[10].Value.ToString());

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataSet fnGetMessageDetails(int iMessageId, string strLoginName)
        {
            try
            {
                SqlParameter[] sPrms = new SqlParameter[2];
                SqlParameter sPrm = new SqlParameter();

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@MessageId";
                sPrm.Value = iMessageId;
                sPrms[0] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@LoginName";
                sPrm.Value = strLoginName;
                sPrms[1] = sPrm;


                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetMessageDetailsDS", sPrms);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void fnCheckObjectStatus(int iObjectTypeId, int iObjectID, ref bool iISDeleted, ref bool iIsActive, ref bool iIsArchived)
        {
            try
            {
                SqlParameter[] sPrms = new SqlParameter[5];
                SqlParameter sPrm = new SqlParameter();

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ObjectTypeId";
                sPrm.Value = iObjectTypeId;
                sPrms[0] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ObjectId";
                sPrm.Value = iObjectID;
                sPrms[1] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Output;
                sPrm.ParameterName = "@iIsActive";
                sPrm.Value = iIsActive;
                sPrms[2] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Output;
                sPrm.ParameterName = "@IsDeleted";
                sPrm.Value = iISDeleted;  // while getting count of records it should be 0, while updating status it should be 1
                sPrms[3] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Output;
                sPrm.ParameterName = "@IsArchived ";
                sPrm.Value = iIsArchived;
                sPrms[4] = sPrm;


                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spCheckObjectsStatus", sPrms);


                iIsActive = Convert.ToBoolean(sPrms[2].Value.ToString());
                iISDeleted = Convert.ToBoolean(sPrms[3].Value.ToString());
                iIsArchived = Convert.ToBoolean(sPrms[4].Value.ToString());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool fnInsertMessages(int IMPriorityId, string strUserName, string strToPersons, string strToOU, string strSubject, string strMessage, int iSendEmail, int iSendSMS, int iObjTypeId, int iObjId, int iMessageId, ref bool bRefresh, bool bIsReplyToAll, int iLinkFileId, int iLanguageID)
        {

            try
            {

                SqlParameter[] Params = new SqlParameter[15];
                int iIndex = 0;

                Params[iIndex] = new SqlParameter("@PriorityId", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = IMPriorityId;

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

                Params[++iIndex] = new SqlParameter("@Message", SqlDbType.NVarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = strMessage;

                Params[++iIndex] = new SqlParameter("@SendEmail", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iSendEmail;

                Params[++iIndex] = new SqlParameter("@SendSMS", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iSendSMS;

                Params[++iIndex] = new SqlParameter("@ObjectTypeId", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iObjTypeId;

                Params[++iIndex] = new SqlParameter("@ObjectId", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iObjId;

                Params[++iIndex] = new SqlParameter("@MessageId", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iMessageId;

                Params[++iIndex] = new SqlParameter("@IsReplyToAll", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bIsReplyToAll;

                Params[++iIndex] = new SqlParameter("@LinkFileId", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iLinkFileId;

                Params[++iIndex] = new SqlParameter("@UILanguageID", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                //Params[iIndex].Value = iLanguageID;
                Params[iIndex].Value = CommonVariable.iLanguageId;

                Params[++iIndex] = new SqlParameter("@IsRefresh", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Output;

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spInsertMessage", Params);
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
