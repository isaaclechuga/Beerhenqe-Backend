// Contracts/IUserRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using MVCconCapasGraphQL.DataBase;
using MVCconCapasGraphQL.DataBase.Model.User;

namespace MVCconCapasGraphQL.DataAccess.Contracts
{
    public interface IUserRepository
    {
        Task<(GeneralResponse meta, List<UserModel> items)> GetUsersAsync(
            string search, int page, int pageSize, string orderBy, string orderDir);

        Task<(GeneralResponse meta, UserModel item)> GetUserAsync(int? id, string? email);
        Task<GeneralResponse> UpdateUserAsync(int id, string? email, string? displayName, string? password);
    }
}
