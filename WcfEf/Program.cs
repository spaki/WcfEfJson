using Autofac;
using Autofac.Integration.Wcf;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using WcfEf.Contract;
using WcfEf.DataContext;
using WcfEf.Service;
using WcfEf.WcfCustom;

namespace WcfEf
{
    public class Program
    {
        static void Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Register(c => new DefaultDbContext());
            containerBuilder.Register(c => new ProductService(c.Resolve<DefaultDbContext>())).As<IProductService>();

            using (var container = containerBuilder.Build())
            {
                var urls = new Uri[]
                {
                    new Uri("http://localhost:7171/"),
                    new Uri("net.tcp://localhost:7272/")
                };

                using (var host = new ServiceHost(typeof(ProductService), urls))
                {
                    // rest
                    var webEndpoint = host.AddServiceEndpoint(typeof(IProductService), new WebHttpBinding(WebHttpSecurityMode.None) { CrossDomainScriptAccessEnabled = true }, "Produto.svc");
                    webEndpoint.EndpointBehaviors.Add(new WebHttpBehavior { HelpEnabled = true, FaultExceptionEnabled = true });
                    webEndpoint.EndpointBehaviors.Add(new EnableCrossOriginResourceSharingBehavior());

                    // net tcp
                    host.AddServiceEndpoint(typeof(IProductService), new NetTcpBinding(), "Produto.svc");

                    // mex
                    host.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true });
                    host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

                    // IoC
                    host.AddDependencyInjectionBehavior<IProductService>(container);

                    host.Open();

                    Console.WriteLine("Serviço rodando...");
                    Console.WriteLine("Tecle para finalizar.");
                    Console.ReadLine();
                }
            }
        }
    }
}
