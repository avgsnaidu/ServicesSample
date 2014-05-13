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




    }
}
