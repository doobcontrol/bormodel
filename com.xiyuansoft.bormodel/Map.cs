using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using com.xiyuansoft.bormodel.metadata;
using System.Data;

namespace com.xiyuansoft.bormodel
{
    public class Map : BaseModel
    {
        /**
         * 生成字段信息集，由子类覆盖，并总是先调用父类
         */
        protected override void createFieldsAl()
        {
            base.createFieldsAl();
        }

        public KBoModel getMapKBoModelByField(string fieldCode)
        {
            return (fieldsHt[fieldCode] as Hashtable)[Field.fForeignTableClass] as KBoModel;
        }

        public DataTable selectByOneFieldByMap(
            string fFieldCode,
            string fFieldValue,
            string SourceField,
            string TargetField
            )
        {
            KBoModel SourceModel = getMapKBoModelByField(SourceField);
            KBoModel TargetModel = getMapKBoModelByField(TargetField);
            string sqlStr = "select tT.* "
                + "from " + SourceModel.getTableCode() + " sT,"
                + tableCode + " mT,"
                + TargetModel.getTableCode() + " tT"
                + " where sT." + fFieldCode + "='" + fFieldValue + "' "
                + "   and sT." + KBoModel.fID + "=mT." + SourceField
                + "   and tT." + KBoModel.fID + "=mT." + TargetField
                ;

            return exeSqlForDataSet(sqlStr);
        }
    }
}
