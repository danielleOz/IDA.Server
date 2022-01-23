
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
        [Route("Login")]
        [HttpGet]
        public User Login([FromQuery] string userName, [FromQuery] string pass)
        {
            User user = context.Login(userName, pass);

            //Check user name and password
            if (user != null)
            {
                HttpContext.Session.SetObject("theUser", user);

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


        [Route("GetServices")]
        [HttpGet]
        public List<Service> GetServices()
        {
            return context.Services.ToList();
        }


        [Route("WorkerRegister")]
        [HttpPost]
        public Worker WorkerRegister([FromBody] Worker w)
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


        [Route("CustomerRegister")]
        [HttpPost]
        public Customer CustomerRegister([FromBody] Customer c)
        {
            if (c != null)
            {
                try
                {
                    c = this.context.CustomerRegister(c);
                    HttpContext.Session.SetObject("theUser", c);
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return c;
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

    }
}