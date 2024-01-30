using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Data.SqlClient;

namespace FTP.Common
{
    public class Repository
    {
        public readonly string _connectionString;

        public Repository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private const int SQL_COMMAND_STORE = 4;

        private void CloseSqlReader(SqlDataReader reader)
        {
            if (reader != null)
            {
                reader.Dispose();
                reader.Close();
            }
        }

        private void AddParametersFromDictionary(SqlCommand command, int sqlType, Dictionary<string, object> parameters)
        {
            command.CommandTimeout = 3600;
            command.CommandType = (CommandType)sqlType;
            if (parameters == null || !parameters.Any()) return;
            foreach (var p in parameters)
            {
                command.Parameters.AddWithValue(p.Key, (p.Value == null || p.Value.ToString().Length == 0) ? DBNull.Value : p.Value);
            }
        }

        private void AddParametersFromObject(SqlCommand command, int sqlType, object obj)
        {
            command.CommandTimeout = 0;
            command.CommandType = (CommandType)sqlType;
            foreach (PropertyInfo p in obj.GetType().GetProperties())
            {
                var val = p.GetValue(obj, null);
                command.Parameters.AddWithValue("@" + p.Name, (val == null || val.ToString().Length == 0) ? DBNull.Value : val);
            }
        }

        private void CloseConnection(SqlConnection connection, SqlCommand command)
        {
            command.Dispose();
            connection.Dispose();
            connection.Close();
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        private string GetTableSchema(Type type)
        {
            string name = "";
            var tableattr =
                type.GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name == "TableSchemaAttribute")
                    as
                    dynamic;
            if (tableattr != null) name = tableattr.Name + ".";

            return name;
        }

        private static string GetTableName(Type type)
        {
            var tableattr =
                type.GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name == "TableNameAttribute") as
                    dynamic;
            string name = tableattr != null ? tableattr.Name : type.Name;

            return name;
        }

        public Tuple<string, string> GetNote(string storeName, int sqlType, Dictionary<string, object> parameters)
        {
            int rowsEffect;
            Tuple<string, string> output;
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(storeName, connection))
            {
                connection.Open();

                try
                {
                    AddParametersFromDictionary(command, sqlType, parameters);
                    command.Parameters.Add("@OutputId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@OutputId2", SqlDbType.Int).Direction = ParameterDirection.Output;
                    rowsEffect = command.ExecuteNonQuery();
                    output = new Tuple<string, string>(command.Parameters["@OutputId"].Value.ToString(),
                        command.Parameters["@OutputId2"].Value.ToString());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    CloseConnection(connection, command);
                }
            }
            return output;
        }

        public object GetObject(string query, int sqlType, Dictionary<string, object> parameters)
        {
            object item = null;
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                try
                {
                    AddParametersFromDictionary(command, sqlType, parameters);
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        item = Extension.GetValue<object>(reader, 0);
                    }
                    CloseSqlReader(reader);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    CloseConnection(connection, command);
                }
            }
            return item;
        }

        public List<T> GetListFromParameters<T>(string sql, int sqlType, Dictionary<string, object> parameters)
        {
            List<T> items;
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                try
                {
                    AddParametersFromDictionary(command, sqlType, parameters);
                    var reader = command.ExecuteReader();
                    items = reader.ToList<T>();
                    CloseSqlReader(reader);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    CloseConnection(connection, command);
                }
            }
            return items;
        }

        public bool ExecuteStoreProcedureFromObject(string storeName, int sqlType, object obj)
        {
            int rowsEffect;
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(storeName, connection))
            {
                connection.Open();
                try
                {
                    AddParametersFromObject(command, sqlType, obj);
                    rowsEffect = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    CloseConnection(connection, command);
                }
            }
            return rowsEffect != 0;
        }

        public int ExecuteSQLFromParameters(string storeName, int sqlType, Dictionary<string, object> parameters)
        {
            int rowsEffect;
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(storeName, connection))
            {
                connection.Open();

                try
                {
                    AddParametersFromDictionary(command, sqlType, parameters);
                    rowsEffect = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    CloseConnection(connection, command);
                }
            }
            return rowsEffect;
        }

        public string ExecuteStoreProcedureRef(string storeName, int sqlType, Dictionary<string, object> parameters)
        {
            int rowsEffect;
            string refSave = "";
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(storeName, connection))
            {
                connection.Open();

                try
                {
                    AddParametersFromDictionary(command, sqlType, parameters);
                    command.Parameters.Add("@OutputId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    rowsEffect = command.ExecuteNonQuery();
                    //string id = command.Parameters["@OutputId"].Value.ToString();
                    refSave = command.Parameters["@OutputId"].Value.ToString();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    CloseConnection(connection, command);
                }
            }
            return refSave;
        }

        public T GetObject<T>(string storeName, int sqlType, Dictionary<string, object> parameters)
        {
            var items = GetListFromParameters<T>(storeName, sqlType, parameters);
            return items.FirstOrDefault();
        }

        public Tuple<List<T1>, List<T2>> GetMultiList<T1, T2>(string storeName, int sqlType, Dictionary<string, object> parameters)
        {
            List<T1> items;
            List<T2> itemsU;

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(storeName, connection))
            {
                connection.Open();
                try
                {
                    AddParametersFromDictionary(command, sqlType, parameters);
                    var reader = command.ExecuteReader();
                    items = reader.ToList<T1>();
                    reader.NextResult();
                    itemsU = reader.ToList<T2>();
                    CloseSqlReader(reader);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    CloseConnection(connection, command);
                }
            }
            return new Tuple<List<T1>, List<T2>>(items, itemsU);
        }

        public IEnumerable<Dictionary<string, object>> GetDictionary(string storeName, int sqlType, Dictionary<string, object> parameters)
        {
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(storeName, connection))
            {
                connection.Open();

                try
                {
                    AddParametersFromDictionary(command, sqlType, parameters);
                    var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                    while (reader.Read())
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();

                        for (int i = 0; i < reader.VisibleFieldCount; i++)
                        {
                            object value = reader.GetValue(i);
                            row[reader.GetName(i)] = value is DBNull ? null : value;
                        }
                        items.Add(row);

                        // Yield rows as we get them and avoid buffering them so we can easily handle
                        // large datasets without memory issues
                        // yield return row;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    CloseConnection(connection, command);
                }
            }
            return items;
        }

        public int GetValueByKey(string storeName, int sqlType, string keyName, Dictionary<string, object> parameters)
        {
            var item = 0;
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(storeName, connection))
            {
                connection.Open();
                try
                {
                    AddParametersFromDictionary(command, sqlType, parameters);
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        item = Extension.GetValue<int>(reader, keyName);
                    }
                    CloseSqlReader(reader);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    CloseConnection(connection, command);
                }
            }
            return item;
        }

        private T GetSingleValueFromParametersOrObject<T>(string storeName, int sqlType, string keyName, object obj)
        {
            T item = default(T);
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(storeName, connection))
            {
                connection.Open();
                try
                {
                    var dict = obj as Dictionary<string, object>;
                    if (dict != null)
                    {
                        AddParametersFromDictionary(command, sqlType, dict);
                    }
                    else
                    {
                        AddParametersFromObject(command, sqlType, obj);
                    }

                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        item = Extension.GetValue<T>(reader, keyName);
                    }
                    CloseSqlReader(reader);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    CloseConnection(connection, command);
                }
            }
            return item;
        }

        public int GetSingleValueFromObject(string storeName, int sqlType, object obj)
        {
            var objReturn = GetSingleValueFromParametersOrObject<int>(storeName, sqlType, null, obj);
            return objReturn;
        }

        public object GetSingleValueFromParameters<T>(string storeName, int sqlType, string keyName, Dictionary<string, object> parameters)
        {
            var obj = GetSingleValueFromParametersOrObject<object>(storeName, sqlType, keyName, parameters);
            return obj;
        }

        public long GetSingleValue(string storeName, int sqlType, Dictionary<string, object> parameters)
        {
            long item = 0;
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(storeName, connection))
            {
                connection.Open();
                try
                {
                    AddParametersFromDictionary(command, sqlType, parameters);

                    var returnParameter = command.Parameters.Add("@ReturnVal", SqlDbType.BigInt);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    var reader = command.ExecuteReader();

                    item = Convert.ToInt64(returnParameter.Value);

                    CloseSqlReader(reader);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    CloseConnection(connection, command);
                }
            }
            return item;
        }

        public List<T> GetListFromObject<T>(string storeName, int sqlType, object obj)
        {
            List<T> items;
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(storeName, connection))
            {
                connection.Open();
                try
                {
                    AddParametersFromObject(command, sqlType, obj);
                    var reader = command.ExecuteReader();
                    items = reader.ToList<T>();
                    CloseSqlReader(reader);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    CloseConnection(connection, command);
                }
            }
            return items;
        }

        public T GetObjectFromObject<T>(string storeName, int sqlType, object obj)
        {
            var items = GetListFromObject<T>(storeName, sqlType, obj);
            return items.FirstOrDefault();
        }
    }
}
