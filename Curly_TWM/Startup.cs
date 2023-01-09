using Curly_TWM.Infrastructure.DbaseContext;
using Curly_TWM.Infrastructure.Helpers;
using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(Curly_TWM.Startup))]

namespace Curly_TWM
{
    public partial class Startup
    {
        TWMDB context = new TWMDB();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            SeedData.SeedIdentities(context);
        }
    }
    //public class Startup
    //{
    //    public void Configuration(IAppBuilder app)
    //    {
    //        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
    //    }
    //}
}
