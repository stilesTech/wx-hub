using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Entities;
using Register;
using SqlSugar;

namespace Business
{
    public class BaseService
    {
        private BaseService() { }
        public static BaseService<T> Build<T>() where T : class, new()
        {
            return new BaseService<T>();
        }
    }

    public class BaseService<T> where T : class, new()
    {
        
        public List<T> GetList()
        {
            return new DbContext<T>().TableDb.GetList();
        }

        public List<T> GetList(Expression<Func<T, bool>> whereExpression)
        {
            return new DbContext<T>().TableDb.GetList(whereExpression);
        }

        public T GetSingle(Expression<Func<T, bool>> whereExpression)
        {
            return new DbContext<T>().TableDb.GetSingle(whereExpression);
        }

        public T GetById(int id)
        {
            return new DbContext<T>().TableDb.GetById(id);
        }



        public List<T> GetPageList(Expression<Func<T, bool>> whereExpression,int pageIndex,int pageSize, Expression<Func<T, object>> orderByExpression,OrderByType orderByType= OrderByType.Asc)
        {
            return new DbContext<T>().Db.Queryable<T>().Where(whereExpression).OrderBy(orderByExpression, orderByType).ToPageList(pageIndex, pageSize);
        }


        public bool Insert(T t)
        {
            return new DbContext<T>().TableDb.Insert(t);
        }
        public int InsertReturnIdentity(T t)
        {
            return new DbContext<T>().TableDb.InsertReturnIdentity(t);
        }

        public bool Update(T t)
        {
            return new DbContext<T>().TableDb.Update(t);
        }


        public bool Delete(T t)
        {
            return new DbContext<T>().TableDb.Delete(t);
        }

        public bool DeleteById(int id)
        {
            return new DbContext<T>().TableDb.DeleteById(id);
        }

        public int Count(Expression<Func<T, bool>> whereExpression)
        {
            return new DbContext<T>().TableDb.Count(whereExpression);
        }

    }
}
