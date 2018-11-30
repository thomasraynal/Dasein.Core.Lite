using StructureMap;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public class AppRegistry : Registry
    {
        public AppRegistry()
        {
            For<IAppContainer>().Use<AppContainer>().Singleton();

            Scan(scanner =>
            {
                scanner.AssembliesAndExecutablesFromApplicationBaseDirectory();
                scanner.LookForRegistries();
                scanner.WithDefaultConventions();
            });
        }
    }
}
