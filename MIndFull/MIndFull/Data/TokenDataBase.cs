using MIndFull.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MIndFull.Data
{
    public class TokenDataBase
    {
        static object locker = new object();

        SQLiteConnection database;


        public TokenDataBase()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTable<UserToken>();
            database.CreateTable<OpcionesMenu>();
        }

        public UserToken GetToken(int id)
        {
            lock (locker)
            {
                return database.Table<UserToken>().FirstOrDefault(x => x.ID == id);
            }
        }

        public UserToken GetLastToken()
        {
            lock (locker)
            {
                return database.Table<UserToken>().LastOrDefault();
            }
        }

        public IEnumerable<OpcionesMenu> GetUserMenu() {
            lock (locker) {
                return database.Table<OpcionesMenu>();
            }
        }

        public int SaveUserMenu(IEnumerable<OpcionesMenu> options) {
            lock (locker)
            {
                database.InsertAll(options);
            }
            return options.Count();
        }

        public IEnumerable<UserToken> GetTokenFilter(string usToken)
        {
            lock (locker)
            {
                return database.Query<UserToken>("SELECT * FROM [UserToken] WHERE [UsToken] = " + usToken);
            }
        }

        public int SaveToken(UserToken UsTok)
        {
            lock (locker)
            {
                if (UsTok.ID != 0)
                {
                    database.Update(UsTok);
                    return UsTok.ID;
                }
                else
                {
                    return database.Insert(UsTok);
                }
            }
        }

        public int DeleteToken(int id)
        {
            lock (locker)
            {
                return database.Delete<UserToken>(id);
            }
        }

        public void DeleteAll()
        {
            //database.delete from your_table;
            database.DeleteAll<UserToken>();
            database.DeleteAll<OpcionesMenu>();
                //.delete from sqlite_sequence where name = 'your_table'
        }
    }
}
