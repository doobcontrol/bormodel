using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.xiyuansoft.bormodel.metadata
{
    public class Table : KBoModel
    {
        #region 单例模式
        protected Table()
        {
            boModelID = Table.BoModelID;
            tableName = Table.TableName;   //表名（显示名称）
            tableCode = Table.TableCode;   //表物理名（数据库操作名）	
        }

        private static Table single;

        /**
         * 返回本类唯一实例，饿汉式单例类
         */
        public static Table getnSingInstance()
        {
            if (single == null)
            {
                single = new Table();
            }
            return single;
        }

        #endregion

        static public String BoModelID = "4AFDB7B3-0127-4326-AAB4-88355F5987F6";
        static public String TableName = "业务模型表";   //表名（显示名称）
        static public String TableCode = "tTable";   //表物理名（数据库操作名）

        //字段（属性）名称定义
        static public String fModelName = "fModelName";  //模型名称
        static public String fModelCode = "fModelCode";  //模型代码，同时作为该模型的物理表名



        /**
         * 生成字段信息集，由子类覆盖，并总是先调用父类
         */
        protected override void createFieldsAl()
        {
            base.createFieldsAl();
            base.addAStringField("模型名称", fModelName, 100);  //模型名称
            base.addAStringField("模型代码", fModelCode, 50);  //模型代码，同时作为该模型的物理表名
		
	    }
    }
}
