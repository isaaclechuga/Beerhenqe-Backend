using GraphQL.Types;
using MVCconCapasGraphQL.DataAccess.Contracts;
using MVCconCapasGraphQL.Types;
using MVCconCapasGraphQL.Types.Users;
using System.Collections.Generic;
using MVCconCapasGraphQL.DataBase.Model.User;

public class GraphQLMutation : ObjectGraphType
{
    public GraphQLMutation(IUserRepository userRepository)
    {
        Name = "Mutation";

        FieldAsync<GeneralResponseType<UserType>>(
            "updateUsuario",
            description: "Actualiza Email/DisplayName/Password del usuario. Devuelve code/message y (opcional) items vacíos.",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" },
                new QueryArgument<StringGraphType> { Name = "email" },
                new QueryArgument<StringGraphType> { Name = "displayName" },
                new QueryArgument<StringGraphType> { Name = "password" }
            ),
            resolve: async ctx =>
            {
                var id = ctx.GetArgument<int>("id");
                var email = ctx.GetArgument<string>("email");
                var displayName = ctx.GetArgument<string>("displayName");
                var password = ctx.GetArgument<string>("password");

                var meta = await userRepository.UpdateUserAsync(id, email, displayName, password);

                return new
                {
                    code = meta.Code,
                    message = meta.Message,
                    items = (List<UserModel>)null
                };
            }
        );
    }
}
