using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace com.xiyuansoft.bormodel
{
    /// <summary>
    /// 通过fID一一对应地扩展KBoModel业务对象，KBoModel字段太多，需要分成多个表，或具有集中业务意义的字段集时使用
    /// </summary>
    public class KBoModelExt : KBoModel
    {
        protected KBoModel refKBoModel;

        /**
         * 生成字段信息集，由子类覆盖，并总是先调用父类
         */
        protected override void createFieldsAl()
        {
            subClassCreateKey = true;
            base.createFieldsAl();
            base.addAMapKeyField("记录标识", KBoModel.fID,
                refKBoModel.getTableCode(), refKBoModel);
        }

        public DataTable selectMainAndMe(string whereStr)
        {
            string sqlStr = "select " + tableCode + ".*," + refKBoModel.getTableCode()
                + ".* from " + tableCode + "," + refKBoModel.getTableCode()
                + " where " + tableCode + "." + fID + "=" + refKBoModel.getTableCode()
                + "." + fID
                + " and (" + whereStr + ")"
                ;
            return exeSqlForDataSet(sqlStr);
        }

        //不区分主副表字段，因此要求除主键外，主副表不能有重名字段，否则出错
        public DataTable selectMainAndMeByOneField(string field,string value)
        {
            string sqlStr = "select " + tableCode + ".*," + refKBoModel.getTableCode()
                + ".* from " + tableCode + "," + refKBoModel.getTableCode()
                + " where " + tableCode + "." + fID + "=" + refKBoModel.getTableCode()
                + "." + fID
                + " and (" + field + "='" + value + "')"
                ;
            return exeSqlForDataSet(sqlStr);
        }
    }
}
