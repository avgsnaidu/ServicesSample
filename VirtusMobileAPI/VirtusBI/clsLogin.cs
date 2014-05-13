using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace VirtusBI
{
    public class clsLogin
    {
        public DataSet fnCheckUser(string strUserName)
        {
            try
            {

                SqlParameter[] Params = new SqlParameter[1];
                int iIndex = 0;


                Params[iIndex] = new SqlParameter("@UserName", SqlDbType.NVarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = strUserName;

                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetUserDetailsDS", Params);

                //string strSql = "";
                //strSql = "select addresseid as userID,Gender,FullName as Name,";
                //strSql += " loginusertypeid as usertype,corrLanguage,loginpassword,isActive,isnull(Initials,'') as Initials,UseWindowsPassword from Addresses ";
                //strSql += " where loginname=" + Common.EncodeString(strUserName);
                //return Common.dbMgr.ExecuteDataSet(CommandType.Text, strSql);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        public void fnSaveCurrentLoginDeatils(int iLoginUserId, string strIPAddress, string strWindowsUser)
        {
            try
            {
                SqlParameter[] Params = new SqlParameter[3];
                int iIndex = 0;


                Params[iIndex] = new SqlParameter("@IPAddress", SqlDbType.VarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = strIPAddress;


                Params[++iIndex] = new SqlParameter("@LoginUserId", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = iLoginUserId;

                Params[++iIndex] = new SqlParameter("@WindowsUser", SqlDbType.NVarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = strWindowsUser;

                Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, "spSaveCurrentLoggedUser", Params);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool fnCheckUserName(string strUserName, ref bool bIsWindowsPassword)
        {
            bool bIsAutologin = false;
            bool bIsActive = false;
            bool bIsWindowsPWd = bIsWindowsPassword;
            return fnCheckUserName(strUserName, ref bIsAutologin, ref bIsWindowsPassword, ref bIsActive);
        }

        public bool fnCheckUserName(string strUserName, ref bool bIsAutologin, ref bool bIsWindowsPassword, ref bool bIsActive)
        {
            try
            {
                //string strSql = "select AutoLogin,UseWindowsPassword,LoginPassword,LoginName,IsActive,LoginUserTypeId from addresses where loginName=" + Common.EncodeString(strUserName);
                //DataSet ds = Common.dbMgr.ExecuteDataSet(CommandType.Text, strSql);


                SqlParameter[] Params = new SqlParameter[1];
                int iIndex = 0;


                Params[iIndex] = new SqlParameter("@UserName", SqlDbType.NVarChar);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = strUserName;

                DataSet ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetUserDetailsDS", Params);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if ((ds.Tables[0].Rows[0]["UseWindowsPassword"] != null) && (ds.Tables[0].Rows[0]["UseWindowsPassword"].ToString() != ""))
                        bIsWindowsPassword = Convert.ToBoolean(ds.Tables[0].Rows[0]["UseWindowsPassword"]);

                    if ((ds.Tables[0].Rows[0]["AutoLogin"] != null) && (ds.Tables[0].Rows[0]["AutoLogin"].ToString() != ""))
                        bIsAutologin = Convert.ToBoolean(ds.Tables[0].Rows[0]["AutoLogin"]);


                    if ((ds.Tables[0].Rows[0]["IsActive"] != null) && (ds.Tables[0].Rows[0]["IsActive"].ToString() != ""))
                        bIsActive = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsActive"]);


                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
