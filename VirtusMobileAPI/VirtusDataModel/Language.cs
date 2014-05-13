using System.Runtime.Serialization;
[DataContract]
public class Language
{
	[DataMember]
	public string LanguageCode
	{ 
		get { return _LanguageCode; }
		set { _LanguageCode = value; }
	}
	private string _LanguageCode;

[DataMember]
	public string LanguageTitle
	{ 
		get { return _LanguageTitle; }
		set { _LanguageTitle = value; }
	}
	private string _LanguageTitle;

    //[DataMember]
    //public int DisplayIndex
    //{ 
    //    get { return _DisplayIndex; }
    //    set { _DisplayIndex = value; }
    //}
	private int _DisplayIndex;

    [DataMember]
	public bool AllowRightToLeft
	{ 
		get { return _AllowRightToLeft; }
		set { _AllowRightToLeft = value; }
	}
	private bool _AllowRightToLeft;

 
    //public LanguagesList(string LanguageCode, 
    //    string LanguageTitle, 
    //    int DisplayIndex, 
    //    bool AllowRightToLeft)
    //{
    //    this._LanguageCode = LanguageCode;
    //    this._LanguageTitle = LanguageTitle;
    //    this._DisplayIndex = DisplayIndex;
    //    this._AllowRightToLeft = AllowRightToLeft;
    //}
}
