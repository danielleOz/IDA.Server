using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDA.ServerBL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IDA.ServerBL.Models
{
    public partial class IDADBContext : DbContext
    {
        #region Login
        public User Login(string userName, string pswd)
        {
            return this.Users.Where(u => u.UserName == userName && u.UserPswd == pswd).FirstOrDefault();

        }
        #endregion

        #region WorkerRegister
        public Worker WorkerRegister(Worker w)
        {
            try
            {
                this.Entry(w.UserNameNavigation).State = EntityState.Added;
                this.Entry(w).State = EntityState.Added;
                foreach (WorkerService ws in w.WorkerServices)
                    this.Entry(ws).State = EntityState.Added;
                //this.Workers.Add(w);
                this.SaveChanges();
                return w;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }
        #endregion


        #region CustomerRegister
        public Customer CustomerRegister (Customer c)
        {
            try
            {
                c.UserName = c.UserNameNavigation.UserName;
                this.Customers.Add(c);
                this.SaveChanges();
                return c;
            }
            
            
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }
        #endregion


        #region UserNameExist
        public bool UserNameExist(string userName)
        {
            try
            {
                return this.Users.Any(u => u.UserName == userName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return true;
            }
        }
        #endregion

        #region AvailbleWorker

        public bool AvailbleWorker(Worker w)
        {
            try
            {
                Worker currentWorker = this.Workers
                .Where( worker => worker.UserName == w.UserName).FirstOrDefault();

                currentWorker.Availble = w.Availble;

                this.SaveChanges();//..
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        #endregion


        //public User UpdateUser(User user, User updatedUser)
        //{
        //    try
        //    {
        //        User currentUser = this.Users
        //        .Where(u => u.Id == user.Id).FirstOrDefault();

        //        currentUser.FirstName = updatedUser.FirstName;
        //        currentUser.LastName = updatedUser.LastName;
        //        currentUser.UserPswd = updatedUser.UserPswd;
        //        currentUser.BirthDate = updatedUser.BirthDate;
        //        currentUser.PhoneNum = updatedUser.PhoneNum;
        //        currentUser.City = updatedUser.City;
        //        currentUser.Street = updatedUser.Street;
        //        currentUser.HouseNum = updatedUser.HouseNum;

        //        this.SaveChanges();
        //        return currentUser;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return null;
        //    }
        //}
    }
}
