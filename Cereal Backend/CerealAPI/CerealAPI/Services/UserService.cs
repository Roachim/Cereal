using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CerealAPI.DTO;
using Microsoft.Data.SqlClient;
using System.IO.MemoryMappedFiles;
using System.Reflection;
using System.Data.SqlTypes;
using Microsoft.AspNetCore.Http;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace CerealAPI.Services
{
    /// <summary>
    /// all the services provided to the user of the app
    /// Creating, editing, deleting etc etc.
    /// Same for disciplines and their models + their associations
    /// </summary>
    public static class UserService
    {
        /// <summary>
        /// Get and return all cereals currently in the database
        /// </summary>
        /// <param name="connectionString">String for connecting to the database</param>
        /// <returns>List of cereal objects</returns>
        public static List<CerealDTO> GetAllCereal(SqlConnection conn, SqlTransaction transaction)
        {
            //List that will be returned
            List<CerealDTO> returnList = new List<CerealDTO>();
            //query for the database
            string queryString = "SELECT * FROM dbo.Cereal;";

            //make procedure to execute query, using the queryString and the connection
            SqlCommand command = new SqlCommand(queryString, conn);
            command.Transaction = transaction;

            //read the data from the database using the datareader
            using (SqlDataReader reader = command.ExecuteReader())
            {
                //as long as data continues to be streamed(for all data), read and parse data into objects
                while (reader.Read())
                {
                    int Id = (int)reader[0];
                    string Name = (string)reader[1];
                    string Mfr = (string)reader[2];
                    string Type = (string)reader[3];
                    int Calories = (int)reader[4];
                    int Protein = (int)reader[5];
                    int Fat = (int)reader[6];
                    int Sodium = (int)reader[7];
                    double Fiber = (double)reader[8];
                    double Carbo = (double)reader[9];
                    int Sugars = (int)reader[10];
                    int Potass = (int)reader[11];
                    int Vitamins = (int)reader[12];
                    int Shelf = (int)reader[13];
                    double Weight = (double)reader[14];
                    double Cups = (double)reader[15];
                    double Rating = (double)reader[16];
                    //add object to list
                    returnList.Add(new CerealDTO(Id, Name, Mfr, Type, Calories, Protein, Fat, Sodium, Fiber, Carbo, Sugars, Potass, Vitamins,
                        Shelf, Weight, Cups, Rating));
                }
            }

            return returnList;
        }

        /// <summary>
        /// Get a single cereal from the database
        /// </summary>
        /// <param name="connectionString">The connection string for the database</param>
        /// <param name="cerealId">The id of the cereal</param>
        /// <returns>Id and name of cereal</returns>
        public static CerealDTO GetCereal(SqlConnection conn, SqlTransaction transaction, int cerealId)
        {
            //query for the database
            string queryString = "SELECT * FROM dbo.Cereal WHERE Id = @Id";

            //Object that will be returned
            CerealDTO returnObject = new CerealDTO(0, "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        0, 0, 0, 0);

            //make procedure to execute query, using the queryString and the connection
            SqlCommand command = new SqlCommand(queryString, conn);
            command.Transaction = transaction;

            //Alter the query string safely to help prevent SQL injection
            //The id input at the start of the method is put in here. Keyword here is :Prepared Statement
            command.Parameters.Add("@Id", System.Data.SqlDbType.Int);
            command.Parameters["@Id"].Value = cerealId;


            //read the data from the database using the datareader
            using (SqlDataReader reader = command.ExecuteReader())
            {
                //as long as data continues to be streamed(for a single instance), read and parse data into objects
                while (reader.Read())
                {
                    int Id = (int)reader[0];
                    string Name = (string)reader[1];
                    string Mfr = (string)reader[2];
                    string Type = (string)reader[3];
                    int Calories = (int)reader[4];
                    int Protein = (int)reader[5];
                    int Fat = (int)reader[6];
                    int Sodium = (int)reader[7];
                    double Fiber = (double)reader[8];
                    double Carbo = (double)reader[9];
                    int Sugars = (int)reader[10];
                    int Potass = (int)reader[11];
                    int Vitamins = (int)reader[12];
                    int Shelf = (int)reader[13];
                    double Weight = (double)reader[14];
                    double Cups = (double)reader[15];
                    double Rating = (double)reader[16];
                    //add object to list
                    returnObject = new CerealDTO(Id, Name, Mfr, Type, Calories, Protein, Fat, Sodium, Fiber, Carbo, Sugars, Potass, Vitamins,
                        Shelf, Weight, Cups, Rating);
                }
            }
            return returnObject;
        }

        public static CerealPicturesDTO GetCerealPicture(SqlConnection conn, SqlTransaction transaction, int cerealId)
        {
            //query for the database
            string queryString = "SELECT * FROM dbo.CerealPictures WHERE Id = @Id";

            //Object that will be returned
            Byte[] e = new byte[10];
            CerealPicturesDTO returnObject = new CerealPicturesDTO(0, e);

            //make procedure to execute query, using the queryString and the connection
            SqlCommand command = new SqlCommand(queryString, conn);
            command.Transaction = transaction;

            //Alter the query string safely to help prevent SQL injection
            //The id input at the start of the method is put in here. Keyword here is :Prepared Statement
            command.Parameters.Add("@Id", System.Data.SqlDbType.Int);
            command.Parameters["@Id"].Value = cerealId;


            //read the data from the database using the datareader
            using (SqlDataReader reader = command.ExecuteReader())
            {
                //as long as data continues to be streamed(for a single instance), read and parse data into objects
                while (reader.Read())
                {
                    int Id = (int)reader[0];
                    Byte[] Picture = (Byte[])reader[1];
                   
                    //add object to list
                    returnObject = new CerealPicturesDTO(Id, Picture);
                }
            }
            return returnObject;
        }

        /// <summary>
        /// Get and return all cereals having specific values in certain paramters
        /// </summary>
        /// <param name="connectionString">String for connecting to the database</param>
        /// <param name="projectId">Id of project</param>
        /// <returns>List of project objects</returns>
        public static List<CerealDTO> GetAllCerealWithCertainCategoryValue(SqlConnection conn, SqlTransaction transaction, CerealDTO dtoObject)
        {
            //List that will be returned
            List<CerealDTO> returnList = new List<CerealDTO>();
            //query for the database
            string queryString =
                "select * from dbo.Cereal ";

            queryString = constructQueryString(queryString, dtoObject);
            //make procedure to execute query, using the queryString and the connection
            SqlCommand command = new SqlCommand(queryString, conn);
            command.Transaction = transaction;

            //Alter the query string safely to help prevent SQL injection
            //The id input at the start of the method is put in here. Keyword here is :Prepared Statement
            //command.Parameters.Add("@Id", System.Data.SqlDbType.Int);
            //command.Parameters["@Id"].Value = projectId;

            //Seperate method for command parameters. VOID
            ParameterInjection(command, dtoObject);

            //read the data from the database using the datareader
            using (SqlDataReader reader = command.ExecuteReader())
            {
                //as long as data continues to be streamed(for all data), read and parse data into objects
                while (reader.Read())
                {
                    int Id = (int)reader[0];
                    string Name = (string)reader[1];
                    string Mfr = (string)reader[2];
                    string Type = (string)reader[3];
                    int Calories = (int)reader[4];
                    int Protein = (int)reader[5];
                    int Fat = (int)reader[6];
                    int Sodium = (int)reader[7];
                    double Fiber = (double)reader[8];
                    double Carbo = (double)reader[9];
                    int Sugars = (int)reader[10];
                    int Potass = (int)reader[11];
                    int Vitamins = (int)reader[12];
                    int Shelf = (int)reader[13];
                    double Weight = (double)reader[14];
                    double Cups = (double)reader[15];
                    double Rating = (double)reader[16];
                    //add object to list
                    returnList.Add(new CerealDTO(Id, Name, Mfr, Type, Calories, Protein, Fat, Sodium, Fiber, Carbo, Sugars, Potass, Vitamins,
                        Shelf, Weight, Cups, Rating));
                }
            }

            return returnList;
        }

        public static List<CerealDTO> GetAllCerealWithWithRestriction(SqlConnection conn, SqlTransaction transaction, CerealDTO dtoObject, string mathSign)
        {
            //List that will be returned
            List<CerealDTO> returnList = new List<CerealDTO>();
            //query for the database
            string queryString =
                "select * from dbo.Cereal ";

            queryString = constructQueryString(queryString, dtoObject, mathSign);
            //make procedure to execute query, using the queryString and the connection
            SqlCommand command = new SqlCommand(queryString, conn);
            command.Transaction = transaction;

            //Alter the query string safely to help prevent SQL injection
            //The id input at the start of the method is put in here. Keyword here is :Prepared Statement
            //command.Parameters.Add("@Id", System.Data.SqlDbType.Int);
            //command.Parameters["@Id"].Value = projectId;

            //Seperate method for command parameters. VOID
            ParameterInjection(command, dtoObject);

            //read the data from the database using the datareader
            using (SqlDataReader reader = command.ExecuteReader())
            {
                //as long as data continues to be streamed(for all data), read and parse data into objects
                while (reader.Read())
                {
                    int Id = (int)reader[0];
                    string Name = (string)reader[1];
                    string Mfr = (string)reader[2];
                    string Type = (string)reader[3];
                    int Calories = (int)reader[4];
                    int Protein = (int)reader[5];
                    int Fat = (int)reader[6];
                    int Sodium = (int)reader[7];
                    double Fiber = (double)reader[8];
                    double Carbo = (double)reader[9];
                    int Sugars = (int)reader[10];
                    int Potass = (int)reader[11];
                    int Vitamins = (int)reader[12];
                    int Shelf = (int)reader[13];
                    double Weight = (double)reader[14];
                    double Cups = (double)reader[15];
                    double Rating = (double)reader[16];
                    //add object to list
                    returnList.Add(new CerealDTO(Id, Name, Mfr, Type, Calories, Protein, Fat, Sodium, Fiber, Carbo, Sugars, Potass, Vitamins,
                        Shelf, Weight, Cups, Rating));
                }
            }

            return returnList;
        }


        /// <summary>
        /// Get method for all cereal with paramters for detailing what cereal should be returned
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="transaction"></param>
        /// <param name="dtoMathObject"></param>
        /// <returns></returns>
        public static List<CerealDTO> GetAllCerealWithWithRestrictions(SqlConnection conn, SqlTransaction transaction, CerealMathDTO dtoMathObject)
        {
            //List that will be returned
            List<CerealDTO> returnList = new List<CerealDTO>();
            //query for the database
            string queryString =
                "select * from dbo.Cereal ";

            queryString = constructQueryStringForMath(queryString, dtoMathObject);
            //make procedure to execute query, using the queryString and the connection
            SqlCommand command = new SqlCommand(queryString, conn);
            command.Transaction = transaction;

            //Seperate method for command parameters. VOID
            ParameterInjectionForMath(command, dtoMathObject);

            //read the data from the database using the datareader
            using (SqlDataReader reader = command.ExecuteReader())
            {
                //as long as data continues to be streamed(for all data), read and parse data into objects
                while (reader.Read())
                {
                    int Id = (int)reader[0];
                    string Name = (string)reader[1];
                    string Mfr = (string)reader[2];
                    string Type = (string)reader[3];
                    int Calories = (int)reader[4];
                    int Protein = (int)reader[5];
                    int Fat = (int)reader[6];
                    int Sodium = (int)reader[7];
                    double Fiber = (double)reader[8];
                    double Carbo = (double)reader[9];
                    int Sugars = (int)reader[10];
                    int Potass = (int)reader[11];
                    int Vitamins = (int)reader[12];
                    int Shelf = (int)reader[13];
                    double Weight = (double)reader[14];
                    double Cups = (double)reader[15];
                    double Rating = (double)reader[16];
                    //add object to list
                    returnList.Add(new CerealDTO(Id, Name, Mfr, Type, Calories, Protein, Fat, Sodium, Fiber, Carbo, Sugars, Potass, Vitamins,
                        Shelf, Weight, Cups, Rating));
                }
            }

            return returnList;
        }

        private static string constructQueryString(string queryString, CerealDTO cerealObject, string math = "=")
        {
            queryString = queryString + " WHERE";
            string originalString = queryString;
            
            if (cerealObject.Name != "0")
            {
                queryString += " Name "+math+" @Name";
            }
            if (cerealObject.Mfr != "0")
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Mfr "+math+" @Mfr";
            }
            if (cerealObject.Type != "0")
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Type "+math+" @Type";
            }
            if (cerealObject.Calories != 0)
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Calories "+math+" @Calories";
            }
            if (cerealObject.Protein != 0)
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Protein "+math+" @Protein";
            }
            if (cerealObject.Fat != 0)
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Fat "+math+" @Fat";
            }
            if (cerealObject.Sodium != 0)
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Sodium "+math+" @Sodium";
            }
            if (cerealObject.Fiber != 0)
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Fiber "+math+" @Fiber";
            }
            if (cerealObject.Carbo != 0)
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Carbo "+math+" @Carbo";
            }
            if (cerealObject.Sugars != 0)
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Sugars "+math+" @Sugars";
            }
            if (cerealObject.Potass != 0)
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Potass "+math+" @Potass";
            }
            if (cerealObject.Vitamins != 0)
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Vitamins "+math+" @Vitamins";
            }
            if (cerealObject.Shelf != 0)
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Shelf "+math+" @Shelf";
            }
            if (cerealObject.Weight != 0)
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Weight "+math+" @Weight";
            }
            if (cerealObject.Cups != 0)
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Cups "+math+" @Cups";
            }
            if (cerealObject.Rating != 0)
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Rating "+math+" @Rating";
            }


            queryString += ";";
            return queryString;
        }


        /// <summary>
        /// construct query for GetAllCerealWithWithRestrictions
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="cmo"></param>
        /// <returns>String</returns>
        private static string constructQueryStringForMath(string queryString, CerealMathDTO cmo)
        {
            queryString = queryString + " WHERE";
            
            string originalString = queryString;

            if (cmo.Name != "0")
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Name " + "=" + " @Name";
            }
            if (cmo.Mfr != "0")
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Mfr " + "=" + " @Mfr";
            }
            if (cmo.Type != "0")
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Type " + "=" + " @Type";
            }

            if (cmo.Calories != "0")
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Calories " + ReturnMathSign(cmo.Calories) + " @Calories";
            }
            if (cmo.Protein != "0")
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Protein " + ReturnMathSign(cmo.Protein) + " @Protein";
            }
            if (cmo.Fat != "0")
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Fat " + ReturnMathSign(cmo.Fat) + " @Fat";
            }
            if (cmo.Sodium != "0")
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Sodium " + ReturnMathSign(cmo.Sodium) + " @Sodium";
            }
            if (cmo.Fiber != "0")
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Fiber " + ReturnMathSign(cmo.Fiber) + " @Fiber";
            }
            if (cmo.Carbo != "0")
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Carbo " + ReturnMathSign(cmo.Carbo) + " @Carbo";
            }
            if (cmo.Sugars != "0")
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Sugars " + ReturnMathSign(cmo.Sugars) + " @Sugars";
            }
            if (cmo.Potass != "0")
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Potass " + ReturnMathSign(cmo.Potass) + " @Potass";
            }
            if (cmo.Vitamins != "0")
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Vitamins " + ReturnMathSign(cmo.Vitamins) + " @Vitamins";
            }
            if (cmo.Shelf != "0")
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Shelf " + ReturnMathSign(cmo.Shelf) + " @Shelf";
            }
            if (cmo.Weight != "0")
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Weight " + ReturnMathSign(cmo.Weight) + " @Weight";
            }
            if (cmo.Cups != "0")
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Cups " + ReturnMathSign(cmo.Cups) + " @Cups";
            }
            if (cmo.Rating != "0")
            {
                queryString = PutAndIfNecessary(originalString, queryString);
                queryString += " Rating " + ReturnMathSign(cmo.Rating) + " @Rating";
            }


            queryString += ";";
            return queryString;
        }


        /// <summary>
        /// Check whether a string is fit for use with the constructQueryStringForMath
        /// </summary>
        /// <param name="str">string to check</param>
        /// <returns></returns>
        private static bool StringContainsMath(string str)
        {
            if(str.Contains(">") || str.Contains("<") || str.Contains("=") || str.Contains("!=") || str.Contains(">=") || str.Contains("<="))
            {
                return true;
            }
            return false;
        }

        private static string GetNumberFromString(string str)
        {
            return new string(str.Where(c => char.IsDigit(c)).ToArray());
        }

        private static string ReturnMathSign(string str)
        {
            if (str.Contains("<")){ return "<"; }
            if (str.Contains(">")) { return ">"; }
            if (str.Contains("<=")) { return "<="; }
            if (str.Contains("<=")) { return "=>"; }
            if (str.Contains("=")) { return "="; }
            if (str.Contains("!=")) { return "!="; }
            return "=";
        }


        /// <summary>
        /// Small function for checking whether an "AND" should be applied to a string for a query
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        private static string PutAndIfNecessary(string origin, string current)
        {
            if(origin == current)
            {
                return current;
            }
            return current += " AND";
        }
        private static void ParameterInjection(SqlCommand command, CerealDTO cerealObject)
        {
            if(cerealObject.Name != null)
            {
                command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar);
                command.Parameters["@Name"].Value = cerealObject.Name;
            }
            if (cerealObject.Mfr != null)
            {
                command.Parameters.Add("@Mfr", System.Data.SqlDbType.VarChar);
                command.Parameters["@Mfr"].Value = cerealObject.Mfr;
            }
            if (cerealObject.Type != null)
            {
                command.Parameters.Add("@Type", System.Data.SqlDbType.VarChar);
                command.Parameters["@Type"].Value = cerealObject.Type;
            }
            if (cerealObject.Calories != null)
            {
                command.Parameters.Add("@Calories", System.Data.SqlDbType.Int);
                command.Parameters["@Calories"].Value = cerealObject.Calories;
            }
            if (cerealObject.Protein != null)
            {
                command.Parameters.Add("@Protein", System.Data.SqlDbType.Int);
                command.Parameters["@Protein"].Value = cerealObject.Protein;
            }
            if (cerealObject.Fat != null)
            {
                command.Parameters.Add("@Fat", System.Data.SqlDbType.Int);
                command.Parameters["@Fat"].Value = cerealObject.Fat;
            }
            if (cerealObject.Sodium != null)
            {
                command.Parameters.Add("@Sodium", System.Data.SqlDbType.Int);
                command.Parameters["@Sodium"].Value = cerealObject.Sodium;
            }
            if (cerealObject.Fiber != null)
            {
                command.Parameters.Add("@Fiber", System.Data.SqlDbType.Float);
                command.Parameters["@Fiber"].Value = cerealObject.Fiber;
            }
            if (cerealObject.Carbo != null)
            {
                command.Parameters.Add("@Carbo", System.Data.SqlDbType.Float);
                command.Parameters["@Carbo"].Value = cerealObject.Carbo;
            }
            if (cerealObject.Sugars != null)
            {
                command.Parameters.Add("@Sugars", System.Data.SqlDbType.Int);
                command.Parameters["@Sugars"].Value = cerealObject.Sugars;
            }
            if (cerealObject.Potass != null)
            {
                command.Parameters.Add("@Potass", System.Data.SqlDbType.Int);
                command.Parameters["@Potass"].Value = cerealObject.Potass;
            }
            if (cerealObject.Vitamins != null)
            {
                command.Parameters.Add("@Vitamins", System.Data.SqlDbType.Int);
                command.Parameters["@Vitamins"].Value = cerealObject.Vitamins;
            }
            if (cerealObject.Shelf != null)
            {
                command.Parameters.Add("@Shelf", System.Data.SqlDbType.Int);
                command.Parameters["@Shelf"].Value = cerealObject.Shelf;
            }
            if (cerealObject.Weight != null)
            {
                command.Parameters.Add("@Weight", System.Data.SqlDbType.Float);
                command.Parameters["@Weight"].Value = cerealObject.Weight;
            }
            if (cerealObject.Cups != null)
            {
                command.Parameters.Add("@Cups", System.Data.SqlDbType.Float);
                command.Parameters["@Cups"].Value = cerealObject.Cups;
            }
            if (cerealObject.Rating != null)
            {
                command.Parameters.Add("@Rating", System.Data.SqlDbType.Float);
                command.Parameters["@Rating"].Value = cerealObject.Rating;
            }
        }


        /// <summary>
        /// instantiate parameters for math function of get cereal
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cmo"></param>
        private static void ParameterInjectionForMath(SqlCommand command, CerealMathDTO cmo)
        {
            if (cmo.Name != null)
            {
                command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar);
                command.Parameters["@Name"].Value = cmo.Name;
            }
            if (cmo.Mfr != null)
            {
                command.Parameters.Add("@Mfr", System.Data.SqlDbType.VarChar);
                command.Parameters["@Mfr"].Value = cmo.Mfr;
            }
            if (cmo.Type != null)
            {
                command.Parameters.Add("@Type", System.Data.SqlDbType.VarChar);
                command.Parameters["@Type"].Value = cmo.Type;
            }


            if (cmo.Calories != null)
            {
                command.Parameters.Add("@Calories", System.Data.SqlDbType.Int);
                command.Parameters["@Calories"].Value = GetNumberFromString(cmo.Calories);
            }
            if (cmo.Protein != null)
            {
                command.Parameters.Add("@Protein", System.Data.SqlDbType.Int);
                command.Parameters["@Protein"].Value = GetNumberFromString(cmo.Protein);
            }
            if (cmo.Fat != null)
            {
                command.Parameters.Add("@Fat", System.Data.SqlDbType.Int);
                command.Parameters["@Fat"].Value = GetNumberFromString(cmo.Fat);
            }
            if (cmo.Sodium != null)
            {
                command.Parameters.Add("@Sodium", System.Data.SqlDbType.Int);
                command.Parameters["@Sodium"].Value = GetNumberFromString(cmo.Sodium);
            }
            if (cmo.Fiber != null)
            {
                command.Parameters.Add("@Fiber", System.Data.SqlDbType.Float);
                command.Parameters["@Fiber"].Value = GetNumberFromString(cmo.Fiber);
            }
            if (cmo.Carbo != null)
            {
                command.Parameters.Add("@Carbo", System.Data.SqlDbType.Float);
                command.Parameters["@Carbo"].Value = GetNumberFromString(cmo.Carbo);
            }
            if (cmo.Sugars != null)
            {
                command.Parameters.Add("@Sugars", System.Data.SqlDbType.Int);
                command.Parameters["@Sugars"].Value = GetNumberFromString(cmo.Sugars);
            }
            if (cmo.Potass != null)
            {
                command.Parameters.Add("@Potass", System.Data.SqlDbType.Int);
                command.Parameters["@Potass"].Value = GetNumberFromString(cmo.Potass);
            }
            if (cmo.Vitamins != null)
            {
                command.Parameters.Add("@Vitamins", System.Data.SqlDbType.Int);
                command.Parameters["@Vitamins"].Value = GetNumberFromString(cmo.Vitamins);
            }
            if (cmo.Shelf != null)
            {
                command.Parameters.Add("@Shelf", System.Data.SqlDbType.Int);
                command.Parameters["@Shelf"].Value = GetNumberFromString(cmo.Shelf);
            }
            if (cmo.Weight != null)
            {
                command.Parameters.Add("@Weight", System.Data.SqlDbType.Float);
                command.Parameters["@Weight"].Value = GetNumberFromString(cmo.Weight);
            }
            if (cmo.Cups != null)
            {
                command.Parameters.Add("@Cups", System.Data.SqlDbType.Float);
                command.Parameters["@Cups"].Value = GetNumberFromString(cmo.Cups);
            }
            if (cmo.Rating != null)
            {
                command.Parameters.Add("@Rating", System.Data.SqlDbType.Float);
                command.Parameters["@Rating"].Value = GetNumberFromString(cmo.Rating);
            }
        }


        /// <summary>
        /// Create a new cereal product in the database
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="projectName">Name of project</param>
        /// <returns>true for success - false for failure</returns>
        /// <exception cref="BadHttpRequestException"></exception>
        public static bool CreateCereal(SqlConnection conn, SqlTransaction transaction, CerealDTO newCereal)
        {
            //query for the database
            string queryString =
                "INSERT INTO Cereal (Name, Mfr, Type, Calories, Protein, Fat, Sodium, Fiber, Carbo, Sugars, Potass, " +
                "Vitamins, Shelf, Weight, Cups, Rating)" +
                " VALUES (@Name, @Mfr, @Type, @Calories, @Protein, @Fat, @Sodium, @Fiber, @Carbo, @Sugars, @Potass, " +
                "@Vitamins, @Shelf, @Weight, @Cups, @Rating);";

            bool state = false;

            //make procedure to execute query, using the queryString and the connection
            SqlCommand command = new SqlCommand(queryString, conn);
            command.Transaction = transaction;

            //Alter the query string safely to help prevent SQL injection
            //The id input at the start of the method is put in here. Keyword here is :Prepared Statement
            command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar);
            command.Parameters["@Name"].Value = newCereal.Name;

            command.Parameters.Add("@Mfr", System.Data.SqlDbType.NVarChar);
            command.Parameters["@Mfr"].Value = newCereal.Mfr;

            command.Parameters.Add("@Type", System.Data.SqlDbType.NVarChar);
            command.Parameters["@Type"].Value = newCereal.Type;

            command.Parameters.Add("@Calories", System.Data.SqlDbType.Int);
            command.Parameters["@Calories"].Value = newCereal.Calories;

            command.Parameters.Add("@Protein", System.Data.SqlDbType.Int);
            command.Parameters["@Protein"].Value = newCereal.Protein;

            command.Parameters.Add("@Fat", System.Data.SqlDbType.Int);
            command.Parameters["@Fat"].Value = newCereal.Fat;

            command.Parameters.Add("@Sodium", System.Data.SqlDbType.Int);
            command.Parameters["@Sodium"].Value = newCereal.Sodium;

            command.Parameters.Add("@Fiber", System.Data.SqlDbType.Float);
            command.Parameters["@Fiber"].Value = newCereal.Fiber;

            command.Parameters.Add("@Carbo", System.Data.SqlDbType.Float);
            command.Parameters["@Carbo"].Value = newCereal.Carbo;

            command.Parameters.Add("@Sugars", System.Data.SqlDbType.Int);
            command.Parameters["@Sugars"].Value = newCereal.Sugars;

            command.Parameters.Add("@Potass", System.Data.SqlDbType.Int);
            command.Parameters["@Potass"].Value = newCereal.Potass;

            command.Parameters.Add("@Vitamins", System.Data.SqlDbType.Int);
            command.Parameters["@Vitamins"].Value = newCereal.Vitamins;

            command.Parameters.Add("@Shelf", System.Data.SqlDbType.Int);
            command.Parameters["@Shelf"].Value = newCereal.Shelf;

            command.Parameters.Add("@Weight", System.Data.SqlDbType.Float);
            command.Parameters["@Weight"].Value = newCereal.Weight;

            command.Parameters.Add("@Cups", System.Data.SqlDbType.Float);
            command.Parameters["@Cups"].Value = newCereal.Cups;

            command.Parameters.Add("@Rating", System.Data.SqlDbType.Float);
            command.Parameters["@Rating"].Value = newCereal.Rating;


            int num = command.ExecuteNonQuery();
            if (num > 0) state = true;


            return state;
        }

        /// <summary>
        /// Update an existing cereal product in the database
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="disciplineId">Id of discipline to be updated</param>
        /// <param name="disciplineName">The new name</param>
        /// <returns>true for success - false for failure</returns>
        /// <exception cref="BadHttpRequestException"></exception>
        public static bool UpdateCereal(SqlConnection conn, SqlTransaction transaction, CerealDTO cereal, int cerealId)
        {
            //query for the database
            string queryString =
                "Update Cereal " +
                "SET Name = @Name, Mfr = @Mfr, Type = @Type, Calories = @Calories, Protein = @Protein, Fat = @Fat, " +
                "Sodium = @Sodium, Fiber = @Fiber, Carbo = @Carbo, Sugars = @Sugars, Potass = @Potass, Vitamins = @Vitamins, " +
                "Shelf = @Shelf, Weight = @Weight, Cups = @Cups, Rating = @Rating " +
                "WHERE Id = @Id;";

            //return error for non existing id
            if (GetCereal(conn, transaction, cerealId) == null) 
            { throw new BadHttpRequestException("Id does not exist. Cannot update."); }

            bool state = false;

            //make procedure to execute query, using the queryString and the connection
            SqlCommand command = new SqlCommand(queryString, conn);
            command.Transaction = transaction;

            //Alter the query string safely to help prevent SQL injection
            //Prepared Statement
            command.Parameters.Add("@Id", System.Data.SqlDbType.Int);
            command.Parameters["@Id"].Value = cerealId;

            command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar);
            command.Parameters["@Name"].Value = cereal.Name;

            command.Parameters.Add("@Mfr", System.Data.SqlDbType.NVarChar);
            command.Parameters["@Mfr"].Value = cereal.Mfr;

            command.Parameters.Add("@Type", System.Data.SqlDbType.NVarChar);
            command.Parameters["@Type"].Value = cereal.Type;

            command.Parameters.Add("@Calories", System.Data.SqlDbType.Int);
            command.Parameters["@Calories"].Value = cereal.Calories;

            command.Parameters.Add("@Protein", System.Data.SqlDbType.Int);
            command.Parameters["@Protein"].Value = cereal.Protein;

            command.Parameters.Add("@Fat", System.Data.SqlDbType.Int);
            command.Parameters["@Fat"].Value = cereal.Fat;

            command.Parameters.Add("@Sodium", System.Data.SqlDbType.Int);
            command.Parameters["@Sodium"].Value = cereal.Sodium;

            command.Parameters.Add("@Fiber", System.Data.SqlDbType.Float);
            command.Parameters["@Fiber"].Value = cereal.Fiber;

            command.Parameters.Add("@Carbo", System.Data.SqlDbType.Float);
            command.Parameters["@Carbo"].Value = cereal.Carbo;

            command.Parameters.Add("@Sugars", System.Data.SqlDbType.Int);
            command.Parameters["@Sugars"].Value = cereal.Sugars;

            command.Parameters.Add("@Potass", System.Data.SqlDbType.Int);
            command.Parameters["@Potass"].Value = cereal.Potass;

            command.Parameters.Add("@Vitamins", System.Data.SqlDbType.Int);
            command.Parameters["@Vitamins"].Value = cereal.Vitamins;

            command.Parameters.Add("@Shelf", System.Data.SqlDbType.Float);
            command.Parameters["@Shelf"].Value = cereal.Shelf;

            command.Parameters.Add("@Weight", System.Data.SqlDbType.Float);
            command.Parameters["@Weight"].Value = cereal.Weight;

            command.Parameters.Add("@Cups", System.Data.SqlDbType.Float);
            command.Parameters["@Cups"].Value = cereal.Cups;

            command.Parameters.Add("@Rating", System.Data.SqlDbType.Float);
            command.Parameters["@Rating"].Value = cereal.Rating;

            int num = command.ExecuteNonQuery();
            if (num > 0) state = true;


            return state;
        }

        /// <summary>
        /// Delete a cereal product from the database
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="projectId">Id of cereal product to be deleted</param>
        /// <returns>true for success - false for failure</returns>
        /// <exception cref="BadHttpRequestException"></exception>
        public static bool DeleteCereal(SqlConnection conn, SqlTransaction transaction, int cerealId)
        {
            //query for the database
            string queryString =
                "DELETE FROM dbo.Cereal " +
                "WHERE Id = @Id;";

            bool state = false;

            //make procedure to execute query, using the queryString and the connection
            SqlCommand command = new SqlCommand(queryString, conn);
            command.Transaction = transaction;

            //Alter the query string safely to help prevent SQL injection
            //Prepared Statement
            command.Parameters.Add("@Id", System.Data.SqlDbType.Int);
            command.Parameters["@Id"].Value = cerealId;


            int num = command.ExecuteNonQuery();
            if (num > 0) state = true;


            return state;
        }


        /// <summary>
        /// Get a single User from the database
        /// </summary>
        /// <param name="connectionString">The connection string for the database</param>
        /// <param name="userId">The id of the cereal</param>
        /// <returns>Id, name and password of User</returns>
        public static UserDTO GetUser(SqlConnection conn, SqlTransaction transaction, string userName, string userPassword)
        {
            //query for the database
            string queryString = "SELECT * FROM dbo.Account WHERE Username = @Name AND Password = @Password;";

            //Object that will be returned
            UserDTO returnObject = new UserDTO("", "");

            //make procedure to execute query, using the queryString and the connection
            SqlCommand command = new SqlCommand(queryString, conn);
            command.Transaction = transaction;

            //Alter the query string safely to help prevent SQL injection
            //The id input at the start of the method is put in here. Keyword here is :Prepared Statement
            command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar);
            command.Parameters["@Name"].Value = userName;

            command.Parameters.Add("@Password", System.Data.SqlDbType.VarChar);
            command.Parameters["@Password"].Value = userPassword;


            //read the data from the database using the datareader
            using (SqlDataReader reader = command.ExecuteReader())
            {
                //as long as data continues to be streamed(for a single instance), read and parse data into objects
                while (reader.Read())
                {
                    int Id = (int)reader[0];
                    string Name = (string)reader[1];
                    string Password = (string)reader[2];
                    //add object to list
                    returnObject = new UserDTO(Name, Password);
                }
            }
            return returnObject;
        }
    }
}
