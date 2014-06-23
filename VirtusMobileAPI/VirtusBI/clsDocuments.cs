using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using EPMEnums;
using Microsoft.VisualBasic;

namespace VirtusBI
{
    public class clsDocuments : clsBaseBI
    {
        public DataSet fnGetFileDeatils(int iFileId)
        {
            try
            {
                string strSql = "Select FV.FileID,max(FV.VersionNumber) as VersionNumber ,Password,FName,IsCheckedOut ";
                strSql += " from Files F Inner Join fileversions FV on F.FileId=FV.FileId Where FV.FileID=" + iFileId;
                strSql += " group by FV.FileID,Password,FName,IsCheckedOut";

                return Common.dbMgr.ExecuteDataSet(CommandType.Text, strSql);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetDocumentTypeList()
        {
            try
            {
                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetDocumentTyepListDs");
            }
            catch (Exception ex)
            { throw ex; }
        }


        public DataSet GetDocDataset(Enums.VTSObjects objType, int iRecordId, bool bShowFromSubObjects, int iUILanguageId, bool bProjectViewRight, bool bTaskViewRight, bool bDecisionViewRight, bool bAgendaItemViewRight, bool bMeetingViewRight, string strLoginName)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sPrms = new SqlParameter[9];
                SqlParameter sPrm = new SqlParameter();
                int iIndex = 0;


                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ObjectTypeId";
                sPrm.Value = (int)objType;
                sPrms[iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ObjectId";
                sPrm.Value = iRecordId;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@UILanguageId";
                sPrm.Value = iUILanguageId;
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
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@LoginName";
                sPrm.Value = strLoginName;
                sPrms[++iIndex] = sPrm;

                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetDocTabDS", sPrms);

                if (bShowFromSubObjects)
                {

                    DataSet dsSub = GetSubObjectsDataset(objType, iRecordId, iUILanguageId, bProjectViewRight, bTaskViewRight, bDecisionViewRight, bAgendaItemViewRight, bMeetingViewRight, strLoginName);
                    if (dsSub != null)
                    {
                        ds.Tables[0].Merge(dsSub.Tables[0]);
                    }
                }

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["Password"] != null && dr["Password"].ToString() != "")
                    {
                        dr["Password"] = Common.GetDecryptedValue(dr["Password"].ToString());
                    }
                    if (dr["DocumentArchiveTitleIds"] != DBNull.Value && dr["DocumentArchiveTitleIds"].ToString() != "")
                    {
                        dr["Signature"] = string.Empty;
                        //dr["Signature"] = fnSetSignature(dr["DocumentArchiveTitleIds"].ToString());
                    }
                }

                return ds;
            }
            catch (Exception ex)
            { throw ex; }
        }


        private DataSet GetSubObjectsDataset(Enums.VTSObjects objType, int iRecordId, int iUILanguageId, bool bProjectViewRight, bool bTaskViewRight, bool bDecisionViewRight, bool bAgendaItemViewRight, bool bMeetingViewRight, string strLoginName)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sPrms = new SqlParameter[9];
                SqlParameter sPrm = new SqlParameter();
                int iIndex = 0;

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ObjectTypeId";
                sPrm.Value = (int)objType;
                sPrms[iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ObjectId";
                sPrm.Value = iRecordId;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@UILanguageId";
                sPrm.Value = iUILanguageId;
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
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@LoginName";
                sPrm.Value = strLoginName;
                sPrms[++iIndex] = sPrm;


                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetDocSubObjectsDS", sPrms);
                return ds;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public DataSet GetListViewDataSet(string sWhereCondition, string strSearchCondition, int iNoOfRecords, string strOrderBy, string strSortOrder, int iUILanguageId, ref int iTotCount, bool bProjectViewRight, bool bTaskViewRight, bool bDecisionViewRight, bool bAgendaItemViewRight, bool bMeetingViewRight, string strLoginName, bool bIsDeletedListview)
        {
            try
            {
                iTotCount = 0;
                SqlParameter[] Params = new SqlParameter[13];
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

                Params[++iIndex] = new SqlParameter("@UILanguageId", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iUILanguageId;

                Params[++iIndex] = new SqlParameter("@LoginName", SqlDbType.NVarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = strLoginName;

                Params[++iIndex] = new SqlParameter("@ProjectViewRight", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bProjectViewRight;


                Params[++iIndex] = new SqlParameter("@TaskViewRight", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bTaskViewRight;

                Params[++iIndex] = new SqlParameter("@DecisionViewRight", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bDecisionViewRight;

                Params[++iIndex] = new SqlParameter("@AgendaItemViewRight", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bAgendaItemViewRight;

                Params[++iIndex] = new SqlParameter("@MeetingViewRight", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bMeetingViewRight;

                Params[++iIndex] = new SqlParameter("@TotCount", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Output;


                DataSet ds;
                if (bIsDeletedListview)
                    ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetDeletedDocsListviewDS", Params);
                else
                    ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetDocsListviewDS", Params);
                iTotCount = int.Parse(Params[iIndex].Value.ToString());

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["DocumentArchiveTitleIds"] != DBNull.Value && dr["DocumentArchiveTitleIds"].ToString() != "")
                    {
                        dr["Signature"] = string.Empty;
                        //dr["Signature"] = fnSetSignature(dr["DocumentArchiveTitleIds"].ToString());
                    }
                }

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool fnSaveRecord(Enums.VTSObjects objType, DataSet ds, int iRecordId, string sLoginUserId)
        {
            try
            {
                string strSql = "";
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["ChangeType"].ToString() != "U")
                    {
                        strSql = "";
                        if (row["ChangeType"].ToString() == "D")
                        {
                            if (row["FileId"] != DBNull.Value)
                            {
                                if (row["DocRecordType"].ToString() == ((int)Enums.DocRecordType.Link).ToString())
                                {
                                    //delete the link only.
                                    //it link id will be returned in case of linked files in place of file id to the front end.
                                    strSql = "Delete From FileLinks Where LinkId=" + row["FileId"].ToString();
                                    Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                                }
                                else
                                {
                                    //delete the actual file.
                                    strSql = "Delete From FileLinks Where LinkFileId=" + row["FileId"].ToString();
                                    Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                                    strSql = "Delete from FileVersions Where FileId=" + row["FileId"].ToString();
                                    Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                                    //Delete File Keywords
                                    strSql = "Delete from FileKeywords Where FileId=" + row["FileId"].ToString();
                                    Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                                    //Updating Message table
                                    strSql = "Update Messages Set LinkFileId=null where LinkFileId=" + row["FileId"].ToString();
                                    Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                                    strSql = "Delete from Files Where FileId=" + row["FileId"].ToString();
                                    Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                                }
                            }
                        }
                        else
                        {
                            if (row["FileId"] == DBNull.Value || row["FileId"].ToString() == "")
                            {
                                if (row["DocRecordType"].ToString() == ((int)Enums.DocRecordType.Link).ToString())
                                {
                                    //create only the link.
                                    strSql = "Insert into FileLinks(ObjectType, ObjectId, LinkFileId, ";
                                    strSql += " LinkCreatedBy, LinkCreatedOn) Values(";
                                    strSql += ((int)objType).ToString() + "," + iRecordId.ToString() + ",";
                                    strSql += row["NewLinkFileId"].ToString() + ",";
                                    strSql += Common.EncodeNString(sLoginUserId) + ",";
                                    strSql += "getdate())";
                                    Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                                }
                                else
                                {
                                    //insert the record.
                                    strSql = "Insert into Files(ObjectType, ObjectId, FName, FExtension,";
                                    strSql += " FType, FLocation, IsVersioned, IsProtected, Password, IsCheckedOut,  ";
                                    strSql += " CheckedOutBy, CheckedOutOn, RecordType,DocumentArchiveTitleIds,DocumentType,ReceivedFrom) Values(";
                                    strSql += ((int)objType).ToString() + "," + iRecordId.ToString() + ",";
                                    strSql += Common.EncodeNString(row["FileName"].ToString()) + ",";
                                    strSql += Common.EncodeNString(row["Extension"].ToString().ToLower()) + ",";
                                    strSql += Common.EncodeNString(row["FileType"].ToString()) + ",";
                                    strSql += Common.EncodeNString(row["Location"].ToString()) + ",";

                                    if ((bool)row["IsVersioned"] == true)
                                    { strSql += "1,"; }
                                    else
                                    { strSql += "0,"; }

                                    if ((bool)row["IsProtected"] == true && row["Password"].ToString() != "")
                                    {
                                        strSql += "1,";
                                    }
                                    else
                                    { strSql += "0,"; }
                                    if (row["Password"].ToString() != "")
                                        strSql += Common.EncodeNString(Common.GetEncriptedValue(row["Password"].ToString())) + ",";
                                    else
                                        strSql += "Null,";

                                    if (row["IsCheckedOut"] != DBNull.Value && (bool)row["IsCheckedOut"] == true)
                                    {
                                        strSql += "1,";
                                        strSql += Common.EncodeNString(row["CheckedOutBy"].ToString()) + ",";
                                        long dtCheckedOutOn = 0;
                                        if (row["CheckedOutOn"].ToString() != "")
                                            dtCheckedOutOn = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(row["CheckedOutOn"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);
                                        if (dtCheckedOutOn == 0)
                                        { strSql += "Null,"; }
                                        else
                                        { strSql += "cast(" + dtCheckedOutOn.ToString() + " as datetime),"; }

                                    }
                                    else
                                    { strSql += "0,NULL,NULL,"; }

                                    strSql += row["DocRecordType"].ToString() + ",";
                                    if (row["DocumentArchiveTitleIds"].ToString() != "")
                                        strSql += Common.EncodeString(row["DocumentArchiveTitleIds"].ToString()) + ",";
                                    else
                                        strSql += "Null, ";

                                    if (row["DocumentType"].ToString() != "")
                                        strSql += row["DocumentType"].ToString() + ",";
                                    else
                                        strSql += "Null, ";

                                    if (row["ReceivedFrom"].ToString() != "")
                                        strSql += row["ReceivedFrom"].ToString();
                                    else
                                        strSql += "Null ";

                                    strSql += ")";

                                    Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                                    strSql = "Select isnull(Max(FileId),1) as NId from Files where ObjectType=" + ((int)objType).ToString() + " And  ObjectId=" + iRecordId.ToString();
                                    string sNewFileId = Common.dbMgr.ExecuteScalar(CommandType.Text, strSql).ToString();
                                    if (sNewFileId != "")
                                    {
                                        long dtFileCreationDate = 0;
                                        long dtFileModificationDate = 0;
                                        if (row["CreationDate"].ToString() != "")
                                            dtFileCreationDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(row["CreationDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);

                                        if (row["ModificationDate"].ToString() != "")
                                            dtFileModificationDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(row["ModificationDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);

                                        strSql = "Insert into FileVersions(FileId, VersionNumber, FileSize, FileSizeBytes, ";
                                        strSql += "CreationDate, ModificationDate, Title, Subject, Author, Category,";
                                        strSql += "Keywords, Comments,FileContent,SmallThumb,LargeThumb, ";
                                        strSql += "RecordCreatedBy, RecordCreatedOn,ShortcutPath) Values(";
                                        strSql += sNewFileId + "," + row["Version"].ToString() + ",";
                                        strSql += Common.EncodeNString(row["Size"].ToString()) + ",";
                                        strSql += Common.EncodeString(row["SizeBytes"].ToString()) + ",";
                                        if (dtFileCreationDate > 0)
                                            strSql += "cast(" + dtFileCreationDate.ToString() + " as datetime),";
                                        else
                                            strSql += "NULL,";

                                        if (dtFileModificationDate > 0)
                                            strSql += "cast(" + dtFileModificationDate.ToString() + " as datetime),";
                                        else
                                            strSql += "NULL,";
                                        strSql += Common.EncodeNString(row["Title"].ToString()) + ",";
                                        strSql += Common.EncodeNString(row["Subject"].ToString()) + ",";
                                        strSql += Common.EncodeNString(row["Author"].ToString()) + ",";
                                        strSql += Common.EncodeNString(row["Category"].ToString()) + ",";
                                        strSql += Common.EncodeNString(row["Keywords"].ToString()) + ",";
                                        strSql += Common.EncodeNString(row["Comments"].ToString()) + ",";
                                        strSql += "@FileContent,";
                                        strSql += "@SmallThumb,";
                                        strSql += "@LargeThumb,";
                                        strSql += Common.EncodeString(sLoginUserId) + ",";
                                        strSql += "getdate() , ";
                                        strSql += Common.EncodeNString(row["ShortcutPath"].ToString());
                                        strSql += ")";


                                        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                                        cmd.CommandType = CommandType.Text;

                                        cmd.Parameters.Add("@FileContent", SqlDbType.Image);
                                        cmd.Parameters.Add("@SmallThumb", SqlDbType.Image);
                                        cmd.Parameters.Add("@LargeThumb", SqlDbType.Image);

                                        if (row["PhysicalPath"] != null && row["PhysicalPath"].ToString() != "")
                                        {
                                            if (File.Exists(row["PhysicalPath"].ToString()))
                                            {
                                                byte[] strFile = null;



                                                strFile = GetByteArray(row["PhysicalPath"].ToString());
                                                if (strFile != null)
                                                    cmd.Parameters["@FileContent"].Value = strFile;
                                                else
                                                    cmd.Parameters["@FileContent"].Value = DBNull.Value;

                                                if (row["SmallThumb"] != null)
                                                    cmd.Parameters["@SmallThumb"].Value = row["SmallThumb"];
                                                else
                                                    cmd.Parameters["@SmallThumb"].Value = DBNull.Value;

                                                if (row["LargeThumb"] != null)
                                                    cmd.Parameters["@LargeThumb"].Value = row["LargeThumb"];
                                                else
                                                    cmd.Parameters["@LargeThumb"].Value = DBNull.Value;
                                            }
                                            else
                                            {
                                                cmd.Parameters["@FileContent"].Value = DBNull.Value;
                                                cmd.Parameters["@SmallThumb"].Value = DBNull.Value;
                                                cmd.Parameters["@LargeThumb"].Value = DBNull.Value;
                                            }
                                        }
                                        else
                                        {
                                            cmd.Parameters["@FileContent"].Value = DBNull.Value;
                                            cmd.Parameters["@SmallThumb"].Value = DBNull.Value;
                                            cmd.Parameters["@LargeThumb"].Value = DBNull.Value;
                                        }

                                        cmd.CommandText = strSql;
                                        Common.dbMgr.ExecuteNonQuery(cmd);


                                        //insert file keywords.
                                        DataRow[] drs = ds.Tables[2].Select("RecordId=" + row["RecordId"].ToString());
                                        foreach (DataRow d in drs)
                                        {
                                            strSql = "Insert into FileKeywords (FileId,KeywordId) Values (";
                                            strSql += sNewFileId + "," + d["KeywordId"].ToString() + ")";

                                            Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Generated File Id cannot be read.");
                                    }
                                }
                            }
                            else
                            {
                                if (row["ChangeType"].ToString() == "M")
                                {
                                    //update the record.
                                    strSql = "Update Files Set ";
                                    strSql += "FName=";
                                    strSql += Common.EncodeNString(row["FileName"].ToString());
                                    strSql += ",FExtension=";
                                    strSql += Common.EncodeNString(row["Extension"].ToString().ToLower());
                                    strSql += ",FType=";
                                    strSql += Common.EncodeNString(row["FileType"].ToString());
                                    strSql += ",FLocation=";
                                    strSql += Common.EncodeNString(row["Location"].ToString());

                                    if ((bool)row["IsVersioned"])
                                        strSql += ",IsVersioned=1";
                                    else
                                        strSql += ",IsVersioned=0";

                                    if ((bool)row["IsProtected"] && row["Password"].ToString() != "")
                                    {
                                        strSql += ",IsProtected=1";
                                        strSql += ",Password=";
                                        strSql += Common.EncodeNString(Common.GetEncriptedValue(row["Password"].ToString()));
                                    }
                                    else
                                    { strSql += ",IsProtected=0,Password=NULL"; }



                                    if ((bool)row["IsCheckedOut"])
                                    {
                                        strSql += ",IsCheckedOut=1";
                                        strSql += ",CheckedOutBy=";
                                        strSql += Common.EncodeNString(row["CheckedOutBy"].ToString());

                                        long dtCheckedOutOn = 0;
                                        if (row["CheckedOutOn"].ToString() != "")
                                            dtCheckedOutOn = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(row["CheckedOutOn"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);

                                        if (dtCheckedOutOn > 0)
                                            strSql += ",CheckedOutOn=cast(" + dtCheckedOutOn.ToString() + " as datetime)";
                                        else
                                            strSql += ",CheckedOutOn=NULL ";
                                    }

                                    else
                                    { strSql += ",IsCheckedOut=0,CheckedOutBy =NULL, CheckedOutOn = NULL "; }

                                    if (row["DocumentArchiveTitleIds"].ToString() != "")
                                        strSql += " ,DocumentArchiveTitleIds = " + Common.EncodeString(row["DocumentArchiveTitleIds"].ToString());
                                    else
                                        strSql += " ,DocumentArchiveTitleIds = Null";

                                    if (row["DocumentType"].ToString() != "")
                                        strSql += " ,DocumentType = " + row["DocumentType"].ToString();

                                    if (row["ReceivedFrom"].ToString() != "")
                                        strSql += " ,ReceivedFrom = " + row["ReceivedFrom"].ToString();

                                    strSql += " Where FileId=" + row["FileId"].ToString();
                                    Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);



                                    //with the current version, if file exists, update it.  if it is new version number, then insert it.
                                    strSql = "Select Count(1)  From FileVersions Where FileId=" + row["FileId"].ToString();
                                    strSql += " and VersionNumber=" + row["Version"].ToString();
                                    if (Common.dbMgr.ExecuteScalar(CommandType.Text, strSql) != "0")
                                    {
                                        //Update the record as this version already exists.
                                        strSql = "Update FileVersions Set ";

                                        strSql += "FileSize=";
                                        strSql += Common.EncodeNString(row["Size"].ToString());
                                        strSql += ",FileSizeBytes=" + Common.EncodeString(row["SizeBytes"].ToString());

                                        long dtFCreationDate = 0;
                                        long dtFModificationDate = 0;
                                        if (row["CreationDate"].ToString() != "")
                                            dtFCreationDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(row["CreationDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);

                                        if (row["ModificationDate"].ToString() != "")
                                            dtFModificationDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(row["ModificationDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);

                                        if (dtFCreationDate > 0)
                                            strSql += ",CreationDate=cast(" + dtFCreationDate.ToString() + " as datetime)";
                                        else
                                            strSql += ",CreationDate=NULL";

                                        if (dtFModificationDate > 0)
                                            strSql += ",ModificationDate=cast(" + dtFModificationDate.ToString() + " as datetime)";
                                        else
                                            strSql += ",ModificationDate=NULL";

                                        strSql += ",Title=";
                                        strSql += Common.EncodeNString(row["Title"].ToString());
                                        strSql += ",Subject=";
                                        strSql += Common.EncodeNString(row["Subject"].ToString());
                                        strSql += ",Author=";
                                        strSql += Common.EncodeNString(row["Author"].ToString());
                                        strSql += ",Category=";
                                        strSql += Common.EncodeNString(row["Category"].ToString());
                                        strSql += ",Keywords=";
                                        strSql += Common.EncodeNString(row["Keywords"].ToString());
                                        strSql += ",Comments=";
                                        strSql += Common.EncodeNString(row["Comments"].ToString());
                                        strSql += ",RecordModifiedBy=";
                                        strSql += Common.EncodeNString(sLoginUserId);
                                        strSql += ",RecordModifiedOn=getdate()";

                                        strSql += ",ShortcutPath=";
                                        strSql += Common.EncodeNString(row["ShortcutPath"].ToString());


                                        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                                        cmd.CommandType = CommandType.Text;

                                        if (row["PhysicalPath"] != null && row["PhysicalPath"].ToString() != "")
                                        {
                                            cmd.Parameters.Add("@FileContent", SqlDbType.Image);
                                            strSql += ",FileContent=@FileContent ";
                                        }

                                        cmd.Parameters.Add("@SmallThumb", SqlDbType.Image);
                                        strSql += ",SmallThumb=@SmallThumb ";

                                        cmd.Parameters.Add("@LargeThumb", SqlDbType.Image);
                                        strSql += ",LargeThumb=@LargeThumb ";

                                        if (row["PhysicalPath"] != null && row["PhysicalPath"].ToString() != "")
                                        {
                                            if (File.Exists(row["PhysicalPath"].ToString()))
                                            {
                                                byte[] strFile = null;

                                                strFile = GetByteArray(row["PhysicalPath"].ToString());
                                                if (strFile != null)
                                                    cmd.Parameters["@FileContent"].Value = strFile;
                                                else
                                                    cmd.Parameters["@FileContent"].Value = DBNull.Value;

                                                if (row["SmallThumb"] != null)
                                                    cmd.Parameters["@SmallThumb"].Value = row["SmallThumb"];
                                                else
                                                    cmd.Parameters["@SmallThumb"].Value = DBNull.Value;

                                                if (row["LargeThumb"] != null)
                                                    cmd.Parameters["@LargeThumb"].Value = row["LargeThumb"];
                                                else
                                                    cmd.Parameters["@LargeThumb"].Value = DBNull.Value;

                                            }
                                            else
                                            {
                                                cmd.Parameters["@FileContent"].Value = DBNull.Value;
                                                cmd.Parameters["@SmallThumb"].Value = DBNull.Value;
                                                cmd.Parameters["@LargeThumb"].Value = DBNull.Value;
                                            }
                                        }
                                        else
                                        {
                                            if (row["SmallThumb"] != null)
                                                cmd.Parameters["@SmallThumb"].Value = row["SmallThumb"];
                                            else
                                                cmd.Parameters["@SmallThumb"].Value = DBNull.Value;

                                            if (row["LargeThumb"] != null)
                                                cmd.Parameters["@LargeThumb"].Value = row["LargeThumb"];
                                            else
                                                cmd.Parameters["@LargeThumb"].Value = DBNull.Value;
                                        }

                                        strSql += " Where FileId=" + row["FileId"].ToString();
                                        strSql += " and VersionNumber=" + row["Version"].ToString();

                                        cmd.CommandText = strSql;
                                        Common.dbMgr.ExecuteNonQuery(cmd);

                                    }
                                    else
                                    {
                                        //Insert the new record as this is non existing version.
                                        long dtFileCreationDate = 0;
                                        long dtFileModificationDate = 0;
                                        if (row["CreationDate"].ToString() != "")
                                            dtFileCreationDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(row["CreationDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);

                                        if (row["ModificationDate"].ToString() != "")
                                            dtFileCreationDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(row["ModificationDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);

                                        strSql = "Insert into FileVersions(FileId, VersionNumber, FileSize,FileSizeBytes, ";
                                        strSql += "CreationDate, ModificationDate, Title, Subject, Author, Category,";
                                        strSql += "Keywords, Comments,FileContent,SmallThumb,LargeThumb,";
                                        strSql += "RecordCreatedBy, RecordCreatedOn) Values(";
                                        strSql += row["FileId"].ToString() + "," + row["Version"].ToString() + ",";
                                        strSql += Common.EncodeNString(row["Size"].ToString()) + ",";
                                        strSql += Common.EncodeString(row["SizeBytes"].ToString()) + ",";

                                        if (dtFileCreationDate > 0)
                                            strSql += "cast(" + dtFileCreationDate.ToString() + " as datetime),";
                                        else
                                            strSql += "NULL,";

                                        if (dtFileModificationDate > 0)
                                            strSql += "cast(" + dtFileModificationDate.ToString() + " as datetime),";
                                        else
                                            strSql += "NULL,";

                                        strSql += Common.EncodeNString(row["Title"].ToString()) + ",";
                                        strSql += Common.EncodeNString(row["Subject"].ToString()) + ",";

                                        strSql += Common.EncodeNString(row["Author"].ToString()) + ",";
                                        strSql += Common.EncodeNString(row["Category"].ToString()) + ",";

                                        strSql += Common.EncodeNString(row["Keywords"].ToString()) + ",";
                                        strSql += Common.EncodeNString(row["Comments"].ToString()) + ",";
                                        strSql += "@FileContent,";
                                        strSql += "@SmallThumb,";
                                        strSql += "@LargeThumb,";
                                        strSql += Common.EncodeNString(sLoginUserId) + ",";
                                        strSql += "getdate())";

                                        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                                        cmd.CommandType = CommandType.Text;

                                        cmd.Parameters.Add("@FileContent", SqlDbType.Image);
                                        cmd.Parameters.Add("@SmallThumb", SqlDbType.Image);
                                        cmd.Parameters.Add("@LargeThumb", SqlDbType.Image);

                                        if (row["PhysicalPath"] != null && row["PhysicalPath"].ToString() != "")
                                        {
                                            if (File.Exists(row["PhysicalPath"].ToString()))
                                            {
                                                byte[] strFile = null;

                                                strFile = GetByteArray(row["PhysicalPath"].ToString());


                                                if (strFile != null)
                                                    cmd.Parameters["@FileContent"].Value = strFile;
                                                else
                                                    cmd.Parameters["@FileContent"].Value = DBNull.Value;

                                                if (row["SmallThumb"] != null)
                                                    cmd.Parameters["@SmallThumb"].Value = row["SmallThumb"];
                                                else
                                                    cmd.Parameters["@SmallThumb"].Value = DBNull.Value;

                                                if (row["LargeThumb"] != null)
                                                    cmd.Parameters["@LargeThumb"].Value = row["LargeThumb"];
                                                else
                                                    cmd.Parameters["@LargeThumb"].Value = DBNull.Value;

                                            }
                                            else
                                            {
                                                cmd.Parameters["@FileContent"].Value = DBNull.Value;
                                                cmd.Parameters["@SmallThumb"].Value = DBNull.Value;
                                                cmd.Parameters["@LargeThumb"].Value = DBNull.Value;
                                            }
                                        }
                                        else
                                        {
                                            cmd.Parameters["@FileContent"].Value = DBNull.Value;
                                            cmd.Parameters["@SmallThumb"].Value = DBNull.Value;
                                            cmd.Parameters["@LargeThumb"].Value = DBNull.Value;
                                        }

                                        cmd.CommandText = strSql;
                                        Common.dbMgr.ExecuteNonQuery(cmd);

                                    }

                                    //update keywords for this file.
                                    //1. remove existing keywords.
                                    strSql = "Delete From FileKeywords Where FileId=" + row["FileId"].ToString();
                                    Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                                    //2. insert new keywords (presently selected)

                                    //insert file keywords.
                                    DataRow[] drs = ds.Tables[2].Select("RecordId=" + row["RecordId"].ToString());
                                    foreach (DataRow d in drs)
                                    {
                                        strSql = "Insert into FileKeywords (FileId,KeywordId) Values (";
                                        strSql += row["FileId"].ToString() + "," + d["KeywordId"].ToString() + ")";

                                        Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                                    }

                                }
                                //For new file insert file keywords.
                                else if (row["ChangeType"].ToString() == "N")
                                {
                                    DataRow[] drs = ds.Tables[2].Select("RecordId=" + row["RecordId"].ToString());
                                    foreach (DataRow d in drs)
                                    {
                                        strSql = "Insert into FileKeywords (FileId,KeywordId) Values (";
                                        strSql += row["FileId"].ToString() + "," + d["KeywordId"].ToString() + ")";

                                        Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);
                                    }
                                }
                            }
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

        public bool fnSaveAttachFile(int objType, DataRow row, int iRecordId, string sLoginUserId, ref string sNewFileId)
        {
            try
            {
                string strSql = "";
                string strContentType = "doc";
                if ((row["Extension"].ToString().ToLower() == ".html") ||
                    (row["Extension"].ToString().ToLower() == ".htm"))
                    strContentType = "html";
                else if (row["Extension"].ToString().ToLower() == ".xml")
                    strContentType = "xml";
                else if (row["Extension"].ToString().ToLower() == ".pdf")
                    strContentType = "pdf";
                else if (row["Extension"].ToString().ToLower() == ".txt")
                    strContentType = "txt";
                else if (row["Extension"].ToString().ToLower() == ".docx")
                    strContentType = "docx";
                else if (row["Extension"].ToString().ToLower() == ".docm")
                    strContentType = "docm";
                else if (row["Extension"].ToString().ToLower() == ".pptx")
                    strContentType = "pptx";
                else if (row["Extension"].ToString().ToLower() == ".pptm")
                    strContentType = "pptm";
                else if (row["Extension"].ToString().ToLower() == ".xlsm")
                    strContentType = "xlsm";
                else if (row["Extension"].ToString().ToLower() == ".xlsx")
                    strContentType = "xlsx";
                else if (row["Extension"].ToString().ToLower() == ".xlsb")
                    strContentType = "xlsb";
                else if (row["Extension"].ToString().ToLower() == ".zip")
                    strContentType = "zip";

                if (row["FileId"] == DBNull.Value || row["FileId"].ToString() == "")
                {
                    //insert the record.
                    strSql = "Insert into Files(ObjectType, ObjectId, FName, FExtension,";
                    strSql += " FType, FLocation, IsVersioned, IsProtected, Password, IsCheckedOut, CheckedOutBy, CheckedOutOn, RecordType,IsInvoice) Values(";
                    strSql += ((int)objType).ToString() + "," + iRecordId.ToString() + ",";
                    strSql += Common.EncodeNString(row["FileName"].ToString()) + ",";
                    strSql += Common.EncodeNString(row["Extension"].ToString().ToLower()) + ",";
                    strSql += Common.EncodeNString(row["FileType"].ToString()) + ",";
                    strSql += Common.EncodeNString(row["Location"].ToString()) + ",";




                    if ((bool)row["IsVersioned"] == true)
                    { strSql += "1,"; }
                    else
                    { strSql += "0,"; }

                    if ((bool)row["IsProtected"] == true && row["Password"].ToString() != "")
                    {
                        strSql += "1,";
                    }
                    else
                    { strSql += "0,"; }
                    if (row["Password"].ToString() != "")
                        strSql += Common.EncodeNString(Common.GetEncriptedValue(row["Password"].ToString())) + ",";
                    else
                        strSql += "Null,";

                    if (row["IsCheckedOut"] != DBNull.Value && (bool)row["IsCheckedOut"] == true)
                    {
                        strSql += "1,";
                        strSql += Common.EncodeNString(row["CheckedOutBy"].ToString()) + ",";
                        long dtCheckedOutOn = 0;
                        if (row["CheckedOutOn"].ToString() != "")
                            dtCheckedOutOn = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(row["CheckedOutOn"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);
                        if (dtCheckedOutOn == 0)
                        { strSql += "Null,"; }
                        else
                        { strSql += "cast(" + dtCheckedOutOn.ToString() + " as datetime),"; }

                    }
                    else
                    { strSql += "0,NULL,NULL,"; }

                    strSql += row["DocRecordType"].ToString();

                    if ((bool)row["IsInvoice"] == true)
                    { strSql += ",1"; }
                    else
                    { strSql += ",0"; }
                    strSql += ")";

                    Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);

                    strSql = "Select @@Identity as NId";
                    sNewFileId = Common.dbMgr.ExecuteScalar(CommandType.Text, strSql).ToString();
                    if (sNewFileId != "")
                    {
                        long dtFileCreationDate = 0;
                        long dtFileModificationDate = 0;
                        if (row["CreationDate"].ToString() != "")
                            dtFileCreationDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(row["CreationDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);

                        if (row["ModificationDate"].ToString() != "")
                            dtFileModificationDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(row["ModificationDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);



                        strSql = "Insert into FileVersions(FileId, VersionNumber, FileSize, FileSizeBytes, ";
                        strSql += "CreationDate, ModificationDate, Title, Subject, Author, Category,";
                        strSql += "Keywords, Comments,FileContent,SmallThumb,LargeThumb, ";
                        strSql += "RecordCreatedBy, RecordCreatedOn,ContentType,ShortcutPath) Values(";
                        strSql += sNewFileId + "," + row["Version"].ToString() + ",";
                        strSql += Common.EncodeNString(row["Size"].ToString()) + ",";
                        strSql += Common.EncodeNString(row["SizeBytes"].ToString()) + ",";
                        if (dtFileCreationDate > 0)
                            strSql += "cast(" + dtFileCreationDate.ToString() + " as datetime),";
                        else
                            strSql += "NULL,";

                        if (dtFileModificationDate > 0)
                            strSql += "cast(" + dtFileModificationDate.ToString() + " as datetime),";
                        else
                            strSql += "NULL,";
                        strSql += Common.EncodeNString(row["Title"].ToString()) + ",";
                        strSql += Common.EncodeNString(row["Subject"].ToString()) + ",";
                        strSql += Common.EncodeNString(row["Author"].ToString()) + ",";
                        strSql += Common.EncodeNString(row["Category"].ToString()) + ",";
                        strSql += Common.EncodeNString(row["Keywords"].ToString()) + ",";
                        strSql += Common.EncodeNString(row["Comments"].ToString()) + ",";
                        strSql += "@FileContent,";
                        strSql += "@SmallThumb,";
                        strSql += "@LargeThumb,";
                        strSql += Common.EncodeNString(sLoginUserId) + ",";
                        strSql += "getdate(),";
                        strSql += Common.EncodeNString(strContentType) + ",";
                        strSql += Common.EncodeNString(row["ShortcutPath"].ToString()) + ")";

                        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.Add("@FileContent", SqlDbType.Image);
                        cmd.Parameters.Add("@SmallThumb", SqlDbType.Image);
                        cmd.Parameters.Add("@LargeThumb", SqlDbType.Image);

                        if (row["PhysicalPath"] != null && row["PhysicalPath"].ToString() != "")
                        {
                            if (File.Exists(row["PhysicalPath"].ToString()))
                            {
                                byte[] strFile = null;
                                strFile = GetByteArray(row["PhysicalPath"].ToString());
                                if (strFile != null)
                                    cmd.Parameters["@FileContent"].Value = strFile;
                                else
                                    cmd.Parameters["@FileContent"].Value = DBNull.Value;

                                if (row["SmallThumb"] != null)
                                    cmd.Parameters["@SmallThumb"].Value = row["SmallThumb"];
                                else
                                    cmd.Parameters["@SmallThumb"].Value = DBNull.Value;

                                if (row["LargeThumb"] != null)
                                    cmd.Parameters["@LargeThumb"].Value = row["LargeThumb"];
                                else
                                    cmd.Parameters["@LargeThumb"].Value = DBNull.Value;
                            }
                            else
                            {
                                cmd.Parameters["@FileContent"].Value = DBNull.Value;
                                cmd.Parameters["@SmallThumb"].Value = DBNull.Value;
                                cmd.Parameters["@LargeThumb"].Value = DBNull.Value;
                            }
                        }
                        else
                        {
                            cmd.Parameters["@FileContent"].Value = DBNull.Value;

                            if (row["ShortcutPath"] != null && row["ShortcutPath"].ToString() != "")
                            {
                                if (File.Exists(row["ShortcutPath"].ToString()))
                                {
                                    byte[] strFile = null;
                                    strFile = GetByteArray(row["ShortcutPath"].ToString());
                                    if (row["SmallThumb"] != null)
                                        cmd.Parameters["@SmallThumb"].Value = row["SmallThumb"];
                                    else
                                        cmd.Parameters["@SmallThumb"].Value = DBNull.Value;

                                    if (row["LargeThumb"] != null)
                                        cmd.Parameters["@LargeThumb"].Value = row["LargeThumb"];
                                    else
                                        cmd.Parameters["@LargeThumb"].Value = DBNull.Value;
                                }
                                else
                                {
                                    cmd.Parameters["@SmallThumb"].Value = DBNull.Value;
                                    cmd.Parameters["@LargeThumb"].Value = DBNull.Value;
                                }
                            }
                            else
                            {
                                cmd.Parameters["@SmallThumb"].Value = DBNull.Value;
                                cmd.Parameters["@LargeThumb"].Value = DBNull.Value;
                            }
                        }

                        cmd.CommandText = strSql;
                        Common.dbMgr.ExecuteNonQuery(cmd);
                    }
                    else
                    {
                        throw new Exception("Generated File Id cannot be read.");
                    }

                }
                else
                {

                    strSql = "Update Files Set RecordType=" + Common.EncodeString(row["DocRecordType"].ToString());
                    strSql += " Where FileId=" + row["FileId"].ToString();
                    Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSql);


                    //Update the record as this version already exists.
                    strSql = "Update FileVersions Set ";
                    strSql += "FileSize=";
                    strSql += Common.EncodeNString(row["Size"].ToString());
                    strSql += ",FileSizeBytes=" + Common.EncodeString(row["SizeBytes"].ToString());

                    long dtFCreationDate = 0;
                    long dtFModificationDate = 0;
                    if (row["CreationDate"].ToString() != "")
                        dtFCreationDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(row["CreationDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);

                    if (row["ModificationDate"].ToString() != "")
                        dtFModificationDate = DateAndTime.DateDiff(DateInterval.Day, Common.SQLServerZeroDate, DateTime.Parse(row["ModificationDate"].ToString()), FirstDayOfWeek.System, FirstWeekOfYear.System);

                    if (dtFCreationDate > 0)
                        strSql += ",CreationDate=cast(" + dtFCreationDate.ToString() + " as datetime)";
                    else
                        strSql += ",CreationDate=NULL";

                    if (dtFModificationDate > 0)
                        strSql += ",ModificationDate=cast(" + dtFModificationDate.ToString() + " as datetime)";
                    else
                        strSql += ",ModificationDate=NULL";

                    strSql += ",Title=";
                    strSql += Common.EncodeNString(row["Title"].ToString());
                    strSql += ",Subject=";
                    strSql += Common.EncodeNString(row["Subject"].ToString());
                    strSql += ",Author=";
                    strSql += Common.EncodeNString(row["Author"].ToString());
                    strSql += ",Category=";
                    strSql += Common.EncodeNString(row["Category"].ToString());
                    strSql += ",Keywords=";
                    strSql += Common.EncodeNString(row["Keywords"].ToString());
                    strSql += ",Comments=";
                    strSql += Common.EncodeNString(row["Comments"].ToString());
                    strSql += ",RecordModifiedBy=";
                    strSql += Common.EncodeNString(sLoginUserId);
                    strSql += ",RecordModifiedOn=getdate()";
                    strSql += ",ContentType=";
                    strSql += Common.EncodeNString(strContentType);
                    strSql += ",ShortcutPath=";
                    strSql += Common.EncodeNString(row["ShortcutPath"].ToString());

                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    if (row["PhysicalPath"] != null && row["PhysicalPath"].ToString() != "")
                    {
                        cmd.Parameters.Add("@FileContent", SqlDbType.Image);
                        strSql += ",FileContent=@FileContent ";

                        if (File.Exists(row["PhysicalPath"].ToString()))
                        {
                            byte[] strFile = null;
                            strFile = GetByteArray(row["PhysicalPath"].ToString());
                            //Common.GetThumbs(row["PhysicalPath"].ToString(), ref strSmallThumb, ref strLargeThumb);

                            if (strFile != null)
                                cmd.Parameters["@FileContent"].Value = strFile;
                            else
                                cmd.Parameters["@FileContent"].Value = DBNull.Value;

                        }
                        else
                        {
                            cmd.Parameters["@FileContent"].Value = DBNull.Value;

                        }
                    }
                    else
                    {
                        strSql += ",FileContent=NULL ";
                    }

                    strSql += " Where FileId=" + row["FileId"].ToString();
                    strSql += " and VersionNumber=" + row["Version"].ToString();

                    cmd.CommandText = strSql;
                    Common.dbMgr.ExecuteNonQuery(cmd);


                }


                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        



        //private byte[] GetThumb(bool bSmall, string sFilePath)
        //{
        //    try
        //    {
        //        Image img = null;
        //        Image thumbnailImage = null;

        //        try
        //        {
        //            img = Image.FromFile(sFilePath, true);

        //            if (bSmall)
        //            {
        //                thumbnailImage = FixedSize(img, 64, 64);
        //                return imageToByteArray(thumbnailImage);
        //            }
        //            else
        //            {
        //                thumbnailImage = FixedSize(img, 128, 128);
        //                return imageToByteArray(thumbnailImage);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            FileInfo fi = new FileInfo(sFilePath);
        //            IconExtractor IconExtractor = new IconExtractor();
        //            System.Drawing.Icon Icon;
        //            Icon = IconExtractor.Extract(fi.Extension, IconSize.Large);
        //            img = Icon.ToBitmap();

        //            if (bSmall)
        //            {
        //                thumbnailImage = FixedSize1(img, 64, 64);
        //                return imageToByteArray(thumbnailImage);
        //            }
        //            else
        //            {
        //                thumbnailImage = FixedSize1(img, 128, 128);
        //                return imageToByteArray(thumbnailImage);
        //            }
        //            ex = null;

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ex = null;
        //        return null;
        //    }
        //}


        public bool fnSaveGeneratedTenderLetter(int iTenderID, string sReportPath, string sLoginUserId)
        {
            Common.dbMgr.BeginTrans();
            try
            {
                FileInfo fi = new FileInfo(sReportPath);
                byte[] strFile = null;
                if (sReportPath != "")
                {
                    if (File.Exists(sReportPath))
                    {
                        strFile = GetByteArray(sReportPath);
                    }
                }

                string sSql = "";
                //insert new file.
                sSql = "Insert into Files(ObjectType, ObjectId, FName, FExtension,";
                sSql += " IsVersioned, IsProtected, IsCheckedOut, RecordType) Values(";
                sSql += ((int)Enums.VTSObjects.Tenders).ToString() + ",";
                sSql += iTenderID.ToString() + ",";
                sSql += Common.EncodeNString(fi.Name) + ",";
                sSql += Common.EncodeNString(fi.Extension) + ",";
                sSql += "0,0,0," + ((int)Enums.DocRecordType.Actual).ToString() + ")";

                Common.dbMgr.ExecuteNonQuery(CommandType.Text, sSql);

                sSql = "Select @@Identity as NId";
                string sNewFileId = Common.dbMgr.ExecuteScalar(CommandType.Text, sSql).ToString();
                if (sNewFileId != "")
                {
                    //insert file content in file versions.

                    sSql = "Insert into FileVersions(FileId, VersionNumber, ";
                    sSql += "CreationDate, ModificationDate, FileContent,";
                    sSql += "RecordCreatedBy, RecordCreatedOn) Values(";
                    sSql += sNewFileId + ",1,";
                    sSql += "getdate(),getdate(),";
                    sSql += "@FileContent,";

                    sSql += Common.EncodeNString(sLoginUserId) + ",";
                    sSql += "getdate())";

                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@FileContent", SqlDbType.Image);

                    if (strFile != null)
                        cmd.Parameters["@FileContent"].Value = strFile;
                    else
                        cmd.Parameters["@FileContent"].Value = DBNull.Value;

                    cmd.CommandText = sSql;
                    Common.dbMgr.ExecuteNonQuery(cmd);
                }
                else
                {
                    throw new Exception("Generated File Id cannot be read.");
                }

                Common.dbMgr.CommitTrans();
                return true;
            }
            catch (Exception ex)
            {
                Common.dbMgr.RoolbackTrans();
                throw ex;
            }
        }

        public static byte[] GetByteArray(string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);

                byte[] bytearray = br.ReadBytes((int)fs.Length);

                br.Close();
                fs.Close();

                return bytearray;
            }
            catch (Exception ex)
            {
                return GetByteArray_AfterCopy(filePath);
            }
        }

        private static byte[] GetByteArray_AfterCopy(string sFilePath)
        {
            try
            {
                FileInfo fi = new FileInfo(sFilePath);
                string sNewPath = Common.GetEPMTempPath() + "EPM-" + fi.Name;

                if (File.Exists(sNewPath))
                {
                    File.Delete(sNewPath);
                }

                File.Copy(sFilePath, sNewPath, true);
                fi = null;

                fi = new FileInfo(sNewPath);
                if ((fi.Attributes & FileAttributes.ReadOnly) > 0)
                {
                    fi.Attributes -= FileAttributes.ReadOnly;
                }
                fi = null;

                FileStream fs = new FileStream(sNewPath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);

                byte[] bytearray = br.ReadBytes((int)fs.Length);

                br.Close();
                fs.Close();

                File.Delete(sNewPath);
                return bytearray;
            }
            catch (Exception ex)
            {
                ex = null;
                return null;
            }
        }

        public bool GetFileReader(string sFileId, string sVersionNumber, string sChoosenPath, ref string filePath, string sSpecialFileName, bool bDonotShowFileIdAndVersion)
        {
            return Common.dbMgr.GetFileFromDB(sFileId, sVersionNumber, sChoosenPath, ref filePath, sSpecialFileName, false, bDonotShowFileIdAndVersion);
        }


        public DataSet GetDocDatasetSingleFile(int iRecordId, string sFileId, int iUILanguageId, string strLoginName)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sPrms = new SqlParameter[4];
                SqlParameter sPrm = new SqlParameter();
                int iIndex = 0;

                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ObjectId";
                sPrm.Value = iRecordId;
                sPrms[iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@FileId";
                sPrm.Value = int.Parse(sFileId);
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@UILanguageId";
                sPrm.Value = CommonVariable.iLanguageId;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@LoginName";
                sPrm.Value = strLoginName;
                sPrms[++iIndex] = sPrm;

                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetDocDSSingleFile", sPrms);
                return ds;
            }
            catch (Exception ex)
            { throw ex; }
        }



    }
}
