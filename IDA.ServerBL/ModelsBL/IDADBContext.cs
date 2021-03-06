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
                    .Include(u => u.JobOffers).ThenInclude(j => j.User).Include(u => u.JobOffers).ThenInclude(j => j.Service).Include(f=> f.Worker).ThenInclude(g=> g.WorkerServices)
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

        #region user Register
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
        public User UpdateUser( User updatedUser, User user)
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

        #region worker User
        public Worker UpdateWorker(Worker w, Worker updatedWorker)
        {
            try
            {

                w.IdNavigation.FirstName = updatedWorker.IdNavigation.FirstName;
                w.IdNavigation.LastName = updatedWorker.IdNavigation.LastName;
                w.IdNavigation.UserPswd = updatedWorker.IdNavigation.UserPswd;
                w.IdNavigation.Birthday = updatedWorker.IdNavigation.Birthday;
                w.IdNavigation.Apartment = updatedWorker.IdNavigation.Apartment;
                w.IdNavigation.City = updatedWorker.IdNavigation.City;
                w.IdNavigation.Street = updatedWorker.IdNavigation.Street;
                w.IdNavigation.HouseNumber = updatedWorker.IdNavigation.HouseNumber;
                w.RadiusKm = updatedWorker.RadiusKm;
                w.WorkerServices = updatedWorker.WorkerServices;


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
