using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Entity;
using System.Data;
using System.IO;
using System.Data.SQLite;
namespace Business
{
    public static class UserInfoDal
    {
        private static readonly string fileName = string.Format(@"{0}waddb\user.db3", AppDomain.CurrentDomain.BaseDirectory);
        private static readonly string connstr = string.Format(CreateDbFile.connectionStringTemp, fileName);
        public static void CreateDb()
        {
            if (!File.Exists(fileName))
            {
                SQLiteConnection.CreateFile(fileName);
            }

            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                if (!IsTableExist(conn))
                {
                    CreateTable(conn);
                }
            }
        }

        private static void CreateTable(SQLiteConnection conn)
        {
            string sql = string.Format(@"create table tb_User (
                                                           Account TEXT NOT NULL,
                                                           PassWord TEXT NOT NULL,
                                                           UserName TEXT NOT NULL,
                                                           Level INT NOT NULL
                )");
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();
        }

        private static bool IsTableExist(SQLiteConnection conn)
        {
            string sql = string.Format(@"SELECT COUNT(*) FROM sqlite_master where type='table' and name='tb_User'");
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
        public static void AddOne(ref UserInfo info)
        {
            string sql = @"insert into tb_User (Account,PassWord,UserName,Level) select @account,@password,@userName,@level where not exists (select 1 from tb_User where Account=@account);
            select last_insert_rowid();";
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql,conn))
                {
                    cmd.Parameters.AddWithValue("@account", info.Account);
                    cmd.Parameters.AddWithValue("@password", info.PassWord);
                    cmd.Parameters.AddWithValue("@userName", info.UserName);
                    cmd.Parameters.AddWithValue("@level", info.Level);
                    info.ID = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public static void UpdateOne(UserInfo info)
        {
            string sql = "update tb_User set PassWord=@password,UserName=@userName,Level=@level where rowid=@id";
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    // 账号不能改
                    //cmd.Parameters.AddWithValue("@account", info.Account);
                    cmd.Parameters.AddWithValue("@password", info.PassWord);
                    cmd.Parameters.AddWithValue("@userName", info.UserName);
                    cmd.Parameters.AddWithValue("@level", info.Level);
                    cmd.Parameters.AddWithValue("@id", info.ID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static UserInfo GetOneByID(int id)
        {
            UserInfo user = new UserInfo();
            string sql = "select Account,PassWord,UserName,Level from tb_User where rowid=@id";

            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user.ID = id;
                            user.Account = reader.GetString(0);
                            user.PassWord = reader.GetString(1);
                            user.UserName = reader.GetString(2);
                            user.Level = (EM_UserType)reader.GetInt32(3);
                        }
                        else
                        {
                            user = null;
                        }
                    }
                }
            }

            return user;
        }
        public static UserInfo GetOneByUser(string account,string password)
        {
            UserInfo user = new UserInfo();
            string sql = "select Account,PassWord,UserName,Level,rowid from tb_User where PassWord=@password and Account=@account";

            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@account", account);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user.Account = reader.GetString(0);
                            user.PassWord = reader.GetString(1);
                            user.UserName = reader.GetString(2);
                            user.Level = (EM_UserType)reader.GetInt32(3);
                            user.ID = reader.GetInt32(4);
                        }
                        else
                        {
                            user = null;
                        }
                    }
                }
            }

            return user;
        }
    }
}
