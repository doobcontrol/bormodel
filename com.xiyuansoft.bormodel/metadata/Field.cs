using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.xiyuansoft.bormodel.metadata
{
    public class Field : KBoModel
    {
        #region 单例模式
        protected Field()
        {
            boModelID = Field.BoModelID;
            tableName = Field.TableName;   //表名（显示名称）
            tableCode = Field.TableCode;   //表物理名（数据库操作名）	
        }

        private static Field single;

        /**
         * 返回本类唯一实例，饿汉式单例类
         */
        public static Field getnSingInstance()
        {
            if (single == null)
            {
                single = new Field();
            }
            return single;
        }

        #endregion


        static public String BoModelID = "2D736F3D-B89D-4F91-80F5-348103D2F89E";
        static public String TableName = "业务模型属性表";   //表名（显示名称）
        static public String TableCode = "tField";   //表物理名（数据库操作名）	

        //字段（属性）名称定义
        static public String fFieldName = "fFieldName";  //字段名称
        static public String fFieldCode = "fFieldCode";  //字段代码，同时作为该属性的物理字段名

        static public String fDataType = "fDataType";  //数据类型
        static public String fDataLength = "fDataLength";  //数据长度

        static public String fIsPrimaryKey = "fIsPrimaryKey";  //是否主键

        static public String fIsForeignKey = "fIsForeignKey";  //是否外键	
        static public String fForeignKeyTable = "fForeignKeyTable"; //外键表名
        static public String fForeignKeyField = "fForeignKeyField"; //外键字段名
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

            base.addAStringField("数据类型", fDataType, 20);  //数据类型
            base.addAIntField("数据长度", fDataLength);  //数据长度

            base.addAStringField("是否主键", fIsPrimaryKey, 1);  //是否主键

            base.addAStringField("是否外键", fIsForeignKey, 1);  //是否外键	
            base.addAStringField("外键表名", fForeignKeyTable, 50); //外键表名
            base.addAStringField("外键字段名", fForeignKeyField,50); //外键字段名
            base.addAStringField("通用列表ID", fCommListID, 50);      //通用列表ID（有无必要单独实现？？）

            base.addAKBoFKeyField("所属模型", fModelID,
                    Table.TableCode, Table.getnSingInstance());		

        }
    }
}
