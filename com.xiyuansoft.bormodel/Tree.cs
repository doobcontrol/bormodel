using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.xiyuansoft.bormodel.commdata;
using System.Data;
using System.Collections;

namespace com.xiyuansoft.bormodel
{
    public class Tree : KBoModel
    {
        //字段（属性）名称定义
        static public String fUID = "fUID";   //父记录标识
        static public String fGrade = "fGrade";   //记录级次

        //可选字段
        //是否区分叶节点
        static public String fIsMinor = "fIsMinor";   //是否区分叶节点。由子类决定是否实现（默认不实现）	
        protected bool minored = false;
        /**
         * @return the minored
         */
        public bool isMinored()
        {
            return minored;
        }

        private String topNodesID = "0";

        public String TopNodesID
        {
            get { return topNodesID; }
            set { topNodesID = value; }
        }
        
        /**
         * 生成字段信息集，由子类覆盖，并总是先调用父类
         */
        protected override void createFieldsAl()
        {
		    base.createFieldsAl();

            base.addAStringField("父记录标识", fUID, 100);
            base.addAIntField("记录级次", fGrade);

            if (minored)
            {
                base.addAStringField("叶节点", fIsMinor, 1);
                //base.addAComFKeyField("叶节点", Tree.fIsMinor, CommListType.ID_YesNo); //似乎导致死循环,why???
            }
        }


        #region SQL操作

        public DataTable selectChildByParent(string ParentID)
        {
            return selectByOneField(fUID, ParentID);
        }
        
        //取所有级别的子节点  20151214
        public DataTable selectDescendantByParent(string ParentID)
        {
            string sqlStr = "";
            sqlStr += " select * from " + tableCode + " where " + fID
                + " in (select " + TreeQueryHelper.fObjID + " from " + TreeQueryHelper.TableCode
                + " where " + TreeQueryHelper.fUObjID + "='" + ParentID + "') "
                ;
            if (indexed)
            {
                sqlStr += " order by " + fIndex;
            }
            return exeSqlForDataSet(sqlStr); ;
        }
        public DataTable selectDescendantByParentIncludp(string ParentID)
        {
            string sqlStr = "";
            sqlStr += " select * from " + tableCode + " where " + fID
                + " in (select " + TreeQueryHelper.fObjID + " from " + TreeQueryHelper.TableCode
                + " where " + TreeQueryHelper.fUObjID + "='" + ParentID 
                + "' or " + TreeQueryHelper.fObjID + "='" + ParentID + "') "
                ;
            if (indexed)
            {
                sqlStr += " order by " + fIndex;
            }
            return exeSqlForDataSet(sqlStr); ;
        }
        public DataTable selectDescendantByParentOrder(string ParentID, string orderField)
        {
            string sqlStr = "";
            sqlStr += " select * from " + tableCode + " where " + fID
                + " in (select " + TreeQueryHelper.fObjID + " from " + TreeQueryHelper.TableCode
                + " where " + TreeQueryHelper.fUObjID + "='" + ParentID + "') "
                ;
            sqlStr += " order by " + orderField;
            return exeSqlForDataSet(sqlStr); ;
        }
        public DataTable selectDescendantByParentIncludpOrder(string ParentID, string orderField)
        {
            string sqlStr = "";
            sqlStr += " select * from " + tableCode + " where " + fID
                + " in (select " + TreeQueryHelper.fObjID + " from " + TreeQueryHelper.TableCode
                + " where " + TreeQueryHelper.fUObjID + "='" + ParentID
                + "' or " + TreeQueryHelper.fObjID + "='" + ParentID + "') "
                ;
            sqlStr += " order by " + orderField;
            return exeSqlForDataSet(sqlStr); ;
        }
        public string getSelectDescendantByParentIncludpString(string ParentID)
        {
            string sqlStr = "";
            sqlStr += " (select " + TreeQueryHelper.fObjID + " from " + TreeQueryHelper.TableCode
                + " where " + TreeQueryHelper.fUObjID + "='" + ParentID
                + "' or " + TreeQueryHelper.fObjID + "='" + ParentID + "') "
                ;
            return sqlStr;
        }

        public DataTable selectTopNodes()
        {
            return selectByOneField(fGrade, "1");  //子类需保证正确生成fGrade字段值
        }
        public DataTable selectTopNodesindexed()
        {
            return selectByOneFieldAndOrderByOneField(fGrade, "1", fIndex);  //子类需保证正确生成fGrade字段值  findex字段
        }

        public string getAllSubFIDSelectStringByParent(string pID,string modelID)
        {
            string retStr = "";

            retStr = "select " + tableCode + ".fID from " + tableCode + "," + TreeQueryHelper.TableCode
                + " where " + TreeQueryHelper.TableCode + "." + TreeQueryHelper.fModel + "='" + modelID + "'"
                + " and " + TreeQueryHelper.TableCode + "." + TreeQueryHelper.fObjID + "=" + tableCode + "." + fID + ""
                + " and " + TreeQueryHelper.TableCode + "." + TreeQueryHelper.fUObjID + "='" + pID + "'"
                ;

            return retStr;
        }

        //生成新ID并返回
        public String insertNode(String upNodeID,Hashtable nodeHt)
        {
            nodeHt.Add(fUID, upNodeID);
            string newPk = insertMainRecord(nodeHt);

            //写辅助查询表记录
            TreeQueryHelper.getnSingInstance().insertNode(boModelID, newPk, upNodeID);

            return newPk;
        }
        //不生成新ID（参数传入）
        public String insertNode(string NodeID, String upNodeID, Hashtable nodeHt)
        {
            nodeHt.Add(fUID, upNodeID);
            insertMainRecord(NodeID, nodeHt);

            //写辅助查询表记录
            try
            {
                TreeQueryHelper.getnSingInstance().insertNode(boModelID, NodeID, upNodeID);
            }
            catch (Exception e)
            {

            }
            return NodeID;
        }

        //删除节点，存在下级节点时报错
        public void deleteNode(String NodeID)
        {
            checkDelete(NodeID);
            TreeQueryHelper.getnSingInstance().deleteNode(boModelID,NodeID);
            base.deleteByPKey(NodeID);

        }
        //删除节点及所有下级节点
        public void deleteNodeIncludeChild(String NodeID)
        {
            //删除子节点
            string sqlStr = "";
            sqlStr += " delete " + tableCode + " where " + fID
                + " in (select " + fID + " from " + TreeQueryHelper.TableCode 
                + " where " + fUID + "='" + NodeID + "')"
                ;

            exeSql(sqlStr);

            //删除辅助查询表数据
            TreeQueryHelper.getnSingInstance().deleteNode(boModelID, NodeID);

            //删除本节点
            base.deleteByPKey(NodeID);
        }

        //检查是否可能删除某条记录
        public new void checkDelete(string inFID)
        {
            base.checkDelete(inFID);
            DataTable tempDt = selectChildByParent(inFID);
            if (tempDt.Rows.Count != 0)
            {
                throw new ApplicationException("本节点下存在下级节点，请先删除下级节点");
            }
        }

        //重新生成所有帮助数据
        public void refreshTreeQueryHelper()
        {
            DataTable dt = selectAll();
            foreach (DataRow dr in dt.Rows)
            {
                string NodeID = dr[Tree.fID].ToString();
                string upNodeID = dr[Tree.fUID].ToString();
                if (upNodeID == "")
                {
                    continue;
                }
                List<string> cdnList = new List<string>();
                cdnList.Add(" fModel='" + boModelID + "'");
                cdnList.Add(" fObjID='" + NodeID + "'");
                cdnList.Add(" fUObjID='" + upNodeID + "'");
                DataTable tqhdt = TreeQueryHelper.getnSingInstance().fullSelect(cdnList);
                if (tqhdt.Rows.Count == 0)
                {
                    TreeQueryHelper.getnSingInstance().insertNode(boModelID, NodeID, upNodeID);
                }
            }
        }

        #endregion
    }
}
