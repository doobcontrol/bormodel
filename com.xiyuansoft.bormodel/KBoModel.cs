using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace com.xiyuansoft.bormodel
{
    public class KBoModel : BaseModel
    {
        static public String fID = "fID";   //主键名

        protected ArrayList modelRefMeAl;

        public ArrayList ModelRefMeAl
        {
            get { return modelRefMeAl; }
            //set { modelRefMeAl = value; }
        }
        protected KBoModel()
        {
            modelRefMeAl=new ArrayList();
	    }

        //对象是否排序
        static public String fIndex = "fIndex";   //排序号。由子类决定是否实现（默认不实现）	
        protected bool indexed = false;
        /**
         * @return the indexed
         */
        public bool isIndexed()
        {
            return indexed;
        }

        //对象是否明确命名
        static public String fName = "fName";   //对象名。由子类决定是否实现（默认不实现）	
        protected bool named = false;
        /**
         * @return the named
         */
        public bool isNamed()
        {
            return named;
        }

        protected bool subClassCreateKey=false;

        /**
         * 生成字段信息集，由子类覆盖，并总是先调用父类
         */
        protected override void createFieldsAl()
        {
		    base.createFieldsAl();
            if (!subClassCreateKey) //扩展型子类需生成含引用的主键
            {
                addAMainPkField("记录标识", KBoModel.fID);
            }

            if (indexed)
            {
                base.addAIntField("序号", KBoModel.fIndex);
            }
            if (named)
            {
                base.addAStringField("名称", KBoModel.fName,100);
            }
        }

        #region  返回List Dictionary的查询

        //20170511add
        public Dictionary<string, Dictionary<string, string>> TransDataTableToKeyedDic(DataTable dt)
        {
            if (!dt.Columns.Contains(fID))
            {
                throw new ApplicationException("不是KBoModel数据表，不支持此转换");
            }

            Dictionary<string, Dictionary<string, string>> retDic = new Dictionary<string, Dictionary<string, string>>();

            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, string> rowDic = new Dictionary<string, string>();
                retDic.Add(dr[fID].ToString(), rowDic);
                foreach (DataColumn dc in dt.Columns)
                {
                    rowDic.Add(dc.ColumnName, dr[dc.ColumnName].ToString());
                }
            }

            return retDic;
        }

        public Dictionary<string, Dictionary<string, string>> selectByOneField_KeyedDic(string fFieldCode, string fFieldValue)
        {
            return TransDataTableToKeyedDic(selectByOneField(fFieldCode, fFieldValue));
        }
        public Dictionary<string, Dictionary<string, string>> selectByOneFieldLike_KeyedDic(string fFieldCode, string fFieldValue)
        {
            return TransDataTableToKeyedDic(selectByOneFieldLike(fFieldCode, fFieldValue));
        }
        public Dictionary<string, Dictionary<string, string>> selectByOneFieldAndOrderByOneField_KeyedDic(string fFieldCode, string fFieldValue, string indexField)
        {
            return TransDataTableToKeyedDic(selectByOneFieldAndOrderByOneField(fFieldCode, fFieldValue, indexField));
        }
        public Dictionary<string, Dictionary<string, string>> selectByOneFieldAndOrderByMutiField_KeyedDic(string fFieldCode, string fFieldValue, List<string> indexFieldList)
        {
            return TransDataTableToKeyedDic(selectByOneFieldAndOrderByMutiField(fFieldCode, fFieldValue, indexFieldList));
        }


        #endregion

        #region  SQL操作
        public DataTable selectByPKey(string PKeyValue)
        {
            return selectByOneField(fID,PKeyValue);
        }
        public int selectCountByPKey(string PKeyValue)   //只判断是否存在,是否应改为exist，返回bool？？？
        {
            return selectCountByOneField(fID, PKeyValue);
        }

        public Dictionary<string, DataRow> getDicRowsByID()
        {
            Dictionary<string, DataRow> retDic = new Dictionary<string, DataRow>();

            DataTable dt = selectAll();
            foreach (DataRow dr in dt.Rows)
            {
                retDic.Add(dr[fID].ToString(), dr);
            }
            return retDic;
        }

        //生成新ID并返回
        public string insertMainRecord(Hashtable recordHt)
        {
            string pKValue = System.Guid.NewGuid().ToString("N");
            recordHt.Add(fID, pKValue);

            base.insertRecord(recordHt);

            return pKValue;
        }
        public string insertMainRecord(Dictionary<string,string> recordHt)
        {
            string pKValue = System.Guid.NewGuid().ToString("N");
            recordHt.Add(fID, pKValue);

            base.insertRecord(recordHt);

            return pKValue;
        }
        public void insertMainRecordList(List<Dictionary<string, string>> recordList)
        {
            BeginTrans();
            foreach(Dictionary<string, string> recordHt in recordList)
            {
                insertMainRecord(recordHt);
            }
            CommitTrans();
        }

        //不生成新ID（参数传入）
        public string insertMainRecord(string pKValue, Hashtable recordHt)
        {
            recordHt.Add(fID, pKValue);

            base.insertRecord(recordHt);

            return pKValue;
        }

        public void updateByPKey(string PKeyValue,Hashtable recordHt)
        {
            base.updateByOneField(fID,PKeyValue,recordHt);
        } 
        public void updateByPKey(string PKeyValue, Dictionary<string, string> recordHt)
        {
            base.updateByOneField(fID,PKeyValue,recordHt);
        }
        public void updateByPKey(
            string PKeyValue,
            string tFieldCode,
            string tFieldValue)
        {
            base.updateByOneField(fID,PKeyValue, tFieldCode, tFieldValue);
        }
        public void deleteByPKey(string PKeyValue)
        {
            base.deleteByOneField(fID, PKeyValue);
        }

        //检查是否可能删除某条记录
        public void checkDelete(string inFID)
        {
            if (this.tableCode == TreeQueryHelper.TableCode)
            {
                return;  //由树类自行处理  
            }

            IEnumerator enumerator = ModelRefMeAl.GetEnumerator();
            while (enumerator.MoveNext())
            {
                BaseModel refModel = enumerator.Current as BaseModel;

                if (refModel.checkRefExist(this,inFID))
                {
                    throw new ApplicationException("其它业务对象引用了本对象，因此不允许删除");
                }
            }
        }
        public void checkDelete(string inFID, ArrayList exCludeModelAl)
        {
            if (this.tableCode == TreeQueryHelper.TableCode)
            {
                return;  //由树类自行处理  
            }            

            IEnumerator enumerator = ModelRefMeAl.GetEnumerator();
            while (enumerator.MoveNext())
            {
                BaseModel refModel = enumerator.Current as BaseModel;
                if (exCludeModelAl.Contains(refModel))
                {
                    continue;
                }
                if (refModel.checkRefExist(this, inFID))
                {
                    throw new ApplicationException("其它业务对象引用了本对象，因此不允许删除");
                }
            }
        }


        #region NewSelecter
        
        /// <summary>
        /// 由ID进行全查询
        /// </summary>
        /// <param name="idValue">fID值</param>
        /// <returns></returns>
        public DataTable fullSelectByID(string idValue)
        {
            List<string> condictionList = new List<string>();
            condictionList.Add(this.tableCode + "." + fID + "='" + idValue + "'");

            return fullSelect(condictionList);
        }

        #endregion

        #region SqlCreater

        /// <summary>
        /// 获取逗号分隔格式的字段字符串,不含fID
        /// </summary>
        /// <returns></returns>
        public string getAllFieldsSelectCommaStrNoID()
        {
            string retStr = null;
            foreach (string fStr in fieldsHt.Keys)
            {
                if (fStr != fID)
                {
                    if (retStr == null)
                    {
                        retStr = this.tableCode + "." + fStr;
                    }
                    else
                    {
                        retStr += "," + this.tableCode + "." + fStr;
                    }
                }
            }
            return retStr;
        }
        /// <summary>
        /// 获取字段List<string>格式，只含字段名,不含fID
        /// </summary>
        /// <returns></returns>
        public List<string> getAllFieldsSelectListNoID()
        {
            List<string> retList = new List<string>();
            foreach (string fStr in fieldsHt.Keys)
            {
                if (fStr != fID)
                {
                    retList.Add(this.tableCode + "." + fStr);
                }
            }
            return retList;
        }

        #endregion
        
        #endregion
    }
}
