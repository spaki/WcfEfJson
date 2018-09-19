using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using WcfEf.Contract;
using WcfEf.Service;
using WcfEf.WcfCustom;

namespace WcfEf
{
    public class Program
    {
        static void Main(string[] args)
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
                webEndpoint.EndpointBehaviors.Add(new WebHttpBehavior { HelpEnabled = true, FaultExceptionEnabled = true});
                webEndpoint.EndpointBehaviors.Add(new EnableCrossOriginResourceSharingBehavior());

                // net tcp
                host.AddServiceEndpoint(typeof(IProductService), new NetTcpBinding(), "Produto.svc");

                //mex
                host.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true });
                host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

                host.Open();

                Console.WriteLine("Serviço rodando...");
                Console.WriteLine("Tecle para finalizar.");
                Console.ReadLine();
            }
        }
    }
}
