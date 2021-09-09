using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace AutoMapperEnumValidation
{
    public class EnumMappingValidator
    {
        public static Action<ValidationContext> ValidateNamesMatch()
        {
            return validationContext =>
            {
                var sourceEnumType = GetEnumType(validationContext.Types.SourceType);

                if (sourceEnumType == null)
                    return;
                
                var destinationEnumType = GetEnumType(validationContext.Types.DestinationType);

                if (destinationEnumType == null) throw new ArgumentException("Unexpected Enum to Non-Enum Map");

                var sourceEnumNames = sourceEnumType.GetFields().Select(x => x.Name).ToList();
                var destinationEnumNames = destinationEnumType.GetFields().Select(x => x.Name).ToList();

                var errors = new List<string>();

                foreach (var sourceEnumName in sourceEnumNames)
                {
                    if (destinationEnumNames.All(x => x.ToLower() != sourceEnumName.ToLower()))
                        errors.Add($"Expected enum {destinationEnumType} to contain enum \"{sourceEnumName}\".");
                }

                if (errors.Any())
                    throw new ArgumentException(string.Join(Environment.NewLine, errors));
            };
        }

        private static Type? GetEnumType(Type type)
        {
            if (type.IsEnum) return type;

            var nullableUnderlyingType = Nullable.GetUnderlyingType(type);
            if (nullableUnderlyingType?.IsEnum ?? false) return nullableUnderlyingType;

            return null;
        }
    }
}