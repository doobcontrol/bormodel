using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using com.xiyuansoft.bormodel.metadata;
using com.xiyuansoft.bormodel.commdata;
using System.Data;
using System.Xml;

namespace com.xiyuansoft.bormodel
{
    public class BaseModel : com.xiyuansoft.DataBasePro.DbService
    {
        protected String boModelID;
        protected String tableName;   //表名（显示名称）
        protected String tableCode;   //表物理名（数据库操作名）
        public String getBoModelID()
        {
            return boModelID;
        }
        public String getTableName()
        {
            return tableName;
        }
        public String getTableCode()
        {
            return tableCode;
        }
        
        protected ArrayList initDataAl; //初始记录集
        public ArrayList getInitDataAl()
        {
            return initDataAl;
        }

        protected ArrayList testDataAl; //测试记录集
        public ArrayList TestDataAl
        {
            get { return testDataAl; }
            set { testDataAl = value; }
        }

        protected BaseModel()
        {
    	    createFieldsAl();
	    }

        #region 字段信息定义及操作

        //查询附加字段后缀定义
        public static String fAddFName = "NAME";  //转通用列表引用ID字段为Name字段时用
        public static String fAddFCode = "CODE";  //转通用列表引用ID字段为TCode字段时用
        public static String fAddFFormate = "FMT";//转date类型字段为格式化时间时用

        public static String fAddFMax = "MAX";//转date类型字段为格式化时间时用

        public static string isNew = "isNew"; //编辑时的新建标志传值参数名，本应放在spaServer里，但客户端不需引用该包

        #region 通用备选字段定义
        //对象是否备注
        static public String fNote = "fNote";   //备注。由子类决定是否实现（默认不实现）	
        protected bool noted = false;
        /**
         * @return the noted
         */
        public bool isNoted()
        {
            return noted;
        } 
        #endregion

        //protected SortedList fieldsHt;   //字段集
        protected Dictionary<string, Hashtable> fieldsHt;   //字段集 Dictionary<string, Hashtable>才保证了按加入顺序取出
        /**
         * @return the fieldsHt
         */
        public Dictionary<string, Hashtable> getFieldsHt()
        {
            return fieldsHt;
        }
        public ArrayList getFieldsAl()
        {
            ArrayList retAl = new ArrayList();

            IDictionaryEnumerator enumerator = fieldsHt.GetEnumerator();
            while (enumerator.MoveNext())
            {
                retAl.Add(enumerator.Value);
            }

            return retAl;
        }
        /**
         * @根据字段代码查询到相应原字段，然后返回该字段下指定的字段信息
         */
        public String getFieldInfo(String fCode, String infoName)
        {
            return ((Hashtable)fieldsHt[fCode])[infoName].ToString();
        }

        /**
         * 生成字段信息集，由子类覆盖，并总是先调用父类
         */
        protected virtual void createFieldsAl()
        {
            fieldsHt = new Dictionary<string, Hashtable>();
            initDataAl = new ArrayList();
            if (noted)
            {
                addAStringField("备注", BaseModel.fNote, 200);
            }
        }

        /**
          * 新增一个字段信息
          * @param fFieldName字段名称
          * @param fFieldCode字段代码，同时作为该属性的物理字段名
          * @param fDataType数据类型
          * @param fDataLength数据长度
          * @param fIsPrimaryKey是否主键
          * @param fIsForeignKey是否外键	
          * @param fForeignKeyTable外键表名
          * @param fForeignKeyField外键字段名
          * @param fCommListID通用列表ID
          * @param ForeignTableClass外键业务模型（运行时动态获取模型信息）	
          */
        protected void addAField(
                String fFieldName,//
                String fFieldCode,//
                String fDataType,//
                String fDataLength,//
                String fIsPrimaryKey,//
                String fIsForeignKey,//
                String fForeignKeyTable,//
                String fForeignKeyField,//
                String fCommListID,
                KBoModel ForeignTableClass//	
        )
        {
            Hashtable fDataHp = new Hashtable();
            fieldsHt.Add(fFieldCode, fDataHp);

            fDataHp.Add(Field.fFieldName, fFieldName);
            fDataHp.Add(Field.fFieldCode, fFieldCode);
            fDataHp.Add(Field.fDataType, fDataType);
            fDataHp.Add(Field.fDataLength, fDataLength);
            fDataHp.Add(Field.fIsPrimaryKey, fIsPrimaryKey);
            fDataHp.Add(Field.fIsForeignKey, fIsForeignKey);
            fDataHp.Add(Field.fForeignKeyTable, fForeignKeyTable);
            fDataHp.Add(Field.fForeignKeyField, fForeignKeyField);
            fDataHp.Add(Field.fCommListID, fCommListID);
            fDataHp.Add(Field.fForeignTableClass, ForeignTableClass);

            if (fIsForeignKey == "1")
            {
                ArrayList fkModelRefAl = ForeignTableClass.ModelRefMeAl;
                if (fkModelRefAl.IndexOf(this) == -1)
                {
                    fkModelRefAl.Add(this);
                }
            }
        }
        protected void addAField(Hashtable fDataHp)
        {
            fieldsHt.Add(fDataHp[Field.fFieldCode].ToString(), fDataHp);
        }
        protected void addAField(String fFieldCode,Hashtable fDataHp)
        {
            fieldsHt.Add(fFieldCode, fDataHp);
        }

        /**
         * 新增一个非键字段信息
         * @param fFieldName字段名称
         * @param fFieldCode字段代码，同时作为该属性的物理字段名
         * @param fDataType数据类型
         * @param fDataLength数据长度
         */
        protected void addANotKeyField(
                String fFieldName,//
                String fFieldCode,//
                String fDataType,//
                String fDataLength
        )
        {
            Hashtable fDataHp = new Hashtable();
            fieldsHt.Add(fFieldCode, fDataHp);

            fDataHp.Add(Field.fFieldName, fFieldName);
            fDataHp.Add(Field.fFieldCode, fFieldCode);
            fDataHp.Add(Field.fDataType, fDataType);
            fDataHp.Add(Field.fDataLength, fDataLength);
            fDataHp.Add(Field.fIsPrimaryKey, "0");
            fDataHp.Add(Field.fIsForeignKey, "0");
        }
        protected void addAStringField(
                String fFieldName,//
                String fFieldCode,
                int fDataLength
        )
        {
            Hashtable fDataHp = new Hashtable();
            fieldsHt.Add(fFieldCode, fDataHp);

            fDataHp.Add(Field.fFieldName, fFieldName);
            fDataHp.Add(Field.fFieldCode, fFieldCode);
            fDataHp.Add(Field.fDataType, Field.DataType_text);
            fDataHp.Add(Field.fDataLength, fDataLength.ToString());
            fDataHp.Add(Field.fIsPrimaryKey, "0");
            fDataHp.Add(Field.fIsForeignKey, "0");
        }
        protected void addAIntField(
                String fFieldName,//
                String fFieldCode
        )
        {
            Hashtable fDataHp = new Hashtable();
            fieldsHt.Add(fFieldCode, fDataHp);

            fDataHp.Add(Field.fFieldName, fFieldName);
            fDataHp.Add(Field.fFieldCode, fFieldCode);
            fDataHp.Add(Field.fDataType, Field.DataType_int);
            fDataHp.Add(Field.fIsPrimaryKey, "0");
            fDataHp.Add(Field.fIsForeignKey, "0");            
        }
        protected void addAFloatField(
                String fFieldName,//
                String fFieldCode
        )
        {
            Hashtable fDataHp = new Hashtable();
            fieldsHt.Add(fFieldCode, fDataHp);

            fDataHp.Add(Field.fFieldName, fFieldName);
            fDataHp.Add(Field.fFieldCode, fFieldCode);
            fDataHp.Add(Field.fDataType, Field.DataType_float);
            fDataHp.Add(Field.fIsPrimaryKey, "0");
            fDataHp.Add(Field.fIsForeignKey, "0");
        }
        
        /**
         * 加主键，非引用型
         * @param fFieldName字段名称
         * @param fFieldCode字段代码，同时作为该属性的物理字段名
         */
        protected void addAMainPkField(
                String fFieldName,//
                String fFieldCode
        )
        {
            Hashtable fDataHp = new Hashtable();
            fieldsHt.Add(fFieldCode, fDataHp);

            fDataHp.Add(Field.fFieldName, fFieldName);
            fDataHp.Add(Field.fFieldCode, fFieldCode);
            fDataHp.Add(Field.fDataType, Field.DataType_text);
            fDataHp.Add(Field.fDataLength, "50");
            fDataHp.Add(Field.fIsPrimaryKey, "1");
            fDataHp.Add(Field.fIsForeignKey, "0");
        }

        protected void addAKBoFKeyField(
                String fFieldName,//
                String fFieldCode,//
                String fForeignKeyTable,//
                KBoModel ForeignTableClass//	
        )
        {
            Hashtable fDataHp = new Hashtable();
            fieldsHt.Add(fFieldCode, fDataHp);

            fDataHp.Add(Field.fFieldName, fFieldName);
            fDataHp.Add(Field.fFieldCode, fFieldCode);
            fDataHp.Add(Field.fDataType, Field.DataType_text);
            fDataHp.Add(Field.fDataLength, "50");
            fDataHp.Add(Field.fIsPrimaryKey, "0");
            fDataHp.Add(Field.fIsForeignKey, "1");
            fDataHp.Add(Field.fForeignKeyTable, fForeignKeyTable);
            fDataHp.Add(Field.fForeignKeyField, KBoModel.fID);
            fDataHp.Add(Field.fCommListID, "");
            fDataHp.Add(Field.fForeignTableClass, ForeignTableClass);
            
            ArrayList fkModelRefAl = ForeignTableClass.ModelRefMeAl;
            if (fkModelRefAl.IndexOf(this) == -1)
            {
                fkModelRefAl.Add(this);
            }
        }

        protected void addAComFKeyField(
                String fFieldName,//
                String fFieldCode,//
                String fCommListID	
        )
        {
            Hashtable fDataHp = new Hashtable();
            fieldsHt.Add(fFieldCode, fDataHp);

            fDataHp.Add(Field.fFieldName, fFieldName);
            fDataHp.Add(Field.fFieldCode, fFieldCode);
            fDataHp.Add(Field.fDataType, Field.DataType_text);
            fDataHp.Add(Field.fDataLength, "50");
            fDataHp.Add(Field.fIsPrimaryKey, "0");
            fDataHp.Add(Field.fIsForeignKey, "1");
            fDataHp.Add(Field.fForeignKeyTable, CommList.TableCode);
            fDataHp.Add(Field.fForeignKeyField, KBoModel.fID);
            fDataHp.Add(Field.fCommListID, fCommListID);
            fDataHp.Add(Field.fForeignTableClass, CommList.getnSingInstance());

            ArrayList fkModelRefAl = CommList.getnSingInstance().ModelRefMeAl;
            if (fkModelRefAl.IndexOf(this) == -1)
            {
                fkModelRefAl.Add(this);
            }
        }

        /**
         * 新增一个作为本模型主键的MainKey外键字段信息（主要用于Map类模型）
         * @param fFieldName字段名称
         * @param fFieldCode字段代码，同时作为该属性的物理字段名
         * @param ForeignTableClass外键业务模型（运行时动态获取模型信息）
         */
        protected void addAMapKeyField(
                String fFieldName,//
                String fFieldCode,//
                String fForeignKeyTable,//
                KBoModel ForeignTableClass//	
        )
        {
            Hashtable fDataHp = new Hashtable();
            fieldsHt.Add(fFieldCode, fDataHp);

            fDataHp.Add(Field.fFieldName, fFieldName);
            fDataHp.Add(Field.fFieldCode, fFieldCode);
            fDataHp.Add(Field.fDataType, Field.DataType_text);
            fDataHp.Add(Field.fDataLength, "50");
            fDataHp.Add(Field.fIsPrimaryKey, "1");
            fDataHp.Add(Field.fIsForeignKey, "1");
            fDataHp.Add(Field.fForeignKeyTable, fForeignKeyTable);
            fDataHp.Add(Field.fForeignKeyField, KBoModel.fID);
            fDataHp.Add(Field.fCommListID, "");
            fDataHp.Add(Field.fForeignTableClass, ForeignTableClass);

            ArrayList fkModelRefAl = ForeignTableClass.ModelRefMeAl;
            if (fkModelRefAl.IndexOf(this) == -1)
            {
                fkModelRefAl.Add(this);
            }
        }    
        #endregion

        #region 物理结构生成
        public bool isTableCreated = false;  //用于递归生成表时标识本模型初始数据是否已写入
        /**
         * 生成物理表
         */
        public void createTable()
        {
            if (isTableCreated)
            {
                return;
            }

            foreach (Hashtable fieldHt in fieldsHt.Values)
            {
                if (fieldHt[Field.fIsForeignKey].ToString() == "1")
                {
                    //先写入外键表数据
                    BaseModel ForeignTableClass = (BaseModel)fieldHt[Field.fForeignTableClass];
                    ForeignTableClass.createTable();
                }
            }

            //生成本模型物理表    	
            String tempSql = "";
            tempSql += " create table ";
            tempSql += this.tableCode;
            tempSql += " ( ";
            String fieldString = "";            //构建字段的sql串
            String primaryString = "";            //主键串
            String foreignString = "";            //主键串

            foreach (Hashtable fieldHt in fieldsHt.Values)
            {
                if (fieldString != "")
                {
                    fieldString += ", ";
                }

                fieldString += fieldHt[Field.fFieldCode].ToString() + " ";
                String tDataType = fieldHt[Field.fDataType].ToString();

                if (tDataType==Field.DataType_text)
                {
                    fieldString += " char varying(";
                    fieldString += fieldHt[Field.fDataLength].ToString() + " ";
                    fieldString += ")";
                }
                else if (tDataType==Field.DataType_int)
                {
                    fieldString += " int ";
                }
                else if (tDataType==Field.DataType_float)
                {
                    fieldString += " float ";
                }
                else if (tDataType==Field.DataType_date)
                {
                    fieldString += " datetime ";
                }
                else
                {
                    fieldString += tDataType;
                }

                //是否主键
                if (fieldHt[Field.fIsPrimaryKey].ToString()=="1")
                {
                    fieldString += " not null ";

                    if (primaryString != "")
                    {
                        primaryString += ", ";
                    }
                    primaryString += fieldHt[Field.fFieldCode].ToString();

                }
                else
                {
                    fieldString += " null ";
                }

                if (DataBaseType == "com.xiyuansoft.DataBasePro.SQLite.SQLiteDbAccess,com.xiyuansoft.DataBasePro.SQLite"
                        ||
                        DataBaseType
                        == "com.xiyuansoft.DataBasePro.SQLite64.SQLiteDbAccess,com.xiyuansoft.DataBasePro.SQLite64"
                        )
                {
                    if (fieldHt[Field.fIsForeignKey].ToString() == "1")
                    {
                        //SQLite不支持ALTER TABLE加外键，需在此处支持，但需本方法递归避免生成表顺序不正确
                        foreignString += ", ";
                        foreignString += "FOREIGN KEY ("
                            + fieldHt[Field.fFieldCode].ToString() + ") REFERENCES "
                            + fieldHt[Field.fForeignKeyTable].ToString() + "("
                            + fieldHt[Field.fForeignKeyField].ToString() + ")";
                    }
                }
            }

            tempSql += fieldString + ",constraint PK_"
                + this.tableCode+"_" + System.Guid.NewGuid().ToString("N")
                + " primary key (" + primaryString + ")" + foreignString + " )  ";

            resetAccess();   //对于初始化时生成表，系统启动时无连接参数，因此Access未建立。在执行本操作前应保证已设置连接参数
            exeSql(tempSql);

            isTableCreated = true;
	    }
        public void createTable(bool isRecursion)
        {
            if (isTableCreated)
            {
                return;
            }

            if (isRecursion)
            {
                foreach (Hashtable fieldHt in fieldsHt.Values)
                {
                    if (fieldHt[Field.fIsForeignKey].ToString() == "1")
                    {
                        //先写入外键表数据
                        BaseModel ForeignTableClass = (BaseModel)fieldHt[Field.fForeignTableClass];
                        ForeignTableClass.createTable();
                    }
                }
            }

            //生成本模型物理表    	
            String tempSql = "";
            tempSql += " create table ";
            tempSql += this.tableCode;
            tempSql += " ( ";
            String fieldString = "";            //构建字段的sql串
            String primaryString = "";            //主键串
            String foreignString = "";            //主键串

            foreach (Hashtable fieldHt in fieldsHt.Values)
            {
                if (fieldString != "")
                {
                    fieldString += ", ";
                }

                fieldString += fieldHt[Field.fFieldCode].ToString() + " ";
                String tDataType = fieldHt[Field.fDataType].ToString();

                if (tDataType==Field.DataType_text)
                {
                    fieldString += " char varying(";
                    fieldString += fieldHt[Field.fDataLength].ToString() + " ";
                    fieldString += ")";
                }
                else if (tDataType==Field.DataType_int)
                {
                    fieldString += " int ";
                }
                else if (tDataType==Field.DataType_float)
                {
                    fieldString += " float ";
                }
                else if (tDataType==Field.DataType_date)
                {
                    fieldString += " datetime ";
                }
                else
                {
                    fieldString += tDataType;
                }

                //是否主键
                if (fieldHt[Field.fIsPrimaryKey].ToString()=="1")
                {
                    fieldString += " not null ";

                    if (primaryString != "")
                    {
                        primaryString += ", ";
                    }
                    primaryString += fieldHt[Field.fFieldCode].ToString();

                }
                else
                {
                    fieldString += " null ";
                }

                if (DataBaseType == "com.xiyuansoft.DataBasePro.SQLite.SQLiteDbAccess,com.xiyuansoft.DataBasePro.SQLite"
                        ||
                        DataBaseType
                        == "com.xiyuansoft.DataBasePro.SQLite64.SQLiteDbAccess,com.xiyuansoft.DataBasePro.SQLite64"
                        )
                {
                    if (fieldHt[Field.fIsForeignKey].ToString() == "1")
                    {
                        //SQLite不支持ALTER TABLE加外键，需在此处支持，但需本方法递归避免生成表顺序不正确
                        foreignString += ", ";
                        foreignString += "FOREIGN KEY ("
                            + fieldHt[Field.fFieldCode].ToString() + ") REFERENCES "
                            + fieldHt[Field.fForeignKeyTable].ToString() + "("
                            + fieldHt[Field.fForeignKeyField].ToString() + ")";
                    }
                }
            }

            tempSql += fieldString + ",constraint PK_"
                + this.tableCode+"_" + System.Guid.NewGuid().ToString("N")
                + " primary key (" + primaryString + ")" + foreignString + " )  ";

            resetAccess();   //对于初始化时生成表，系统启动时无连接参数，因此Access未建立。在执行本操作前应保证已设置连接参数
            exeSql(tempSql);

            isTableCreated = true;
	    }
        /**
	     * 生成外键约束
	     * @throws Exception
	     */
        public void createFKey()
        {
            String foreignsqlStr = "";     //生成外键的sql串   

            foreach (Hashtable fieldHt in fieldsHt.Values)
            {
                if (fieldHt[Field.fIsForeignKey].ToString()=="1")
                {
                    if (DataBaseType 
                        == "com.xiyuansoft.DataBasePro.SQLite.SQLiteDbAccess,com.xiyuansoft.DataBasePro.SQLite"
                        ||
                        DataBaseType
                        == "com.xiyuansoft.DataBasePro.SQLite64.SQLiteDbAccess,com.xiyuansoft.DataBasePro.SQLite64"
                        )
                        
                    {
                        //SQLite不支持
                        //foreignsqlStr += " ALTER TABLE " 
                        //    + this.tableCode 
                        //    + " ADD FOREIGN KEY "
                        //    + fieldHt[Field.fFieldCode].ToString() + " "
                        //    + fieldHt[Field.fForeignKeyTable].ToString() + "(" 
                        //    + fieldHt[Field.fForeignKeyField].ToString() + ") ";
                    }
                    else
                    {
                        foreignsqlStr += " alter table ";
                        foreignsqlStr += this.tableCode;

                        foreignsqlStr += " add constraint FK_"
                            + System.Guid.NewGuid().ToString("N");

                        foreignsqlStr += " foreign key (";
                        foreignsqlStr += fieldHt[Field.fFieldCode].ToString();
                        foreignsqlStr += ") ";
                        foreignsqlStr += " references ";

                        foreignsqlStr += fieldHt[Field.fForeignKeyTable].ToString();
                        foreignsqlStr += " (";
                        foreignsqlStr += fieldHt[Field.fForeignKeyField].ToString();
                        foreignsqlStr += ")  ";

                    }
                }
            }

            if (foreignsqlStr!="")
            {
                exeSql(foreignsqlStr);
            }
        }
        /*
	     * 生成元数据信息（表信息及字段信息）
	     * （基本信息在数据在模型对象中可直接取，扩展业务信息应需从数据库中读？？此外动态数据结构需要？）
	     * 
	     */
        public void createMetaData()
        {
            //暂不实现
        }

        public bool isInitDataIn = false;  //用于递归生成表时标识本模型初始数据是否已写入
        public List<IInitData> initDataList = new List<IInitData>();
        /**
	     * 生成初始数据记录
	     * @throws Exception
	     */
        public void createInitRecords()
        {
		    if(isInitDataIn){
			    return;
            }

            isInitDataIn = true;

            foreach (IInitData initData in initDataList)
            {
                initData.getInitData(this);
            }

            foreach (Hashtable fieldHt in fieldsHt.Values)
            {
                if(fieldHt[Field.fIsForeignKey].ToString()=="1"){
            	    //先写入外键表数据
            	    BaseModel ForeignTableClass=(BaseModel)fieldHt[Field.fForeignTableClass];
            	    ForeignTableClass.createInitRecords();
                }            
	        }
	    
	        String initDatasqlStr = "";     //生成外键的sql串   

	        String fieldsStr="";
	        String valuesStr="";
            BeginTrans();
            foreach (Hashtable fDatasHt in initDataAl)
            {
                //tree, write TreeQueryHelper table
                if (this is Tree)
                {
                    string NodeID = fDatasHt[Tree.fID].ToString();
                    fDatasHt.Remove(Tree.fID);
                    string upNodeID = fDatasHt[Tree.fUID].ToString();
                    fDatasHt.Remove(Tree.fUID);
                    (this as Tree).insertNode(NodeID, upNodeID, fDatasHt);
                }
                else
                {
                    insertRecord(fDatasHt);
                }
	        }
            CommitTrans();
        }

        public bool isTestDataIn = false;  //用于递归生成表时标识本模型初始数据是否已写入
        public ITestData testDataGen = null;  //仅用于向后兼容
        public List<ITestData> testDataList = new List<ITestData>();
        /**
	     * 生成初始数据记录
	     * @throws Exception
	     */
        public void createTestRecords()
        {

            if (isTestDataIn)
            {
			    return;
            }

            isTestDataIn = true;

            //仅用于向后兼容
            if (testDataGen == null)
            {
                //return;
            }
            else
            {
                testDataGen.getTestData(this);
            }
            
            foreach (ITestData initData in testDataList)
            {
                initData.getTestData(this);
            }

            if (testDataAl == null)
            {
                return;
            }

            foreach (Hashtable fieldHt in fieldsHt.Values)
            {
                if(fieldHt[Field.fIsForeignKey].ToString()=="1"){
            	    //先写入外键表数据
            	    BaseModel ForeignTableClass=(BaseModel)fieldHt[Field.fForeignTableClass];
                    ForeignTableClass.createTestRecords();
                }            
	        }
	    
	        String testDatasqlStr = "";     //生成外键的sql串   

	        String fieldsStr="";
	        String valuesStr="";
            BeginTrans();
            foreach (Hashtable fDatasHt in testDataAl)
            {
                //tree, write TreeQueryHelper table
                if (this is Tree)
                {
                    string NodeID=fDatasHt[Tree.fID].ToString();
                    fDatasHt.Remove(Tree.fID);
                    string upNodeID = fDatasHt[Tree.fUID].ToString();
                    fDatasHt.Remove(Tree.fUID);
                    (this as Tree).insertNode(NodeID, upNodeID, fDatasHt);
                }
                else
                {
                    insertRecord(fDatasHt);
                }
            }
            CommitTrans();
        }
        
        
        #endregion

        #region 数据字典



        #endregion

        #region SQL操作
        
        #region 查询操作

        public DataTable selectAll()
        {
            string sqlStr = "select * from " + tableCode;
            return exeSqlForDataSet(sqlStr);
        }
        public DataTable selectAllAndIndexed()
        {
            string sqlStr = "select * from " + tableCode;

            if (fieldsHt[KBoModel.fIndex] != null)
            {
                sqlStr += " order by " + KBoModel.fIndex;
            }

            return exeSqlForDataSet(sqlStr);
        }
        public DataTable selectAllAndIndexed(bool DESC)
        {
            string sqlStr = "select * from " + tableCode;

            if (fieldsHt[KBoModel.fIndex] != null)
            {
                sqlStr += " order by " + KBoModel.fIndex;
                if (DESC)
                {
                    sqlStr += " DESC ";
                }
            }

            return exeSqlForDataSet(sqlStr);
        }
        public DataTable selectAllOrderByOneField(string indexField)
        {
            string sqlStr = "select * from " + tableCode;

            sqlStr += " order by " + indexField; 

            return exeSqlForDataSet(sqlStr);
        }
        public DataTable selectAll_JoCom()
        {
            //查询静态列表引用字段关计算sql生成相关字符串
            String addSelectString = "";
            String leftJoinString = "";

            Hashtable tempV = createLeftJoinAndAddFieldStringsForCommList();
            addSelectString += tempV["addSelectString"].ToString();
            leftJoinString += tempV["leftJoinString"].ToString();

            //addSelectString += createAddFieldStringsForDateField(); //暂不处理日期字段

            String tempSql = "";
            tempSql += " select " + tableCode + ".*" + addSelectString + " from " + tableCode + leftJoinString;

            return exeSqlForDataSet(tempSql);
        }
        public DataTable selectAll_JoPro(Hashtable fTFields)
        {
            //查询静态列表引用字段关计算sql生成相关字符串
            String addSelectString = "";
            String leftJoinString = "";

            Hashtable tempV = createLeftJoinAndAddFieldStringsForCommList();
            addSelectString += tempV["addSelectString"].ToString();
            leftJoinString += tempV["leftJoinString"].ToString();

            tempV = createLeftJoinAndAddFieldStringsForGenBiz(fTFields);
            addSelectString += tempV["addSelectString"].ToString();
            leftJoinString += tempV["leftJoinString"].ToString();

            //addSelectString += createAddFieldStringsForDateField(); //暂不处理日期字段

            String tempSql = "";
            tempSql += " select " + tableCode + ".*" + addSelectString + " from " + tableCode + leftJoinString;

            return exeSqlForDataSet(tempSql);
        }
        public DataTable selectAllOrderByOneField_JoCom(string indexField)
        {
            //查询静态列表引用字段关计算sql生成相关字符串
            String addSelectString = "";
            String leftJoinString = "";

            Hashtable tempV = createLeftJoinAndAddFieldStringsForCommList();
            addSelectString += tempV["addSelectString"].ToString();
            leftJoinString += tempV["leftJoinString"].ToString();

            //addSelectString += createAddFieldStringsForDateField(); //暂不处理日期字段

            String tempSql = "";
            tempSql += " select " + tableCode + ".*" + addSelectString + " from " + tableCode + leftJoinString;
            tempSql += " order by " + indexField; 
            return exeSqlForDataSet(tempSql);
        }
       
        public int selectAllCount()
        {
            string sqlStr = "select count(*) from " + tableCode;
            return int.Parse(exeSqlForDataSet(sqlStr).Rows[0][0].ToString());
        }

        public DataTable selectByOneField(string fFieldCode, string fFieldValue)
        {
            string sqlStr = "select * from " + tableCode + " where " + fFieldCode + "='" + fFieldValue + "'";
            return exeSqlForDataSet(sqlStr);
        }
        public DataTable selectByOneFieldLike(string fFieldCode, string fFieldValue)
        {
            string sqlStr = "select * from " + tableCode + " where " + fFieldCode + " like '%" + fFieldValue + "%'";
            return exeSqlForDataSet(sqlStr);
        }
        public DataTable selectByOneFieldAndOrderByOneField(string fFieldCode, string fFieldValue, string indexField)
        {
            string sqlStr = "select * from " + tableCode + " where " + fFieldCode + "='" + fFieldValue + "'";
            sqlStr += " order by " + indexField; 
            return exeSqlForDataSet(sqlStr);
        }
        public DataTable selectByOneFieldAndOrderByMutiField(string fFieldCode, string fFieldValue, List<string> indexFieldList)
        {
            string sqlStr = "select * from " + tableCode + " where " + fFieldCode + "='" + fFieldValue + "'";
            string indexStr = null;
            foreach (string tempIndexField in indexFieldList)
            {
                if (indexStr == null)
                {
                    indexStr = tempIndexField;
                }
                else
                {
                    indexStr += "," + tempIndexField;
                }
            }
            if (indexStr != null)
            {
                sqlStr += " order by " + indexStr;
            }

            return exeSqlForDataSet(sqlStr);
        }

        public int selectCountByOneField(string fFieldCode, string fFieldValue)
        {
            string sqlStr = "select count(*) from " + tableCode + " where " + fFieldCode + "='" + fFieldValue + "'";
            return int.Parse(exeSqlForDataSet(sqlStr).Rows[0][0].ToString());
        }

        /**
	     * 单表查询-条件任意一个字段及期值，并左连接静态数据列表查询出静态列表数据,并格式化日期字段
	     * @param fieldName字段名称
	     * @param fieldValue字段值
	     * @return
	     * @throws Exception
	     */
	    public DataTable selectByOneField_JoCom(
			    String fieldName,
			    String fieldValue
			    ){
		    //查询静态列表引用字段关计算sql生成相关字符串
		    String addSelectString="";
		    String leftJoinString="";
		
		    Hashtable tempV=createLeftJoinAndAddFieldStringsForCommList();
		    addSelectString+=tempV["addSelectString"].ToString();
		    leftJoinString+=tempV["leftJoinString"].ToString();

            //addSelectString += createAddFieldStringsForDateField(); //暂不处理日期字段
		
    	    String tempSql="";
    	    tempSql += " select "+tableCode+".*"+addSelectString+" from " + tableCode + leftJoinString
    		    + " where "+tableCode+"."+fieldName+"='"+fieldValue+"'";

            return exeSqlForDataSet(tempSql);
	    }
	    /**
	     * 单表查询-条件任意一个字段及期值，并左连接静态数据列表查询出静态列表数据,并左连接非静态数据表数据，并格式化日期字段
	     * @param fieldName字段名称
	     * @param fieldValue字段值
	     * @param fTFields非静态列表类外键表取值字段
	     * @return
	     * @throws Exception
	     */
        public DataTable selectByOneField_JoPro(
			    String fieldName,
			    String fieldValue,
			    Hashtable fTFields
			    ){
		    //查询静态列表引用字段关计算sql生成相关字符串
		    String addSelectString="";
		    String leftJoinString="";

            Hashtable tempV = createLeftJoinAndAddFieldStringsForCommList();
            addSelectString += tempV["addSelectString"].ToString();
            leftJoinString += tempV["leftJoinString"].ToString();
		
		    tempV=createLeftJoinAndAddFieldStringsForGenBiz(fTFields);
            addSelectString += tempV["addSelectString"].ToString();
            leftJoinString += tempV["leftJoinString"].ToString();
		
		    //addSelectString+=createAddFieldStringsForDateField(); //暂不处理日期字段
		
    	    String tempSql="";
    	    tempSql += " select "+tableCode+".*"+addSelectString+" from " + tableCode + leftJoinString
    		    + " where "+tableCode+"."+fieldName+"='"+fieldValue+"'";

            return exeSqlForDataSet(tempSql);
	    }

        public DataTable select_JoPro(
                String whereStr,
                Hashtable fTFields
                )
        {
            //查询静态列表引用字段关计算sql生成相关字符串
            String addSelectString = "";
            String leftJoinString = "";

            Hashtable tempV = createLeftJoinAndAddFieldStringsForCommList();
            addSelectString += tempV["addSelectString"].ToString();
            leftJoinString += tempV["leftJoinString"].ToString();

            if (fTFields != null)
            {
                tempV = createLeftJoinAndAddFieldStringsForGenBiz(fTFields);
                addSelectString += tempV["addSelectString"].ToString();
                leftJoinString += tempV["leftJoinString"].ToString();
            }
            //addSelectString+=createAddFieldStringsForDateField(); //暂不处理日期字段

            String tempSql = "";
            tempSql += " select " + tableCode + ".*" + addSelectString + " from " + tableCode + leftJoinString
                + " where " + whereStr;

            return exeSqlForDataSet(tempSql);
        }
        
        public DataTable select_JoMutiField(
                String whereStr,
                Dictionary<string,List<string>> DFields
                )
        {
            //查询静态列表引用字段关计算sql生成相关字符串
            String addSelectString = "";
            String leftJoinString = "";

            Hashtable tempV = createLeftJoinAndAddFieldStringsForCommList();
            addSelectString += tempV["addSelectString"].ToString();
            leftJoinString += tempV["leftJoinString"].ToString();

            if (DFields != null)
            {
                Dictionary<string, string> tempD = createLeftJoinAndAddFieldStringsForGenBizMF(DFields);
                addSelectString += tempD["addSelectString"].ToString();
                leftJoinString += tempD["leftJoinString"].ToString();
            }
            //addSelectString+=createAddFieldStringsForDateField(); //暂不处理日期字段

            String tempSql = "";
            tempSql += " select " + tableCode + ".*" + addSelectString + " from " + tableCode + leftJoinString
                + " where " + whereStr;

            return exeSqlForDataSet(tempSql);
        }

        #region 返回List Dictionary的查询
        public Dictionary<string,string> DtRecordToDic(DataRow dr)
        {
            Dictionary<string, string> retDic = new Dictionary<string, string>();
            
            foreach(DataColumn dc in dr.Table.Columns)// fieldsHt.Keys)
            {
                retDic.Add(dc.ColumnName, dr[dc.ColumnName].ToString());
            }
            return retDic;
        }
        public List<Dictionary<string,string>> DtToDicList(DataTable Dt)
        {
            List<Dictionary<string, string>> retList = new List<Dictionary<string, string>>();
            foreach(DataRow dr in Dt.Rows)
            {
                retList.Add(DtRecordToDic(dr));
            }
            return retList;
        }

        //20170511add
        public Dictionary<string, List<Dictionary<string, string>>>
            TransDataTableToMapedList(DataTable dt, string keyField)
        {
            if (!dt.Columns.Contains(keyField))
            {
                throw new ApplicationException("键字段不存在，不支持此转换");
            }

            Dictionary<string, List<Dictionary<string, string>>> retDic
                = new Dictionary<string, List<Dictionary<string, string>>>();
            List<Dictionary<string, string>> tList;
            foreach (DataRow dr in dt.Rows)
            {
                string keyString = dr[keyField].ToString();
                if (!retDic.ContainsKey(keyString))
                {
                    retDic.Add(keyString, new List<Dictionary<string, string>>());
                }
                tList = retDic[keyString];

                Dictionary<string, string> rowDic = new Dictionary<string, string>();
                tList.Add(rowDic);
                foreach (DataColumn dc in dt.Columns)
                {
                    rowDic.Add(dc.ColumnName, dr[dc.ColumnName].ToString());
                }
            }

            return retDic;
        }

        public List<Dictionary<string, string>> selectAllList()
        {
            return DtToDicList(selectAll());
        }
        public List<Dictionary<string, string>> fullSelectList()
        {
            return DtToDicList(fullSelect());
        }

        #endregion

        #region NewSelecter

        /// <summary>
        /// 执行全查询（含所有外键连接表记录，用leftjoin方式）
        /// </summary>
        /// <returns></returns>
        public DataTable fullSelect()
        {
            return exeSqlForDataSet(getfullSelectStr());
        }
        /// <summary>
        /// 执行全查询（含所有外键连接表记录，用leftjoin方式）
        /// </summary>
        /// <returns></returns>
        public DataTable fullSelect(List<string> condictionList)
        {
            return exeSqlForDataSet(getfullSelectStr(condictionList));
        }
        /// <summary>
        /// 执行全查询（含所有外键连接表记录，用leftjoin方式）
        /// </summary>
        /// <returns></returns>
        public DataTable fullSelect(List<string> condictionList, List<string> indexFieldList)
        {
            return exeSqlForDataSet(getfullSelectStr(condictionList, indexFieldList));
        }

        #endregion

        #region SqlCreater

        /// <summary>
        /// select语句生成器
        /// </summary>
        /// <param name="selectFieldList">选择字段集</param>
        /// <param name="selectTableList">选择表集</param>
        /// <param name="condictionList">选择条件集</param>
        /// <param name="indexFieldList">结果排序字段集</param>
        /// <returns></returns>
        public string SelectCreater(
            List<string> selectFieldList,
            List<string> selectTableList,
            List<string> condictionList,
            List<string> indexFieldList
            )
        {
            if (selectFieldList == null || selectFieldList.Count == 0)
            {
                throw new Exception("无选择字段");
            }
            if (selectTableList == null || selectTableList.Count == 0)
            {
                throw new Exception("无选择表");
            }
            string selectFieldString = ListStrToCommaString(selectFieldList);
            string selectTableString = ListStrToCommaString(selectTableList);
            string condictionString = ListStrToAndString(condictionList);
            string indexFieldString = ListStrToCommaString(indexFieldList);

            string sqlStr = "SELECT " + selectFieldString + " FROM " + selectTableString;
            if (condictionList != null)
            {
                sqlStr += " WHERE " + condictionString;
            }
            if (indexFieldString != null)
            {
                sqlStr += " ORDER BY " + indexFieldString;
            }

            return sqlStr;
        }
       
        /// <summary>
        /// 获取执行全查询（含所有外键连接表记录，用leftjoin方式）sql串
        /// </summary>
        /// <returns></returns>
        public string getfullSelectStr()
        {
            Dictionary<string, List<string>> leftJoinDic = getAllLeftJoinData();
            
            List<string> selectFieldList = getAllFieldsSelectList();
            selectFieldList.AddRange(leftJoinDic["leftJoinFieldList"]);

            List<string> selectTableList = leftJoinDic["leftJoinTableList"];

            List<string> condictionList = null;

            List<string> indexFieldList = null;

            string sqlStr = SelectCreater(
                selectFieldList,
                selectTableList,
                condictionList,
                indexFieldList
                );

            return sqlStr;
        }
        /// <summary>
        /// 获取执行全查询（含所有外键连接表记录，用leftjoin方式）sql串
        /// </summary>
        /// <returns></returns>
        public string getfullSelectStr(List<string> condictionList)
        {
            Dictionary<string, List<string>> leftJoinDic = getAllLeftJoinData();

            List<string> selectFieldList = getAllFieldsSelectList();
            selectFieldList.AddRange(leftJoinDic["leftJoinFieldList"]);

            List<string> selectTableList = leftJoinDic["leftJoinTableList"];
            
            List<string> indexFieldList = null;

            string sqlStr = SelectCreater(
                selectFieldList,
                selectTableList,
                condictionList,
                indexFieldList
                );

            return sqlStr;
        }
        /// <summary>
        /// 获取执行全查询（含所有外键连接表记录，用leftjoin方式）sql串
        /// </summary>
        /// <returns></returns>
        public string getfullSelectStr(List<string> condictionList, List<string> indexFieldList)
        {
            Dictionary<string, List<string>> leftJoinDic = getAllLeftJoinData();

            List<string> selectFieldList = getAllFieldsSelectList();
            selectFieldList.AddRange(leftJoinDic["leftJoinFieldList"]);

            List<string> selectTableList = leftJoinDic["leftJoinTableList"];

            string sqlStr = SelectCreater(
                selectFieldList,
                selectTableList,
                condictionList,
                indexFieldList
                );

            return sqlStr;
        }

        /// <summary>
        /// 把List<string>转换成以逗号分隔的string
        /// </summary>
        /// <returns></returns>
        public string ListStrToCommaString(List<string> stringList)
        {
            string retStr = null;
            if (stringList != null)
            {
                foreach (string fStr in stringList)
                {
                    if (retStr == null)
                    {
                        retStr = fStr;
                    }
                    else
                    {
                        retStr += "," + fStr;
                    }
                }
            }

            return retStr;
        }
        /// <summary>
        /// 把List<string>转换成以and分隔的string
        /// </summary>
        /// <returns></returns>
        public string ListStrToAndString(List<string> stringList)
        {
            string retStr = null;
            if (stringList != null)
            {
                foreach (string fStr in stringList)
                {
                    if (retStr == null)
                    {
                        retStr = fStr;
                    }
                    else
                    {
                        retStr += " AND " + fStr;
                    }
                }
            }

            return retStr;
        }

        /// <summary>
        /// 获取逗号分隔格式的字段字符串
        /// </summary>
        /// <returns></returns>
        public string getAllFieldsSelectCommaStr()
        {
            string retStr = null;
            foreach (string fStr in fieldsHt.Keys)
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
            return retStr;
        }
        /// <summary>
        /// 获取字段List<string>格式，只含字段名
        /// </summary>
        /// <returns></returns>
        public List<string> getAllFieldsSelectList()
        {
            List<string> retList = new List<string>();
            foreach (string fStr in fieldsHt.Keys)
            {
                retList.Add(this.tableCode + "." + fStr);
            }
            return retList;
        }

        /// <summary>
        /// 处理所有外键leftjoin连接，生成外表字段集及外表leftjoin串
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<string>> getAllLeftJoinData()
        {
            Dictionary<string, List<string>> retD = new Dictionary<string, List<string>>();
            List<string> leftJoinFieldList = new List<string>();
            List<string> leftJoinTableList = new List<string>();
            retD.Add("leftJoinFieldList", leftJoinFieldList);
            retD.Add("leftJoinTableList", leftJoinTableList);

            String tableString = this.tableCode;

            IDictionaryEnumerator iter;

            iter = fieldsHt.GetEnumerator();
            while (iter.MoveNext())
            {
                Hashtable fDataHp = iter.Value as Hashtable;
                if (fDataHp[Field.fIsForeignKey].ToString() == "1")
                {
                    if (fDataHp[Field.fForeignKeyTable].ToString() == CommList.TableCode)
                    {
                        String leftJoinTable = CommList.TableCode;
                        String leftJoinTablePro = CommList.TableCode + fDataHp[Field.fFieldCode].ToString();
                        foreach (string fStr in CommList.getnSingInstance().fieldsHt.Keys)
                        {
                            leftJoinFieldList.Add(leftJoinTablePro + "." + fStr + " " + fDataHp[Field.fFieldCode].ToString() + fStr);
                        }
                        tableString += " left join " + leftJoinTable + " " + leftJoinTablePro + " on " + tableCode + "."
                            + fDataHp[Field.fFieldCode].ToString() + "=" + leftJoinTablePro + "." + CommList.fID
                            ;
                    }
                    else
                    {
                        String leftJoinTable = fDataHp[Field.fForeignKeyTable].ToString();
                        String leftJoinTablePro = leftJoinTable + fDataHp[Field.fFieldCode].ToString();
                        KBoModel leftjoinObj = fDataHp[Field.fForeignTableClass] as KBoModel;
                        foreach (string fStr in leftjoinObj.fieldsHt.Keys)
                        {
                            leftJoinFieldList.Add(leftJoinTablePro + "." + fStr + " " + fDataHp[Field.fFieldCode].ToString() + fStr);
                        }
                        tableString += " left join " + leftJoinTable + " " + leftJoinTablePro + " on " + tableCode + "."
                            + fDataHp[Field.fFieldCode].ToString() + "=" + leftJoinTablePro + "." + KBoModel.fID
                            ;
                    }
                }
            }

            leftJoinTableList.Add(tableString);
            return retD;
        }

        #endregion
        
        #endregion
        
        #region 更新操作

        public void insertRecord(Hashtable recordHt)
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
            sqlStr = "insert into " + tableCode + "(" + fieldsStr + ") values(" + valuesStr + ")";
            exeSql(sqlStr);
        }
        public void insertRecord(Dictionary<string, string> recordHt)
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
            sqlStr = "insert into " + tableCode + "(" + fieldsStr + ") values(" + valuesStr + ")";
            exeSql(sqlStr);
        }


        public void updateAll(Hashtable recordHt)
        {
            string sqlStr = "";
            string updateStr = "";

            foreach (string field in recordHt.Keys)
            {
                if (updateStr != "")
                {
                    updateStr += ",";
                }
                updateStr += field + "='" + recordHt[field] + "'";
            }
            sqlStr = "update " + tableCode + " set " + updateStr;
            exeSql(sqlStr);
        }
        public void updateAll(Dictionary<string, string> recordHt)
        {
            string sqlStr = "";
            string updateStr = "";

            foreach (string field in recordHt.Keys)
            {
                if (updateStr != "")
                {
                    updateStr += ",";
                }
                if (recordHt[field] == null)
                {
                    updateStr += field + "=null";
                }
                else
                {
                    updateStr += field + "='" + recordHt[field] + "'";
                }
            }
            sqlStr = "update " + tableCode + " set " + updateStr;
            exeSql(sqlStr);
        }

        public void updateByOneField(string fFieldCode, string fFieldValue,Hashtable recordHt)
        {
            string sqlStr = "";
            string updateStr = "";

            foreach (string field in recordHt.Keys)
            {
                if (updateStr != "")
                {
                    updateStr += ",";
                }
                updateStr += field + "='" + recordHt[field] + "'";
            }
            sqlStr = "update " + tableCode + " set " + updateStr + " where " + fFieldCode + "='" + fFieldValue + "'";
            exeSql(sqlStr);
        }
        public void updateByOneField(string fFieldCode, string fFieldValue,Dictionary<string,string> recordHt)
        {
            string sqlStr = "";
            string updateStr = "";

            foreach (string field in recordHt.Keys)
            {
                if (updateStr != "")
                {
                    updateStr += ",";
                }
                if (recordHt[field] == null)
                {
                    updateStr += field + "=null";
                }
                else
                {
                    updateStr += field + "='" + recordHt[field] + "'";
                }
            }
            sqlStr = "update " + tableCode + " set " + updateStr + " where " + fFieldCode + "='" + fFieldValue + "'";
            exeSql(sqlStr);
        }

        public void updateByOneField(
            string fFieldCode, 
            string fFieldValue, 
            string tFieldCode,
            string tFieldValue)
        {
            string sqlStr = "";
            string updateStr = "";

            updateStr += tFieldCode + "='" + tFieldValue + "'";
            sqlStr = "update " + tableCode + " set " + updateStr + " where " + fFieldCode + "='" + fFieldValue + "'";
            exeSql(sqlStr);
        }
        public void updateByMutiField(Hashtable fieldHt, Hashtable recordHt)
        {
            string sqlStr = "";
            string updateStr = "";
            string whereStr = "";

            foreach (string field in recordHt.Keys)
            {
                if (updateStr != "")
                {
                    updateStr += ",";
                }
                updateStr += field + "='" + recordHt[field] + "'";
            }

            foreach (string field in fieldHt.Keys)
            {
                if (whereStr != "")
                {
                    whereStr += " and ";
                }
                whereStr += field + "='" + fieldHt[field] + "'";
            }

            sqlStr = "update " + tableCode + " set " + updateStr;
            if (whereStr != "")
            {
                sqlStr += " where " + whereStr;
            }

            exeSql(sqlStr);
        }
        public void updateByMutiField(Dictionary<string, string> fieldHt, Dictionary<string, string> recordHt)
        {
            string sqlStr = "";
            string updateStr = "";
            string whereStr = "";

            foreach (string field in recordHt.Keys)
            {
                if (updateStr != "")
                {
                    updateStr += ",";
                }
                if (recordHt[field] == null)
                {
                    updateStr += field + "=null";
                }
                else
                {
                    updateStr += field + "='" + recordHt[field] + "'";
                }
            }

            foreach (string field in fieldHt.Keys)
            {
                if (whereStr != "")
                {
                    whereStr += " and ";
                }
                whereStr += field + "='" + fieldHt[field] + "'";
            }

            sqlStr = "update " + tableCode + " set " + updateStr;
            if (whereStr != "")
            {
                sqlStr += " where " + whereStr;
            }

            exeSql(sqlStr);
        }

        public void deleteAll()
        {
            exeSql("DELETE FROM " + tableCode);
        }
        public void deleteByOneField(string fFieldCode, string fFieldValue)
        {
            string sqlStr = "";
            sqlStr = "delete from " + tableCode + " where " + fFieldCode + "='" + fFieldValue + "'";
            exeSql(sqlStr);
        }

        public void deleteByMutiField(Hashtable infieldsHt)
        {
            string sqlStr = "";
            string whereStr = "";

            foreach (string field in infieldsHt.Keys)
            {
                if (whereStr != "")
                {
                    whereStr += " and ";
                }
                whereStr += field + "='" + infieldsHt[field] + "'";
            }
            sqlStr = "delete from " + tableCode + " where " + whereStr;
            exeSql(sqlStr);
        }
        #endregion

        #region 统计类查询

        public DataTable selectMaxValueByOnField(
            String maxFieldName,
            String fFieldCode,
            String fFieldValue
            )
        {
            String tempSql = "";
            tempSql += " select " + fFieldCode + ",max(" + maxFieldName + ") " + maxFieldName + fAddFMax
                + " from " + tableCode
                + " where " + fFieldCode + "='" + fFieldValue + "'"
                + " group by " + fFieldCode;

            return exeSqlForDataSet(tempSql);
        }

        public DataTable selectMaxValueByGroup(
        String maxFieldName,
        String groupFieldName
        )
        {
            String tempSql = "";
            tempSql += " select " + groupFieldName + ",max(" + maxFieldName + ") " + maxFieldName + fAddFMax 
                + " from " + tableCode
                + " group by " + groupFieldName;

            return exeSqlForDataSet(tempSql);
        }

        #endregion

        #region SQL语句生成辅助方法

	    /**
	     * 针对通用数据引用生成查询所需的leftjoin串，及引用字段对应的外键表中值的字段名称附加select串，
	     * 在返回的Hashtable中分别存储在leftJoinString及addSelectString键下
	     */
	    public Hashtable createLeftJoinAndAddFieldStringsForCommList(){
		    Hashtable tempHM=new Hashtable();
		    String addSelectString="";
		    String leftJoinString="";
		
            IDictionaryEnumerator iter;

            iter = fieldsHt.GetEnumerator();
            while (iter.MoveNext())
            {
                Hashtable fDataHp = iter.Value as Hashtable;
                if(fDataHp[Field.fIsForeignKey].ToString()=="1" && 
            		    fDataHp[Field.fForeignKeyTable].ToString()==CommList.TableCode){
            	    String leftJoinTable=CommList.TableCode;
            	    String leftJoinTablePro=CommList.TableCode+fDataHp[Field.fFieldCode].ToString();
            	    addSelectString+=","+leftJoinTablePro
            		    +"."+CommList.fName+" "+fDataHp[Field.fFieldCode].ToString()+fAddFName
            		    +","+leftJoinTablePro
            		    +"."+CommList.fTCode+" "+fDataHp[Field.fFieldCode].ToString()+fAddFCode
            		    ;
            	    leftJoinString+=" left join "+leftJoinTable+" "+leftJoinTablePro+" on "+tableCode+"."
            		    +fDataHp[Field.fFieldCode].ToString()+"="+leftJoinTablePro+"."+CommList.fID
            		    ;

                }
	        }
	    
		    tempHM.Add("addSelectString", addSelectString);
		    tempHM.Add("leftJoinString", leftJoinString);
		    return tempHM;
	    }

	    /**
	     * 针对Date类型字段生成附加select串，在原字段名后加fAddFFormate，以格式化输出日期串，
	     */
	    public String createAddFieldStringsForDateField(){
		    String addSelectString="";		

		    //int lJoinCount=0;
		
		    IDictionaryEnumerator iter;
    	
	        iter = fieldsHt.GetEnumerator();
	        while (iter.MoveNext())
            {
	    	    Hashtable fDataHp = iter.Value as Hashtable;
                if(fDataHp[Field.fDataType].ToString()=="date"){
            	    addSelectString+=",to_char("+tableCode+"."+fDataHp[Field.fFieldCode].ToString()
            		    +",'yyyy-mm-dd') " + fDataHp[Field.fFieldCode].ToString()+fAddFFormate;
                }
	        }
	    
		    return addSelectString;
	    }

	    /**
	     * 针对指定的外键业务对象引用生成查询所需的leftjoin串，及引用字段对应的外键表中值的字段名称附加select串，
	     * 在返回的Hashtable中分别存储在leftJoinString及addSelectString键下
	     */
	    public Hashtable createLeftJoinAndAddFieldStringsForGenBiz(Hashtable fTFields){
		    Hashtable tempHM=new Hashtable();
		    String addSelectString="";
		    String leftJoinString="";
		
		    IDictionaryEnumerator iter;
    	
	        iter = fieldsHt.GetEnumerator();
	        while (iter.MoveNext())
            {
	    	    Hashtable fDataHp = iter.Value as Hashtable;
                if(fDataHp[Field.fIsForeignKey].ToString()=="1" &&
                        fDataHp[Field.fForeignKeyTable].ToString() != CommList.TableCode)
                {
            	    String leftJoinTable=fDataHp[Field.fForeignKeyTable].ToString();            	
            	    if(fTFields[leftJoinTable]!= null){
            		    String leftJoinForField=fTFields[leftJoinTable].ToString();
	            	    String leftJoinTablePro=leftJoinTable+fDataHp[Field.fFieldCode].ToString();
	            	    addSelectString+=","+leftJoinTablePro
	            		    +"."+leftJoinForField+" "+fDataHp[Field.fFieldCode].ToString()+fAddFName
	            		    ;
	            	    leftJoinString+=" left join "+leftJoinTable+" "+leftJoinTablePro+" on "+tableCode+"."
	            		    +fDataHp[Field.fFieldCode].ToString()+"="+leftJoinTablePro+"."+KBoModel.fID
	            		    ;
	
            	    }
                }
	        }
	    
		    tempHM.Add("addSelectString", addSelectString);
		    tempHM.Add("leftJoinString", leftJoinString);
		    return tempHM;
	    }

        public Dictionary<string, string> 
            createLeftJoinAndAddFieldStringsForGenBizMF(Dictionary<string, List<string>> DFields)
        {
            Dictionary<string, string> tempHM = new Dictionary<string, string>();
            String addSelectString = "";
            String leftJoinString = "";

            IDictionaryEnumerator iter;

            iter = fieldsHt.GetEnumerator();
            while (iter.MoveNext())
            {
                Hashtable fDataHp = iter.Value as Hashtable;
                if (fDataHp[Field.fIsForeignKey].ToString() == "1" &&
                        fDataHp[Field.fForeignKeyTable].ToString() != CommList.TableCode)
                {
                    String leftJoinTable = fDataHp[Field.fForeignKeyTable].ToString();
                    if (DFields.ContainsKey(fDataHp[Field.fFieldCode].ToString()))
                    {
                        String leftJoinTablePro = leftJoinTable + fDataHp[Field.fFieldCode].ToString();

                        foreach (string leftJoinForField in DFields[fDataHp[Field.fFieldCode].ToString()])
                        {
                            addSelectString += "," + leftJoinTablePro
                                + "." + leftJoinForField + " " 
                                + fDataHp[Field.fFieldCode].ToString() + leftJoinTable + leftJoinForField
                                ;
                        }

                        leftJoinString += " left join " + leftJoinTable + " " + leftJoinTablePro + " on " + tableCode + "."
                            + fDataHp[Field.fFieldCode].ToString() + "=" + leftJoinTablePro + "." + KBoModel.fID
                            ;

                    }
                }
            }

            tempHM.Add("addSelectString", addSelectString);
            tempHM.Add("leftJoinString", leftJoinString);
            return tempHM;
        }
        #endregion

        #region 检查本表是否存在对某特定主表的引用记录

        public bool checkRefExist(KBoModel refModel,string refString)
        {
            string whereStr = "";

            IDictionaryEnumerator dEnum = fieldsHt.GetEnumerator();
            while (dEnum.MoveNext())
            {
                Hashtable fDataHt = dEnum.Value as Hashtable;
                if (fDataHt[Field.fIsForeignKey].ToString() == "1"
                    && fDataHt[Field.fForeignTableClass].Equals(refModel))
                {
                    if (whereStr == "")
                    {
                        whereStr += " where " + dEnum.Key as string + "='" + refString + "'";
                    }
                    else
                    {
                        whereStr += " or " + dEnum.Key as string + "='" + refString + "'";
                    }
                }
            }

            string sqlStr = "select count(*) from " + tableCode + whereStr;

            bool retBo = false;
            if (exeSqlForDataSet(sqlStr).Rows[0][0].ToString() == "0")
            {
                retBo = false;
            }
            else
            {
                retBo = true;
            }

            return retBo;
        }

        #endregion


        #endregion

        #region 导入导出
        protected bool doExportData = false;
        public virtual void exportData(XmlDocument exportDocument)
        {
            //if (!doExportData)
            //{
            //    return;
            //}

            System.Xml.XmlElement Root = exportDocument.DocumentElement;
            XmlElement tableNode;
            tableNode = exportDocument.CreateElement(tableCode);
            Root.AppendChild(tableNode);

            XmlElement rowNode;

            DataTable dataRowsDt = selectAll();
            foreach (DataRow dr in dataRowsDt.Rows)
            {
                rowNode = exportDocument.CreateElement("row");
                tableNode.AppendChild(rowNode);
                foreach (Hashtable fieldHt in fieldsHt.Values)
                {
                    string fieldName = fieldHt[Field.fFieldCode].ToString();
                    if (dr.Table.Columns[fieldName]!=null && dr[fieldName].ToString() != "")
                    {
                        rowNode.SetAttribute(
                            fieldName,
                            dr[fieldName].ToString());
                    }
                }
            }
        }

        private bool isImported = false;  //用于递归生成表时标识本模型初始数据是否已写入
        /**
	     * 生成初始数据记录
	     * @throws Exception
	     */
        public virtual void importData(XmlDocument importDocument)
        {
            if (isImported)
            {
                return;
            }

            foreach (Hashtable fieldHt in fieldsHt.Values)
            {
                if (fieldHt[Field.fIsForeignKey].ToString() == "1")
                {
                    //先写入外键表数据
                    BaseModel ForeignTableClass = (BaseModel)fieldHt[Field.fForeignTableClass];
                    ForeignTableClass.importData(importDocument);
                }
            }

            XmlElement tableNode = (XmlElement)(importDocument.GetElementsByTagName(tableCode)[0]);
            if (tableNode == null)
            {
                return;
            }

            string sqlStr = "";
            string insertStr = "";
            string valuesStr = "";
            
            foreach (XmlElement recordNode in tableNode)
            {
                insertStr = "";
                valuesStr = "";
                foreach (XmlAttribute attr in recordNode.Attributes)
                {
                    Hashtable fieldRow = fieldsHt[attr.Name];
                    if (fieldRow != null)
                    {
                        if (!(
                            fieldRow[Field.fIsForeignKey].ToString() == "1"
                            && attr.Value.Trim() == ""
                            ))
                        {
                            if (insertStr != "")
                            {
                                insertStr += ",";
                            }
                            insertStr += attr.Name;
                            if (valuesStr != "")
                            {
                                valuesStr += ",";
                            }
                            valuesStr += "'" + attr.Value + "'";
                        }
                    }
                }
                sqlStr += " insert into " + tableCode + "("
                    + insertStr + ") values("
                    + valuesStr + ") ";
            }
            if (valuesStr != "")
            {
                exeSql(sqlStr);
            }
            isImported = true;
        }

        #endregion
    }
}
