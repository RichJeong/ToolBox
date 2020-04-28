using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Web.Routing;

namespace ToolBox
{
    public partial class Helper
    {
        public class DBCore
        {
            protected static SqlDataReader get_DataReader(
                SqlConnection localConn,
                string pStoredProcedure,
                dynamic pParams = null,
                string pLogging = null,
                bool pb_UseDictionaryParam = false)
            {
                try
                {
                    var cmd = new SqlCommand(pStoredProcedure, localConn)
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = 7200
                    };
                    set_SqlParam(cmd, pParams, pLogging, pb_UseDictionaryParam);
                    if (localConn.State == ConnectionState.Closed)
                    {
                        localConn.Open();
                    }
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    return dr;
                }
                catch (Exception ex)
                {
                    throw new Exception("SP:" + pStoredProcedure + " Param: " + Data.GetSafeStr(Data.ConvertObjectToJson(pParams)) + " EX: " +
                        Core.GetCommonErrorMessage(ex));
                }

            }

            protected static SqlDataReader get_DataReaderInline(SqlConnection localConn, string inlineSql, string pLogging = null)
            {
                try
                {
                    var cmd = new SqlCommand(inlineSql, localConn)
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = 7200
                    };
                    if (localConn.State == ConnectionState.Closed)
                    {
                        localConn.Open();
                    }

                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    return dr;
                }
                catch (Exception ex)
                {
                    throw new Exception("SQL:" + inlineSql + "EX: " + Core.GetCommonErrorMessage(ex));
                }
            }

            protected static int get_NoneInline(SqlConnection localConn, string inlineSql, string pLogging = null)
            {
                try
                {
                    var cmd = new SqlCommand(inlineSql, localConn)
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = 7200
                    };

                    try
                    {
                        if (localConn.State == ConnectionState.Closed)
                        {
                            localConn.Open();
                        }
                        return cmd.ExecuteNonQuery();
                    }
                    finally
                    {
                        localConn.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("SQL: " + inlineSql + " EX: " + Core.GetCommonErrorMessage(ex));
                }
            }

            protected static int set_Data(SqlConnection localConn, string pStroedProcedure, dynamic pParams = null, string pLogging = null, bool pb_UseDictionaryParam = false)
            {
                return get_ResultAsInt(localConn, pStroedProcedure, pParams, pLogging, pb_UseDictionaryParam);
            }

            protected static int set_DataInline(SqlConnection localConn, string inlineSql, string pLogging = null)
            {
                if (string.IsNullOrWhiteSpace(inlineSql))
                    return 0;

                var cmd = new SqlCommand(inlineSql, localConn)
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 7200
                };

                try
                {
                    if (localConn.State == ConnectionState.Closed)
                    {
                        localConn.Open();
                    }

                    int count = cmd.ExecuteNonQuery();
                    return count;
                }
                finally
                {
                    localConn.Close();
                }
            }

            protected static int get_ResultAsInt(SqlConnection localConn, string pStoredProcedure, dynamic pParams = null, string pLogging = null, bool pb_UseDictionaryParam = false)
            {
                var cmd = new SqlCommand(pStoredProcedure, localConn)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 7200
                };
                cmd.Parameters.Add("RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

                set_SqlParam(cmd, pParams, pLogging, pb_UseDictionaryParam);
                try
                {
                    if (localConn.State == ConnectionState.Closed)
                    {
                        localConn.Open();
                    }
                    cmd.ExecuteNonQuery();
                    localConn.Close();
                }
                finally
                {
                    localConn.Close();
                }
                return Data.Get<int>(cmd.Parameters["RETURN_VALUE"].Value);
            }

            protected static string get_ResultAsStr(SqlConnection localConn, string pStoredProcedure,  dynamic pParams = null, string pLogging = null, bool pb_UseDictionaryParam = false)
            {
                const string OutName = "@RowsAffected";

                var cmd = new SqlCommand(pStoredProcedure, localConn)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 7200
                };
                set_SqlParam(cmd, pParams, pLogging, pb_UseDictionaryParam);

                var OutVal = new SqlParameter(OutName, SqlDbType.NVarChar, 2147483647)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(OutVal);
                try
                {
                    if(localConn.State == ConnectionState.Closed)
                    {
                        localConn.Open();
                    }
                    cmd.ExecuteNonQuery();
                    localConn.Close();
                }
                finally
                {
                    localConn.Close();
                }
                return Checker.IsEmpty(OutVal.Value) ? "" : ((string)OutVal.Value).Trim();
            }

            protected static string get_ResultAsStr(SqlConnection localConn, string pStoredProcedure, string pOutputParam, dynamic pParams = null, string pLogging = null, bool pb_UseDictionaryParam = false)
            {
                string OutName = "@" + pOutputParam;

                var cmd = new SqlCommand(pStoredProcedure, localConn)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 7200
                };

                set_SqlParam(cmd, pParams, pLogging, pb_UseDictionaryParam);

                var OutVal = new SqlParameter(OutName, SqlDbType.NVarChar, 2147483647)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(OutVal);
                try
                {
                    if (localConn.State == ConnectionState.Closed)
                    {
                        localConn.Open();
                    }
                    cmd.ExecuteNonQuery();
                    localConn.Close();
                }
                finally
                {
                    localConn.Close();
                }
                return Checker.IsEmpty(OutVal.Value) ? string.Empty : ((string)OutVal.Value).Trim();
            }

            protected static long? get_ResultAsLong(SqlConnection localConn, string pStoredProcedure, string pOutputParam, dynamic pParams = null, string pLogging = null, bool pb_UseDictionaryParam = false)
            {
                string OutName = "@" + pOutputParam;

                var cmd = new SqlCommand(pStoredProcedure, localConn)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 7200
                };

                set_SqlParam(cmd, pParams, pLogging, pb_UseDictionaryParam);

                var OutVal = new SqlParameter(OutName, SqlDbType.NVarChar, 2147483647)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(OutVal);
                try
                {
                    if (localConn.State == ConnectionState.Closed)
                    {
                        localConn.Open();
                    }
                    cmd.ExecuteNonQuery();
                    localConn.Close();
                }
                finally
                {
                    localConn.Close();
                }
                return Checker.IsEmpty(OutVal.Value) ? null : Data.Get<long?>(OutVal.Value);
            }

            protected static List<object> get_ResultAsObjectList(SqlConnection localConn, string pStoredProcedure, string[] pOutputParam, dynamic pParams = null, string pLogging = null, bool pb_UseDictionaryParam = false)
            {
                List<object> resultSet = new List<object>();
                List<SqlParameter> outVals = new List<SqlParameter>();

                var cmd = new SqlCommand(pStoredProcedure, localConn)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 7200
                };

                set_SqlParam(cmd, pParams, pLogging, pb_UseDictionaryParam);

                for(int i =0; i < pOutputParam.Length; i++)
                {
                    outVals.Add(new SqlParameter("@" + pOutputParam[i], SqlDbType.NVarChar, 2147483647)
                    {
                        Direction = ParameterDirection.Output
                    });
                }

                for(int i=0; i < pOutputParam.Length; i++)
                {
                    cmd.Parameters.Add(outVals[i]);
                }
                try
                {
                    if(localConn.State == ConnectionState.Closed)
                    {
                        localConn.Open();
                    }
                    cmd.ExecuteNonQuery();
                    localConn.Close();
                }
                finally
                {
                    localConn.Close();
                }

                foreach(SqlParameter param in outVals)
                {
                    resultSet.Add(param.Value);
                }
                return resultSet;
            }

            protected static string get_ResultAsJson(SqlConnection localConn, string pStoredProcedure, dynamic pParams = null, string pLogging = null, bool pb_UseDictionaryParam = true, bool pSetFieldNameToLower = true)
            {
                List<object> result = new List<object>();
                try
                {
                    bool hasNext = true;
                    using (SqlDataReader dr = get_DataReader(localConn, pStoredProcedure, pParams, pLogging, pb_UseDictionaryParam))
                    {
                        while (hasNext)
                        {
                            result.Add(get_DictionaryTable(dr, pSetFieldNameToLower));
                            hasNext = dr.NextResult();
                        }
                    }
                    if (result.Count == 1)
                        return Data.ConvertObjectToJson(result[0]);
                    else
                        return Data.ConvertObjectToJson(result);
                }
                catch(Exception ex)
                {
                    throw new Exception("003-" + "Helper DB get_ResultAsJson :" + pStoredProcedure + "=" + 
                        Data.ConvertObjectToJson(pParams) + "-"  + Core.GetCommonErrorMessage(ex));
                }
               
            }

            protected static List<IEnumerable<Dictionary<string,object>>> get_ResultAsDictionaryList(
                SqlConnection localConn, string pStoredProcedure, string pOutputParam, dynamic pParams = null, string pLogging = null, bool pb_UseDictionaryParam = true, bool pSetFieldNameToLower = true)
            {
                List<IEnumerable<Dictionary<string, object>>> result = new List<IEnumerable<Dictionary<string, object>>>();
                try
                {
                    bool hasNext = true;
                    using(SqlDataReader dr = get_DataReader(localConn, pStoredProcedure, pParams, pLogging, pb_UseDictionaryParam))
                    {
                        while (hasNext)
                        {
                            result.Add(get_DictionaryTable(dr, pSetFieldNameToLower));
                            hasNext = dr.NextResult();
                        }
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("GetResultAsDictionaryList :" + Core.GetCommonErrorMessage(ex));
                }
            }

            protected static List<Table> get_ResultAsTableList(SqlConnection localConn, string pStoredProcedure, string pOutputParam, dynamic pParams = null, string pLogging = null, bool pb_UseDictionaryParam = true, bool pSetFieldNameToLower = true)
            {
                List<Table> result = new List<Table>();
                try
                {
                    bool hasNext = true;
                    using(SqlDataReader dr = get_DataReader(localConn, pStoredProcedure, pParams, pLogging, pb_UseDictionaryParam))
                    {
                        while (hasNext)
                        {
                            result.Add(get_Table(dr, pSetFieldNameToLower));
                            hasNext = dr.NextResult();
                        }
                    }
                    return result;
                }catch(Exception ex)
                {
                    throw new Exception("GetResultAsTableList:" + Core.GetCommonErrorMessage(ex));
                }
            }

            protected static Table get_ResultAsTable(SqlConnection localConn, string pStoredProcedure, dynamic pParams = null, string pLogging = null, bool pb_UseDictionaryParam = true, bool pSetFieldNameToLower = true)
            {
                Table result = new Table();
                try
                {
                    using (SqlDataReader dr = get_DataReader(localConn, pStoredProcedure, pParams, pLogging, pb_UseDictionaryParam))
                    {
                        var cols = new List<string>();
                        for(var i = 0; i < dr.FieldCount; i++)
                        {
                            if (pSetFieldNameToLower)
                                cols.Add(dr.GetName(i).ToLower());
                            else
                                cols.Add(dr.GetName(i));
                        }

                        while (dr.Read())
                            result.Add(get_Record(cols,dr,pSetFieldNameToLower));

                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("GetResultAsTable:" + Core.GetCommonErrorMessage(ex));
                }
            }

            protected static Dictionary<string, object> get_RecordAsDictionary(SqlConnection localConn, string pStoredProcedure, dynamic pParams = null, string pLogging = null, bool pb_UseDictionaryParam = false, bool pSetFieldNameToLower = true)
            {
                Dictionary<string, object> result = null;
                try
                {
                    using (SqlDataReader dr = get_DataReader(localConn, pStoredProcedure, pParams, pLogging, pb_UseDictionaryParam))
                    {
                        var cols = new List<string>();

                        for (var i = 0; i < dr.FieldCount; i++)
                        {
                            if (pSetFieldNameToLower)
                                cols.Add(dr.GetName(i).ToLower());
                            else
                                cols.Add(dr.GetName(i));
                        }

                        if (dr.Read())
                        {
                            result = get_DictionaryRow(cols, dr, pSetFieldNameToLower);
                        }
                    }

                    return result != null && result.Count > 0 ? result : null;
                }
                catch (Exception ex)
                {
                    throw new Exception("GetRecordAsDictionary:" + Core.GetCommonErrorMessage(ex));
                }
            }

            protected static Record get_ResultAsRecord(SqlConnection localConn, string pStoredProcedure, dynamic pParams = null, string pLogging = null, bool pb_UseDictionaryParam = false, bool pSetFieldNameToLower = true)
            {
                Record result = null;
                try
                {
                    using (SqlDataReader dr = get_DataReader(localConn, pStoredProcedure, pParams, pLogging, pb_UseDictionaryParam))
                    {
                        var cols = new List<string>();

                        for (var i = 0; i < dr.FieldCount; i++)
                        {
                            if (pSetFieldNameToLower)
                                cols.Add(dr.GetName(i).ToLower());
                            else
                                cols.Add(dr.GetName(i));
                        }

                        if (dr.Read())
                        {
                            result = get_Record(cols, dr, pSetFieldNameToLower);
                        }
                    }

                    return result == null || result.Count < 1 ? null : result;
                }
                catch (Exception ex)
                {
                    throw new Exception("GetRecordAsDictionary:" + Core.GetCommonErrorMessage(ex));
                }
            }


            protected static bool has_Row(SqlConnection localConn, string pStoredProcedure, dynamic pParams = null, bool pb_UseDictionaryParam = true)
            {
                try
                {
                    bool result = false;

                    using (SqlDataReader dr = get_DataReader(localConn, pStoredProcedure, pParams, null, pb_UseDictionaryParam))
                    {
                        result = dr.Read();
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("has_Row:" + Core.GetCommonErrorMessage(ex));
                }
            }

            protected static Table  get_Table(SqlDataReader reader, bool pSetFieldNameToLower)
            {
                try
                {
                    var results = new Table();
                    var cols = new List<string>();
                    for(var i = 0; i < reader.FieldCount; i++)
                    {
                        if (pSetFieldNameToLower)
                            cols.Add(reader.GetName(i).ToLower());
                        else
                            cols.Add(reader.GetName(i));
                    }

                    while (reader.Read())
                    {
                        results.Add(get_Record(cols, reader, pSetFieldNameToLower));
                    }

                    return results;
                }
                catch (Exception ex)
                {
                    throw new Exception("get_Table:" + Core.GetCommonErrorMessage(ex));
                }
            }


            protected static Record get_Record(IEnumerable<string> cols, SqlDataReader reader, bool pSetFieldNameToLower)
            {
                try
                {
                    var result = new Record();
                    
                    foreach(var col in cols)
                    {
                        if (pSetFieldNameToLower)
                            result.Add(col.ToLower(), reader[col]);
                        else
                            result.Add(col, reader[col]);
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("get_Record:" + Core.GetCommonErrorMessage(ex));
                }
            }

            protected static Record get_Record(SqlDataReader reader, bool pSetFieldNameToLower)
            {
                try
                {
                    var result = new Record();
                    string field = "";

                    for(var i =0; i < reader.FieldCount; i++)
                    {
                        field = reader.GetName(i);
                        if (pSetFieldNameToLower)
                            result.Add(field.ToLower(), reader[field]);
                        else
                            result.Add(field, reader[field]);
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("get_Record:" + Core.GetCommonErrorMessage(ex));
                }
            }


            protected static IEnumerable<Dictionary<string,object>> get_DictionaryTable(SqlDataReader reader, bool pSetFieldNameToLower)
            {
                try
                {
                    var result = new List<Dictionary<string, object>>();
                    var cols = new List<string>();

                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        if (pSetFieldNameToLower)
                            cols.Add(reader.GetName(i).ToLower());
                        else
                            cols.Add(reader.GetName(i));

                    }
                    while (reader.Read())
                        result.Add(get_DictionaryRow(cols, reader, pSetFieldNameToLower));

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("get_Record:" + Core.GetCommonErrorMessage(ex));
                }
            }

            protected static Dictionary<string, object> get_DictionaryRow(IEnumerable<string> cols,SqlDataReader reader,bool pSetFieldNameToLower)
            {
                try
                {
                    var result = new Dictionary<string, object>();
                    
                    foreach(var col in cols)
                    {
                        if (pSetFieldNameToLower)
                            result.Add(col.ToLower(), reader[col]);
                        else
                            result.Add(col, reader[col]);
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("get_Record:" + Core.GetCommonErrorMessage(ex));
                }
            }

            protected static void set_SqlParam(SqlCommand cmd, dynamic pParams,string logging=null, bool pb_UseDictionaryParam = false)
            {
                if (pParams == null)
                    return;

                if (!pb_UseDictionaryParam)
                {
                    var paramArray = pParams.GetType().GetProperies();

                    if(pParams.GetType().ToString() == "System.Dynamic.ExpandoObject")
                    {
                        IDictionary<string, object> paramList = new RouteValueDictionary(pParams);
                        foreach(KeyValuePair<string,object> param in (IDictionary<string,object>)paramList)
                        {
                            cmd.Parameters.AddWithValue("@" + param.Key, param.Value);
                        }
                    }
                    else
                    {
                        foreach(System.Reflection.PropertyInfo param in paramArray)
                        {
                            var fieldValue = param.GetValue(pParams, null);

                            if (fieldValue != null)
                            {
                                Type type = fieldValue.GetType();
                                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                                    cmd.Parameters.AddWithValue("@" + param.Name, convert_ToDataTable(fieldValue));
                                else
                                    cmd.Parameters.AddWithValue("@" + param.Name, fieldValue);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@" + param.Name, null);
                            }
                        }
                    }
                }
                else
                {
                    foreach(KeyValuePair<string,object> param in (IDictionary<string,object>)pParams)
                    {
                        cmd.Parameters.AddWithValue("@" + param.Key, param.Value);
                    }
                }
                foreach(SqlParameter parameter in cmd.Parameters)
                {
                    if(parameter.Value == null)
                    {
                        parameter.Value = DBNull.Value;
                    }
                }
                if (!String.IsNullOrWhiteSpace(logging))
                    cmd.Parameters.AddWithValue("@logging", logging);
            }

            protected static DataTable convert_ToDataTable<T>(List<T> items)
            {
                DataTable result = new DataTable(typeof(T).Name);

                PropertyInfo[] Props = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                
                foreach(PropertyInfo prop in Props)
                {
                    result.Columns.Add(prop.Name, prop.PropertyType);
                }
                foreach(T item in items)
                {
                    var values = new object[Props.Length];
                    for(int i =0; i < Props.Length; i++)
                    {
                        values[i] = Props[i].GetValue(item, null);
                    }
                    result.Rows.Add(values);
                }
                return result;
            }

        }
    }
}
