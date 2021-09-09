using System;
using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoMapperEnumValidation
{
    [TestClass]
    public class EnumMappingValidatorTests
    {
        enum Small { A, B }
        enum Big { A, B, C }

        class WithSmallEnum { public Small Enum { get; set; } }
        class WithBigEnum { public Big Enum { get; set; } }
        class WithBigString { public string Enum { get; set; }}


        [TestMethod]
        public void ValidateNamesMatch_EnumToNonEnumMapping_ShouldThrow()
        {
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.Advanced.Validator(EnumMappingValidator.ValidateNamesMatch());
                config.CreateMap<WithBigEnum, WithBigString>();
            });

            Action validate = () => mapperConfiguration.AssertConfigurationIsValid();

            validate.Should().Throw<ArgumentException>().WithMessage("Unexpected Enum to Non-Enum Map");
        }

        [TestMethod]
        public void ValidateNamesMatch_SameTypesMap_ShouldBeValid()
        {
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.Advanced.Validator(EnumMappingValidator.ValidateNamesMatch());
                config.CreateMap<WithBigEnum, WithBigEnum>();
            });

            Action validate = () => mapperConfiguration.AssertConfigurationIsValid();

            validate.Should().NotThrow();
        }

        [TestMethod]
        public void ValidateNamesMatch_SmallerToBiggerEnum_ShouldBeValid()
        {
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.Advanced.Validator(EnumMappingValidator.ValidateNamesMatch());
                config.CreateMap<WithSmallEnum, WithBigEnum>();
            });

            Action validate = () => mapperConfiguration.AssertConfigurationIsValid();

            validate.Should().NotThrow();
        }

        [TestMethod]
        public void ValidateNamesMatch_BiggerToSmallerEnum_ShouldThrow()
        {
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.Advanced.Validator(EnumMappingValidator.ValidateNamesMatch());
                config.CreateMap<WithBigEnum, WithSmallEnum>();
            });

            Action validate = () => mapperConfiguration.AssertConfigurationIsValid();

            validate.Should().Throw<Exception>().WithMessage($"Expected enum {typeof(Small)} to contain enum \"C\".");
        }
    }
}