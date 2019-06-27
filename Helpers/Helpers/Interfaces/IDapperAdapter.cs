using Jupiter.Helpers.Models.Utils;
using System;
using System.Data;

namespace Jupiter.Helpers.Interfaces
{
    public interface IDapperAdapter
    {
        Result<T> Execute<T>(Func<IDbConnection, Result<T>> action);
        
        Result Execute(Func<IDbConnection, Result> action);

        T Execute<T>(Func<IDbConnection, T> action);
    }
}
