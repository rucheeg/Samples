using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.Data;
using ServiceStack.Text;

namespace AMS.Service
{
    public class AMSQueryServiceBase : ServiceStack.Service
    {
        public IAutoQueryDb AutoQuery { get; set; }

        private readonly string _superAdminRoleName =
            ConfigurationManager.AppSettings["SuperAdminRoleName"] ?? "Trace3Admin";

        public void SetSessionBeforeCall(IDbConnection db, Action<IDbConnection> dataCall)
        {
            //var session = this.SessionAs<InventoryUserSession>();
            //var sessionContextSql = "EXEC sp_set_session_context 'Admin',1"; //!session.Roles.IsNullOrEmpty() && session.Roles.Exists(role => role == _superAdminRoleName)
            //    //? "EXEC sp_set_session_context 'Admin',1"
            //    //: "EXEC sp_set_session_context 'ClientId',{0}".FormatWith(session.ClientId);
            //db.ExecuteSql(sessionContextSql);
            dataCall(db);
        }

        public QueryResponse<TK> ExecuteQuery<T, TK>(QueryDb<T, TK> req, Dictionary<string, string> queryArgs = null)
        {
            QueryResponse<TK> queryResponse = null;
            SetSessionBeforeCall(Db, dbConnection =>
            {
                if(queryArgs == null) queryArgs = Request.GetRequestParams();
                var q = AutoQuery.CreateQuery(req, queryArgs, Request);
                queryResponse = new QueryResponse<TK>()
                {
                    Offset = q.Offset.GetValueOrDefault(0),
                    Total = (int) dbConnection.Count(q),
                    Results = dbConnection.LoadSelect<TK, T>(q)
                };
            });
            return queryResponse;
        }

        public QueryResponse<T> ExecuteSimpleQuery<T>(QueryDb<T> req, Dictionary<string, string> queryArgs = null)
        {
            QueryResponse<T> queryResponse = null;
            SetSessionBeforeCall(Db, dbConnection =>
            {
                if (queryArgs == null) queryArgs = Request.GetRequestParams();
                var q = AutoQuery.CreateQuery(req, queryArgs, Request);
                queryResponse = new QueryResponse<T>()
                {
                    Offset = q.Offset.GetValueOrDefault(0),
                    Total = (int)dbConnection.Count(q),
                    Results = dbConnection.LoadSelect<T>(q)
                };
            });
            return queryResponse;
        }
    }
}
