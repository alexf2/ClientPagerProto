using AutoMapper;


using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;


namespace NorthWind.Dal.LinqToSql.MsSql
{
    
    public sealed class Installer : IWindsorInstaller
    {
        //Резолвинг массивов: http://mikehadlow.blogspot.ru/2008/09/resolving-arrays-with-windsor.html
        //http://mikehadlow.blogspot.ru/2009/03/castle-windsor-registering-and.html
        //однако, CollectionResolver не желателен, так как не проверяет ссылочные циклы и app падает

        //Registering components by conventions 
        //http://docs.castleproject.org/Default.aspx?Page=Registering-components-by-conventions&NS=Windsor&AspxAutoDetectCookieSupport=1

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            MappingsRegistrar.EnsureRegistered();
            Mapper.AssertConfigurationIsValid();

            /*container.Register(
                Classes.FromAssemblyInDirectory(new AssemblyFilter("Parsers")). //берём все сбоки из каталога Parsers
                //BasedOn<IMessageParser>().WithService.FromInterface(typeof(IMessageParser)).LifestyleTransient(),
                    BasedOn<IMessageParserFactory>().WithService.FirstInterface(),

                //используем явную регистрацию вместо подключения CollectionResolver (kernel.Resolver.AddSubResolver)
                //Component.For<IEnumerable<IMessageParser>>().UsingFactoryMethod( k => k.ResolveAll<IMessageParser>() ),
                Component.For<IEnumerable<IMessageParserFactory>>().UsingFactoryMethod(k => k.ResolveAll<IMessageParserFactory>())

                //Component.For<IMsgStorage>().ImplementedBy<MsgStorageMemory>()
            );*/
        }
    }
}
