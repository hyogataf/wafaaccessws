using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Data;

namespace WafaAccessWS.Models
{
    public class AuditlogRepository
    {
        WafaaccessContext context = new WafaaccessContext();

        public IQueryable<Auditlog> All
        {
            get { return context.Auditlog; }
        }

        public Auditlog Find(long id)
        {
            return context.Auditlog.Find(id);
        }

        public void InsertOrUpdate(Auditlog Auditlog)
        {
            if (Auditlog.AuditlogId == default(long))
            {
                // New entity
                context.Auditlog.Add(Auditlog);
            }
            else
            {
                // Existing entity
                context.Entry(Auditlog).State = EntityState.Modified;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Create(string action, string userAction, string login, string filialeId, string ribCompte, string timestamp, string wsSignature, string errorCode, int? returnCode, string returnMessage)
        {
            var Auditlog = new Auditlog();
            Auditlog.Action = action;
            Auditlog.DateAction = DateTime.Now;
            Auditlog.login = login;
            Auditlog.filialeId = filialeId;
            Auditlog.ribCompte = ribCompte;
            Auditlog.timestamp = timestamp;
            Auditlog.wsSignature = wsSignature;
            Auditlog.returnCode = returnCode;
            Auditlog.errorCode = errorCode;
            Auditlog.returnMessage = returnMessage;
            Auditlog.UserAction = userAction;
            InsertOrUpdate(Auditlog);
            Save();
        }


        public void Dispose()
        {
            context.Dispose();
        }

    }


    public interface IAuditlogRepository : IDisposable
    {
        IQueryable<Auditlog> All { get; }
        //  IQueryable<Auditlog> AllIncluding(params Expression<Func<Auditlog, object>>[] includeProperties);
        Auditlog Find(long id);
        IQueryable<Auditlog> FindByRequest(long id);
        void InsertOrUpdate(Auditlog Auditlog);
        void Create(string action, string userAction, string login, string filialeId, string ribCompte, string timestamp, string wsSignature, string errorCode, int? returnCode, string returnMessage);
        //  void Delete(long id);
        void Save();
    }
}