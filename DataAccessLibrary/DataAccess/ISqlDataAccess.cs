﻿namespace DataAccessLibrary.DataAccess
{
    public interface ISqlDataAccess
    {
        Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionString = "AppDataDbConnection");
        Task SaveData<T>(string storedProcedure, T parameters, string connectionString = "AppDataDbConnection");
    }
}