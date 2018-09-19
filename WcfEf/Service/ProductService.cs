using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using WcfEf.Contract;
using WcfEf.DataContext;
using WcfEf.Model;

namespace WcfEf.Service
{
    public class ProductService : IProductService
    {
        private readonly DefaultDbContext database;

        public ProductService(DefaultDbContext database)
        {
            this.database = database;
        }

        public void Save(Product entity)
        {
            this.database.Products.Add(entity);
            this.database.SaveChanges();
        }

        public PagedResult<Product> Search(string value, int page, int pageSize)
        {
            //BypassCrossDomain();

            if (page < 1)
                page = 1;

            if (pageSize < 1)
                pageSize = 6;

            var query = database
                .Products
                .Where(e =>
                    value == null
                    || e.Name.Contains(value)
                    || e.Description.Contains(value)
                )
                .OrderBy(e => e.Name);

            var totalItems = query.Count();

            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            var startIndex = ((page - 1) * pageSize);
            var items = query.Skip(startIndex).Take(pageSize).ToList();
            var result = new PagedResult<Product>
            {
                Items = items,
                Page = page,
                TotalPages = totalPages
            };

            return result;
        }

        public List<Product> GetAll()
        {
            return database.Products.ToList();
        }

        //private void BypassCrossDomain()
        //{
        //    WebOperationContext.Current?.OutgoingResponse?.Headers?.Add("Access-Control-Allow-Origin", "*");
        //}
    }
}
