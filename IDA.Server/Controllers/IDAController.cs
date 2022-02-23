
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
                User u = new User
                {
                    Id = w.Id,
                    Email = w.Email,
                    FirstName = w.FirstName,
                    LastName = w.LastName,
                    UserPswd = w.UserPswd,
                    City = w.City,
                    Street = w.Street,
                    Apartment = w.Apartment,
                    HouseNumber = w.HouseNumber,
                    Birthday = w.Birthday,
                    IsWorker = w.IsWorker,
                };
                Worker worker = new Worker
                {
                    Id = w.Id,
                    IsAvailbleUntil = w.IsAvailbleUntil,
                    RadiusKm = w.RadiusKm,
                    IdNavigation = u
                };
                this.context.WorkerRegister(worker);

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
        public User UserRegister([FromBody] User u)
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
        public bool IsEmailExist([FromQuery] string email)
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

        [Route("UpdateWorkerAvailbilty")]
        [HttpPost]
        public bool UpdateWorkerAvailbilty([FromBody] DateTime availablity)//..
        {
            
            WorkerDto currentWorker = HttpContext.Session.GetObject<WorkerDto>("theUser");
            //Check if user logged in and its ID is the same as the contact user ID
            if (currentWorker != null)
            {
               
               bool success = context.AvailbleWorker(currentWorker.Id, availablity);

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