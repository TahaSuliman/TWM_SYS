using Curly_TWM.Domain.Interfaces;
using Curly_TWM.Infrastructure.DbaseContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Curly_TWM.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class

    {
        internal TWMDB _context;
        internal DbSet<T> _dbSet;
        public GenericRepository(TWMDB context)
        {
            this._context = context;
            this._dbSet = _context.Set<T>();
        }

        public GenericRepository()
        {
        }

        public void BulkInsert(IEnumerable<T> tobj)
        {
            _dbSet.AddRange(tobj);
        }

        public void DeleteObj(T tobj)
        {
            _dbSet.Remove(tobj);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return (IEnumerable<T>)_dbSet.Where(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetObjByID(T Id)
        {
            return _dbSet.Find(Id);
        }
        public T GetObjByID(int Id)
        {
            return _dbSet.Find(Id);
        }
        public void InsertObj(T tobj)
        {
            _dbSet.Add(tobj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public T SingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public void UpdateObj(T tobj)
        {
            _context.Entry(tobj).State = EntityState.Modified;
        }

        //-------------------------------

        public string UploadFile(HttpPostedFileBase file, string folderName)
        {
            try
            {
                string newfilename = "";
                string str = "";
                if (file.ContentLength > 0)
                {
                    str = String.Format("{0:d-M-yyyy-HH-mm-ss}", DateTime.Now) + RandomString(4).ToString();
                    //str = fileName;

                    string path = HttpContext.Current.Server.MapPath("~/Uploads/" + folderName);
                    string pathString = System.IO.Path.Combine(path.ToString());
                    string fileName1 = Path.GetFileNameWithoutExtension(file.FileName);
                    string ext1 = System.IO.Path.GetExtension(file.FileName);
                    newfilename = str + ext1.ToLower().ToString();
                    bool isExists = System.IO.Directory.Exists(pathString);
                    if (!isExists) System.IO.Directory.CreateDirectory(pathString);
                    var uploadpath = string.Format("{0}\\{1}", pathString, newfilename);
                    file.SaveAs(uploadpath);
                    //do processing of second file here

                }

                return newfilename;
            }
            catch
            {
                return "Error file not save ...";
            }
        }
        //------------------------------

        public string UpdateFile(HttpPostedFileBase file, string oldFileName, string folderName)
        {
            try
            {
                string newfilename = "";
                string str = "";
                if (file != null)
                {
                    str = String.Format("{0:d-M-yyyy-HH-mm-ss}", DateTime.Now) + RandomString(4).ToString();
                    //str = fileName;

                    string path = HttpContext.Current.Server.MapPath("~/Uploads/" + folderName);
                    string pathString = System.IO.Path.Combine(path.ToString());
                    string fileName1 = Path.GetFileNameWithoutExtension(file.FileName);
                    string ext1 = System.IO.Path.GetExtension(file.FileName);
                    newfilename = str + ext1.ToLower().ToString();
                    bool isExists = System.IO.Directory.Exists(pathString);
                    if (!isExists) System.IO.Directory.CreateDirectory(pathString);
                    var uploadpath = string.Format("{0}\\{1}", pathString, newfilename);
                    file.SaveAs(uploadpath);

                    //do processing of second file here
                    //---------------------------------------------
                    string fullPath = Path.Combine(path, oldFileName);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    //-------------------------------
                }

                return newfilename;
            }
            catch
            {
                return "Error file not save ...";
            }
        }
        //-------------------------------
        public string DeleteFile(string FileName, string folderName)
        {
            try
            {
                if (FileName != null)
                {

                    string path = HttpContext.Current.Server.MapPath("~/Uploads/" + folderName);

                    //---------------------------------------------
                    string fullPath = Path.Combine(path, FileName);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                        return "deleted";
                    }
                    //-------------------------------
                }
                return "file not deleted ...";

            }
            catch
            {
                return "Error file not save ...";
            }
        }
        //-------------------------------

        private readonly Random _random = new Random();

        public string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        //-------------------------------
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        //------------------------------
        public void UpdateRange(List<T> tobj_list)
        {
            foreach (T record in tobj_list)
            {
                _context.Entry(record).State = EntityState.Modified;
            }
        }
        //------------------------------
    }
}