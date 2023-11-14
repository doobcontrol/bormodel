using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.xiyuansoft.bormodel.commdata;
using System.Collections;
using System.Data;

namespace com.xiyuansoft.bormodel.kmodelExt
{
    public class KMExtTable : KBoModel
    {
        #region 单例模式
        protected KMExtTable()
        {
            boModelID = KMExtTable.BoModelID;
            tableName = KMExtTable.TableName;   //表名（显示名称）
            tableCode = KMExtTable.TableCode;   //表物理名（数据库操作名）	
        }

        private static KMExtTable single;

        /**
         * 返回本类唯一实例，饿汉式单例类
         */
        public static KMExtTable getnSingInstance()
        {
            if (single == null)
            {
                single = new KMExtTable();
            }
            return single;
        }

        #endregion

        static public String BoModelID = "774a70cc271f479c9bfae5ab992b6d9c";
        static public String TableName = "扩展模型表";   //表名（显示名称）
        static public String TableCode = "tKMExtTable";   //表物理名（数据库操作名）

        //字段（属性）名称定义
        static public String fExtTableName = "fExtTableName";  //扩展物理表名
        static public String fMainTableName = "fMainTableName";  //主表物理表名
        static public String fTaleCreated = "fTaleCreated";  //表生成标志，暂不支持表结构更改

        /**
         * 生成字段信息集，由子类覆盖，并总是先调用父类
         */
        protected override void createFieldsAl()
        {
            base.createFieldsAl();
            base.addAStringField("扩展物理表名", fExtTableName, 50);
            base.addAStringField("主表物理表名", fMainTableName, 50);

            base.addAComFKeyField("表生成标志", fTaleCreated, CommListType.ID_YesNo);
            CommList.getnSingInstance().addOptInitItems(CommListType.ID_YesNo);
	    }

        public void addInitItem(
            string ExtTableID,
            string ExtTableName,
            string MainTableName
            )
        {
            Hashtable tempRecordHt;

            tempRecordHt = new Hashtable();
            initDataAl.Add(tempRecordHt);
            tempRecordHt.Add(fID, ExtTableID);
            tempRecordHt.Add(fExtTableName, ExtTableName);
            tempRecordHt.Add(fMainTableName, MainTableName);
            tempRecordHt.Add(fTaleCreated, CommListType.ID_YesNo_n);
        }

        public void createPhTable(string extTableID)
        {
            DataRow tableDataRow = selectByPKey(extTableID).Rows[0];
            DataTable fieldDatas = KMExtField.getnSingInstance().selectByOneField(KMExtField.fModelID, extTableID);
            String tempSql = "";
            tempSql += " create table ";
            tempSql += tableDataRow[fExtTableName].ToString();
            tempSql += " ( ";
            String fieldString = fID + " char varying(50) not null ";            //构建字段的sql串
            String primaryString = "";            //主键串
            String foreignString = "";            //主键串

            String foreignsqlStr = "";     //生成外键的sql串   加fID
            foreignsqlStr += " alter table ";
            foreignsqlStr += tableDataRow[fExtTableName].ToString();
            foreignsqlStr += " add constraint FK_" + System.Guid.NewGuid().ToString("N");
            foreignsqlStr += " foreign key (";
            foreignsqlStr += fID;
            foreignsqlStr += ") ";
            foreignsqlStr += " references ";
            foreignsqlStr += tableDataRow[fMainTableName].ToString();
            foreignsqlStr += " (";
            foreignsqlStr += fID;
            foreignsqlStr += ")  ";

            foreach (DataRow fieldDataRow in fieldDatas.Rows)
            {
                fieldString += ", ";

                fieldString += fieldDataRow[KMExtField.fFieldCode].ToString() + " ";
                String tDataType = fieldDataRow[KMExtField.fDataType].ToString();

                if (tDataType == KMExtField.DataType_text)
                {
                    fieldString += " char varying(";
                    fieldString += fieldDataRow[KMExtField.fDataLength].ToString() + " ";
                    fieldString += ")";
                }
                else if (tDataType == KMExtField.DataType_int)
                {
                    fieldString += " int ";
                }
                else if (tDataType == KMExtField.DataType_float)
                {
                    fieldString += " float ";
                }
                else if (tDataType == KMExtField.DataType_date)
                {
                    fieldString += " datetime ";
                }
                else
                {
                    fieldString += tDataType;
                }

                //是否主键
                if (fieldDataRow[KMExtField.fIsPrimaryKey].ToString() == "1")
                {
                    fieldString += " not null ";

                    if (primaryString != "")
                    {
                        primaryString += ", ";
                    }
                    primaryString += fieldDataRow[KMExtField.fFieldCode].ToString();

                }
                else
                {
                    fieldString += " null ";
                }

                if (fieldDataRow[KMExtField.fIsForeignKey].ToString() == "1")
                {
                    if (DataBaseType == "com.xiyuansoft.DataBasePro.SQLite.SQLiteDbAccess,com.xiyuansoft.DataBasePro.SQLite")
                    {
                        //SQLite不支持ALTER TABLE加外键，需在此处支持，但需本方法递归避免生成表顺序不正确
                        foreignString += ", ";
                        foreignString += "FOREIGN KEY ("
                            + fieldDataRow[KMExtField.fFieldCode].ToString() + ") REFERENCES "
                            + fieldDataRow[KMExtField.fForeignKeyTable].ToString() + "("
                            + fieldDataRow[KMExtField.fForeignKeyField].ToString() + ")";
                    }
                    else
                    {
                            foreignsqlStr += " alter table ";
                            foreignsqlStr += tableDataRow[fExtTableName].ToString();

                            foreignsqlStr += " add constraint FK_"
                                + System.Guid.NewGuid().ToString("N");

                            foreignsqlStr += " foreign key (";
                            foreignsqlStr += fieldDataRow[KMExtField.fFieldCode].ToString();
                            foreignsqlStr += ") ";
                            foreignsqlStr += " references ";

                            foreignsqlStr += fieldDataRow[KMExtField.fForeignKeyTable].ToString();
                            foreignsqlStr += " (";
                            foreignsqlStr += fieldDataRow[KMExtField.fForeignKeyField].ToString();
                            foreignsqlStr += ")  ";
                    }
                }
            }

            tempSql += fieldString + ",constraint PK_"
                + tableDataRow[fExtTableName].ToString() + "_" + System.Guid.NewGuid().ToString("N")
                + " primary key (" + fID + ")" + foreignString + " )  ";

            exeSql(tempSql);
            exeSql(foreignsqlStr);
        }

        public void insertExtRecord(string ExtTableCode, Hashtable recordHt)
        {
            string sqlStr = "";
            string fieldsStr = "";
            string valuesStr = "";

            foreach (string field in recordHt.Keys)
            {
                if (fieldsStr != "")
                {
                    fieldsStr += ",";
                    valuesStr += ",";
                }
                fieldsStr += field;
                valuesStr += "'" + recordHt[field] + "'";
            }
            sqlStr = "insert into " + ExtTableCode + "(" + fieldsStr + ") values(" + valuesStr + ")";
            exeSql(sqlStr);
        }
        public void deleteExtRecord(string ExtTableCode, string recordID)
        {
            string sqlStr = "";
            sqlStr = "delete " + ExtTableCode + " where " + fID + "='" + recordID + "'";
            exeSql(sqlStr);
        }

        /// <summary>
        /// 以某个ExtTable为基表的全查询
        /// </summary>
        /// <param name="extTableID">ExtTable表ID</param>
        /// <returns></returns>
        public DataTable selectExtTableAll(string extTableID)
        {
            DataRow tableRow = selectByPKey(extTableID).Rows[0];
            string extTableName = tableRow[fExtTableName].ToString();
            string mainTableName = tableRow[fMainTableName].ToString();

            string selectFieldString = extTableName + ".*," + mainTableName + ".*";
            string selectTableString = extTableName + " left join " + mainTableName + " on "
                + extTableName + "." + fID + "=" + mainTableName + "." + fID
                ;

            DataTable fieldsTable
                = KMExtField.getnSingInstance().selectByOneField(KMExtField.fModelID, extTableID);
                ;

            foreach(DataRow fieldDr in fieldsTable.Rows){
                if(fieldDr[KMExtField.fIsForeignKey].ToString()=="1"){
                    string fieldCode = fieldDr[KMExtField.fFieldCode].ToString();
                    string fTable = fieldDr[KMExtField.fForeignKeyTable].ToString();
                    string fField = fieldDr[KMExtField.fForeignKeyField].ToString();
                    string fShowField = fieldDr[KMExtField.fForeignShowField].ToString();
                    selectTableString+=" left join " + fTable + " " + fieldCode + fTable + " on "
                        + extTableName + "." + fieldCode + "=" + fieldCode + fTable + "." + fID
                        ;
                    selectFieldString += "," + fieldCode + fTable + "." + fShowField + " " + fieldCode + fShowField
                        ;
                }
            }

            string sqlStr = "SELECT " + selectFieldString + " FROM " + selectTableString;

            return exeSqlForDataSet(sqlStr);
        }

        /// <summary>
        /// 以主表为基表，联接某个ExtTable表的全查询，两个表都实现有全查询
        /// </summary>
        /// <param name="extTableID">ExtTable表ID</param>
        /// <returns></returns>
        public DataTable selectExtTableAllByMainTable(KBoModel mainModel, string extTableID)
        {
            string mTableFullSelectStr = mainModel.getfullSelectStr();
            string eTableFullSelectStr = getExtTablefullSelectStr(extTableID);

            string sqlStr = "SELECT a.*,b.* FROM (" + mTableFullSelectStr + ") a,(" + eTableFullSelectStr + ") b where a." + fID + "=b." + fID;

            return exeSqlForDataSet(sqlStr);
        }

        public string getExtTablefullSelectStr(string extTableID)
        {
            DataRow tableRow = selectByPKey(extTableID).Rows[0];
            string extTableName = tableRow[fExtTableName].ToString();

            string selectFieldString = extTableName + ".*";
            string selectTableString = extTableName
                ;

            DataTable fieldsTable
                = KMExtField.getnSingInstance().selectByOneField(KMExtField.fModelID, extTableID);
            ;

            foreach (DataRow fieldDr in fieldsTable.Rows)
            {
                if (fieldDr[KMExtField.fIsForeignKey].ToString() == "1")
                {
                    string fieldCode = fieldDr[KMExtField.fFieldCode].ToString();
                    string fTable = fieldDr[KMExtField.fForeignKeyTable].ToString();
                    string fField = fieldDr[KMExtField.fForeignKeyField].ToString();
                    string fShowField = fieldDr[KMExtField.fForeignShowField].ToString();
                    selectTableString += " left join " + fTable + " " + fieldCode + fTable + " on "
                        + extTableName + "." + fieldCode + "=" + fieldCode + fTable + "." + fID
                        ;
                    selectFieldString += "," + fieldCode + fTable + "." + fShowField + " " + fieldCode + fShowField
                        ;
                }
            }

            string sqlStr = "SELECT " + selectFieldString + " FROM " + selectTableString;

            return sqlStr;
        }
    }
}
