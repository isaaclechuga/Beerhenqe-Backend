using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using MVCconCapasGraphQL.Queries;

namespace MVCconCapasGraphQL.Schema
{
    public class GraphQLSchema : GraphQL.Types.Schema
    {
        public GraphQLSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<GraphQLQuery>();
            Mutation = resolver.Resolve<GraphQLMutation>();
        }
    }
}