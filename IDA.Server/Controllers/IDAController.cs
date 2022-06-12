
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IDA.Server.DTO;
using IDA.Server.Helper;
//Add the below
using IDA.ServerBL.Models;
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata;
using SendGridLib;

namespace IDA.Server.Controllers
{
    [Route("IDAAPI")]
    [ApiController]
    public class IDAController : ControllerBase
    {
        IDADBContext context;
        public IDAController(IDADBContext context)
        {
            try
            {
                this.context = context;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


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
                        .Include(u => u.JobOffers).ThenInclude(u=> u.Service)
                        .Include(u => u.WorkerServices).Include(u=>u.JobOffers).ThenInclude(j=>j.User.JobOffers).Include(u => u.JobOffers).ThenInclude(u => u.ChosenWorker).FirstOrDefault();
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
                        .Include(u => u.JobOffers).ThenInclude(j=>j.Service)
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
                    JobOffers = w.JobOffers,
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
        public List<JobOfferDto> GetAllWR()
        {
            try
            {
                User user = HttpContext.Session.GetObject<User>("theUser");
                //Check if user logged in and its ID is the same as the contact user ID
                if (user != null && user.IsWorker)
                {
                    List<JobOffer> jobOffers;
                    List<JobOfferDto> jobOffersDto = new List<JobOfferDto>();
                    jobOffers = context.JobOffers.Include(j => j.Service).Include(j => j.Status).Include(j => j.User).Include(j => j.ChosenWorker.IdNavigation).ToList();
                    foreach (JobOffer j in jobOffers)
                    {
                        jobOffersDto.Add(new JobOfferDto(j));
                    }
                    return jobOffersDto;
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


        #region Worker Update 
        [Route("WorkerUpdate")]
        [HttpPost]
        public bool WorkerUpdate([FromBody] WorkerDto w)
        {
            try
            {
                if (w == null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    return false;
                }

                WorkerDto currentWorker = HttpContext.Session.GetObject<WorkerDto>("theUser");

                Worker current = context.Workers.Where(cw => cw.Id == currentWorker.Id).Include(w => w.IdNavigation).Include(w=>w.WorkerServices).ThenInclude(ws=>ws.Service).FirstOrDefault();
                if (current != null)
                {

                    current.RadiusKm = w.RadiusKm;
                   // current.WorkerServices = w.WorkerServices;
                    current.IdNavigation.Email = w.Email;
                    current.IdNavigation.FirstName = w.FirstName;
                    current.IdNavigation.LastName = w.LastName;
                    current.IdNavigation.Birthday = w.Birthday;
                    current.IdNavigation.Apartment = w.Apartment;
                    current.IdNavigation.City = w.City;
                    current.IdNavigation.HouseNumber = w.HouseNumber;
                    current.IdNavigation.Street = w.Street;
                    
                    //הוספת השירותים החדשים
                    foreach(WorkerService s in w.WorkerServices)
                    {
                        bool found = false;
                        foreach(WorkerService cs in current.WorkerServices)
                        {
                            if (cs.Service.Id == s.Service.Id)
                            {
                                found = true;
                                context.Entry(cs).State = EntityState.Unchanged;
                            }
                        }
                        if (!found)
                        {
                            current.WorkerServices.Add(s);
                            context.Entry(s).State = EntityState.Added;
                        }

                    }
                    //מחיקת השירותים שעודכנו
                    foreach (WorkerService s in current.WorkerServices)
                    {
                        bool found = false;
                        foreach (WorkerService cs in w.WorkerServices)
                        {
                            if (cs.Service.Id == s.Service.Id)
                            {
                                found = true;
                            }
                        }
                        if(!found)
                        {
                            current.WorkerServices.Remove(s);
                            context.Entry(s).State = EntityState.Deleted;
                        }
                    }
                        context.SaveChanges();

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
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return false;
            }


        }

        #endregion


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
                User updatedUser = context.UpdateUser(user, currentUser);

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


        #region get availble workers 

        [Route("GetAvailableWorkrs")]
        [HttpGet]
        public List<WorkerDto> GetAvailableWorkrs()
        {
            try
            {
                User user = HttpContext.Session.GetObject<User>("theUser");
                //Check if user logged in 
                if (user != null)
                {
                    List<Worker> workers = context.Workers.Where(w => w.AvailbleUntil >= DateTime.Now)
                        .Include(w => w.IdNavigation)
                        .Include(w => w.WorkerServices)
                        .ThenInclude(s => s.Service).ToList();
                    List<WorkerDto> workersDto = new List<WorkerDto>();
                    foreach (Worker w in workers)
                        workersDto.Add(new WorkerDto(w));
                    return workersDto;
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

        #region send email 

        [Route("SendMail")]
        [HttpPost]
        public async Task <bool> SendMail(WorkerDto w)
        {
            try
            {
                User user = HttpContext.Session.GetObject<User>("theUser");
                //Check if user logged in 
                if (user != null)
                {
                    Worker current = context.Workers.Where(cw => cw.Id == w.Id).Include(w => w.IdNavigation).FirstOrDefault();
                    string FName = user.FirstName;
                    string LName = user.LastName;
                    string WEmail = current.IdNavigation.Email;
                    string WFName = current.IdNavigation.FirstName;
                    string WLName = current.IdNavigation.LastName;

                    string subject = "New Job Offer";
                    string body = "Hi i would like to schedule Job with you ";
                    var from = FName + " " + LName;
                    var to = WEmail;
                    var toName = WFName + " " + WLName;
                    string html;
                    bool success = await MailSender.SendEmail(from, to, toName, subject, body, "");
                    return success;
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


        #region Job Offer
        [Route("JobOffer")]
        [HttpPost]
        public JobOfferDto JobOffer([FromBody] JobOfferDto j)
        {
            try
            {
                if (j != null && j.Id <= 0)
                {
                    JobOffer Joboffer = new JobOffer
                    {
                        UserId = j.UserId,
                        ChosenWorkerId = j.ChosenWorkerId,
                        Description = j.Description,
                        PublishDate = j.PublishDate,
                        ServiceId = j.ServiceId,
                        StatusId = j.StatusId,
                        WorkerReviewDate = j.WorkerReviewDate,
                        WorkerReviewRate = j.WorkerReviewRate,
                        WorkerReviewDescriptipon = j.WorkerReviewDescriptipon
                    };
                    this.context.JobOffer(Joboffer);

                }
                else if (j.Id > 0)
                {
                    JobOffer offer = context.JobOffers.Include(jo => jo.Service).Include(jo => jo.User).Include(jo => jo.ChosenWorker).Where(jo => jo.Id == j.Id).FirstOrDefault();
                    offer.WorkerReviewDate = j.WorkerReviewDate;
                    offer.WorkerReviewDescriptipon = j.WorkerReviewDescriptipon;
                    offer.WorkerReviewRate = j.WorkerReviewRate;
                    context.Entry(offer).State = EntityState.Modified;
                    context.SaveChanges();
                    
                    j.Service = offer.Service;
                    j.User = offer.User;

                }
                else
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return null;
                }


                HttpContext.Session.SetObject("theJobOffer", j);
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return j;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }
        #endregion

        #region get users joboffers

        [Route("GetUserJobOffer")]
        [HttpGet]
        public List<JobOfferDto> GetUserJobOffer([FromQuery] int id)
        {
            try
            {
                User user = HttpContext.Session.GetObject<User>("theUser");
                //Check if user logged in and its ID is the same as the contact user ID
                if (user != null)
                {
                   User u= context.Users.Where(u => u.Id==id)
                    .Include(u => u.JobOffers).ThenInclude(j => j.User).Include(u => u.JobOffers).ThenInclude(j => j.Service).Include(j=>j.JobOffers).ThenInclude(j=>j.ChosenWorker).Include(u=>u.JobOffers).ThenInclude(j=>j.User).Include(j => j.JobOffers).ThenInclude(j=>j.ChosenWorker.JobOffers).FirstOrDefault();
                  
                    List<JobOfferDto> jobOffersDto = new List<JobOfferDto>();
                  
                    foreach (JobOffer j in u.JobOffers)
                    {
                        jobOffersDto.Add(new JobOfferDto(j));
                    }
                    return jobOffersDto;
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
    }

}