using System;
using System.Collections.Generic;
using Ninject;
using Ninject.Modules;
using Ninject.Parameters;

namespace Utils.Resolution
{
    public static class IoC
    {
        internal static StandardKernel StandardKernel;

        static IoC()
        {
            StandardKernel = new StandardKernel();
        }

        public static void AddSingleton<T>()
        {
            if (CanResolve<T>())
                StandardKernel.Rebind<T>().ToSelf().InSingletonScope();
            else
                StandardKernel.Bind<T>().ToSelf().InSingletonScope();
        }

        public static void AddSingleton<TInt, TImp>() where TImp : TInt
        {
            if (CanResolve<TInt>())
                StandardKernel.Rebind<TInt>().To<TImp>().InSingletonScope();
            else
                StandardKernel.Bind<TInt>().To<TImp>().InSingletonScope();
        }

        public static void AddPrototype<T>()
        {
            if (CanResolve<T>())
                StandardKernel.Rebind<T>().ToSelf();
            else
                StandardKernel.Bind<T>().ToSelf();
        }

        public static void AddPrototype<TInt, TImp>() where TImp : TInt
        {
            if (CanResolve<TInt>())
                StandardKernel.Rebind<TInt>().To<TImp>();
            else
                StandardKernel.Bind<TInt>().To<TImp>();
        }

        public static void ResolveWith<TInt>(TInt obj)
        {
            if (CanResolve<TInt>())
                StandardKernel.Rebind<TInt>().ToConstant(obj);
            else
                StandardKernel.Bind<TInt>().ToConstant(obj);
        }

        public static T Get<T>()
        {
            return StandardKernel.Get<T>();
        }

        static bool CanResolve<T>()
        {
            var request = StandardKernel.CreateRequest(typeof(T), x => true, new List<IParameter>(), false, false);
            return StandardKernel.CanResolve(request);
        }

        public static void AddModule<T>()
        {
            if (StandardKernel.HasModule(typeof(T).ToString()))
                StandardKernel.Unload(typeof(T).ToString());

            StandardKernel.Load(new[] {
                (NinjectModule)Activator.CreateInstance(typeof(T))
            });
        }

        public static void Reset()
        {
            StandardKernel.Dispose();
        }
    }
}