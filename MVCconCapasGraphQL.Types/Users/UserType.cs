// Types/Users/UserType.cs
using GraphQL.Types;
using MVCconCapasGraphQL.DataBase.Model.User;

namespace MVCconCapasGraphQL.Types.Users
{
    public class UserType : ObjectGraphType<UserModel>
    {
        public UserType()
        {
            Name = "User";
            Field(x => x.Id);
            Field(x => x.Email);
            Field(x => x.PasswordHash, nullable: true);
            Field(x => x.DisplayName);
            Field(x => x.IsActive);
            Field(x => x.CreatedAt);
        }
    }
}
