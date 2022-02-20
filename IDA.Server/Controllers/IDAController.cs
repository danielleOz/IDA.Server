
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IDA.Server.DTO;

//Add the below
using IDA.ServerBL.Models;
using System.IO;

namespace IDA.Server.Controllers
{
    [Route("IDAAPI")]
    [ApiController]
    public class IDAController : ControllerBase
    {
        IDADBContext context;
        public IDAController(IDADBContext context)
        {
            this.context = context;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pass"></param>
        /// <returns></returns>

        #region Login
        [Route("Login")]
        [HttpGet]
        public User Login([FromQuery] string email, [FromQuery] string pass)
        {
            User user = context.Login(email, pass);
            
            //Check user name and password
            if (user != null)
            {
                Worker w = context.Workers.Where(w => w.Id == user.Id).FirstOrDefault();
                if (w == null)
                    HttpContext.Session.SetObject("theUser", user);
                else
                {
                    WorkerDto worker = new WorkerDto()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserPswd = user.UserPswd,
                        City = user.City,
                        Street = user.Street,
                        Apartment = user.Apartment,
                        HouseNumber = user.HouseNumber,
                        Birthday = user.Birthday,
                        IsWorker = user.IsWorker,
                        IsAvailbleUntil = w.IsAvailbleUntil,
                        RadiusKm = w.RadiusKm
                    };

                    HttpContext.Session.SetObject("theUser", worker);
                    user = worker;
                }

                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                //Important! Due to the Lazy Loading, the user will be returned with all of its contects!!
                return user;
            }
            else
            {

                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
        }
        #endregion


        #region GetServices
        [Route("GetServices")]
        [HttpGet]
        public List<Service> GetServices()
        {
            return context.Services.ToList();
        }
        #endregion


        #region WorkerRegister
        [Route("WorkerRegister")]
        [HttpPost]
        public WorkerDto WorkerRegister([FromBody] WorkerDto w)
        {
            if (w != null)
            {
                this.context.WorkerRegister(w);

                HttpContext.Session.SetObject("theUser", w);
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return w;
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }

        }
        #endregion


        #region CustomerRegister
        [Route("UserRegister")]
        [HttpPost]
        public User CustomerRegister([FromBody] User u)
        {
            if (u != null)
            {
                try
                {
                    u = this.context.UserRegistration(u);
                    HttpContext.Session.SetObject("theUser", u);
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return u;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    return null;
                }
               
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }

        }
        #endregion


        #region IsUserNameExist
        [Route("IsEmailExist")]
        [HttpGet]
        public bool IsUserNameExist([FromQuery] string email)
        {
            bool isExist = this.context.EmailExist(email);
            if (isExist)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return true;
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return false;
            }
        }
        #endregion

        #region WorkerAvailbilty

        [Route("WorkerAvailbilty")]
        [HttpPost]
        public bool WorkerAvailbilty([FromBody] Worker worker)//..
        {
            //If user is null the request is bad
            if (worker == null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return false;
            }

            User currentWorker = HttpContext.Session.GetObject<User>("theUser");
            //Check if user logged in and its ID is the same as the contact user ID
            if (currentWorker != null && currentWorker.Id == worker.Id)
            {
               bool success = context.AvailbleWorker(worker);

                if (!success)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    return false;
                }

                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return true;

            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return false;
            }
        }
        #endregion

        

    }

}