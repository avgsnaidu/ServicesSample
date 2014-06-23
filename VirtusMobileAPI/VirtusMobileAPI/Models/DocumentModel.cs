using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtusMobileAPI.Models
{
    public class DocumentActionData
    {
        /// <summary>
        /// Id of the Current uploading /saving documentId.For new record it may 0.
        /// </summary>
        public int FileId { get; set; }
        
        /// <summary>
        /// Size of the Documents like "2 MB", "1 GB", 200 byte etc..
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// Size in Bytes
        /// </summary>
        public int SizeBytes { get; set; }
        /// <summary>
        /// Document Record Type Values for =>   Actual = 0, ManualEntry = 1, Link = 2, SubObject = 3, MinutesBook = 4, Shortcut = 5
        /// </summary>
        public int DocRecordType { get; set; }

        /// <summary>
        /// Pass one of the string type -ObjectType Name in the list {"Organisation" ,"Person",  "Contact" ,"Project", "Task" ,"Decision" ,"Meeting","Agenda Item" , "User Request", "Design" , "Tender","Contract" , "Gatepass"}
        /// </summary>
        public string ObjectType { get; set; }

        /// <summary>
        /// Pass the code of the related object record to,to which current file is attaching.
        /// </summary>
        public string ObjectCode { get; set; }
        /// <summary>
        ///  Pass the Title of the related object record to,to which current file is attaching.
        /// </summary>
        public string ObjectTitle { get; set; }
        /// <summary>
        /// Name of the file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Extension of the file
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// provide the "File type " which is from file properties.
        /// </summary>
        public string FileType { get; set; }

        public string Location { get; set; }
       
          
        /// <summary>
        /// If it is protected then specify the password other wise empty.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// If the file is new then,spcifiy the "Version"=1 other wise increment of previous version
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// By Default Empty
        /// </summary>
        public string DBVersion { get; set; }
        /// <summary>
        /// Title of the File
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Subject of the File if any
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// author of the file.
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// category of the file
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// key words if the file contains any.
        /// </summary>
        public string Keywords { get; set; }
        public string Comments { get; set; }
        /// <summary>
        /// if the document is checked out then provide the checkedout by login name.
        /// </summary>
        public string CheckedOutBy { get; set; }

        /// <summary>
        /// File Record created by(login username)
        /// </summary>
        public string RecordCreatedBy { get; set; }
        public string RecordModifiedBy { get; set; }
        /// <summary>
        /// Change Type is like "N" for new , "M" for Modified , "D" for Deleted etc.
        /// </summary>
        public string ChangeType { get; set; }


        /// <summary>
        /// if the record already exists and doing updations then IsVersioned=true.
        /// </summary>
        public bool IsVersioned { get; set; }
        /// <summary>
        /// By default false
        /// </summary>
        public bool IsProtected { get; set; }
        /// <summary>
        /// If the file is Checked out then it's value is true.
        /// </summary>
        public bool IsCheckedOut { get; set; }
        /// <summary>
        /// if the file related to Invoice type "IsInVoice"=true else it is flase.
        /// </summary>
        public bool IsInvoice { get; set; }
        /// <summary>
        /// These value is from file properties
        /// </summary>
        public DateTime? CreationDate { get; set; }
        /// <summary>
        /// These value is from file properties.
        /// </summary>
        public DateTime? ModificationDate { get; set; }
        /// <summary>
        /// If any body checked out then specify the checkedout date
        /// </summary>
        public DateTime? CheckedOutOn { get; set; }
        /// <summary>
        /// File or document Uploaded date
        /// </summary>
        public DateTime? RecordCreatedOn { get; set; }
        /// <summary>
        /// For new File or Document it should be null
        /// </summary>
        public DateTime? RecordModifiedOn { get; set; }

        /// <summary>
        /// Pass the content of the file as array of bytes
        /// </summary>
        public byte[] FileContent { get; set; }

        /// <summary>
        /// pass the file content that's matches for large thumbs , for viewing in Large thumbs 
        /// </summary>
        public byte[] LargeThumb { get; set; }

        /// <summary>
        /// pass the file content that's matches for format of small thumbs, for viewing in small thumbs
        /// </summary>
        public byte[] SmallThumb { get; set; }



    }

}