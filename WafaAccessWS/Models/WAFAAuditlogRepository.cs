using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Data;
using System.Diagnostics;

namespace WafaAccessWS.Models
{
    public class WAFAAuditlogRepository
    {
        WafaaccessContext context = new WafaaccessContext();

        public IQueryable<WAFAAuditlog> All
        {
            get { return context.WAFAAuditlog; }
        }

        public WAFAAuditlog Find(long id)
        {
            return context.WAFAAuditlog.Find(id);
        }

        public void InsertOrUpdate(WAFAAuditlog WAFAAuditlog)
        {
            if (WAFAAuditlog.WAFAAuditlogId == default(long))
            {
                // New entity
                context.WAFAAuditlog.Add(WAFAAuditlog);
            }
            else
            {
                // Existing entity
                context.Entry(WAFAAuditlog).State = EntityState.Modified;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Create(string action, string userAction, string login, string filialeId, string ribCompte, string timestamp, string wsSignature, string errorCode, int? returnCode, string returnMessage)
        {
            try
            {
                var WAFAAuditlog = new WAFAAuditlog();
                WAFAAuditlog.Action = action;
                WAFAAuditlog.DateAction = DateTime.Now;
                WAFAAuditlog.login = login;
                WAFAAuditlog.filialeId = filialeId;
                WAFAAuditlog.ribCompte = ribCompte;
                WAFAAuditlog.timestamp = timestamp;
                WAFAAuditlog.wsSignature = wsSignature;
                WAFAAuditlog.returnCode = returnCode;
                WAFAAuditlog.errorCode = errorCode;
                WAFAAuditlog.returnMessage = returnMessage;
                WAFAAuditlog.UserAction = userAction;

                InsertOrUpdate(WAFAAuditlog);
                Save();
            }
            catch (Exception e)
            {
                Debug.WriteLine(" Create exception = " + e.StackTrace);
            }
        }


        public void Dispose()
        {
            context.Dispose();
        }

    }


    public interface IWAFAAuditlogRepository : IDisposable
    {
        IQueryable<WAFAAuditlog> All { get; }
        //  IQueryable<WAFAAuditlog> AllIncluding(params Expression<Func<WAFAAuditlog, object>>[] includeProperties);
        WAFAAuditlog Find(long id);
        IQueryable<WAFAAuditlog> FindByRequest(long id);
        void InsertOrUpdate(WAFAAuditlog WAFAAuditlog);
        void Create(string action, string userAction, string login, string filialeId, string ribCompte, string timestamp, string wsSignature, string errorCode, int? returnCode, string returnMessage);
        //  void Delete(long id);
        void Save();
    }
}