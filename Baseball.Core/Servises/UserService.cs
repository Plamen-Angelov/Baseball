using Baseball.Common.ViewModels.UserViewModels;
using Baseball.Core.Contracts;
using Baseball.Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;

namespace Baseball.Core.Servises
{
    public class UserService : IUserService
    {
        private readonly IRepository repository;

        public UserService(IRepository repository)
        {
            this.repository = repository;
        }
        public IEnumerable<UserViewModel> GetByEmail(string email)
        {
            var users = repository.GetAll<IdentityUser>()
                .Where(u => u.Email == email)
                .Select(u => new UserViewModel()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email
                })
                .ToList();

            return users;
        }
    }
}
