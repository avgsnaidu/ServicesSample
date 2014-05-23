using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtusBI
{
    public class clsBaseBI
    {
        public bool BeginTrans()
        {
            return Common.dbMgr.BeginTrans();
        }
        public bool CommitTrans()
        {
            return Common.dbMgr.CommitTrans();
        }
        public bool RollbackTrans()
        {
            return Common.dbMgr.RoolbackTrans();
        }

        #region OracleTransations

        #endregion
    }
}
