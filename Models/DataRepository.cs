using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Web;

namespace GenericRepository.Models
{
    public class DataRepository : IDisposable
    {
        #region Context
        private BlogEntities _Context;
        public BlogEntities Context
        {
            get
            {
                if (_Context == null)
                {
                    _Context = new BlogEntities();
                }
                return _Context;
            }

        }
        #endregion

        /// <summary>
        /// Retrieve will support 3 cases :
        ///1) retrieve ALL records : if both parameters ID and PRED are empty : it's sensible, isn't it?
        ///2) retrieve just ONE record : if ID contains a value
        ///3) retrieve SELECTED records according to a PREDICATE : if the PRED parameter is set :
        /// </summary>       
        public IQueryable<T> Retrieve<T>(int? Id, Func<T, bool> pred) where T : class
        {
            List<T> list = new List<T>();
            if (Id.HasValue)
            {
                list.Add(Context.Set<T>().Find(Id.Value));
            }
            else if (pred != null)
            {
                list = Context.Set<T>().Where(pred).ToList();
            }
            else list = Context.Set<T>().ToList();
            return list.AsQueryable();
        }


        public void Create<T>(T entity) where T : class
        {
            Context.Set<T>().Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            var e = Context.Entry<T>(entity);
            e.State = EntityState.Modified;
        }

        public void Delete<T>(T entity) where T : class
        {
            Context.Set<T>().Remove(entity);
        }



        public bool Save(object target, int RecordsNumber)
        {
            try
            {
                return Context.SaveChanges() == RecordsNumber;
            }
            catch (OptimisticConcurrencyException)
            {
                ObjectContext ctx = ((IObjectContextAdapter)Context).ObjectContext;
                ctx.Refresh(RefreshMode.ClientWins, target);
                return Context.SaveChanges() == RecordsNumber;
            }
        }

        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
                GC.Collect();
            }
        }
    }
}
