using AutoMapper;
using AutoMapper.Configuration;
using Kira.LaconicInvoicing.Purchase.Entities;
using Kira.LaconicInvoicing.Sale.Entities;
using Kira.LaconicInvoicing.Warehouse.Dtos;
using Kira.LaconicInvoicing.Warehouse.Entities;
using OSharp.AutoMapper;
using OSharp.Mapping;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace App.Test.AutoMapper
{
    public class AutoMapperTest
    {
        private void SetMapper()
        {
            MapperConfigurationExpression cfg = new MapperConfigurationExpression();

            var config = new Kira.LaconicInvoicing.Warehouse.Dtos.AutoMapperConfiguration();
            config.CreateMaps(cfg);

            Mapper.Initialize(cfg);
            var mapper = new AutoMapperMapper();
            MapperExtensions.SetMapper(mapper);
        }

        [Fact]
        public void InventoryMapTest()
        {
            SetMapper();

            var invetory = new Inventory()
            {
                Id = Guid.NewGuid(),
                Number = "001",
                Amount = 123,
                WarehouseId = Guid.NewGuid(),
                GoodsCategory = Kira.LaconicInvoicing.Warehouse.GoodsCategory.Material
            };

            var material = new Material()
            {
                Id = Guid.NewGuid(),
                Number = "002",
                Name = "XXX1",
                Price = 100.0
            };

            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Number = "003",
                Name = "XXX2",
                CostPrice = 75
            };

            var dto1 = invetory.MapTo<InventoryOutputDto>().Map(material);
            Assert.Equal(dto1.Id, invetory.Id);
            Assert.Equal(dto1.Number, invetory.Number);
            Assert.Equal(dto1.Price, material.Price);

            var dto2 = invetory.MapTo<InventoryOutputDto>().Map(product);
            Assert.Equal(dto2.Id, invetory.Id);
            Assert.Equal(dto2.Number, invetory.Number);
            Assert.Equal(dto2.Price, product.CostPrice);
        }
    }
}
