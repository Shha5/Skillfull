using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace DataAccessLibrary.DataAccess
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _configuration;

        public SqlDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionString = "AppDataDbConnection")
        {
            using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString(connectionString)))
            {
                return await connection.QueryAsync<T>(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task SaveData<T>(string storedProcedure, T parameters, string connectionString = "AppDataDbConnection")
        {
            using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString(connectionString)))
            {
                await connection.ExecuteAsync(storedProcedure, parameters,
                commandType: CommandType.StoredProcedure);
            }
        }


    }
}
