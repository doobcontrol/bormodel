using com.xiyuansoft.bormodel.commdata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace com.xiyuansoft.bormodel
{
    /// <summary>
    /// 业务参数表
    /// </summary>
    public class BizPars : KBoModel
    {
        #region 单例模式
        protected BizPars()
        {
            boModelID = BizPars.BoModelID;
            tableName = BizPars.TableName;   //表名（显示名称）
            tableCode = BizPars.TableCode;   //表物理名（数据库操作名）	

            doExportData = true;
        }

        private static BizPars single;

        /**
         * 返回本类唯一实例，饿汉式单例类
         */
        public static BizPars getnSingInstance()
        {
            if (single == null)
            {
                single = new BizPars();
            }
            return single;
        }

        #endregion

        static public String BoModelID = "404ea3ec814c47f2aff4b6f680c2985e"; //? System.Guid.NewGuid().ToString("N");
        static public String TableName = "业务参数表";   //表名（显示名称）
        static public String TableCode = "tBizPars";   //表物理名（数据库操作名）

        //字段（属性）名称定义
        static public String fBizParName = "fBizParName";
        static public String fBizParValue = "fBizParValue";
        

        //常量定义

        /**
         * 生成字段信息集，由子类覆盖，并总是先调用父类
         */
        protected override void createFieldsAl()
        {
            CommListType commListType = CommListType.getnSingInstance();
            CommList commList = CommList.getnSingInstance();

            base.createFieldsAl();
            base.addAStringField("参数名", fBizParName, 50);  //
            base.addAStringField("参数值", fBizParValue, 5000);  //
        }

        public void addInitItem(
            string bizParName,
            string bizParValue
            )
        {
            Hashtable tempRecordHt = new Hashtable();
            initDataAl.Add(tempRecordHt);

            tempRecordHt.Add(fID, System.Guid.NewGuid().ToString("N"));//System.Guid.NewGuid().ToString("N"));
            tempRecordHt.Add(fBizParName, bizParName);
            tempRecordHt.Add(fBizParValue, bizParValue);
        }

        #region 数据库操作

        private Dictionary<string, string> parsDic;
        public Dictionary<string, string> ParsDic
        {
            get
            {
                if (parsDic == null)
                {
                    DataTable parTb = selectAll();
                    parsDic = new Dictionary<string, string>();
                    foreach (DataRow dr in parTb.Rows)
                    {
                        parsDic.Add(dr[fBizParName].ToString(), dr[fBizParValue].ToString());
                    }
                }

                return parsDic; 
            
            }
        }
        
        public string getPar(string parName)
        {
            string retStr = null;

            try
            {
                retStr = ParsDic[parName];
            }
            catch(Exception e){
                if (e is ArgumentNullException)
                {
                    throw new ApplicationException("无效的参数名");
                }
                else if (e is KeyNotFoundException)
                {
                    throw new ApplicationException("无效的参数名");
                }
                else
                {
                    throw e;
                }
            }

            return retStr;
        }

        public void changePar(string parName, string parValue)
        {
            if (!ParsDic.ContainsKey(parName))
            {
                throw new ApplicationException("无效的参数名");
            }
            Hashtable tHt=new Hashtable();
            tHt.Add(fBizParValue, parValue);
            updateByOneField(fBizParName, parName, tHt);
            ParsDic[parName] = parValue;
        }

        #endregion
   
    }
}
