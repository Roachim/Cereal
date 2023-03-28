using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using CerealAPI.DTO;
using CerealAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace CerealAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class CerealController : Controller
    {
        //string sqlString = Environment.GetEnvironmentVariable("connection string");
        string sqlString = "Server=localhost;Database=master;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;";

        // GET: CerealController
        [HttpGet]
        public IEnumerable<CerealDTO> GetAllCrereals()
        {
            IEnumerable<CerealDTO>? returnValue = null;
           
            using (SqlConnection conn = new SqlConnection(sqlString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    returnValue = UserService.GetAllCereal(conn, transaction);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    conn.Close();
                    throw new BadHttpRequestException("Error: " + ex.Message, 500);
                }
                transaction.Commit();
                conn.Close();
            }
            return returnValue;
        }

        /// GET: api/Projects/5
        [HttpGet]
        [Route("{cerealId}")]
        public CerealDTO GetCereal(int cerealId)
        {
            CerealDTO returnValue = null;
            using (SqlConnection conn = new SqlConnection(sqlString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    returnValue = UserService.GetCereal(conn, transaction, cerealId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    conn.Close();
                    throw new BadHttpRequestException("Error: " + ex.Message, 500);
                }
                transaction.Commit();
                conn.Close();
            }
            return returnValue;
        }

        [HttpGet]
        [Route("Picture/{cerealId}")]
        public CerealPicturesDTO GetCerealPicture(int cerealId)
        {
            CerealPicturesDTO returnValue = null;
            using (SqlConnection conn = new SqlConnection(sqlString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    returnValue = UserService.GetCerealPicture(conn, transaction, cerealId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    conn.Close();
                    throw new BadHttpRequestException("Error: " + ex.Message, 400);
                }
                transaction.Commit();
                conn.Close();
            }
            return returnValue;
        }

        // POST: api/CerealDTO
        [HttpPost] //wait this should be post because of body of data, why did get work?
        [Authorize]
        [Route("category")]
        public IEnumerable<CerealDTO> GetCertainCerealUsingParameter([FromBody] CerealDTO cereal)
        {
            IEnumerable<CerealDTO>? returnValue = null;
            using (SqlConnection conn = new SqlConnection(sqlString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    returnValue = UserService.GetAllCerealWithCertainCategoryValue(conn, transaction, cereal);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    conn.Close();
                    throw new BadHttpRequestException("Error: " + ex.Message, 400);
                }
                transaction.Commit();
                conn.Close();
            }
            return returnValue;
        }


        [HttpPost]//wait this should be post because of body of data, why did get work?
        [Authorize]
        [Route("math")]
        public IEnumerable<CerealDTO> GetCertainCerealUsingMath([FromBody] CerealMathDTO cereal)
        {
            IEnumerable<CerealDTO>? returnValue = null;
            using (SqlConnection conn = new SqlConnection(sqlString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    returnValue = UserService.GetAllCerealWithWithRestrictions(conn, transaction, cereal);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    conn.Close();
                    throw new BadHttpRequestException("Error: " + ex.Message, 500);
                }
                transaction.Commit();
                conn.Close();
            }
            return returnValue;
        }

        // POST: CerealController/Create
        [HttpPost]
        [Authorize]
        public bool CreateCereal([FromBody] CerealDTO cereal)
        {
            bool returnValue = false;
            using (SqlConnection conn = new SqlConnection(sqlString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    returnValue = UserService.CreateCereal(conn, transaction, cereal);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    conn.Close();
                    throw new BadHttpRequestException("Error: " + ex.Message, 400);
                }
                transaction.Commit();
                conn.Close();
            }
            return returnValue;
        }

        // Put: CerealController/Edit/5
        [HttpPost]
        [Authorize]
        [Route("{cerealId}")]
        public bool UpdateCereal(int cerealId, [FromBody] CerealDTO cereal)
        {
            bool returnValue = false;
            using (SqlConnection conn = new SqlConnection(sqlString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    returnValue = UserService.UpdateCereal(conn, transaction, cereal, cerealId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    conn.Close();
                    throw new BadHttpRequestException("Error: " + ex.Message, 400);
                }
                transaction.Commit();
                conn.Close();
            }
            return returnValue;
        }

        // POST: CerealController/Edit/5


        // GET: CerealController/Delete/5


        // DELETE: CerealController/Delete/5
        [HttpDelete]
        [Authorize]
        [Route("{cerealId}")]
        public bool DeleteCereal(int cerealId)
        {
            bool returnValue = false;
            using (SqlConnection conn = new SqlConnection(sqlString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    returnValue = UserService.DeleteCereal(conn, transaction, cerealId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    conn.Close();
                    throw new BadHttpRequestException("Error: " + ex.Message, 400);
                }
                transaction.Commit();
                conn.Close();
            }
            return returnValue;
        }
    }
}
