using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using WcfEf.Model;

namespace WcfEf.Contract
{
    [ServiceContract]
    public interface IProductService
    {
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "product/?value={value}&page={page}&pageSize={pageSize}")]
        PagedResult<Product> Search(string value, int page, int pageSize);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/allproducts")]
        List<Product> GetAll();

        [OperationContract]
        void Save(Product entity);
    }
}
