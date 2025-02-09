[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Parser.MvcClient.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Parser.MvcClient.App_Start.NinjectWebCommon), "Stop")]

namespace Parser.MvcClient.App_Start
{
    using System;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    using Parser.Common.Contracts;
    using Parser.MvcClient.App_Start.NinjectModules;

    public static class NinjectWebCommon
    {
        public static IKernel Kernel { get; private set; }

        public static IStartupTimestampProvider StartupTimestampProvider { get; private set; }

        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                NinjectWebCommon.Kernel = kernel;
                NinjectWebCommon.StartupTimestampProvider = kernel.Get<IStartupTimestampProvider>();

                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Load(new AuthNinjectModule());
            kernel.Load(new AutoMapperNinjectModule());
            kernel.Load(new CommonNinjectModule());
            kernel.Load(new DataNinjectModule());
            kernel.Load(new DataModelsNinjectModule());
            kernel.Load(new DataServicesNinjectModule());
            kernel.Load(new DataViewModelsNinjectModule());
            kernel.Load(new LogFileParserNinjectModule());
            kernel.Load(new SignalRNinjectModule());
        }
    }
}
