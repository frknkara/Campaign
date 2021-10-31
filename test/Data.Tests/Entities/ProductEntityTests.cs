using Data.Entities;
using Model.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace Data.Tests.Entities
{
    public class ProductEntityTests
    {
        [Fact]
        public void Test_Should_Not_Validate_Product_With_Code_Null()
        {
            var product = new Product
            {
                Code = null,
                Price = 10,
                Stock = 100
            };

            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results, true);
            Assert.False(isValid);
        }

        [Fact]
        public void Test_Should_Not_Validate_Product_With_Code_Empty_String()
        {
            var product = new Product
            {
                Code = "",
                Price = 10,
                Stock = 100
            };

            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results, true);
            Assert.False(isValid);
        }

        [Fact]
        public void Test_Should_Not_Validate_Product_With_Code_Exceeds_Max_Length()
        {
            var invalidLengthString = new string(Enumerable.Repeat("ABCDEF", Constraints.CODE_COLUMN_MAX_LENGTH + 1).Select(s => s[new Random().Next(s.Length)]).ToArray());
            var product = new Product
            {
                Code = invalidLengthString,
                Price = 10,
                Stock = 100
            };

            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results, true);
            Assert.False(isValid);
        }

        [Fact]
        public void Test_Should_Validate_Product()
        {
            var validLengthString = new string(Enumerable.Repeat("ABCDEF", Constraints.CODE_COLUMN_MAX_LENGTH).Select(s => s[new Random().Next(s.Length)]).ToArray());
            var product = new Product
            {
                Code = validLengthString,
                Price = 10,
                Stock = 100
            };

            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results, true);
            Assert.True(isValid);
        }
    }
}
