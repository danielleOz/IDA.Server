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
        public User Login(string email, string pswd)
        {
            try
            {
                return this.Users.Where(u => u.Email == email && u.UserPswd == pswd)
                    .Include(u => u.JobOffers).ThenInclude(j => j.User)
                    .Include(u => u.ChatMessageRecievers)
                    .Include(u=> u.ChatMessageSenders).FirstOrDefault();
            }
             
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }
        #endregion


        #region Worker Register
        public Worker WorkerRegister(Worker w)
        {
            try
            {
                this.Entry(w.IdNavigation).State = EntityState.Added;
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


        #region Customer Register
        public User UserRegistration(User u)
        {
            try
            {
                this.Users.Add(u);
                this.SaveChanges();
                return u;
            }


            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }
        #endregion


        #region  Is Email Exist
        public bool EmailExist(string email)
        {
            try
            {
                return this.Users.Any(u => u.Email == email);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return true;
            }
        }
        #endregion


        #region Update User
        public User UpdateUser(User user, User updatedUser)
        {
            try
            {
                User currentUser = this.Users
                .Where(u => u.Id == user.Id).FirstOrDefault();

                currentUser.FirstName = updatedUser.FirstName;
                currentUser.LastName = updatedUser.LastName;
                currentUser.UserPswd = updatedUser.UserPswd;
                currentUser.Birthday = updatedUser.Birthday;
                currentUser.Apartment = updatedUser.Apartment;
                currentUser.City = updatedUser.City;
                currentUser.Street = updatedUser.Street;
                currentUser.HouseNumber = updatedUser.HouseNumber;


                this.SaveChanges();
                return currentUser;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        #endregion

        //#region Update Worker
        //public Worker UpdateWorker(Worker worker, Worker updatedWorker)
        //{
        //    try
        //    {
        //        worker curruntWorker = this.Workers
        //        .Where(w => w.Id == worker.Id).FirstOrDefault();

        //        currentUser.FirstName = updatedUser.FirstName;
        //        currentUser.LastName = updatedUser.LastName;
        //        currentUser.UserPswd = updatedUser.UserPswd;
        //        currentUser.Birthday = updatedUser.Birthday;
        //        currentUser.Apartment = updatedUser.Apartment;
        //        currentUser.City = updatedUser.City;
        //        currentUser.Street = updatedUser.Street;
        //        currentUser.HouseNumber = updatedUser.HouseNumber;


        //        this.SaveChanges();
        //        return currentUser;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return null;
        //    }
        //}
        //#endregion

        #region job offer
        public JobOffer JobOffer(JobOffer j)
        {
            try
            {
                this.JobOffers.Add(j);
                this.SaveChanges();
                return j;
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
