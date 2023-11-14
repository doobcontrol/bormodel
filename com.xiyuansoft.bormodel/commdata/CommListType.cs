using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace com.xiyuansoft.bormodel.commdata
{
    public class CommListType : KBoModel
    {
        #region 单例模式
        protected CommListType()
        {
            boModelID = CommListType.BoModelID;
            tableName = CommListType.TableName;   //表名（显示名称）
            tableCode = CommListType.TableCode;   //表物理名（数据库操作名）	
        }

        private static CommListType single;

        /**
         * 返回本类唯一实例，饿汉式单例类
         */
        public static CommListType getnSingInstance()
        {
            if (single == null)
            {
                single = new CommListType();
            }
            return single;
        }

        #endregion

        static public String BoModelID = "5DA23728-C0D6-4992-BCA8-D4A7BC232A9A";
        static public String TableName = "通用静态数据类型表";   //表名（显示名称）
        static public String TableCode = "tCommListType";   //表物理名（数据库操作名）	

        //字段（属性）名称定义
        static public String fIsTree = "ISTREE"; //非tree不允许多层 

        //系统使用到类型ID定义
        static public String ID_SEX = "7B0D04B8-D03C-4A89-A715-00362DD1FE1E"; //姓别
        //代码ID定义
        static public String ID_SEX_0 = "4DD0D907-0956-4F07-B0BF-17EFC837911E"; //未知
        static public String ID_SEX_1 = "514F8854-95D2-4070-9036-1AFF37B4FE6F"; //男
        static public String ID_SEX_2 = "C7C261A6-747C-4EAF-B186-26C1D730C2FB"; //女
        static public String ID_SEX_9 = "84DC55C3-E0FC-41D4-8758-513C101E90C4"; //未说明

        static public String ID_YesNo = "53CE6A03-1438-4006-A43D-15BB4707DB02"; //是否标志代码
        //代码ID定义
        static public String ID_YesNo_y = "2CDDE808-2462-48A5-9AC6-3396B96CCA45"; //
        static public String ID_YesNo_n = "878BAB3C-B90D-4119-8D61-3488C518B115"; //

        static public String ID_Minority = "F51DFCA5-B2EA-447B-8328-055C208E77D3"; //GB T3304《中国各民族名称罗马字母拼写法和代码》
        //代码ID定义
        static public String ID_Minority_HA = "043961C1-DEE7-4C54-9B8B-01F489014C44";//汉族
        static public String ID_Minority_MG = "6072F948-EDEC-4DC9-9F4F-0E9CACC20DF9";//蒙古族
        static public String ID_Minority_HU = "BD7EEDA5-5BAA-45B9-B4DA-2BC30DE579D8";//回族
        static public String ID_Minority_ZA = "093897E3-3FDA-442C-8A6D-3814887C752D";//藏族
        static public String ID_Minority_UG = "9963A780-F2DA-4191-8E2B-3962D4365DA0";//维吾尔族
        static public String ID_Minority_MH = "84781790-B9F5-47A7-827C-3A2668CE5CDA";//苗族
        static public String ID_Minority_YI = "1ED45851-D3C7-4858-845E-3A284D5DAC40";//彝族
        static public String ID_Minority_ZH = "AF904B72-26A8-4CD2-93E6-3D30735BAE97";//壮族
        static public String ID_Minority_BY = "68CCFCC1-C239-42B1-BA77-3D6FFE0C30DA";//布依族
        static public String ID_Minority_CS = "F1B5F3BA-2CFF-4A62-A4D8-3E3AD7A42260";//朝鲜族
        static public String ID_Minority_MA = "B0F70669-9773-44E0-B459-4472449D6CE9";//满族
        static public String ID_Minority_DO = "6C384400-76C2-4A1B-BCD5-45A4E16DB872";//侗族
        static public String ID_Minority_YA = "2DAD2E65-E180-4FFB-A535-460AF0834895";//瑶族
        static public String ID_Minority_BA = "087D1946-ECEF-4B64-91F4-464D0C688A42";//白族
        static public String ID_Minority_TJ = "C07AB0A6-E37C-4D68-AD5F-4672D226292B";//土家族
        static public String ID_Minority_HN = "9CC806DF-3D6E-4A2B-BB7C-4BFEF3C7A1F7";//哈尼族
        static public String ID_Minority_KZ = "34C919D6-6AD6-4FF7-8650-4C5B725D9B40";//哈萨克族
        static public String ID_Minority_DA = "01ECD38A-2BBA-4495-8CC7-4CF5F684BB2F";//傣族
        static public String ID_Minority_LI = "11CE8E57-CBAB-46CD-AFDF-556190621AB7";//黎族
        static public String ID_Minority_LS = "B42A998E-2071-40F2-ADF4-561A8E9676FB";//傈僳族
        static public String ID_Minority_VA = "5859F223-B5AE-4ADA-99D1-565974D0863F";//佤族
        static public String ID_Minority_SH = "227C8F6B-1081-4F2A-A900-68F8453785DB";//畲族
        static public String ID_Minority_GS = "606358F5-258B-4300-B345-6CB04CA5CE6D";//高山族
        static public String ID_Minority_LH = "FE5090F4-7CE1-41FC-8691-70F53D310523";//拉祜族
        static public String ID_Minority_SU = "B6ABEFD4-2326-4A27-9104-77D363323A0C";//水族
        static public String ID_Minority_DX = "059FF8B3-936A-4A5B-837F-79923DB44717";//东乡族
        static public String ID_Minority_NX = "0FF1C22F-4335-418C-9CD5-7D0EA3AE1A2C";//纳西族
        static public String ID_Minority_JP = "D3358805-F1A1-44E7-8DBE-8961CB602CCF";//景颇族
        static public String ID_Minority_KG = "EB130BF2-E6E6-496D-BF70-8F0474E9348A";//柯尔克孜族
        static public String ID_Minority_TU = "A18184F7-BA7E-41D9-BA86-8F9AB16550ED";//土族
        static public String ID_Minority_DU = "40297E4C-B96B-4881-A312-90D94B6C2ECC";//达斡尔族
        static public String ID_Minority_ML = "EFDDC6EF-AD56-49F3-9489-9EB93CD0A97E";//仫佬族
        static public String ID_Minority_QI = "08FEDA42-14D2-4E7F-88DC-A4B481F5A3E4";//羌族
        static public String ID_Minority_BL = "04FB79D6-692C-46B0-ADC3-A935A2AF5C4D";//布朗族
        static public String ID_Minority_SL = "F6862851-FD84-43EB-B945-A94B97AD04BA";//撒拉族
        static public String ID_Minority_MN = "0C6F055C-E7D2-43B2-9621-AAF129B0F812";//毛难族
        static public String ID_Minority_GL = "AD586E02-1D23-42E6-A843-AF0022996D0B";//仡佬族
        static public String ID_Minority_XB = "E614A235-C697-4D66-93F9-AFC376484377";//锡伯族
        static public String ID_Minority_AC = "F8277DE2-A044-4FE0-AD15-B14425B9AC20";//阿昌族
        static public String ID_Minority_PM = "BE074694-DF20-4351-A38B-B3F27BD681EE";//普米族
        static public String ID_Minority_TA = "F772EB6F-CF42-4243-82AD-B6311E7E8139";//塔吉克族
        static public String ID_Minority_NU = "08BBCCDC-6EC0-43F8-9FF7-BC3DC1B4DA2A";//怒族
        static public String ID_Minority_UZ = "143D2BFD-A5A9-4EA2-BDEE-BEFD94F1AA67";//乌孜别克族
        static public String ID_Minority_RS = "1B057F8F-C764-4286-AA97-CD09F377F0A8";//俄罗斯族
        static public String ID_Minority_EW = "9B507A9F-E4CD-4ADC-85D8-CEE75407AF88";//鄂温克族
        static public String ID_Minority_DE = "D3466DE2-8F3C-4106-840D-CF8A6AF7337E";//德昂族
        static public String ID_Minority_BN = "78BF422C-CB96-452C-8D13-D665B316D12A";//保安族
        static public String ID_Minority_YG = "5192B74B-D209-4C29-850A-D6C28BEF5AFC";//裕固族
        static public String ID_Minority_GI = "45B28E84-A79D-4F59-B083-E4CF556F972B";//京族
        static public String ID_Minority_TT = "4B36832A-34EE-4EEF-884D-E5B4D41B46DF";//塔塔尔族
        static public String ID_Minority_DR = "34EFA489-48B6-4172-AD5C-E84E109DED22";//独龙族
        static public String ID_Minority_OR = "34F0F374-D1A7-42A9-A139-EAE7A42A9A91";//鄂伦春族
        static public String ID_Minority_HZ = "1FAA5575-5852-4930-8A89-EC78B2BB610C";//赫哲族
        static public String ID_Minority_MB = "47AAD19F-138B-4D32-80A5-F3A068DD109F";//门巴族
        static public String ID_Minority_LB = "3387C8E5-4CEC-41BA-923C-FC60C1DB9FB2";//珞巴族
        static public String ID_Minority_JN = "7CD648D6-77D7-4CFD-9981-FDC20D385390";//基诺族


        /**
            * 生成字段信息集，由子类覆盖，并总是先调用父类
            */
        protected override void createFieldsAl()
        {
		    base.noted=true; //加备注字段
            base.named = true; //加名称字段
            base.indexed = true;//加排序字段
		
		    base.createFieldsAl();

            base.addAStringField("是否树结构", fIsTree, 1);
	    }

        public void addInitItem(
            string infID,
            string infName,
            string infNote,
            string infIndex,
            string infIsTree
            )
        {
            Hashtable tempRecordHt = new Hashtable();
            initDataAl.Add(tempRecordHt);
            tempRecordHt.Add(fID, infID);
            tempRecordHt.Add(fName, infName);
            tempRecordHt.Add(fNote, infNote);
            tempRecordHt.Add(fIndex, infIndex);
            tempRecordHt.Add(fIsTree, infIsTree);
        }

        public bool checkInitItemIDExisted(string infID)
        {
            foreach (Hashtable tempHt in initDataAl)
            {
                if (tempHt[fID] as string == infID)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
