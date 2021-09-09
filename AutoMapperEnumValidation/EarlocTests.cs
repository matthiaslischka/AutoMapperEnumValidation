using System;
using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoMapperEnumValidation
{
    /// <summary>
    /// https://stackoverflow.com/questions/65876675/how-to-discover-missing-type-maps-for-mapping-enum-to-enum-in-automapper
    /// </summary>
    /// 
    [TestClass]
    public class EarlocTests
    {
        public enum Source { A, B, C, D, Executer, A1, B2, C3 } // more values than below

        public enum Destination { C, B, X, Y, A, Executor }  //fewer values, different ordering, no D, but X, Y and a Typo


        class SourceType
        {
            public Source[] Enums { get; set; }
        }

        class DestinationType
        {
            public Destination[] Enums { get; set; }
        }

        [TestMethod]
        public void MapWithNotMatchingEnums_ShouldThrow()
        {
            var problematicMapperConfiguration = new MapperConfiguration(config =>
            {
                config.Advanced.Validator(EnumMappingValidator.ValidateNamesMatch());
                config.CreateMap<SourceType, DestinationType>();
            });

            Action validate = () => problematicMapperConfiguration.AssertConfigurationIsValid();

            validate.Should().Throw<Exception>();
        }
    }
}