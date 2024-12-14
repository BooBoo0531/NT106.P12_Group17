using Snake_Royale_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_Royale_Server.Repository
{
    public class AccountRepository
    {   
        public AccountRepository()
        {

        }

        public Account Get(string username)
        {
            using (var context = new SnakeRoyaleDbContext())
            {
                var account = context.Accounts.FirstOrDefault(a => a.Username == username);
                if(account != null)
                {
                    return account;
                }
            }
            return null;
        }

        public List<Account> GetAll()
        {
            using (var context = new SnakeRoyaleDbContext())
            {
                return context.Accounts.ToList();
            }
        }

        public void Add(Account account)
        {
            using (var context = new SnakeRoyaleDbContext())
            {
                var existUsername = context.Accounts.FirstOrDefault(a => a.Username == account.Username);
                if(existUsername != null)
                {
                    return;
                }
                context.Accounts.Add(account);
                context.SaveChanges();
            }
        }

        public void Update(Account account)
        {
            using (var context = new SnakeRoyaleDbContext())
            {
                var accountToUpdate = context.Accounts.FirstOrDefault(a => a.Username == account.Username);
                if(accountToUpdate != null)
                {
                    accountToUpdate.MaxScore = account.MaxScore;
                    accountToUpdate.GamePlayed = account.GamePlayed;
                    context.SaveChanges();
                }
            }
        }

    }
}
