using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Curly_TWM.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class

    {
        IEnumerable<T> GetAll();
        T GetObjByID(T Id);
        T GetObjByID(int Id);

        void InsertObj(T tobj);
        void DeleteObj(T tobj);
        void UpdateObj(T tobj);
        void BulkInsert(IEnumerable<T> tobj);
        //------------------------------
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        T SingleOrDefault(Expression<Func<T, bool>> predicate);
        //T First(Expression<Func<T, bool>> predicate);
        //------------------------------
        void Save();
        string UploadFile(HttpPostedFileBase file, string groupName);
        string UpdateFile(HttpPostedFileBase newfile, string oldFileName, string folderName);

        string DeleteFile(string oldFileName, string folderName);

        //------------------------------
        void UpdateRange(List<T> tobj);
        //------------------------------


    }

}
