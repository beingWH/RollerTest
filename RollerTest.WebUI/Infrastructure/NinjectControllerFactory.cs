using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using System.Web.Mvc;
using System.Web.Routing;
using RollerTest.Domain.Abstract;
using RollerTest.Domain.Concrete;
using RollerTest.WebUI.Models.WTTESTMODEL;
using RollerTest.WebUI.Models.WTTESTMODEL.EFRepository;
using RollerTest.WebUI.Models;

namespace RollerTest.WebUI.Infrastructure
{
    public class NinjectControllerFactory:DefaultControllerFactory
    {
        private IKernel ninjectKernel;
        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBinding();
        }
        protected override IController GetControllerInstance(RequestContext requestContext,Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBinding()
        {
            //添加绑定

            ninjectKernel.Bind<IRecordinfoRepository>().To<EFRecordinfoRepository>();
            ninjectKernel.Bind<ISampleinfoRepository>().To<EFSampleinfoRepository>();
            ninjectKernel.Bind<ITestreportinfoRepository>().To<EFTestreportinfoRepository>();
            ninjectKernel.Bind<IProjectRepository>().To<EFProjectinfoRepository>();
            ninjectKernel.Bind<IBaseRepository>().To<EFBaseRepository>();
            ninjectKernel.Bind<IReadRepository<WTTESTINFO>>().To<EFTESTINFO>();
            ninjectKernel.Bind<IReadRepository<WTTESTEQUIPMENT>>().To<EFTESTEQUIPMENT>();
            ninjectKernel.Bind<IReadRepository<WTSAMPLEINFO>>().To<EFSAMPLEINFO>();
            ninjectKernel.Bind<IReadRepository<WTATTACHMENT>>().To<EFATTACHMENT>();

        }
    }
}