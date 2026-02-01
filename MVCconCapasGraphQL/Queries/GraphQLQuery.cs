using GraphQL.Types;
using MVCconCapasGraphQL.DataAccess.Contracts;
using MVCconCapasGraphQL.Types;
using MVCconCapasGraphQL.Types.Users;
using System.Collections.Generic;                 
using MVCconCapasGraphQL.DataBase.Model.User;
using MVCconCapasGraphQL.Types.Site;
using MVCconCapasGraphQL.DataAccess.Repositories.Contracts;

namespace MVCconCapasGraphQL.Queries
{
    public class GraphQLQuery : ObjectGraphType
    {
        public GraphQLQuery(IUserRepository userRepository, ISiteRepository siteRepository)
        {
            Name = "Query";
            Description = "Root de consultas GraphQL para Beerhenqe.";

            FieldAsync<GeneralResponseType<UserType>>(
              "listarUsuarios",
              arguments: new QueryArguments(
                new QueryArgument<StringGraphType> { Name = "search", DefaultValue = "" },
                new QueryArgument<IntGraphType> { Name = "page", DefaultValue = 1 },
                new QueryArgument<IntGraphType> { Name = "pageSize", DefaultValue = 10 }
              ),
              resolve: async ctx =>
              {
                  var (meta, items) = await userRepository.GetUsersAsync(
                      ctx.GetArgument<string>("search"),
                      ctx.GetArgument<int>("page"),
                      ctx.GetArgument<int>("pageSize"),
                      "Email", "ASC"
                  );
                  return new { code = meta.Code, message = meta.Message, items };
              }
            );

            FieldAsync<GeneralResponseType<UserType>>(
                "obtenerUsuario",
                description: "Obtiene un usuario por Id, Email o ambos.",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "id", Description = "Id del usuario", DefaultValue = null },
                    new QueryArgument<StringGraphType> { Name = "email", Description = "Email del usuario", DefaultValue = null }
                ),
                resolve: async ctx =>
                {
                    var id = ctx.GetArgument<int?>("id");
                    var email = ctx.GetArgument<string>("email");

                    var (meta, item) = await userRepository.GetUserAsync(id, email);

                    return new { code = meta.Code, message = meta.Message, items = item == null ? new List<UserModel>() : new List<UserModel> { item } };
                }
            );

            FieldAsync<GeneralResponseType<NavigationItemType>>(
                "publicNavigation",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "lang", DefaultValue = "es" }
                ),
                resolve: async ctx =>
                {
                    var (meta, items) =
                        await siteRepository.GetNavigationAsync(
                            ctx.GetArgument<string>("lang"));

                    return new { code = meta.Code, message = meta.Message, items };
                }
            );

            FieldAsync<GeneralResponseType<BeerModelType>>(
                "publicBeers",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "lang", DefaultValue = "es" }
                ),
                resolve: async ctx =>
                {
                    var (meta, items) =
                        await siteRepository.GetBeersAsync(
                            ctx.GetArgument<string>("lang"));

                    return new { code = meta.Code, message = meta.Message, items };
                }
            );

            FieldAsync<GeneralResponseType<BeerImageModelType>>(
                "publicBeerImages",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "beerId" },
                    new QueryArgument<StringGraphType> { Name = "lang", DefaultValue = "es" }
                ),
                resolve: async ctx =>
                {
                    var (meta, items) =
                        await siteRepository.GetBeerImagesAsync(
                            ctx.GetArgument<string>("lang"),
                            ctx.GetArgument<int>("beerId"));

                    return new { code = meta.Code, message = meta.Message, items };
                }
            );

            FieldAsync<GeneralResponseType<HomeContentModelType>>(
                "publicHome",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "lang", DefaultValue = "es" }
                ),
                resolve: async ctx =>
                {
                    var (meta, items) =
                        await siteRepository.GetHomeAsync(
                            ctx.GetArgument<string>("lang"));

                    return new { code = meta.Code, message = meta.Message, items };
                }
            );

            FieldAsync<GeneralResponseType<FeaturedBeerModelType>>(
                "publicFeaturedBeers",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "lang", DefaultValue = "es" }
                ),
                resolve: async ctx =>
                {
                    var (meta, items) =
                        await siteRepository.GetFeaturedBeersAsync(
                            ctx.GetArgument<string>("lang"));

                    return new { code = meta.Code, message = meta.Message, items };
                }
            );

            FieldAsync<GeneralResponseType<FooterItemModelType>>(
                "publicFooter",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "lang", DefaultValue = "es" }
                ),
                resolve: async ctx =>
                {
                    var (meta, items) =
                        await siteRepository.GetFooterAsync(
                            ctx.GetArgument<string>("lang"));

                    return new { code = meta.Code, message = meta.Message, items };
                }
            );

            FieldAsync<GeneralResponseType<BannerModelType>>(
                "publicBanners",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "lang", DefaultValue = "es" }
                ),
                resolve: async ctx =>
                {
                    var (meta, items) =
                        await siteRepository.GetBannersAsync(
                            ctx.GetArgument<string>("lang"));

                    return new { code = meta.Code, message = meta.Message, items };
                }
            );

            FieldAsync<GeneralResponseType<SeoModelType>>(
                "publicSeo",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "lang", DefaultValue = "es" }
                ),
                resolve: async ctx =>
                {
                    var (meta, items) =
                        await siteRepository.GetSeoAsync(
                            ctx.GetArgument<string>("lang"));

                    return new { code = meta.Code, message = meta.Message, items };
                }
            );

            FieldAsync<GeneralResponseType<StaticPageModelType>>(
                "publicPage",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "pageCode" },
                    new QueryArgument<StringGraphType> { Name = "lang", DefaultValue = "es" }
                ),
                resolve: async ctx =>
                {
                    var (meta, items) =
                        await siteRepository.GetPageAsync(
                            ctx.GetArgument<string>("lang"),
                            ctx.GetArgument<string>("pageCode"));

                    return new { code = meta.Code, message = meta.Message, items };
                }
            );

        }
    }
}
