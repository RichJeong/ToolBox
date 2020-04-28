using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace ToolBox
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Reflection;


    public partial class Helper
    {
        public class DB : DBCore
        {
            private SqlConnection dbConnection;

            public DB(string dbConn)
            {
                this.dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[dbConn].ConnectionString);
            }

            protected static Table GetTable(SqlDataReader reader,bool pSetFieldNameToLower)
            {
                return get_Table(reader, pSetFieldNameToLower);
            }

            protected static Record GetRecord(SqlDataReader reader, bool pSetFieldNameToLower)
            {
                return get_Record(reader, pSetFieldNameToLower);
            }

            protected static Record GetRecord(IEnumerable<string> cols, SqlDataReader reader, bool pSetFieldNameToLower)
            {
                return get_Record(cols,reader, pSetFieldNameToLower);
            }

            protected SqlDataReader GetDataReader(string pStoredProcedure,dynamic pParams=null, string pLogging = null, bool pb_UseDictionaryParam=false)
            {
                return get_DataReader(this.dbConnection, pStoredProcedure, pParams, pLogging, pb_UseDictionaryParam);
            }

            public SqlDataReader GetDataReaderInline(string pInlineSQL, string pLogging = null)
            {
                return get_DataReaderInline(this.dbConnection, pInlineSQL, pLogging);
            }

            public int SetData(string pStoredProcedure,dynamic pParams = null, string pLogging = null, bool pb_UseDictionaryParam = false)
            {
                return get_ResultAsInt(this.dbConnection, pStoredProcedure, pParams, pLogging, pb_UseDictionaryParam);
            }

            public int SetDataInline(string pInlineSQL, string pLogging = null)
            {
                return set_DataInline(this.dbConnection, pInlineSQL, pLogging);
            }

            public int GetResultAsInt(string pStoredProcedure, dynamic pParams = null, string pLogging = null, bool pb_UseDictionaryParam=false)
            {
                return get_ResultAsInt(this.dbConnection, pStoredProcedure, pParams, pLogging, pb_UseDictionaryParam);
            }

            public string GetSpToInt(string pSP, dynamic pParams = null, bool pb_UseDictionaryParam = false)
            {
                return get_ResultAsInt(this.dbConnection, pSP, pParams, null, pb_UseDictionaryParam);
            }

            public string GetResultAsStr(string pStoredProcedure, dynamic pParams = null, string pLogging = null, bool pb_UseDictionaryParam = false)
            {
                return get_ResultAsStr(this.dbConnection, pStoredProcedure, pParams, pLogging, pb_UseDictionaryParam);
            }

            public string GetResultAsStr(string pStoredProcedure, string pOutputParam, dynamic pParams = null, string pLogging=null, bool pb_UseDictionaryParam = false)
            {
                return get_ResultAsStr(this.dbConnection, pStoredProcedure, pOutputParam, pParams, pLogging, pb_UseDictionaryParam);
            }

            public long? GetResultAsLong(string pStoredProcedure, string pOutputParam, dynamic pParms = null, string pLogging = null, bool pb_UseDictionaryParam = false)
            {
                return get_ResultAsLong(this.dbConnection, pStoredProcedure, pOutputParam, pParms, pLogging, pb_UseDictionaryParam);
            }

            public List<object> GetResultAsObjectList(string pStoredProcedure, string[] pOutputParam, dynamic pParams = null, string pLogging=null, bool pb_UseDictionaryParam = false)
            {
                return get_ResultAsObjectList(this.dbConnection, pStoredProcedure, pOutputParam, pParams, pLogging, pb_UseDictionaryParam);
            }

            public string GetResultAsJson(string pStoredProcedure, dynamic pParams = null, string pLogging = null, bool pb_UseDictionaryParam = true, bool pSetFieldNameToLower=true)
            {
                return get_ResultAsJson(this.dbConnection, pStoredProcedure, pParams, pLogging, pb_UseDictionaryParam, pSetFieldNameToLower);
            }

            public Table GetResultAsTable(string pStoredProcedure, dynamic pParams = null, string pLogging = null, bool pb_UseDictionaryParam = true, bool pSetFieldNameToLower = true)
            {
                return get_ResultAsTable(this.dbConnection, pStoredProcedure, pParams, pLogging, pb_UseDictionaryParam, pSetFieldNameToLower);
            }

            public Dictionary<string,object> GetRecordAsDictionary(string pStoredProcedure, dynamic pParams = null, string pLogging = null, bool pb_UseDictionaryParam = true, bool pSetFieldNameToLower = true)
            {
                return get_RecordAsDictionary(this.dbConnection, pStoredProcedure, pParams, pLogging, pb_UseDictionaryParam, pSetFieldNameToLower);
            }

            public Record GetResultAsRecord(string pStoredProcedure, dynamic pParams = null, string pLogging = null, bool pb_UseDictionaryParam = false, bool pSetFieldNameToLower = true)
            {
                return get_ResultAsRecord(this.dbConnection, pStoredProcedure, pParams, pLogging, pb_UseDictionaryParam, pSetFieldNameToLower);
            }

            public bool HasRows(string pStoredProcedure, dynamic pParams = null, bool pb_UseDictionaryParam = false)
            {
                return has_Row(this.dbConnection, pStoredProcedure, pParams, pb_UseDictionaryParam);
            }

           
        }
    }
}
