using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CarRental.BLL.Interfaces;
using CarRental.BLL.Services;
using Ninject;

namespace CarRental.WEB.Util
{
    /// <summary>
    /// Class for implementing dependency injections (used at NinjectWebCommon)
    /// </summary>
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            _kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            _kernel.Bind<IRentService>().To<RentService>();
        }
    }
}