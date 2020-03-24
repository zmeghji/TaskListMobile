using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using TaskListMobile.Sevices;
using TaskListMobileData.Repositories;

namespace TaskListMobile.Configuration
{
    public static class  DIContainer
    {
        private static IContainer _container;
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            builder.RegisterType<TaskListRepository>().As<ITaskListRepository>().SingleInstance()
                .WithParameter(new TypedParameter(typeof(string), FileService.GetDbFilePath()));

            _container = builder.Build();
        }
        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
