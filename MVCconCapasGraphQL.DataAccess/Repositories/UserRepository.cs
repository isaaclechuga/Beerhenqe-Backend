using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;         
using MVCconCapasGraphQL.DataAccess.Contracts;
using MVCconCapasGraphQL.DataBase;
using MVCconCapasGraphQL.DataBase.Connections;
using MVCconCapasGraphQL.DataBase.Model.User;

namespace MVCconCapasGraphQL.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private readonly string _cs;
        public UserRepository(IOptions<DbOptions> opts)
        {
            _cs = opts.Value.Beerhenqe;
        }

        public async Task<(GeneralResponse meta, List<UserModel> items)> GetUsersAsync(
            string search, int page, int pageSize, string orderBy, string orderDir)
        {
            var meta = new GeneralResponse { Code = -200, Message = "Unexpected error" };
            var items = new List<UserModel>();

            using var conn = new SqlConnection(_cs);
            await conn.OpenAsync();

            using var multi = await conn.QueryMultipleAsync(
                "sec.User_Administration",
                new { Action = "LIST", Search = search ?? "", Page = page, PageSize = pageSize, OrderBy = orderBy ?? "Email", OrderDir = orderDir ?? "ASC" },
                commandType: CommandType.StoredProcedure);

            var cm = await multi.ReadFirstOrDefaultAsync();
            if (cm != null)
            {
                meta.Code = (int)cm.Code;
                meta.Message = (string)cm.Message;
            }

            items = (await multi.ReadAsync<UserModel>()).ToList();

            return (meta, items);
        }

        public async Task<(GeneralResponse meta, UserModel item)> GetUserAsync(int? id, string? email)
        {
            var meta = new GeneralResponse { Code = -200, Message = "Unexpected error" };
            UserModel item = null;

            using var conn = new SqlConnection(_cs);
            await conn.OpenAsync();

            using var multi = await conn.QueryMultipleAsync(
                "sec.User_Administration",
                new { Action = "READ", Id = id, Email = email },
                commandType: CommandType.StoredProcedure);

            var cm = await multi.ReadFirstOrDefaultAsync();
            if (cm != null)
            {
                meta.Code = (int)cm.Code;
                meta.Message = (string)cm.Message;
            }

            item = (await multi.ReadFirstOrDefaultAsync<UserModel>());
            return (meta, item);
        }

        public async Task<GeneralResponse> UpdateUserAsync(int id, string? email, string? displayName, string? password)
        {
            var meta = new GeneralResponse { Code = -200, Message = "Unexpected error" };

            using var conn = new SqlConnection(_cs);
            await conn.OpenAsync();

            var cm = await conn.QueryFirstOrDefaultAsync(
                "sec.User_Administration",
                new { Action = "UPDATE", Id = id, Email = email, DisplayName = displayName, Password = password },
                commandType: System.Data.CommandType.StoredProcedure);

            if (cm != null)
            {
                meta.Code = (int)cm.Code;
                meta.Message = (string)cm.Message;
            }
            /* LINQ
               var responseList = new List<GeneralResponse> { meta };
               
               var filtered = responseList
                   .Where(r => r.Code != -200)           
                   .OrderBy(r => r.Code)                
                   .Select(r => new GeneralResponse     
                   {
                       Code = r.Code,
                       Message = r.Message
                   })
                   .ToList();
             */

            return meta;
        }
    }
}
