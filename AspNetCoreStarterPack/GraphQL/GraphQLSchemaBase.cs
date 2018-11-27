using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarterPack.GraphQL
{
    public abstract class GraphQLSchemaBase : Schema
    {
        public GraphQLSchemaBase()
        {
            DependencyResolver = new GraphQLDependencResolver();
        }
    }
}
