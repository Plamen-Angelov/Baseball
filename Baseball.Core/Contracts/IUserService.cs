using Baseball.Common.ViewModels.UserViewModels;

namespace Baseball.Core.Contracts
{
    public interface IUserService
    {
        IEnumerable<UserViewModel> GetByEmail(string email);
    }
}
