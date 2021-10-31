using Data.Entities;
using Model.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace Data.Tests.Entities
{
    public class SystemConfigTests
    {
        [Fact]
        public void Test_Should_Not_Validate_SystemConfig_With_Code_Null()
        {
            var systemConfig = new SystemConfig
            {
                Code = null,
                Value = "sample"
            };

            var context = new ValidationContext(systemConfig);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(systemConfig, context, results, true);
            Assert.False(isValid);
        }

        [Fact]
        public void Test_Should_Not_Validate_SystemConfig_With_Code_Empty_String()
        {
            var systemConfig = new SystemConfig
            {
                Code = "",
                Value = "sample"
            };

            var context = new ValidationContext(systemConfig);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(systemConfig, context, results, true);
            Assert.False(isValid);
        }

        [Fact]
        public void Test_Should_Not_Validate_SystemConfig_With_Code_Exceeds_Max_Length()
        {
            var invalidLengthString = new string(Enumerable.Repeat("ABCDEF", Constraints.CODE_COLUMN_MAX_LENGTH + 1).Select(s => s[new Random().Next(s.Length)]).ToArray());
            var systemConfig = new SystemConfig
            {
                Code = invalidLengthString,
                Value = "sample"
            };

            var context = new ValidationContext(systemConfig);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(systemConfig, context, results, true);
            Assert.False(isValid);
        }

        [Fact]
        public void Test_Should_Not_Validate_SystemConfig_With_Value_Null()
        {
            var systemConfig = new SystemConfig
            {
                Code = "sample",
                Value = null
            };

            var context = new ValidationContext(systemConfig);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(systemConfig, context, results, true);
            Assert.False(isValid);
        }

        [Fact]
        public void Test_Should_Not_Validate_SystemConfig_With_Value_Empty_String()
        {
            var systemConfig = new SystemConfig
            {
                Code = "sample",
                Value = ""
            };

            var context = new ValidationContext(systemConfig);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(systemConfig, context, results, true);
            Assert.False(isValid);
        }

        [Fact]
        public void Test_Should_Not_Validate_SystemConfig_With_Value_Exceeds_Max_Length()
        {
            var invalidLengthString = new string(Enumerable.Repeat("ABCDEF", Constraints.TEXT_COLUMN_MAX_LENGTH + 1).Select(s => s[new Random().Next(s.Length)]).ToArray());
            var systemConfig = new SystemConfig
            {
                Code = "sample",
                Value = invalidLengthString
            };

            var context = new ValidationContext(systemConfig);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(systemConfig, context, results, true);
            Assert.False(isValid);
        }

        [Fact]
        public void Test_Should_Validate_SystemConfig()
        {
            var validLengthStringCode = new string(Enumerable.Repeat("ABCDEF", Constraints.CODE_COLUMN_MAX_LENGTH).Select(s => s[new Random().Next(s.Length)]).ToArray());
            var validLengthStringValue = new string(Enumerable.Repeat("ABCDEF", Constraints.TEXT_COLUMN_MAX_LENGTH).Select(s => s[new Random().Next(s.Length)]).ToArray());
            var systemConfig = new SystemConfig
            {
                Code = validLengthStringCode,
                Value = validLengthStringValue
            };

            var context = new ValidationContext(systemConfig);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(systemConfig, context, results, true);
            Assert.True(isValid);
        }
    }
}
