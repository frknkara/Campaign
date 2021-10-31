using Data.Entities;
using Model.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace Data.Tests.Entities
{
    public class CampaignEntityTests
    {
        [Fact]
        public void Test_Should_Not_Validate_Campaign_With_Name_Null()
        {
            var campaign = new Campaign
            {
                Name = null,
                ProductId = Guid.NewGuid(),
                Duration = 10,
                PriceManipulationLimit = 5,
                TargetSalesCount = 100
            };

            var context = new ValidationContext(campaign);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(campaign, context, results, true);
            Assert.False(isValid);
        }

        [Fact]
        public void Test_Should_Not_Validate_Campaign_With_Name_Empty_String()
        {
            var campaign = new Campaign
            {
                Name = "",
                ProductId = Guid.NewGuid(),
                Duration = 10,
                PriceManipulationLimit = 5,
                TargetSalesCount = 100
            };

            var context = new ValidationContext(campaign);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(campaign, context, results, true);
            Assert.False(isValid);
        }

        [Fact]
        public void Test_Should_Not_Validate_Campaign_With_Name_Exceeds_Max_Length()
        {
            var invalidLengthString = new string(Enumerable.Repeat("ABCDEF", Constraints.NAME_COLUMN_MAX_LENGTH + 1).Select(s => s[new Random().Next(s.Length)]).ToArray());
            var campaign = new Campaign
            {
                Name = invalidLengthString,
                ProductId = Guid.NewGuid(),
                Duration = 10,
                PriceManipulationLimit = 5,
                TargetSalesCount = 100
            };

            var context = new ValidationContext(campaign);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(campaign, context, results, true);
            Assert.False(isValid);
        }

        [Fact]
        public void Test_Should_Validate_Campaign()
        {
            var validLengthString = new string(Enumerable.Repeat("ABCDEF", Constraints.NAME_COLUMN_MAX_LENGTH).Select(s => s[new Random().Next(s.Length)]).ToArray());
            var campaign = new Campaign
            {
                Name = validLengthString,
                ProductId = Guid.NewGuid(),
                Duration = 10,
                PriceManipulationLimit = 5,
                TargetSalesCount = 100
            };

            var context = new ValidationContext(campaign);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(campaign, context, results, true);
            Assert.True(isValid);
        }
    }
}
