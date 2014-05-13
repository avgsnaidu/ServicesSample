using System.Runtime.Serialization;
namespace VirtusDataModel
{
    [DataContract]
    public class ContactPosition
    {
        [DataMember]
        public string PositionCode
        {
            get { return _PositionCode; }
            set { _PositionCode = value; }
        }

        [DataMember]
        public int PositionId
        {
            get;
            set;
        }


        private string _PositionCode;

        [DataMember]
        public string PositionTitle
        {
            get { return _PositionTitle; }
            set { _PositionTitle = value; }
        }
        private string _PositionTitle;

        //[DataMember]
        //public int DisplayIndex
        //{
        //    get { return _DisplayIndex; }
        //    set { _DisplayIndex = value; }
        //}
        private int _DisplayIndex;

        [DataMember]
        public int PositionFinbaseId
        {
            get { return _PositionFinbaseId; }
            set { _PositionFinbaseId = value; }
        }
        private int _PositionFinbaseId;

        /// <summary>
        /// User defined Contructor
        /// <summary>
        //public ContactPositionList(string PositionCode, 
        //    string PositionTitle, 
        //    int DisplayIndex, 
        //    int PositionFinbaseId)
        //{
        //    this._PositionCode = PositionCode;
        //    this._PositionTitle = PositionTitle;
        //    this._DisplayIndex = DisplayIndex;
        //    this._PositionFinbaseId = PositionFinbaseId;
        //}
    }
}