using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using com.xiyuansoft.bormodel.metadata;
using System.Collections;

namespace com.xiyuansoft.bormodel
{
    public class TreeQueryHelper : BaseModel
    {
        #region 单例模式
        protected TreeQueryHelper()
        {
            boModelID = TreeQueryHelper.BoModelID;
            tableName = TreeQueryHelper.TableName;   //表名（显示名称）
            tableCode = TreeQueryHelper.TableCode;   //表物理名（数据库操作名）	

            doExportData = true;
        }

        private static TreeQueryHelper single;

        /**
         * 返回本类唯一实例，饿汉式单例类
         */
        public static TreeQueryHelper getnSingInstance()
        {
            if (single == null)
            {
                single = new TreeQueryHelper();
            }
            return single;
        }

        #endregion

        static public String BoModelID = "E28EDFB3-A836-4B77-B177-6DBD461E472D";
        static public String TableName = "树结构查询辅助表";   //表名（显示名称）
        static public String TableCode = "tTreeQueryHelper";   //表物理名（数据库操作名）

        //字段（属性）名称定义
        static public String fModel = "fModel";
        static public String fObjID = "fObjID";
        static public String fUObjID = "fUObjID";

        /**
         * 生成字段信息集，由子类覆盖，并总是先调用父类
         */
        protected override void createFieldsAl()
        {
		    base.createFieldsAl();
            //base.addAMapKeyField("所属模型", fModel,
            //        Table.TableCode, Table.getnSingInstance());	 //仅一个外键引用主键，需否使用Map模型（因未写matadata表，因此暂不引用外键）
            base.addAMainPkField("所属模型", fModel);	 //仅一个外键引用主键，需否使用Map模型
            base.addAMainPkField("对象ID", fObjID); //不确定所属树，不能做外键联接，由程序逻辑保证
            base.addAMainPkField("祖先ID", fUObjID); //不确定所属树，不能做外键联接，由程序逻辑保证

        }

        #region SQL操作
        public void insertNode(String modelID,String newNodeID, String upNodeID)
        {

            string sqlStr = "";

            //加入父节点
            Hashtable insertHt = new Hashtable();
            insertHt.Add(fModel, modelID);
            insertHt.Add(fObjID, newNodeID);
            insertHt.Add(fUObjID, upNodeID);
            base.insertRecord(insertHt);

            //加入父节点的祖先节点  Sqlite 下select子句不能加()，测试其它平台下是否这样 sqlserver下通过
            sqlStr += " insert into " + tableCode + "("
                + fModel + ","
                + fObjID + ","
                + fUObjID + ") select '"
                + modelID + "','"
                + newNodeID + "',"
                + fUObjID + " from " + tableCode + " where "
                + fModel + "='"
                + modelID
                + "' and "
                + fObjID + "='"
                + upNodeID 
                + "'  ";

            exeSql(sqlStr);
        }

        public void deleteNode(String modelID,String NodeID)
        {
            Hashtable deleteHt = new Hashtable();
            deleteHt.Add(fModel, modelID);
            deleteHt.Add(fObjID, NodeID);
            base.deleteByMutiField(deleteHt);

            deleteHt = new Hashtable();
            deleteHt.Add(fModel, modelID);
            deleteHt.Add(fUObjID, NodeID);
            base.deleteByMutiField(deleteHt);
        }
        #endregion
    }
}
