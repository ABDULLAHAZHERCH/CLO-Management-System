﻿using System;
using System.Data;
using System.Data.SqlClient;
namespace CRUD_Operations
{
    class Configuration
    {
        String ConnectionStr = @"Data Source=(local);Initial Catalog=ProjectB;Integrated Security=True";
        SqlConnection con;
        private static Configuration _instance;
        public static Configuration getInstance()
        {
            if (_instance == null)
                _instance = new Configuration();
            return _instance;
        }
        private Configuration()
        {
            con = new SqlConnection(ConnectionStr);
            con.Open();
        }
        public SqlConnection getConnection()
        {
            return con;
        }
        public void closeConnection()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
}






