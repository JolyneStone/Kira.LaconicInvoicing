using Kira.LaconicInvoicing;
using Kira.LaconicInvoicing.Purchase.Entities;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace App.Test.QueryCondition
{
    public class QueryConditionTest
    {
        private List<Vendor> GetTestList()
        {
            return new List<Vendor>()
            {
                new Vendor(){ Name = "ABC", Number = "0001" },
                new Vendor(){ Name = "AB", Number = "0002" },
            };
        }

        [Fact]
        public void WhereStrTest()
        {
            var condition = new QueryCondition<Vendor>();
            //condition.AddFilter("Name", ConditionOperator.Equal, "null", LogicOperator.AndAlso);
            condition.AddFilter("Name", ConditionOperator.Equal, "ABC");
            condition.AddFilter("Number", ConditionOperator.StartsWith, "000");

            var vendors = GetTestList();
            var query = vendors.AsQueryable().Where(condition.GetFilter(), condition.GetValues());

            var data = query.ToList();
            Assert.NotNull(data);
            Assert.True(data.Count == 1);
            Assert.True(data[0].Name == "ABC");
            Assert.True(data[0].Number == vendors[0].Number);
        }

        [Fact]
        public void OrderStrTest()
        {
            var condition = new QueryCondition<Vendor>();
            condition.AddSort("Number", "DESC");

            var vendors = GetTestList();
            var query = vendors.AsQueryable().OrderBy(condition.Sorts.ConvertToSortString());

            var data = query.ToList();
            Assert.NotNull(data);
            Assert.True(data.Count == 2);
            Assert.True(data[0].Name == "AB");
            Assert.True(data[0].Number == vendors[1].Number);
            Assert.True(data[1].Name == "ABC");
            Assert.True(data[1].Number == vendors[0].Number);
        }
    }
}
