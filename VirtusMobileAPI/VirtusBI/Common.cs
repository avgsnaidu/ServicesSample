using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using VirtusDB;

namespace VirtusBI
{
    public static class Common
    {
        public static DateTime SQLServerZeroDate = new DateTime(1900, 1, 1);
        //public static DBManager dbMgr = new DBManager(ConfigurationSettings.AppSettings["DBConnectionString"].ToString());

        public static DBManager dbMgr = null;


        #region "Encrypt/Decrypt"

        public static string GetEncriptedValue(string strOriginalValue)
        {
            // First we need to turn the input string into a byte array. 
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(strOriginalValue);

            // Then, we need to turn the password into Key and IV 
            // We are using salt to make it harder to guess our key
            // using a dictionary attack - 
            // trying to guess a password by enumerating all possible words. 
            PasswordDeriveBytes pdb = new PasswordDeriveBytes("EPM",
            new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

            // Now get the key/IV and do the encryption using the
            // function that accepts byte arrays. 
            // Using PasswordDeriveBytes object we are first getting
            // 32 bytes for the Key 
            // (the default Rijndael key length is 256bit = 32bytes)
            // and then 16 bytes for the IV. 
            // IV should always be the block size, which is by default
            // 16 bytes (128 bit) for Rijndael. 
            // If you are using DES/TripleDES/RC2 the block size is
            // 8 bytes and so should be the IV size. 
            // You can also read KeySize/BlockSize properties off
            // the algorithm to find out the sizes. 
            byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));

            // Now we need to turn the resulting byte array into a string. 
            // A common mistake would be to use an Encoding class for that.
            //It does not work because not all byte values can be
            // represented by characters. 
            // We are going to be using Base64 encoding that is designed
            //exactly for what we are trying to do. 
            return Convert.ToBase64String(encryptedData);
        }

        //public static string GetEncriptedValue(string strOriginalValue)
        //{
        //    string strReturn = "";
        //    for (int iCounter = strOriginalValue.Length - 1; iCounter >= 0; iCounter--)
        //    {
        //        strReturn += Microsoft.VisualBasic.Strings.Chr(iCounter + Microsoft.VisualBasic.Strings.Asc(strOriginalValue.Substring(iCounter, 1)));
        //    }
        //    return strReturn;
        //}

        public static string GetDecryptedValue(string strEncryptedValue)
        {
            // First we need to turn the input string into a byte array. 
            // We presume that Base64 encoding was used 
            byte[] cipherBytes = Convert.FromBase64String(strEncryptedValue);

            // Then, we need to turn the password into Key and IV 
            // We are using salt to make it harder to guess our key
            // using a dictionary attack - 
            // trying to guess a password by enumerating all possible words. 
            PasswordDeriveBytes pdb = new PasswordDeriveBytes("EPM",
                new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });


            // Now get the key/IV and do the decryption using
            // the function that accepts byte arrays. 
            // Using PasswordDeriveBytes object we are first
            // getting 32 bytes for the Key 
            // (the default Rijndael key length is 256bit = 32bytes)
            // and then 16 bytes for the IV. 
            // IV should always be the block size, which is by
            // default 16 bytes (128 bit) for Rijndael. 
            // If you are using DES/TripleDES/RC2 the block size is
            // 8 bytes and so should be the IV size. 
            // You can also read KeySize/BlockSize properties off
            // the algorithm to find out the sizes. 
            byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));

            // Now we need to turn the resulting byte array into a string. 
            // A common mistake would be to use an Encoding class for that.
            // It does not work 
            // because not all byte values can be represented by characters. 
            // We are going to be using Base64 encoding that is 
            // designed exactly for what we are trying to do. 
            return System.Text.Encoding.Unicode.GetString(decryptedData);

        }

        // Encrypt a byte array into a byte array using a key and an IV 
        private static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            // Create a MemoryStream to accept the encrypted bytes 
            MemoryStream ms = new MemoryStream();

            // Create a symmetric algorithm. 
            // We are going to use Rijndael because it is strong and
            // available on all platforms. 
            // You can use other algorithms, to do so substitute the
            // next line with something like 
            //      TripleDES alg = TripleDES.Create(); 
            Rijndael alg = Rijndael.Create();

            // Now set the key and the IV. 
            // We need the IV (Initialization Vector) because
            // the algorithm is operating in its default 
            // mode called CBC (Cipher Block Chaining).
            // The IV is XORed with the first block (8 byte) 
            // of the data before it is encrypted, and then each
            // encrypted block is XORed with the 
            // following block of plaintext.
            // This is done to make encryption more secure. 

            // There is also a mode called ECB which does not need an IV,
            // but it is much less secure. 
            alg.Key = Key;
            alg.IV = IV;

            // Create a CryptoStream through which we are going to be
            // pumping our data. 
            // CryptoStreamMode.Write means that we are going to be
            // writing data to the stream and the output will be written
            // in the MemoryStream we have provided. 
            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the encryption 
            cs.Write(clearData, 0, clearData.Length);

            // Close the crypto stream (or do FlushFinalBlock). 
            // This will tell it that we have done our encryption and
            // there is no more data coming in, 
            // and it is now a good time to apply the padding and
            // finalize the encryption process. 
            cs.Close();

            // Now get the encrypted data from the MemoryStream.
            // Some people make a mistake of using GetBuffer() here,
            // which is not the right way. 
            byte[] encryptedData = ms.ToArray();

            return encryptedData;
        }

        // Decrypt a byte array into a byte array using a key and an IV 
        private static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            // Create a MemoryStream that is going to accept the
            // decrypted bytes 
            MemoryStream ms = new MemoryStream();

            // Create a symmetric algorithm. 
            // We are going to use Rijndael because it is strong and
            // available on all platforms. 
            // You can use other algorithms, to do so substitute the next
            // line with something like 
            //     TripleDES alg = TripleDES.Create(); 
            Rijndael alg = Rijndael.Create();

            // Now set the key and the IV. 
            // We need the IV (Initialization Vector) because the algorithm
            // is operating in its default 
            // mode called CBC (Cipher Block Chaining). The IV is XORed with
            // the first block (8 byte) 
            // of the data after it is decrypted, and then each decrypted
            // block is XORed with the previous 
            // cipher block. This is done to make encryption more secure. 
            // There is also a mode called ECB which does not need an IV,
            // but it is much less secure. 
            alg.Key = Key;
            alg.IV = IV;

            // Create a CryptoStream through which we are going to be
            // pumping our data. 
            // CryptoStreamMode.Write means that we are going to be
            // writing data to the stream 
            // and the output will be written in the MemoryStream
            // we have provided. 
            CryptoStream cs = new CryptoStream(ms,
                alg.CreateDecryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the decryption 
            cs.Write(cipherData, 0, cipherData.Length);

            // Close the crypto stream (or do FlushFinalBlock). 
            // This will tell it that we have done our decryption
            // and there is no more data coming in, 
            // and it is now a good time to remove the padding
            // and finalize the decryption process. 
            cs.Close();

            // Now get the decrypted data from the MemoryStream. 
            // Some people make a mistake of using GetBuffer() here,
            // which is not the right way. 
            byte[] decryptedData = ms.ToArray();

            return decryptedData;
        }

        //public static string GetDecryptedValue(string strEncryptedValue)
        //{
        //    string strReturn = "";
        //    for (int iCounter = strEncryptedValue.Length - 1; iCounter >= 0; iCounter--)
        //    {
        //        strReturn += Microsoft.VisualBasic.Strings.Chr( Microsoft.VisualBasic.Strings.Asc(strEncryptedValue.Substring(iCounter, 1)) - (strEncryptedValue.Length - 1 - iCounter));
        //    }
        //    return strReturn;
        //}

        #endregion "Encrypt/Decrypt"

        public static void SetConnectionString(string sConnStr)
        {
            dbMgr = new DBManager(sConnStr);
            //dbMgr.strProductName = strProductName;
        }

        #region OracleConnection
        //public static OraDBManager oraDbMgr = null;

        public static void SetOraConnectionString(string sOraConnStr)
        {
            //oraDbMgr = new OraDBManager(sOraConnStr);
            //oraDbMgr.strProductName = strProductName;

        }

        #endregion

        public static string AppPath()
        {
            string sLocation = "";
            sLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return sLocation;
        }

        public static string GetEPMTempPath()
        {
            string sMosTempPath = Path.GetTempPath();
            if (sMosTempPath.EndsWith(@"\") == false)
            { sMosTempPath += @"\"; }
            sMosTempPath += @"Virtus\";
            if (Directory.Exists(sMosTempPath) == false)
            {
                Directory.CreateDirectory(sMosTempPath);
            }
            return sMosTempPath;
        }

        public static string GetAllObjectsQuery(bool bProjectViewRight, bool bTaskViewRight, bool bDecisionViewRight, bool bAgendaItemViewRight, bool bMeetingViewRight, string sLoginName)
        {
            int iProjectViewRight = 0;
            int iTaskViewRight = 0;
            int iDecisionViewRight = 0;
            int iAgendaItemViewRight = 0;
            int iMeetingViewRight = 0;

            if (bProjectViewRight)
                iProjectViewRight = 1;

            if (bTaskViewRight)
                iTaskViewRight = 1;

            if (bDecisionViewRight)
                iDecisionViewRight = 1;

            if (bAgendaItemViewRight)
                iAgendaItemViewRight = 1;

            if (bMeetingViewRight)
                iMeetingViewRight = 1;



            string strSql = "";

            //strSql = "(Select Top 100000";
            //strSql += " ObjectTypeId,";
            //strSql += " Null As Icon,";
            //strSql += " Case ";
            //strSql += " When ObjectTypeId=" + ((int)Enums.VTSObjects.Organization).ToString() + " then " + Common.EncodeString(Common.TranslateText(Enums.InterFaces.Database, "Organisation", Enums.TextType.FieldValues, CommonVariable.iLanguageId));
            //strSql += " When ObjectTypeId=" + ((int)Enums.VTSObjects.Person).ToString() + " then " + Common.EncodeString(Common.TranslateText(Enums.InterFaces.Database, "Person", Enums.TextType.FieldValues, CommonVariable.iLanguageId));
            //strSql += " When ObjectTypeId=" + ((int)Enums.VTSObjects.Contact).ToString() + " then " + Common.EncodeString(Common.TranslateText(Enums.InterFaces.Database, "Contact", Enums.TextType.FieldValues, CommonVariable.iLanguageId));
            //strSql += " When ObjectTypeId=" + ((int)Enums.VTSObjects.Project).ToString() + " then " + Common.EncodeString(Common.TranslateText(Enums.InterFaces.Database, "Project", Enums.TextType.FieldValues, CommonVariable.iLanguageId));
            //strSql += " When ObjectTypeId=" + ((int)Enums.VTSObjects.Task).ToString() + " then " + Common.EncodeString(Common.TranslateText(Enums.InterFaces.Database, "Task", Enums.TextType.FieldValues, CommonVariable.iLanguageId));
            //strSql += " When ObjectTypeId=" + ((int)Enums.VTSObjects.Decision).ToString() + " then " + Common.EncodeString(Common.TranslateText(Enums.InterFaces.Database, "Decision", Enums.TextType.FieldValues, CommonVariable.iLanguageId));
            //strSql += " When ObjectTypeId=" + ((int)Enums.VTSObjects.AgendaItem).ToString() + " then " + Common.EncodeString(Common.TranslateText(Enums.InterFaces.Database, "Agenda Item", Enums.TextType.FieldValues, CommonVariable.iLanguageId));
            //strSql += " When ObjectTypeId=" + ((int)Enums.VTSObjects.Meeting).ToString() + " then " + Common.EncodeString(Common.TranslateText(Enums.InterFaces.Database, "Meeting", Enums.TextType.FieldValues, CommonVariable.iLanguageId));
            //strSql += " End As Type,";
            //strSql += " Id, Code, Title, Name,FirstName,AdditionalName,isnull(Gender,'') as Gender, ";
            //strSql += " Case When PriorityId=" + ((int)Enums.Priority.Low).ToString() + " then '" + Common.TranslateText(Enums.InterFaces.Database, "Low", Enums.TextType.FieldValues, CommonVariable.iLanguageId) + "'";
            //strSql += " When PriorityId=" + ((int)Enums.Priority.Normal).ToString() + " then '" + Common.TranslateText(Enums.InterFaces.Database, "Normal", Enums.TextType.FieldValues, CommonVariable.iLanguageId) + "'";
            //strSql += " When PriorityId=" + ((int)Enums.Priority.High).ToString() + " then '" + Common.TranslateText(Enums.InterFaces.Database, "High", Enums.TextType.FieldValues, CommonVariable.iLanguageId) + "' End as Priority,";
            //strSql += " Customer,";
            //strSql += " KindTitle,";
            //strSql += " IsPartOf,";
            //strSql += " OnbehalfOf,";
            //strSql += " Address, ZipCodeCity, StateCode, ";
            //strSql += " Country,";
            //strSql += " Phone,Email,Fax,Mobile,Messenger,Pager,URL,";
            //strSql += " IsActive,";
            //strSql += " Case When IsActive = 0 then " + Common.EncodeString(CommonVariable.strNo);
            //strSql += " else " + Common.EncodeString(CommonVariable.strYes) + " End As Active,";

            //strSql += " Case When IsManualCode = 0 then " + Common.EncodeString(CommonVariable.strNo);
            //strSql += " else " + Common.EncodeString(CommonVariable.strYes) + " End As IsManualCode,";

            //strSql += " Case When IsDone = 0 then " + Common.EncodeString(CommonVariable.strNo);
            //strSql += " else " + Common.EncodeString(CommonVariable.strYes) + " End As IsDone,";
            //strSql += " IsInternal,";
            //strSql += " Case When IsInternal = 0 then " + Common.EncodeString(CommonVariable.strNo);
            //strSql += " else " + Common.EncodeString(CommonVariable.strYes) + " End As Internal,";

            //strSql += " CreatedBy, CreatedOn, ModifiedBy, ModifiedOn ";

            //strSql += " From dbo.fnAllObjectsWithFieldSecurity(" + iProjectViewRight.ToString() + "," + iTaskViewRight.ToString() + "," + iDecisionViewRight.ToString() + "," + iAgendaItemViewRight.ToString() + "," + iMeetingViewRight.ToString() + "," + Common.EncodeString(sLoginName) + " )";
            //strSql += ")";

            return strSql;

        }

        public static string EncodeString(string str)
        {
            if (str == null || str.Trim() == "" || str == "00:00")
                return "NULL";
            else
            {
                str = str.Replace("'", "''");
                str = "'" + str.Trim() + "'";
                return str;
            }
        }

        public static string EncodeNString(string str)
        {
            if (str == null || str.Trim() == "" || str == "00:00")
                return "NULL";
            else
            {
                str = str.Replace("'", "''");
                str = "'" + str.Trim() + "'";
                return "N" + str;
            }
        }

        public static string EncodeZeroTime(string str)
        {
            if (str == null || str.Trim() == "")
                return "NULL";
            else
            {
                str = str.Replace("'", "''");
                str = "'" + str.Trim() + "'";
                return str;
            }
        }
        public static string EncodeValue(decimal Val)
        {
            if (Val == 0)
                return "Null";
            else
                return Val.ToString();
        }

        public static string EncodeValue(int Val)
        {
            if (Val == 0)
                return "Null";
            else
                return Val.ToString();
        }

        public static DataSet dsCommonTranslations;
        static DataSet dsTranslations;
        //static Enums.InterFaces curTranslationsDS = Enums.InterFaces.None;
        static int curLanguageId = 0;
        //public static clsLanguages cls = new clsLanguages();

        //public static string TranslateText(Enums.InterFaces interfaceType, string strDefault, Enums.TextType strMessageType, int iLanguageId)
        //{
        //    clsLanguages cls = new clsLanguages();
        //    if ((interfaceType == Enums.InterFaces.ListView || interfaceType == Enums.InterFaces.Home) && iLanguageId == curLanguageId)
        //    {
        //        dsTranslations = dsCommonTranslations;
        //        curTranslationsDS = interfaceType;
        //    }
        //    else if ((interfaceType != curTranslationsDS) || (iLanguageId != curLanguageId))
        //    {
        //        //update the data set
        //        dsTranslations = cls.fnGetTranslatedValues(((int)interfaceType).ToString(), iLanguageId);
        //        curTranslationsDS = interfaceType;
        //        curLanguageId = iLanguageId;
        //        OpenCommonTranslation(iLanguageId);
        //    }
        //    if (dsTranslations != null)
        //    {
        //        DataRow[] dr = dsTranslations.Tables[0].Select("DefaultValue=" + EncodeString(strDefault) + " and TextType = " + EncodeString(((int)strMessageType).ToString()));
        //        if (dr.Length > 0)
        //        {
        //            return dr[0]["TranslatedValue"].ToString();
        //        }
        //        else
        //        {
        //            if (dsCommonTranslations == null)
        //            {
        //                OpenCommonTranslation(iLanguageId);
        //            }
        //            dr = dsCommonTranslations.Tables[0].Select("DefaultValue=" + EncodeString(strDefault) + " and TextType = " + EncodeString(((int)strMessageType).ToString()));
        //            if (dr.Length > 0)
        //            {
        //                return dr[0]["TranslatedValue"].ToString();
        //            }


        //        }
        //    }
        //    return strDefault;
        //}

        //public static void OpenCommonTranslation(int iLanguageId)
        //{
        //    string strInterfaceIds = (int)Enums.InterFaces.Common + "," + (int)Enums.InterFaces.ListView + "," + (int)Enums.InterFaces.Home;
        //    dsCommonTranslations = cls.fnGetTranslatedValues(strInterfaceIds, iLanguageId); ;
        //}

        public static void GetThumbs(string sFilePath, ref Image sFileSmallThumb, ref Image sFileLargeThumb)
        {
            //this procedure is used to generate the thumb image in two sizes (small and large).
            if (File.Exists(sFilePath))
            {
                Image img = null;
                try
                {
                    img = Image.FromFile(sFilePath, true);
                    sFileSmallThumb = FixedSize(img, 64, 64);
                    sFileLargeThumb = FixedSize(img, 128, 128);
                }
                catch (Exception ex)
                {
                    FileInfo fi = new FileInfo(sFilePath);

                    //IconExtractor IconExtractor = new IconExtractor();
                    System.Drawing.Icon Icon;
                    //Icon = IconExtractor.Extract(fi.Extension, IconSize.Large);
                    //img = Icon.ToBitmap();
                    sFileSmallThumb = FixedSize1(img, 64, 64);
                    sFileLargeThumb = FixedSize1(img, 128, 128);
                    fi = null;
                    ex = null;
                }
            }
        }

        private static Image FixedSize(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);

            //if we have to pad the height pad both the top and the bottom 
            //with the difference between the scaled height and the desired height 
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = (int)((Width - (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = (int)((Height - (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);

            grPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto, new Rectangle(destX, destY, destWidth, destHeight), new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);
            //grPhoto.DrawIconUnstretched(imgPhoto, new Rectangle(destX, destY, destWidth, destHeight), new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        private static Image FixedSize1(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;

            Bitmap bmPhoto = new Bitmap(Width, Height);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);

            grPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto, new Rectangle((Width - 32) / 2, (Width - 32) / 2, 32, 32), new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        private static bool ThumbnailCallback()
        {
            return true;

        }

        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        #region [("Enums Usage")]
        //public static DateTime AddDurationToDate(DateTime dt, Enums.Remind_Durations d)
        //{
        //    return AddRemoveDuration(dt, d, false);
        //}
        //public static DateTime RemoveDurationFromDate(DateTime dt, Enums.Remind_Durations d)
        //{
        //    return AddRemoveDuration(dt, d, true);
        //}

        //private static DateTime AddRemoveDuration(DateTime dt, Enums.Remind_Durations d, bool bForRemove)
        //{
        //    int i = 1;

        //    if (bForRemove)
        //        i = -1;

        //    if (d == Enums.Remind_Durations.Minute_1)
        //        return dt.AddMinutes(i * 1);
        //    else if (d == Enums.Remind_Durations.Minutes_5)
        //        return dt.AddMinutes(i * 5);
        //    else if (d == Enums.Remind_Durations.Minutes_10)
        //        return dt.AddMinutes(i * 10);
        //    else if (d == Enums.Remind_Durations.Minutes_15)
        //        return dt.AddMinutes(i * 15);
        //    else if (d == Enums.Remind_Durations.Minutes_30)
        //        return dt.AddMinutes(i * 30);
        //    else if (d == Enums.Remind_Durations.Hour_1)
        //        return dt.AddHours(i * 1);
        //    else if (d == Enums.Remind_Durations.Hours_2)
        //        return dt.AddHours(i * 2);
        //    else if (d == Enums.Remind_Durations.Hours_3)
        //        return dt.AddHours(i * 3);
        //    else if (d == Enums.Remind_Durations.Hours_4)
        //        return dt.AddHours(i * 4);
        //    else if (d == Enums.Remind_Durations.Hours_5)
        //        return dt.AddHours(i * 5);
        //    else if (d == Enums.Remind_Durations.Hours_6)
        //        return dt.AddHours(i * 6);
        //    else if (d == Enums.Remind_Durations.Hours_7)
        //        return dt.AddHours(i * 7);
        //    else if (d == Enums.Remind_Durations.Hours_8)
        //        return dt.AddHours(i * 8);
        //    else if (d == Enums.Remind_Durations.Hours_9)
        //        return dt.AddHours(i * 9);
        //    else if (d == Enums.Remind_Durations.Hours_10)
        //        return dt.AddHours(i * 10);
        //    else if (d == Enums.Remind_Durations.Hours_11)
        //        return dt.AddHours(i * 11);
        //    else if (d == Enums.Remind_Durations.Hours_12)
        //        return dt.AddHours(i * 12);
        //    else if (d == Enums.Remind_Durations.Hours_18)
        //        return dt.AddHours(i * 18);
        //    else if (d == Enums.Remind_Durations.Day_1)
        //        return dt.AddDays(i * 1);
        //    else if (d == Enums.Remind_Durations.Days_2)
        //        return dt.AddDays(i * 2);
        //    else if (d == Enums.Remind_Durations.Days_3)
        //        return dt.AddDays(i * 3);
        //    else if (d == Enums.Remind_Durations.Days_4)
        //        return dt.AddDays(i * 4);
        //    else if (d == Enums.Remind_Durations.Days_5)
        //        return dt.AddDays(i * 5);
        //    else if (d == Enums.Remind_Durations.Days_6)
        //        return dt.AddDays(i * 6);
        //    else if (d == Enums.Remind_Durations.Week_1)
        //        return dt.AddDays(i * 7 * 1);
        //    else if (d == Enums.Remind_Durations.Weeks_2)
        //        return dt.AddDays(i * 7 * 2);
        //    else if (d == Enums.Remind_Durations.Weeks_3)
        //        return dt.AddDays(i * 7 * 3);
        //    else if (d == Enums.Remind_Durations.Month_1)
        //        return dt.AddMonths(1);
        //    else
        //        return dt;
        //}

        //public static string ProcessedByRemindLoginNamesTable(string sobjTypeIds, string sRecordId)
        //{
        //    string sSql = "";
        //    sSql = "Select Distinct ";
        //    sSql += "isnull(D.LoginName,B.LoginName) as LoginName ";
        //    sSql += " From ProcessedBy A ";
        //    sSql += " Inner Join Addresses B on A.PerformedBy=B.AddresseId and A.Remind=1 ";
        //    sSql += " Left Join Addresses C on B.AddresseId=C.OrganizationId ";
        //    sSql += " Left Join Addresses D on C.PersonId=D.AddresseId OR B.PersonId=D.AddresseId";
        //    sSql += " Where ";
        //    sSql += " ObjectType in (" + sobjTypeIds + ")";
        //    sSql += " And ObjectId=" + sRecordId;
        //    sSql += " And isnull(D.LoginName,B.LoginName) is not null ";
        //    sSql += " And A.Remind=1 ";
        //    return sSql;
        //}

        //public static void GetRemindPeriod(Enums.Remind_Durations dur, ref string sPeriodType, ref int iPeriodValue)
        //{
        //    if (dur == Enums.Remind_Durations.Minute_1)
        //    {
        //        sPeriodType = "mi";
        //        iPeriodValue = -1;
        //    }
        //    else if (dur == Enums.Remind_Durations.Minutes_5)
        //    {
        //        sPeriodType = "mi";
        //        iPeriodValue = -5;
        //    }
        //    else if (dur == Enums.Remind_Durations.Minutes_10)
        //    {
        //        sPeriodType = "mi";
        //        iPeriodValue = -10;
        //    }
        //    else if (dur == Enums.Remind_Durations.Minutes_15)
        //    {
        //        sPeriodType = "mi";
        //        iPeriodValue = -15;
        //    }
        //    else if (dur == Enums.Remind_Durations.Minutes_30)
        //    {
        //        sPeriodType = "mi";
        //        iPeriodValue = -30;
        //    }
        //    else if (dur == Enums.Remind_Durations.Hour_1)
        //    {
        //        sPeriodType = "hh";
        //        iPeriodValue = -1;
        //    }
        //    else if (dur == Enums.Remind_Durations.Hours_2)
        //    {
        //        sPeriodType = "hh";
        //        iPeriodValue = -2;
        //    }
        //    else if (dur == Enums.Remind_Durations.Hours_3)
        //    {
        //        sPeriodType = "hh";
        //        iPeriodValue = -3;
        //    }
        //    else if (dur == Enums.Remind_Durations.Hours_4)
        //    {
        //        sPeriodType = "hh";
        //        iPeriodValue = -4;
        //    }
        //    else if (dur == Enums.Remind_Durations.Hours_5)
        //    {
        //        sPeriodType = "hh";
        //        iPeriodValue = -5;
        //    }
        //    else if (dur == Enums.Remind_Durations.Hours_6)
        //    {
        //        sPeriodType = "hh";
        //        iPeriodValue = -6;
        //    }
        //    else if (dur == Enums.Remind_Durations.Hours_7)
        //    {
        //        sPeriodType = "hh";
        //        iPeriodValue = -7;
        //    }
        //    else if (dur == Enums.Remind_Durations.Hours_8)
        //    {
        //        sPeriodType = "hh";
        //        iPeriodValue = -8;
        //    }
        //    else if (dur == Enums.Remind_Durations.Hours_9)
        //    {
        //        sPeriodType = "hh";
        //        iPeriodValue = -9;
        //    }
        //    else if (dur == Enums.Remind_Durations.Hours_10)
        //    {
        //        sPeriodType = "hh";
        //        iPeriodValue = -10;
        //    }
        //    else if (dur == Enums.Remind_Durations.Hours_11)
        //    {
        //        sPeriodType = "hh";
        //        iPeriodValue = -11;
        //    }
        //    else if (dur == Enums.Remind_Durations.Hours_12)
        //    {
        //        sPeriodType = "hh";
        //        iPeriodValue = -12;
        //    }
        //    else if (dur == Enums.Remind_Durations.Hours_18)
        //    {
        //        sPeriodType = "hh";
        //        iPeriodValue = -18;
        //    }
        //    else if (dur == Enums.Remind_Durations.Day_1)
        //    {
        //        sPeriodType = "dd";
        //        iPeriodValue = -1;
        //    }
        //    else if (dur == Enums.Remind_Durations.Days_2)
        //    {
        //        sPeriodType = "dd";
        //        iPeriodValue = -2;
        //    }
        //    else if (dur == Enums.Remind_Durations.Days_3)
        //    {
        //        sPeriodType = "dd";
        //        iPeriodValue = -3;
        //    }
        //    else if (dur == Enums.Remind_Durations.Days_4)
        //    {
        //        sPeriodType = "dd";
        //        iPeriodValue = -4;
        //    }
        //    else if (dur == Enums.Remind_Durations.Days_5)
        //    {
        //        sPeriodType = "dd";
        //        iPeriodValue = -5;
        //    }
        //    else if (dur == Enums.Remind_Durations.Days_6)
        //    {
        //        sPeriodType = "dd";
        //        iPeriodValue = -6;
        //    }
        //    else if (dur == Enums.Remind_Durations.Week_1)
        //    {
        //        sPeriodType = "dd";
        //        iPeriodValue = -7;
        //    }
        //    else if (dur == Enums.Remind_Durations.Weeks_2)
        //    {
        //        sPeriodType = "dd";
        //        iPeriodValue = 2 * -7;
        //    }
        //    else if (dur == Enums.Remind_Durations.Weeks_3)
        //    {
        //        sPeriodType = "dd";
        //        iPeriodValue = 3 * -7;
        //    }
        //    else if (dur == Enums.Remind_Durations.Month_1)
        //    {
        //        sPeriodType = "mm";
        //        iPeriodValue = -1;
        //    }
        //    else
        //    {
        //        sPeriodType = "dd";
        //        iPeriodValue = 0;
        //    }
        //}

        #endregion


    }


    public static class CommonVariable
    {
        public static int iLanguageId = 3;
        public static string strYes = "Yes";
        public static string strNo = "No";
    }
}
