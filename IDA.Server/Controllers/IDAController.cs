
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
            try
            {

                User user = context.Login(email, pass);

                //Check user name and password
                if (user != null)
                {
                    Worker w = context.Workers.Where(w => w.Id == user.Id)
                        .Include(u => u.JobOffers)
                        .Include(u => u.WorkerServices)
                        .FirstOrDefault();
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
                            //IsAvailble = w.IsAvailble,
                            AvailbleUntil = w.AvailbleUntil,
                            RadiusKm = w.RadiusKm,
                            WorkerJobOffers = w.JobOffers,
                            WorkerServices = w.WorkerServices
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        #endregion


        #region Get Services
        [Route("GetServices")]
        [HttpGet]
        public List<Service> GetServices()
        {
            return context.Services.ToList();
        }
        #endregion

        #region Get Worker
        [Route("GetWorker")]
        [HttpGet]
        public WorkerDto GetWorker([FromQuery] int workerId)
        {
            Worker w = context.Workers.Where(w => w.Id == workerId)
                        .Include(u => u.IdNavigation)
                        .Include(u => u.JobOffers)
                        .Include(u => u.WorkerServices)
                        .FirstOrDefault();
            if (w == null)
                return null;
            else
            {
                WorkerDto worker = new WorkerDto()
                {
                    Id = w.IdNavigation.Id,
                    Email = w.IdNavigation.Email,
                    FirstName = w.IdNavigation.FirstName,
                    LastName = w.IdNavigation.LastName,
                    UserPswd = w.IdNavigation.UserPswd,
                    City = w.IdNavigation.City,
                    Street = w.IdNavigation.Street,
                    Apartment = w.IdNavigation.Apartment,
                    HouseNumber = w.IdNavigation.HouseNumber,
                    Birthday = w.IdNavigation.Birthday,
                    IsWorker = w.IdNavigation.IsWorker,
                    //IsAvailble = w.IsAvailble,
                    AvailbleUntil = w.AvailbleUntil,
                    RadiusKm = w.RadiusKm,
                    WorkerJobOffers = w.JobOffers,
                    WorkerServices = w.WorkerServices
                };
                return worker;
            }
        }
        #endregion

        #region Worker Register
        [Route("WorkerRegister")]
        [HttpPost]
        public WorkerDto WorkerRegister([FromBody] WorkerDto w)
        {
            try
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
                        AvailbleUntil = DateTime.MinValue,
                        RadiusKm = w.RadiusKm,
                        IdNavigation = u,
                        WorkerServices = w.WorkerServices

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

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }
        #endregion


        #region User Register
        [Route("UserRegister")]
        [HttpPost]
        public User UserRegister([FromBody] User u)
        {
            try
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }

        #endregion


        #region Is Email Exist
        [Route("IsEmailExist")]
        [HttpGet]
        public bool IsEmailExist([FromQuery] string email)
        {
            try
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

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }


        }

        #endregion


        #region Worker Availbilty

        [Route("UpdateWorkerAvailbilty")]
        [HttpPost]
        public bool UpdateWorkerAvailbilty([FromBody] DateTime d)//..
        {
            try
            {
                WorkerDto currentWorker = HttpContext.Session.GetObject<WorkerDto>("theUser");
                //Check if user logged in and its ID is the same as the contact user ID
                if (currentWorker != null)
                {
                    Worker current = context.Workers.Where(cw => cw.Id == currentWorker.Id).FirstOrDefault();
                    if (current != null)
                    {
                        current.AvailbleUntil = d;
                        currentWorker.AvailbleUntil = d;
                        context.SaveChanges();
                        Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                        //Save worker back to the cookie
                        HttpContext.Session.SetObject("theUser", currentWorker);
                        return true;

                    }
                    else
                    {
                        Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                        return false;
                    }
                }
                else
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }
        #endregion


        #region get workers reviews

        [Route("GetAllWR")]
        [HttpGet]
        public List<JobOffer> GetAllWR()
        {
            try
            {
                User user = HttpContext.Session.GetObject<User>("theUser");
                //Check if user logged in and its ID is the same as the contact user ID
                if (user != null && user.IsWorker)
                {
                    return context.JobOffers.ToList();
                }
                else
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }


        #endregion


        //#region Worker Update 
        //[Route("WorkerUpdate")]
        //[HttpPost]
        //public Worker WorkerUpdate([FromBody] WorkerDto w)
        //{
        //    try
        //    {
        //        if (w != null)
        //        {


        //         WorkerDto NewWorker = HttpContext.Session.GetObject<WorkerDto>("theWorker");
        //        Worker currentWorker =

        //        //Check if user logged in and its ID is the same as the contact user ID
        //        if (currentWorker != null && currentWorker.Id == w.Id)
        //        {
        //            Worker updatedWorker = context.UpdateWorker(currentWorker, w);

        //            if (updatedWorker == null)
        //            {
        //                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
        //                return null;
        //            }

        //            Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
        //            return updatedWorker;

        //        }
        //        else
        //        {
        //            Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
        //            return null;
        //        }
        //    }


        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return null;
        //    }


        //}


        //#endregion


        #region User Update
        [Route("UpdateUser")]
        [HttpPost]
        public User UpdateUser([FromBody] User user)
        {

            if (user == null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return null;
            }

            User currentUser = HttpContext.Session.GetObject<User>("theUser");

            //Check if user logged in and its ID is the same as the contact user ID
            if (currentUser != null && currentUser.Id == user.Id)
            {
                User updatedUser = context.UpdateUser(currentUser, user);

                if (updatedUser == null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return null;
                }

                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return updatedUser;

            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
        }
        #endregion


    }

}