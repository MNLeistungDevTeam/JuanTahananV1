using System.Linq;
using DMS.Web.Models;

namespace DMS.Web.Controllers.Services
{
    public static class ViewModelExtensions
    {
        public static TDestination MergeNonNullData<TDestination, TSource>(this TDestination destination, TSource source, params string[] excludedProperties)
        {
            // Get all properties of the source type
            var sourceProperties = typeof(TSource).GetProperties();

            // Convert excluded properties to lowercase for case-insensitive comparison
            var excludedPropertiesLower = excludedProperties.Select(p => p.ToLower()).ToList();

            // Get the name of the primary Id property
            var primaryIdPropertyName = "Id"; // Change this if your primary Id property has a different name

            // Iterate through each property of the source type
            foreach (var sourceProperty in sourceProperties)
            {
                // Check if the property should be excluded or if it's the primary Id property
                if (excludedPropertiesLower.Contains(sourceProperty.Name.ToLower()) || sourceProperty.Name == primaryIdPropertyName)
                {
                    continue; // Skip this property
                }

                // Find the corresponding property in the destination type
                var destinationProperty = typeof(TDestination).GetProperty(sourceProperty.Name);

                // If the destination property exists and has the same type as the source property
                if (destinationProperty != null && destinationProperty.PropertyType == sourceProperty.PropertyType)
                {
                    // Get the value of the property from the source object
                    var sourceValue = sourceProperty.GetValue(source);

                    // If the value is not null, set it on the destination object
                    if (sourceValue != null)
                    {
                        destinationProperty.SetValue(destination, sourceValue);
                    }
                }
            }

            // Return the merged destination object
            return destination;
        }
    }
}
