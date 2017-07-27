using AMS.UserAuth.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Castle.Core.Internal;
using ServiceStack;
using ServiceStack.OrmLite;

namespace AMS.Service
{
    public class AMSSingleQueryServiceBase<T> : ServiceStack.Service
    {
        public IAutoQueryDb AutoQuery { get; set; }

        private readonly string _superAdminRoleName =
            ConfigurationManager.AppSettings["SuperAdminRoleName"] ?? "Trace3Admin";

        public void SetSessionBeforeCall(IDbConnection db, Action<IDbConnection> dataCall)
        {
            var session = this.SessionAs<InventoryUserSession>();
            var sessionContextSql = !session.Roles.IsNullOrEmpty() && session.Roles.Exists(role => role == _superAdminRoleName)
                ? "EXEC sp_set_session_context 'Admin',1"
                : "EXEC sp_set_session_context 'ClientId',{0}".FormatWith(session.ClientId);
            db.ExecuteSql(sessionContextSql);
            dataCall(db);
        }

        public QueryResponse<T> ExecuteQuery(QueryDb<T> req, Dictionary<string, string> queryArgs = null)
        {
            QueryResponse<T> queryResponse = null;
            SetSessionBeforeCall(Db, dbConnection =>
            {
                if(queryArgs == null) queryArgs = Request.GetRequestParams();
                var q = AutoQuery.CreateQuery(req, queryArgs, Request);
                queryResponse = new QueryResponse<T>()
                {
                    Offset = q.Offset.GetValueOrDefault(0),
                    Total = (int) dbConnection.Count(q),
                    Results = dbConnection.LoadSelect<T>(q)
                };
            });
            return queryResponse;
        }
    }
}
