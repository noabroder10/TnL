﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//DB classes
using System.Data;
using System.Data.OleDb;



namespace TnL_DAL
{
    public class DBHelper
    {
        //Constants
        public const int WRITEDATA_ERROR = -1;


        //class member variables
        private static OleDbConnection conn; //holds the connection for all operations using OleDB
        private static string provider; //holds the provider for the connection
        private static string source; //holds the path to the database file
        private static bool connOpen; //indicates if the connection is opened


        //Open connection to the database. return true if
        //if not succeed return false
        public static bool OpenConnection()
        {
            if (connOpen)
                return true;
            string connString = String.Format(@"Provider={0};Data Source={1};", provider, source);
            conn = new OleDbConnection(connString);
            try
            {
                conn.Open();
                connOpen = true;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("$$" + e.Message);
                return false;
            }

        }


        //Execute SELECT sql commands and return a reference to an OleDbDataReader
        //if execution fails return null
        public static OleDbDataReader ReadData(string sql)
        {
            OpenConnection();
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            OleDbDataReader rd = cmd.ExecuteReader();
            if (rd != null)
            {
                return rd;
            }
            else
            {
                return null;
            }
        }




        //Execute UPDATE or INSERT sql commands and return number of rows
        //return WRITEDATA_ERROR on failure.
        public static int WriteData(string sql)
        {
            OpenConnection();
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            OleDbDataReader rd = cmd.ExecuteReader();
           
            if (rd != null)
            {
                return rd.RecordsAffected;
            }
            else
                return WRITEDATA_ERROR;
        }




        //This function should be used for inserting a single record into a table in the
        //database with an autonmuber key.the format of the sql must be
        //INSERT INTO <TableName> (Fields...) VALUES (values...)
        //the function return the autonumber key generated for the new record or
        //WRITEDATA_ERROR if fail.

        public static int InsertWithAutoNumKey(string sql)
        {
            try
            {
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataReader rd = cmd.ExecuteReader();

                //Check if insert was successful
                if (rd != null && rd.RecordsAffected == 1)
                {
                    //Create a new command for retrieving the new ID
                    //It MUST use the SAME connection!!!!
                    cmd = new OleDbCommand(@"SELECT @@Identity", conn);
                    rd = cmd.ExecuteReader();
                    int newID = -1;
                    while (rd.Read())
                    {
                        //The new ID will be on the first (and only) column
                        newID = (int)rd[0];
                    }
                    return newID;
                }
                else
                {
                    return WRITEDATA_ERROR;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return WRITEDATA_ERROR;
            }

        }



        //This function is closing the connection unless it is already
        public static void CloseConnection()
        {
            if (connOpen)
            {
                try
                {
                    conn.Close();
                    connOpen = false;

                }
                catch (Exception e)
                {
                    Console.WriteLine("22"+e.Message);

                }
            }
        }



        //This function builds the connection string to be used to open connection.
        private static string BuildConnString()
        {
            return String.Format(@"Provider={0};Data Source={1};Persist Security Info=False;", provider, source);
        }


        //This function reads from the database a data table fully cached in memory using a
        //standard SQL SELECT statement.
        //The function returns the data table or null on 
        public static DataTable GetDataTable(string sql)
        {
            OpenConnection();
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            OleDbDataReader rd = cmd.ExecuteReader();

            DataTable dataTable = new DataTable();
            if (rd != null)
            {
                //Populate the reader data into it
                dataTable.Load(rd);
                return dataTable;
            }
            return null;


        }


        //This function reads from the database a data set fully cached in memory using an
        //array of standard SQL SELECT statements.
        //The function returns the data set or null on failure. The table names inside the
        //dataset are sql1, sql2,...
        public static DataSet GetDataSet(string[] sql)
        {

            OpenConnection();
            DataSet dataSet = new DataSet();

            for (int i = 0; i < sql.Length; i++)
            {
                OleDbCommand cmd = new OleDbCommand(sql[i], conn);
                OleDbDataReader rd = cmd.ExecuteReader();

                DataTable dataTable = new DataTable();
                if (rd != null)
                {
                    //Populate the reader data into it
                    dataTable.Load(rd);
                    dataTable.TableName = "Sql" + (i + 1);
                    dataSet.Tables.Add(dataTable);
                }
                else
                {
                    return null;
                }
            }
            return dataSet;

        }

    }
}
