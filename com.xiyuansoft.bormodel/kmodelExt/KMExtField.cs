using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using com.xiyuansoft.bormodel.metadata;
using com.xiyuansoft.bormodel.commdata;

namespace com.xiyuansoft.bormodel.kmodelExt
{
    public class KMExtField : KBoModel
    {
        #region 单例模式
        protected KMExtField()
        {
            boModelID = KMExtField.BoModelID;
            tableName = KMExtField.TableName;   //表名（显示名称）
            tableCode = KMExtField.TableCode;   //表物理名（数据库操作名）	
        }

        private static KMExtField single;

        /**
         * 返回本类唯一实例，饿汉式单例类
         */
        public static KMExtField getnSingInstance()
        {
            if (single == null)
            {
                single = new KMExtField();
            }
            return single;
        }

        #endregion


        static public String BoModelID = "707ea14f4111457398ef648f2b26f354";
        static public String TableName = "扩展模型属性表";   //表名（显示名称）
        static public String TableCode = "tKMExtField";   //表物理名（数据库操作名）	

        //字段（属性）名称定义
        static public String fFieldName = "fFieldName";  //字段名称
        static public String fFieldCode = "fFieldCode";  //字段代码，同时作为该属性的物理字段名
        static public String fShowIndex = "fShowIndex";  //显示顺序

        static public String fDataType = "fDataType";  //数据类型
        static public String fDataLength = "fDataLength";  //数据长度

        static public String fIsPrimaryKey = "fIsPrimaryKey";  //是否主键

        static public String fIsForeignKey = "fIsForeignKey";  //是否外键	
        static public String fForeignKeyTable = "fForeignKeyTable"; //外键表名
        static public String fForeignKeyField = "fForeignKeyField"; //外键字段名
        static public String fForeignShowField = "fForeignShowField"; //外键显示字段名
        static public String fCommListID = "fCommListID";      //通用列表ID（有无必要单独实现？？）
        static public String fForeignTableClass = "fForeignTableClass"; //外键业务模型（运行时动态获取模型信息）

        static public String fModelID = "fModelID";         //本字段所属模型

        //常量定义
        static public String DataType_text = "text";
        static public String DataType_int = "int";
        static public String DataType_float = "float";
        static public String DataType_date = "date"; 

        /**
         * 生成字段信息集，由子类覆盖，并总是先调用父类
         */
        protected override void createFieldsAl()
        {
            base.createFieldsAl();
            base.addAStringField("字段名称", fFieldName, 100);  //字段名称
            base.addAStringField("字段代码", fFieldCode, 50);  //字段代码，同时作为该属性的物理字段名
            base.addAIntField("显示顺序", fShowIndex);  //显示顺序

            base.addAStringField("数据类型", fDataType, 20);  //数据类型
            base.addAIntField("数据长度", fDataLength);  //数据长度

            base.addAStringField("是否主键", fIsPrimaryKey, 1);  //是否主键

            base.addAStringField("是否外键", fIsForeignKey, 1);  //是否外键	
            base.addAStringField("外键表名", fForeignKeyTable, 50); //外键表名
            base.addAStringField("外键字段名", fForeignKeyField, 50); //外键字段名
            base.addAStringField("外键显示字段名", fForeignShowField, 50); //外键显示字段名
            base.addAStringField("通用列表ID", fCommListID, 50);      //通用列表ID（有无必要单独实现？？）

            base.addAKBoFKeyField("所属模型", fModelID,
                    KMExtTable.TableCode, KMExtTable.getnSingInstance());		

        }

        public void addInitItem(
            string ModelID,
            string FieldID,
            string FieldName,
            string FieldCode,
            string ShowIndex,
            string DataType,
            string DataLength,
            string IsPrimaryKey,
            string IsForeignKey,
            string ForeignKeyTable,
            string ForeignKeyField,
            string ForeignShowField,
            string CommListID
            )
        {
            Hashtable tempRecordHt;

            tempRecordHt = new Hashtable();
            initDataAl.Add(tempRecordHt);
            tempRecordHt.Add(fID, FieldID);
            tempRecordHt.Add(fFieldName, FieldName);
            tempRecordHt.Add(fFieldCode, FieldCode);
            tempRecordHt.Add(fShowIndex, ShowIndex);
            tempRecordHt.Add(fDataType, DataType);
            tempRecordHt.Add(fDataLength, DataLength);
            tempRecordHt.Add(fIsPrimaryKey, IsPrimaryKey);
            tempRecordHt.Add(fIsForeignKey, IsForeignKey);
            tempRecordHt.Add(fForeignKeyTable, ForeignKeyTable);
            tempRecordHt.Add(fForeignKeyField, ForeignKeyField);
            tempRecordHt.Add(fForeignShowField, ForeignShowField);
            tempRecordHt.Add(fCommListID, CommListID);
            tempRecordHt.Add(fModelID, ModelID);
        }
        public void addInitItem_FK(
            string ModelID,
            string FieldID,
            string FieldName,
            string FieldCode,
            string ShowIndex,
            string ForeignShowField,
            KBoModel fkModel
            )
        {
            Hashtable tempRecordHt;

            tempRecordHt = new Hashtable();
            initDataAl.Add(tempRecordHt);
            tempRecordHt.Add(fID, FieldID);
            tempRecordHt.Add(fFieldName, FieldName);
            tempRecordHt.Add(fFieldCode, FieldCode);
            tempRecordHt.Add(fShowIndex, ShowIndex);
            tempRecordHt.Add(fDataType, Field.DataType_text);
            tempRecordHt.Add(fDataLength, "50");
            tempRecordHt.Add(fIsPrimaryKey, "0");
            tempRecordHt.Add(fIsForeignKey, "1");
            tempRecordHt.Add(fForeignKeyTable, fkModel.getTableCode());
            tempRecordHt.Add(fForeignKeyField, KBoModel.fID);
            tempRecordHt.Add(fForeignShowField, ForeignShowField);
            tempRecordHt.Add(fCommListID, "");
            tempRecordHt.Add(fModelID, ModelID);
        }
        public void addInitItem_CmFK(
            string ModelID,
            string FieldID,
            string FieldName,
            string FieldCode,
            string ShowIndex,
            string CommListID
            )
        {
            Hashtable tempRecordHt;

            tempRecordHt = new Hashtable();
            initDataAl.Add(tempRecordHt);
            tempRecordHt.Add(fID, FieldID);
            tempRecordHt.Add(fFieldName, FieldName);
            tempRecordHt.Add(fFieldCode, FieldCode);
            tempRecordHt.Add(fShowIndex, ShowIndex);
            tempRecordHt.Add(fDataType, Field.DataType_text);
            tempRecordHt.Add(fDataLength, "50");
            tempRecordHt.Add(fIsPrimaryKey, "0");
            tempRecordHt.Add(fIsForeignKey, "1");
            tempRecordHt.Add(fForeignKeyTable, CommList.TableCode);
            tempRecordHt.Add(fForeignKeyField, KBoModel.fID);
            tempRecordHt.Add(fForeignShowField, CommList.fName);
            tempRecordHt.Add(fCommListID, CommListID);
            tempRecordHt.Add(fModelID, ModelID);
        }
        public void addInitItem_Str(
            string ModelID,
            string FieldID,
            string FieldName,
            string FieldCode,
            string ShowIndex,
            string DataLength
            )
        {
            Hashtable tempRecordHt;

            tempRecordHt = new Hashtable();
            initDataAl.Add(tempRecordHt);
            tempRecordHt.Add(fID, FieldID);
            tempRecordHt.Add(fFieldName, FieldName);
            tempRecordHt.Add(fFieldCode, FieldCode);
            tempRecordHt.Add(fShowIndex, ShowIndex);
            tempRecordHt.Add(fDataType, Field.DataType_text);
            tempRecordHt.Add(fDataLength, DataLength);
            tempRecordHt.Add(fIsPrimaryKey, "0");
            tempRecordHt.Add(fIsForeignKey, "0");

            tempRecordHt.Add(fModelID, ModelID);
        }
        public void addInitItem_Float(
            string ModelID,
            string FieldID,
            string FieldName,
            string FieldCode,
            string ShowIndex
            )
        {
            Hashtable tempRecordHt;

            tempRecordHt = new Hashtable();
            initDataAl.Add(tempRecordHt);
            tempRecordHt.Add(fID, FieldID);
            tempRecordHt.Add(fFieldName, FieldName);
            tempRecordHt.Add(fFieldCode, FieldCode);
            tempRecordHt.Add(fShowIndex, ShowIndex);
            tempRecordHt.Add(fDataType, Field.DataType_float);
            tempRecordHt.Add(fIsPrimaryKey, "0");
            tempRecordHt.Add(fIsForeignKey, "0");

            tempRecordHt.Add(fModelID, ModelID);
        }
    
    }
}
