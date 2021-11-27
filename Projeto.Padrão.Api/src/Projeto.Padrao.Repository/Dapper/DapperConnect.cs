using Dapper;
using Projeto.Padrao.Domain.Helpers;
using Projeto.Padrao.Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Padrao.Repository.Dapper
{
    public abstract class DapperConnect: IDisposable
    {
        private readonly string _connectionString;
        public DapperConnect()
        {
            _connectionString = string.Empty;
        }

        protected SqlConnection ConexaoDB()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        protected IEnumerable<T> GetItems<T>(CommandType commandType, string sql, object parameters = null)
        {
            using (var connection = ConexaoDB())
            {
                return connection.Query<T>(sql, parameters, commandType: commandType);
            }
        }

        protected async Task<IEnumerable<T>> GetItemsAsync<T>(CommandType commandType, string sql, object parameters = null)
        {
            using (var connection = ConexaoDB())
            {
                return await connection.QueryAsync<T>(sql, parameters, commandType: commandType);
            }
        }

        protected IEnumerable<R> GetItems<T, S, R>(CommandType commandType, string sql, Func<T, S, R> map, object parameters = null, string splitOn = "Id")
        {
            using (var connection = ConexaoDB())
            {
                return connection.Query<T, S, R>(sql, map, parameters, splitOn: splitOn, commandType: commandType);
            }
        }

        protected async Task<IEnumerable<R>> GetItemsAsync<T, S, R>(CommandType commandType, string sql, Func<T, S, R> map, object parameters = null, string splitOn = "Id")
        {
            using (var connection = ConexaoDB())
            {
                return await connection.QueryAsync<T, S, R>(sql, map, parameters, splitOn: splitOn, commandType: commandType);
            }
        }

        protected int Execute(CommandType commandType, string sql, object parameters = null)
        {
            using (var connection = ConexaoDB())
            {
                return connection.Execute(sql, parameters, commandType: commandType);
            }
        }

        protected async Task<int> ExecuteAsync(CommandType commandType, string sql, object parameters = null)
        {
            using (var connection = ConexaoDB())
            {
                return await connection.ExecuteAsync(sql, parameters, commandType: commandType);
            }
        }

        protected IEnumerable<T> GetSQL<T>(string sql)
        {
            return GetItems<T>(CommandType.Text, sql);
        }

        protected async Task<IEnumerable<T>> GetSQLAsync<T>(string sql)
        {
            return await GetItemsAsync<T>(CommandType.Text, sql);
        }

        protected IEnumerable<T> GetAll<T>()
        {
            var sql = string.Format("SELECT * FROM {0}", typeof(T).Name);
            return GetItems<T>(CommandType.Text, sql);
        }

        protected async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            var sql = string.Format("SELECT * FROM {0}", typeof(T).Name);
            return await GetItemsAsync<T>(CommandType.Text, sql);
        }

        protected IEnumerable<R> GetAll<T, S, R>(Func<T, S, R> map)
        {
            var sql = string.Format("SELECT * FROM {0} A INNER JOIN {1} B ON A.{1}Id = B.{1}Id ", typeof(T).Name, typeof(S).Name);
            return GetItems<T, S, R>(CommandType.Text, sql, map, null, typeof(S).Name.ToString() + "Id");
        }

        protected async Task<IEnumerable<R>> GetAllAsync<T, S, R>(Func<T, S, R> map)
        {
            var sql = string.Format("SELECT * FROM {0} A INNER JOIN {1} B ON A.{1}Id = B.{1}Id ", typeof(T).Name, typeof(S).Name);
            return await GetItemsAsync<T, S, R>(CommandType.Text, sql, map, null, typeof(S).Name.ToString() + "Id");
        }

        protected T Find<T>(int id)
        {
            var sql = string.Format("SELECT * FROM {0} WHERE {1} = {2}", typeof(T).Name, $"{typeof(T).Name}Id", id);
            return GetItems<T>(CommandType.Text, sql).SingleOrDefault();
        }

        protected Task<T> FindAsync<T>(int id)
        {
            var sql = string.Format("SELECT * FROM {0} WHERE {1} = {2}", typeof(T).Name, $"{typeof(T).Name}Id", id);
            return Task.FromResult(GetItemsAsync<T>(CommandType.Text, sql).Result.SingleOrDefault());
        }

        protected T Find<T>(long id)
        {
            var sql = string.Format("SELECT * FROM {0} WHERE {1} = {2}", typeof(T).Name, $"{typeof(T).Name}Id", id);
            return GetItems<T>(CommandType.Text, sql).SingleOrDefault();
        }

        protected Task<T> FindAsync<T>(long id)
        {
            var sql = string.Format("SELECT * FROM {0} WHERE {1} = {2}", typeof(T).Name, $"{typeof(T).Name}Id", id);
            return Task.FromResult(GetItemsAsync<T>(CommandType.Text, sql).Result.SingleOrDefault());
        }

        protected T Find<T>(Guid id)
        {
            var sql = string.Format("SELECT * FROM {0} WHERE {1} = {2}", typeof(T).Name, $"{typeof(T).Name}Id", id);
            return GetItems<T>(CommandType.Text, sql).SingleOrDefault();
        }

        protected Task<T> FindAsync<T>(Guid id)
        {
            var sql = string.Format("SELECT * FROM {0} WHERE {1} = {2}", typeof(T).Name, $"{typeof(T).Name}Id", id);
            return Task.FromResult(GetItemsAsync<T>(CommandType.Text, sql).Result.SingleOrDefault());
        }


        protected IEnumerable<T> Select<T>(object obj)
        {
            var propertyContainer = new PropertyContainer();

            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                var name = property.Name;
                var value = obj.GetType().GetProperty(property.Name).GetValue(obj);
                propertyContainer.AddValue(name, value);
            }

            var sqlPairs = GetSqlPairs(propertyContainer.AllNames, " AND ");
            var sql = string.Format("SELECT * FROM {0} WHERE {1}", typeof(T).Name, sqlPairs);
            return GetItems<T>(CommandType.Text, sql, propertyContainer.AllPairs);
        }

        protected async Task<IEnumerable<T>> SelectAsync<T>(object obj)
        {
            var propertyContainer = new PropertyContainer();

            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                var name = property.Name;
                var value = obj.GetType().GetProperty(property.Name).GetValue(obj);
                propertyContainer.AddValue(name, value);
            }

            var sqlPairs = GetSqlPairs(propertyContainer.AllNames, " AND ");
            var sql = string.Format("SELECT * FROM {0} WHERE {1}", typeof(T).Name, sqlPairs);
            return await GetItemsAsync<T>(CommandType.Text, sql, propertyContainer.AllPairs);
        }


        protected void Insert<T>(T obj)
        {
            var propertyContainer = ParseProperties(obj);
            var sql = $"INSERT INTO {typeof(T).Name} ({string.Join(", ", propertyContainer.ValueNames)}) VALUES(@{ string.Join(", @", propertyContainer.ValueNames)}); select LAST_INSERT_ID();";

            using (var connection = ConexaoDB())
            {
                var id = connection.Query<int>
                (sql, propertyContainer.ValuePairs, commandType: CommandType.Text).Single();
                SetId(obj, id, propertyContainer.IdPairs);
            }
        }

        protected async Task InsertAsync<T>(T obj)
        {
            var propertyContainer = ParseProperties(obj);
            var sql = $"INSERT INTO {typeof(T).Name} ({string.Join(", ", propertyContainer.ValueNames)}) VALUES(@{ string.Join(", @", propertyContainer.ValueNames)}); select LAST_INSERT_ID();";

            using (var connection = ConexaoDB())
            {
                var id = await Task.FromResult(connection.QueryAsync<int>
                (sql, propertyContainer.ValuePairs, commandType: CommandType.Text).Result.Single());
                SetId(obj, id, propertyContainer.IdPairs);
            }
        }


        protected void Update<T>(T obj)
        {
            var propertyContainer = ParseProperties(obj);
            var sqlIdPairs = GetSqlPairs(propertyContainer.IdNames);
            var sqlValuePairs = GetSqlPairs(propertyContainer.ValueNames);
            var sql = $"UPDATE {typeof(T).Name} SET { sqlValuePairs} WHERE { sqlIdPairs} ";
            Execute(CommandType.Text, sql, propertyContainer.AllPairs);
        }

        protected async Task UpdateAsync<T>(T obj)
        {
            var propertyContainer = ParseProperties(obj);
            var sqlIdPairs = GetSqlPairs(propertyContainer.IdNames);
            var sqlValuePairs = GetSqlPairs(propertyContainer.ValueNames);
            var sql = $"UPDATE {typeof(T).Name} SET { sqlValuePairs} WHERE { sqlIdPairs} ";
            await ExecuteAsync(CommandType.Text, sql, propertyContainer.AllPairs);
        }

        protected void Delete<T>(T obj)
        {
            var propertyContainer = ParseProperties(obj);
            var sqlIdPairs = GetSqlPairs(propertyContainer.IdNames);
            var sql = $"DELETE FROM {typeof(T).Name} WHERE {sqlIdPairs}";
            Execute(CommandType.Text, sql, propertyContainer.IdPairs);
        }

        protected async Task DeleteAsync<T>(T obj)
        {
            var propertyContainer = ParseProperties(obj);
            var sqlIdPairs = GetSqlPairs(propertyContainer.IdNames);
            var sql = $"DELETE FROM {typeof(T).Name} WHERE {sqlIdPairs}";
            await ExecuteAsync(CommandType.Text, sql, propertyContainer.IdPairs);
        }

        private PropertyContainer ParseProperties<T>(T obj)
        {
            var propertyContainer = new PropertyContainer();

            var typeName = typeof(T).Name;
            var validKeyNames = new[] { "Id",
            string.Format("{0}Id", typeName), string.Format("{0}_Id", typeName) };

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                // Skip reference types (but still include string!)
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                    continue;

                // Skip methods without a public setter
                if (property.GetSetMethod() == null)
                    continue;

                // Skip methods specifically ignored
                if (property.IsDefined(typeof(DapperIgnore), false))
                    continue;

                if (!property.PropertyType.IsSerializable)
                    continue;

                var name = property.Name;
                var value = typeof(T).GetProperty(property.Name).GetValue(obj, null);

                if (property.IsDefined(typeof(DapperKey), false) || validKeyNames.Contains(name))
                {
                    propertyContainer.AddId(name, value);
                }
                else
                {
                    propertyContainer.AddValue(name, value);
                }
            }

            return propertyContainer;
        }

        /// <summary>
        /// Create a commaseparated list of value pairs on 
        /// the form: "key1=@value1, key2=@value2, ..."
        /// </summary>
        private static string GetSqlPairs
        (IEnumerable<string> keys, string separator = ", ")
        {
            var pairs = keys.Select(key => string.Format("{0}=@{0}", key)).ToList();
            return string.Join(separator, pairs);
        }

        private void SetId<T>(T obj, int id, IDictionary<string, object> propertyPairs)
        {
            if (propertyPairs.Count == 1)
            {
                var propertyName = propertyPairs.Keys.First();
                var propertyInfo = obj.GetType().GetProperty(propertyName);
                if (propertyInfo.PropertyType == typeof(int))
                {
                    propertyInfo.SetValue(obj, id, null);
                }
            }
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
