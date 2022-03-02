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
                return this.Users.Where(u => u.Email == email && u.UserPswd == pswd).FirstOrDefault();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }
        #endregion

        #region WorkerRegister
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


        #region CustomerRegister
        public User UserRegistration (User u)
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


        #region EmailExist
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
