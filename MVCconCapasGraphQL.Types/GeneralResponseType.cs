using GraphQL.Types;

namespace MVCconCapasGraphQL.Types
{
    public class GeneralResponseType<TGraphType> : ObjectGraphType
        where TGraphType : IGraphType
    {
        public GeneralResponseType()
        {
            Name = $"GeneralResponse_{typeof(TGraphType).Name}";

            Field<IntGraphType>("code");
            Field<StringGraphType>("message");
            Field<ListGraphType<TGraphType>>("items");
        }
    }
}
