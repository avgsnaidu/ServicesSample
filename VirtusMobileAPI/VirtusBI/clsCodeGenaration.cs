using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace VirtusBI
{
    public class clsCodeGeneration
    {
        // It is not using any where (its for old control)
        //public bool InsertCodeGenRules(int iObjectTypeId, string sObjectCounterFormat, int iObjectCounterStart, ref int iRuleId)
        //{
        //    string strSQL = "";        
        //    try
        //    {
        //        strSQL = "select count(1) from CodeGenRules where ObjectTypeId=" + iObjectTypeId;
        //        if (int.Parse(Common.dbMgr.ExecuteScalar(CommandType.Text, strSQL)) != 0)
        //        {
        //            strSQL = "update CodeGenRules set ObjectCounterFormat='" + sObjectCounterFormat + "',ObjectCounterStart=" + iObjectCounterStart + " where ObjectTypeId=" + iObjectTypeId;
        //        }
        //        else
        //        {
        //            strSQL = "Insert into  CodeGenRules(ObjectTypeId,ObjectCounter,ObjectCounterFormat,ObjectCounterStart) values(" + iObjectTypeId + "," + 0 + ",'" + sObjectCounterFormat + "'," + iObjectCounterStart + ")";
        //        }
        //        Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSQL);

        //        strSQL = "select RuleId from CodeGenRules where ObjectTypeId=" + iObjectTypeId;

        //        iRuleId = int.Parse(Common.dbMgr.ExecuteScalar(CommandType.Text, strSQL));
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {

        //        return false;
        //    }
        //}
        public void UpdateManualCode(int iObjectTypeId, int iIsmanualCode)
        {

            try
            {
                //sStr = "update CodeRules set IsmanualCode=" + iIsmanualCode + " where ObjectTypeId=" + iObjectTypeId;
                //Common.dbMgr.ExecuteNonQuery(CommandType.Text, sStr);

                SqlParameter[] sPrms = new SqlParameter[2];
                SqlParameter sPrm = new SqlParameter();
                int iIndex = 0;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ObjectTypeId";
                sPrm.Value = iObjectTypeId;
                sPrms[iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@IsmanualCode";
                sPrm.Value = iIsmanualCode;
                sPrms[++iIndex] = sPrm;

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spUpdateCodeGenerationManualCode", sPrms);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetId(string strSelect)
        {
            try
            {
                return Convert.ToInt32(Common.dbMgr.ExecuteScalar(CommandType.Text, strSelect));
            }
            catch //(Exception ex)
            {
                return 0;
            }
        }
        public DataTable GetCodeGenElements(int iObjectTypeId)
        {
            //string strQuary = "select ElementDisplayIndex,ElementType,ElementFormat from CodeGenElements where RuleId in (select RuleId from CodeGenRules where ObjectTypeId=" + iObjectTypeId + ")order by ElementDisplayIndex";
            //return Common.dbMgr.ExecuteDataTable(CommandType.Text, strQuary);
            SqlParameter[] sPrms = new SqlParameter[1];
            SqlParameter sPrm = new SqlParameter();
            int iIndex = 0;

            sPrm = new SqlParameter();
            sPrm.SqlDbType = System.Data.SqlDbType.Int;
            sPrm.Direction = ParameterDirection.Input;
            sPrm.ParameterName = "@ObjectTypeId";
            sPrm.Value = iObjectTypeId;
            sPrms[iIndex] = sPrm;

            return Common.dbMgr.ExecuteDataTable(CommandType.StoredProcedure, "spGetCodeGenerationElements", sPrms);

        }
        public void DeleteCodeGenElements(int iRuleId)
        {

            try
            {

                SqlParameter[] sPrms = new SqlParameter[1];
                SqlParameter sPrm = new SqlParameter();
                int iIndex = 0;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@RuleId";
                sPrm.Value = iRuleId;
                sPrms[iIndex] = sPrm;

                Common.dbMgr.ExecuteDataTable(CommandType.StoredProcedure, "spDeleteCodeGenerationElements", sPrms);

                //spDeleteCodeGenerationElements
                //sStr = "Delete from CodeGenElements where RuleId=" + iRuleId;
                //Common.dbMgr.ExecuteNonQuery(CommandType.Text, sStr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertCodeGenElements(int iRuleId, int iElementDisplayIndex, int iElementType, string sElementFormat)
        {
            string sStr = "";
            try
            {
                sStr = "Insert into  CodeGenElements(RuleId,ElementDisplayIndex,ElementType,ElementFormat) values(" + iRuleId + "," + iElementDisplayIndex + "," + iElementType + ",";

                sStr += Common.EncodeNString(sElementFormat) + ")";
                return Common.dbMgr.ExecuteNonQuery(CommandType.Text, sStr);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public void InsertCodeRules(int iObjectTypeId, string sCodeString, string sCounterFormat, string sRunningCounter)
        {

            try
            {
                ////strSQL = "select CounterFormat from CodeRules where ObjectTypeId=" + iObjectTypeId;
                //DataTable dt = GetCoderules(iObjectTypeId);
                //if (dt.Rows.Count > 0)
                //{
                //    strSQL = "update CodeRules set ";
                //    strSQL += "CodeString=" + Common.EncodeString(sCodeString);
                //    strSQL += ",CounterFormat=" + Common.EncodeString(sCounterFormat);
                //    if (dt.Rows[0]["CounterFormat"].ToString().Trim() != sCounterFormat.Trim())
                //    {
                //        strSQL += ",RunningCounter=" + Common.EncodeString(sRunningCounter);
                //    }
                //    strSQL += " where ObjectTypeId=" + iObjectTypeId;
                //}
                //else
                //{
                //    sRunningCounter = "0";
                //    strSQL = "Insert into  CodeRules(ObjectTypeId,CodeString,CounterFormat) values(" + iObjectTypeId + "," + Common.EncodeString(sCodeString) + "," + Common.EncodeString(sCounterFormat) + ")";
                //}
                //Common.dbMgr.ExecuteNonQuery(CommandType.Text, strSQL);

                fnInsertCodeGeneration(iObjectTypeId, sCodeString, sCounterFormat, sRunningCounter, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCoderules(int iObjectTypeId)
        {

            // it is using in above commented code
            string strQuary = "select CodeString,CounterFormat,RunningCounter from CodeRules where ObjectTypeId=" + iObjectTypeId;
            return Common.dbMgr.ExecuteDataTable(CommandType.Text, strQuary);
        }
        //using for import data
        public void fnInsertCodeGeneration(int iObjectTypeId, string strCodeString, string strCounterFormat, string strRunningCounter, bool bIsManualCode)
        {
            try
            {
                SqlParameter[] sPrms = new SqlParameter[5];
                SqlParameter sPrm = new SqlParameter();
                int iIndex = 0;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Int;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@ObjectTypeId";
                sPrm.Value = iObjectTypeId;
                sPrms[iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@CodeString";
                sPrm.Value = strCodeString;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@CounterFormat";
                sPrm.Value = strCounterFormat;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.NVarChar;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@RunningCounter";
                sPrm.Value = strRunningCounter;
                sPrms[++iIndex] = sPrm;

                sPrm = new SqlParameter();
                sPrm.SqlDbType = System.Data.SqlDbType.Bit;
                sPrm.Direction = ParameterDirection.Input;
                sPrm.ParameterName = "@IsManualCode";
                sPrm.Value = bIsManualCode;
                sPrms[++iIndex] = sPrm;

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spInsertCodeGeneration", sPrms);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCodeRuleTable()
        {
            try
            {
                SqlParameter[] Parms = new SqlParameter[1];

                SqlParameter Param = new SqlParameter();
                Param.Direction = ParameterDirection.Input;
                Param.ParameterName = "@UILanguageId";
                Param.Value = CommonVariable.iLanguageId;
                Parms[0] = Param;

                return Common.dbMgr.ExecuteDataTable(CommandType.StoredProcedure, "spGetCodeRuleTable", Parms);
            }
            catch (Exception ex)
            { throw ex; }
        }




        #region GetCodeGenString

        public static DataTable GetCoderuleTable(int iObjectTypeId)
        {
            string strQuary = "select CodeString,CounterFormat,RunningCounter from CodeRules where ObjectTypeId=" + iObjectTypeId;
            return Common.dbMgr.ExecuteDataTable(CommandType.Text, strQuary);
        }
        public static int UpdateCodeRules(int iObjectTypeId, string sRunningCounter)
        {
            string sStr = "";
            try
            {
                sStr = "update CodeRules set RunningCounter=" + Common.EncodeString(sRunningCounter) + " where ObjectTypeId=" + iObjectTypeId;
                return Common.dbMgr.ExecuteNonQuery(CommandType.Text, sStr);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private static string fnGetDeptIntials(int iUserRequestId)
        {
            string sStr = "";
            try
            {

                SqlParameter[] Params = new SqlParameter[2];
                int iIndex = 0;

                Params[iIndex] = new SqlParameter("@UserRequestId", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iUserRequestId;

                Params[++iIndex] = new SqlParameter("@DeptIntials", SqlDbType.VarChar, 10);
                Params[iIndex].Direction = ParameterDirection.Output;

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spGetDepartmentInitial", Params);
                sStr = Params[iIndex].Value.ToString();

                return sStr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string GetObjectCode(int iObjectTypeId, string strINIT, string strDeptInit, int iUserRequestId = 0)
        {

            if (iUserRequestId != 0)
            {
                strDeptInit = fnGetDeptIntials(iUserRequestId);
            }

            DataTable dtrecords = GetCoderuleTable(iObjectTypeId);
            string strCodeString = ""; string Str = "";
            if (dtrecords.Rows.Count > 0)
            {
                strCodeString = dtrecords.Rows[0]["CodeString"].ToString();
                string strFormat = "%" + dtrecords.Rows[0]["CounterFormat"].ToString() + "%";
                if (dtrecords.Rows[0]["CounterFormat"].ToString() != "")
                {
                    if (dtrecords.Rows[0]["RunningCounter"].ToString() != "")
                        Str = dtrecords.Rows[0]["RunningCounter"].ToString();
                    else
                        Str = dtrecords.Rows[0]["CounterFormat"].ToString();
                    double Num;
                    bool isNum = double.TryParse(Str, out Num);
                    if (isNum)
                    {
                        if (dtrecords.Rows[0]["RunningCounter"].ToString() == "")
                        {
                            UpdateCodeRules(iObjectTypeId, Convert.ToString(Convert.ToInt32(Str)));
                            Num = Convert.ToDouble(Str);
                        }
                        else
                        {
                            UpdateCodeRules(iObjectTypeId, Convert.ToString(Convert.ToInt32(Str) + 1));
                            Num = Convert.ToDouble(Convert.ToInt32(Str) + 1);
                        }
                        Str = "%" + string.Format("{0:D" + dtrecords.Rows[0]["CounterFormat"].ToString().Length + "}", Convert.ToInt32(Num)) + "%";
                    }
                    else
                    {
                        if (dtrecords.Rows[0]["RunningCounter"].ToString() != "")
                            ShowNextValue(ref Str, Str.Length - 1);
                        UpdateCodeRules(iObjectTypeId, Str);
                    }
                    strCodeString = strCodeString.Replace(strFormat, Str);
                }
                return GetPreview(strCodeString, strINIT, strDeptInit);
            }
            else
            { return ""; }
        }
        public static string GetObjectCode(int iObjectTypeId, string strINIT, int iUserRequestId = 0)
        {

            return GetObjectCode(iObjectTypeId, strINIT, "", iUserRequestId);
        }

        public static void ShowNextValue(ref string sString, int iCharIndex)
        {
            Char c = Convert.ToChar(sString.Substring(iCharIndex, 1));
            if (c == 'z' || c == 'Z')
            {
                if (iCharIndex == 0)
                    if (c == 'z')
                        sString = "aa" + sString.Substring(1);
                    else
                        sString = "AA" + sString.Substring(1);

                else
                {
                    if (c == 'z')
                        sString = sString.Substring(0, iCharIndex) + "a" + sString.Substring(iCharIndex + 1);
                    else
                        sString = sString.Substring(0, iCharIndex) + "A" + sString.Substring(iCharIndex + 1);

                    ShowNextValue(ref sString, iCharIndex - 1);
                }
            }
            else
            {
                //sString = sString.Substring(0, iCharIndex - 1);
                string sTemp = "";
                if (iCharIndex > 0)
                { sTemp = sString.Substring(0, iCharIndex); }
                else
                { sTemp = ""; }
                sTemp += ((Char)((int)c + 1)).ToString();
                sTemp += sString.Substring(iCharIndex + 1);
                sString = sTemp;
            }
        }
        public static string GetPreview(string s, string strINIT, string strDeptInInt = "")
        {
            String[] stringSplit = new String[] { "%" };
            String strNewString = "";
            String[] strSplit = s.Split(stringSplit, StringSplitOptions.None);
            for (int i = 0; i < strSplit.Length; i++)
            {
                if (i % 2 == 0)
                {
                    strSplit[i] = strSplit[i].Replace(";", ",");
                    strNewString += strSplit[i];
                }

                else
                {
                    switch (strSplit[i].ToUpper())
                    {
                        case "YY":
                            {
                                strNewString += DateTime.Now.Year.ToString().Remove(0, 2);
                                break;
                            }
                        case "YYYY":
                            {
                                strNewString += DateTime.Now.Year.ToString();
                                break;
                            }
                        case "M":
                            {
                                strNewString += DateTime.Now.Month.ToString();
                                break;
                            }
                        case "MM":
                            {
                                if (DateTime.Now.Month.ToString().Length == 1)
                                    strNewString += "0";
                                strNewString += DateTime.Now.Month.ToString();
                                break;
                            }
                        case "D":
                            {
                                strNewString += DateTime.Now.Day.ToString();
                                break;
                            }
                        case "DD":
                            {
                                if (DateTime.Now.Day.ToString().Length == 1)
                                    strNewString += "0";
                                strNewString += DateTime.Now.Day.ToString();
                                break;
                            }
                        case "HH":
                            {
                                if (DateTime.Now.Hour.ToString().Length == 1)
                                    strNewString += "0";
                                strNewString += DateTime.Now.Hour.ToString();
                                break;
                            }
                        case "MI":
                            {
                                if (DateTime.Now.Minute.ToString().Length == 1)
                                    strNewString += "0";
                                strNewString += DateTime.Now.Minute.ToString();
                                break;
                            }
                        case "SS":
                            {
                                if (DateTime.Now.Second.ToString().Length == 1)
                                    strNewString += "0";
                                strNewString += DateTime.Now.Second.ToString();
                                break;
                            }
                        case "DN":
                            {
                                if (strDeptInInt != "")
                                    strNewString += strDeptInInt;
                                else
                                    strNewString = strNewString.Substring(0, strNewString.Length - 1);
                                // strNewString = strNewString + strINIT;
                                break;
                            }
                        case "INIT":
                            {
                                strNewString += strINIT;
                                break;
                            }
                        case "TITLE":
                            {
                                strNewString += strSplit[i];
                                break;
                            }
                        default:
                            {
                                strNewString += strSplit[i];
                                break;
                            }
                    }
                }
            }
            return strNewString;
        }
        public static bool GetId(int iObjectTypeId)
        {
            try
            {
                string strSelect = "select IsManualCode from CodeRules where ObjectTypeId=" + iObjectTypeId;
                return Convert.ToBoolean(Common.dbMgr.ExecuteScalar(CommandType.Text, strSelect));
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
