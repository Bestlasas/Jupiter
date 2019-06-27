using Dapper;
using Jupiter.Helpers.Interfaces;
using Jupiter.Helpers.Models.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Jupiter.Helpers.Services
{
    public class DapperMSSQLAdapter : IDapperAdapter
    {
        private string _connectionString;

        public DapperMSSQLAdapter(IOptions<ConnectionProviderOptions> connectionProviderOptions) 
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var connectionString = connectionProviderOptions?.Value?.ConnectionString;
            var username = connectionProviderOptions?.Value?.Username;
            var password = connectionProviderOptions?.Value?.Password;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
                {
                    Password = password,
                    UserID = username
                };
                _connectionString = connectionStringBuilder.ToString();
            }
        }

        private IDbConnection Open()
        {
            return new SqlConnection(_connectionString);
        }

        public T Execute<T>(Func<IDbConnection, T> action)
        {
            var result = new Result<T>();
            try
            {
                using (var connection = Open())
                {
                   return action(connection);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Result<T> Execute<T>(Func<IDbConnection, Result<T>> action)
        {
            var result = new Result<T>();
            try
            {
                using (var connection = Open())
                {
                    result = action(connection);
                }
            }
            catch (Exception ex)
            {
                result.Errors = new List<Error> { new Error { Exception = ex, Message = "Error data access layer" } };
                return result;
            }
            result.Success = true;
            return result;
        }

        public Result Execute(Func<IDbConnection, Result> action)
        {
            var result = new Result();
            try
            {
                using (var connection = Open())
                {
                    result = action(connection);
                }
            }
            catch (Exception ex)
            {
                result.Errors = new List<Error> { new Error { Exception = ex, Message = "Error data access layer" } };
                return result;
            }
            result.Success = true;
            return result;
        }

    }
}
