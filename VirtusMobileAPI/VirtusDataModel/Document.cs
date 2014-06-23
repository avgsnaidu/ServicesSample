using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtusDataModel
{
    public class Document
    {

        /// <summary>
        /// Document RecordId
        /// </summary>
        public int RecordId { get; set; }

        /// <summary>
        /// Size of the Document
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// Size Bytes
        /// </summary>
        public int SizeBytes { get; set; }
        public int DocRecordType { get; set; }


        public string ObjectType { get; set; }
        public string ObjectCode { get; set; }
        public string ObjectTitle { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FileType { get; set; }

        public string Location { get; set; }
        public string VersionedNumber { get; set; }
        public string ProtectedNo { get; set; }
        public string Password { get; set; }

        public string Version { get; set; }
        public string DBVersion { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public string Keywords { get; set; }
        public string Comments { get; set; }
        public string CheckedOutBy { get; set; }
        public string RecordCreatedBy { get; set; }
        public string RecordModifiedBy { get; set; }
        /// <summary>
        /// Change Type is like "N" for new , "M" for Modified , "D" for Deleted etc.
        /// </summary>
        public string ChangeType { get; set; }



        public bool IsVersioned { get; set; }
        public bool IsProtected { get; set; }
        public bool IsCheckedOut { get; set; }
        public bool IsInvoice { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public DateTime CheckedOutOn { get; set; }
        public DateTime RecordCreatedOn { get; set; }
        public DateTime RecordModifiedOn { get; set; }

        //public byte[] MyProperty { get; set; }
        //public byte[] MyProperty { get; set; }
        //public byte[] MyProperty { get; set; }
        //public byte[] MyProperty { get; set; }






    }


    public class DocumentActionData
    {
        /// <summary>
        /// Document RecordId
        /// </summary>
        public int RecordId { get; set; }
        /// <summary>
        /// Size of the Document
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// Size Bytes
        /// </summary>
        public int SizeBytes { get; set; }
        public int DocRecordType { get; set; }
        public string ObjectType { get; set; }
        public string ObjectCode { get; set; }
        public string ObjectTitle { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FileType { get; set; }

        public string Location { get; set; }
        public string VersionedNumber { get; set; }
        public string ProtectedNo { get; set; }
        public string Password { get; set; }

        public string Version { get; set; }
        public string DBVersion { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public string Keywords { get; set; }
        public string Comments { get; set; }
        public string CheckedOutBy { get; set; }
        public string RecordCreatedBy { get; set; }
        public string RecordModifiedBy { get; set; }
        /// <summary>
        /// Change Type is like "N" for new , "M" for Modified , "D" for Deleted etc.
        /// </summary>
        public string ChangeType { get; set; }



        public bool IsVersioned { get; set; }
        public bool IsProtected { get; set; }
        public bool IsCheckedOut { get; set; }
        public bool IsInvoice { get; set; }

        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public DateTime? CheckedOutOn { get; set; }
        public DateTime? RecordCreatedOn { get; set; }
        public DateTime? RecordModifiedOn { get; set; }

        public byte[] FileContent { get; set; }
        public byte[] LargeThumb { get; set; }
        public byte[] SmallThumb { get; set; }
        


    }

}


