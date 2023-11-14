using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace com.xiyuansoft.bormodel.commdata
{
    public class CommList : Tree
    {
        #region 单例模式
        protected CommList()
        {
            boModelID = CommList.BoModelID;
            tableName = CommList.TableName;   //表名（显示名称）
            tableCode = CommList.TableCode;   //表物理名（数据库操作名）	
        }

        private static CommList single;

        /**
         * 返回本类唯一实例，饿汉式单例类
         */
        public static CommList getnSingInstance()
        {
            if (single == null)
            {
                single = new CommList();
            }
            return single;
        }

        #endregion

        static public String BoModelID = "82EE9D64-736A-4BA7-AB56-301C07B19A88";
        static public String TableName = "通用静态数据列表";   //表名（显示名称）
        static public String TableCode = "tCommList";   //表物理名（数据库操作名）

        //字段（属性）名称定义
        static public String fTCode = "fTCode"; //国标类数据有标准代码，或系统自定义代码
        //static public String fCommListType = "fCommListType"; //列表类型（废除，由顶级节点直接指向）
	
        /**
         * 生成字段信息集，由子类覆盖，并总是先调用父类
         */
        protected override void createFieldsAl()
        {
		    base.noted=true; //加备注字段
            base.named = true; //加名称字段
            base.indexed = true;//加排序字段
            base.minored = true;//区分叶节点

            base.createFieldsAl();

            base.addAStringField("标准代码", fTCode, 20);
            //base.addAKBoFKeyField("代码类型", fCommListType,
            //        CommListType.TableCode, CommListType.getnSingInstance());
		
		    //初始数据。系统其余部份硬编码这些记录信息，因此这些记录必需写入
	
	    }

        public void addInitItem(
            string infUID,
            string infID,
            string infGrade,
            string infTCode,
            string infName,
            string infNote,
            string infIndex
            )
        {
            Hashtable tempRecordHt = new Hashtable();
            initDataAl.Add(tempRecordHt);
            tempRecordHt.Add(fUID, infUID);
            tempRecordHt.Add(fID, infID);
            tempRecordHt.Add(fGrade, infGrade);
            tempRecordHt.Add(fTCode, infTCode);
            tempRecordHt.Add(fName, infName);
            tempRecordHt.Add(fNote, infNote);
            tempRecordHt.Add(fIndex, infIndex);		
        }

        public void addOptInitItems(string ListTypeID)
        {
            if (CommListType.getnSingInstance().checkInitItemIDExisted(ListTypeID))
            {
                return;
            }
            if(ListTypeID==CommListType.ID_SEX)
            {
                //GB／T2261《人的性别代码》
                CommListType.getnSingInstance().addInitItem(CommListType.ID_SEX, "姓别", "国标性别数据", "0", "0");
                //addInitItem(CommListType.ID_SEX, CommListType.ID_SEX_0, "1", "0", "未知的性别", "", "0");
                addInitItem(CommListType.ID_SEX, CommListType.ID_SEX_1, "1", "1", "男", "", "1");
                addInitItem(CommListType.ID_SEX, CommListType.ID_SEX_2, "1", "2", "女", "", "2");
                addInitItem(CommListType.ID_SEX, CommListType.ID_SEX_9, "1", "9", "未说明性别", "", "9");
            }
            else if (ListTypeID == CommListType.ID_YesNo)
            {
                //JYT 1001 SFBZ 是否标志代码
                CommListType.getnSingInstance().addInitItem(CommListType.ID_YesNo, "是否标志代码", "JYT 1001 SFBZ 是否标志代码", "2", "0");
                addInitItem(CommListType.ID_YesNo, CommListType.ID_YesNo_n, "1", "0", "否", "", "0");
                addInitItem(CommListType.ID_YesNo, CommListType.ID_YesNo_y, "1", "1", "是", "", "1");	
            }
            else if (ListTypeID == CommListType.ID_Minority)
            {
                //GB T3304《中国各民族名称罗马字母拼写法和代码》
                CommListType.getnSingInstance().addInitItem(CommListType.ID_Minority, "民族", "GB T3304《中国各民族名称罗马字母拼写法和代码》", "2", "0");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_HA, "1", "0", "汉族", "", "0");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_MG, "1", "1", "蒙古族", "", "1");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_HU, "1", "2", "回族", "", "2");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_ZA, "1", "3", "藏族", "", "3");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_UG, "1", "4", "维吾尔族", "", "4");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_MH, "1", "5", "苗族", "", "5");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_YI, "1", "6", "彝族", "", "6");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_ZH, "1", "7", "壮族", "", "7");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_BY, "1", "8", "布依族", "", "8");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_CS, "1", "9", "朝鲜族", "", "9");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_MA, "1", "10", "满族", "", "10");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_DO, "1", "11", "侗族", "", "11");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_YA, "1", "12", "瑶族", "", "12");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_BA, "1", "13", "白族", "", "13");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_TJ, "1", "14", "土家族", "", "14");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_HN, "1", "15", "哈尼族", "", "15");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_KZ, "1", "16", "哈萨克族", "", "16");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_DA, "1", "17", "傣族", "", "17");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_LI, "1", "18", "黎族", "", "18");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_LS, "1", "19", "傈僳族", "", "19");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_VA, "1", "20", "佤族", "", "20");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_SH, "1", "21", "畲族", "", "21");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_GS, "1", "22", "高山族", "", "22");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_LH, "1", "23", "拉祜族", "", "23");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_SU, "1", "24", "水族", "", "24");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_DX, "1", "25", "东乡族", "", "25");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_NX, "1", "26", "纳西族", "", "26");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_JP, "1", "27", "景颇族", "", "27");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_KG, "1", "28", "柯尔克孜族", "", "28");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_TU, "1", "29", "土族", "", "29");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_DU, "1", "30", "达斡尔族", "", "30");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_ML, "1", "31", "仫佬族", "", "31");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_QI, "1", "32", "羌族", "", "32");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_BL, "1", "33", "布朗族", "", "33");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_SL, "1", "34", "撒拉族", "", "34");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_MN, "1", "35", "毛难族", "", "35");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_GL, "1", "36", "仡佬族", "", "36");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_XB, "1", "37", "锡伯族", "", "37");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_AC, "1", "38", "阿昌族", "", "38");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_PM, "1", "39", "普米族", "", "39");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_TA, "1", "40", "塔吉克族", "", "40");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_NU, "1", "41", "怒族", "", "41");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_UZ, "1", "42", "乌孜别克族", "", "42");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_RS, "1", "43", "俄罗斯族", "", "43");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_EW, "1", "44", "鄂温克族", "", "44");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_DE, "1", "45", "德昂族", "", "45");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_BN, "1", "46", "保安族", "", "46");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_YG, "1", "47", "裕固族", "", "47");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_GI, "1", "48", "京族", "", "48");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_TT, "1", "49", "塔塔尔族", "", "49");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_DR, "1", "50", "独龙族", "", "50");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_OR, "1", "51", "鄂伦春族", "", "51");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_HZ, "1", "52", "赫哲族", "", "52");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_MB, "1", "53", "门巴族", "", "53");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_LB, "1", "54", "珞巴族", "", "54");
                addInitItem(CommListType.ID_Minority, CommListType.ID_Minority_JN, "1", "55", "基诺族", "", "55");

            }
        }
    }
}
