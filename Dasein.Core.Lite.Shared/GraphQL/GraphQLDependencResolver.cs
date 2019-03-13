//using GraphQL;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Dasein.Core.Lite.Shared
//{
//    public class GraphQLDependencyResolver : IDependencyResolver
//    {
//        public T Resolve<T>()
//        {
//            return AppCore.Instance.Get<T>();
//        }

//        public object Resolve(Type type)
//        {
//            return AppCore.Instance.ObjectProvider.GetInstance(type);
//        }
//    }
//}
