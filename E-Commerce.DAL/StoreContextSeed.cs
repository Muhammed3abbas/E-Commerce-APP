using E_Commerce.DAL.Entities;
using E_Commerce.DAL.Entities.OrderAggregate;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.DAL
{
    public class StoreContextSeed
    {
        public static async Task InvokeSeed(StoreContext context, ILoggerFactory loggerFactory)
        {

            try
            {
                if (!context.ProductTypes.Any())
                {
                    var typesData = File.ReadAllText("../E-Commerce.DAL/Data/DataSeeds/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    foreach (var type in types)
                        context.ProductTypes.Add(type);
                    await context.SaveChangesAsync();

                }

                if (!context.ProductBrands.Any())
                {
                    var typesData = File.ReadAllText("../E-Commerce.DAL/Data/DataSeeds/brands.json");
                    var types = JsonSerializer.Deserialize<List<ProductBrand>>(typesData);
                    foreach (var type in types)
                        context.ProductBrands.Add(type);
                    await context.SaveChangesAsync();
                }

                if (!context.Products.Any())
                {
                    var typesData = File.ReadAllText("../E-Commerce.DAL/Data/DataSeeds/products.json");
                    var types = JsonSerializer.Deserialize<List<Product>>(typesData);
                    foreach (var type in types)
                        context.Products.Add(type);
                    await context.SaveChangesAsync();
                }


                if (!context.DeliveryMethods.Any())
                {
                    var deliveryData = File.ReadAllText("../E-Commerce.DAL/Data/DataSeeds/delivery.json");
                    var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);
                    context.DeliveryMethods.AddRange(methods);
                    await context.SaveChangesAsync();

                }

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }

        }
    }
}
