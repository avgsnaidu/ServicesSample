using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using VirtusDataModel;

namespace VirtusBI
{
    public class clsAddresses
    {
        public DataSet GetContactPositionsList()
        {
            try
            {
                try
                {
                    string strSql = "";
                    strSql = "SELECT PositionId,PositionCode,PositionTitle,PositionFinbaseId FROM dbo.ContactPositionList Order by DisplayIndex";

                    return Common.dbMgr.ExecuteDataSet(CommandType.Text, strSql);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            catch (Exception ex)
            { throw ex; }

        }

        public DataSet GetContactPositionDetails(int positionId)
        {
            try
            {
                string queryString = string.Empty;
                queryString = "SELECT PositionId,PositionCode,PositionTitle,PositionFinbaseId FROM [dbo].[ContactPositionList] Where PositionId=@PositionId Order by DisplayIndex";

                SqlParameter[] Params = new SqlParameter[1];
                int iIndex = 0;

                Params[iIndex] = new SqlParameter("@PositionId", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = positionId;

                return Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, queryString, Params);

            }
            catch (Exception ex) { throw ex; }
        }

        public int DeleteContactPosition(int positionId)
        {
            try
            {
                string queryString = string.Empty;
                queryString = "DELETE FROM [dbo].[ContactPositionList] WHERE PositionId=@PositionId";

                SqlParameter[] Params = new SqlParameter[1];
                int iIndex = 0;

                Params[iIndex] = new SqlParameter("@PositionId", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = positionId;

                return Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, queryString, Params);
            }
            catch (Exception ex) { throw ex; }
        }

        public int UpdateContactPosition(ContactPosition position)
        {

            try
            {
                string queryString = string.Empty;
                queryString = "UPDATE [dbo].[ContactPositionList] SET [PositionCode] = @PositionCode,[PositionTitle] = @PositionTitle,[PositionFinbaseId] = @PositionFinbaseId WHERE PositionId=@PositionId WHERE PositionId=@PositionId";

                SqlParameter[] Params = new SqlParameter[1];
                int iIndex = 0;

                Params[iIndex] = new SqlParameter("@PositionId", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = position.PositionId;

                Params[iIndex] = new SqlParameter(" @PositionCode", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = position.PositionCode;

                Params[iIndex] = new SqlParameter("@PositionTitle", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = position.PositionTitle;

                Params[iIndex] = new SqlParameter("@PositionFinbaseId", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = position.PositionFinbaseId;

                return Common.dbMgr.ExecuteNonQuery(CommandType.StoredProcedure, queryString, Params);
            }
            catch (Exception ex) { throw ex; }
        }

        public DataSet GetListViewDataSet(string sWhereCondition, int iUILanguageId, string strLoginName, bool bFromMyAddresses, bool bShowFromMyAddressesFromObjects, bool bShowInternalAsMyAddresses, bool bShowOnlyOpenObjects, bool bShowAllAddresses = true)
        {
            int iTotCount = 0;
            return GetListViewDataSet(sWhereCondition, "", 0, "", "", iUILanguageId, strLoginName, ref iTotCount, bFromMyAddresses, bShowFromMyAddressesFromObjects, bShowInternalAsMyAddresses, bShowOnlyOpenObjects, bShowAllAddresses);
        }

        public DataSet GetListViewDataSet(string sWhereCondition, string strSearchCondition, int iNoOfRecords, string strOrderBy, string strSortOrder, int iUILanguageId, string strLoginName, ref int iTotCount, bool bFromMyAddresses, bool bShowFromMyAddressesFromObjects, bool bShowInternalAsMyAddresses, bool bShowOnlyOpenObjects, bool bShowAllAddresses = true)
        {
            DataSet ds = new DataSet();
            try
            {
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

                Params[++iIndex] = new SqlParameter("@IsMyAddList", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bFromMyAddresses;

                Params[++iIndex] = new SqlParameter("@IsMyAddFromMyObjects", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bShowFromMyAddressesFromObjects;

                Params[++iIndex] = new SqlParameter("@ShowIntenalAsMyAddresses", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bShowInternalAsMyAddresses;

                Params[++iIndex] = new SqlParameter("@ShowOnlyOpenObjects", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bShowOnlyOpenObjects;

                Params[++iIndex] = new SqlParameter("@ShowAllAddresses", SqlDbType.Bit);
                Params[iIndex].Direction = ParameterDirection.Input;
                Params[iIndex].Value = bShowAllAddresses;

                Params[++iIndex] = new SqlParameter("@TotCount", SqlDbType.Int);
                Params[iIndex].Direction = ParameterDirection.Output;

                ds = Common.dbMgr.ExecuteDataSet(CommandType.StoredProcedure, "spGetAddressListViewRightsDS", Params);

                iTotCount = int.Parse(Params[iIndex].Value.ToString());

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
