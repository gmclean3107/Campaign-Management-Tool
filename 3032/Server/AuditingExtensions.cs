using System.Text.Json;
using CampaignManagementTool.Server.Repositories.Interfaces;
using CampaignManagementTool.Shared;

namespace CampaignManagementTool.Server;

/// <summary>
/// Utility class for auditing operations.
/// </summary>
public class AuditingExtensions
{

    /// <summary>
    /// Performs a deep copy of an object using JSON serialization.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="input">The object to be deep copied.</param>
    /// <returns>A deep copy of the input object.</returns>
    public static T? DeepCopyJson<T>(T input)
    {
        var jsonString = JsonSerializer.Serialize(input);
        return JsonSerializer.Deserialize<T>(jsonString);
    }

    /// <summary>
    /// Retrieves the differing properties between two objects of the same type.
    /// </summary>
    /// <typeparam name="TObj">The type of the objects.</typeparam>
    /// <param name="value1">The first object.</param>
    /// <param name="value2">The second object.</param>
    /// <returns>A list of differing properties along with their values.</returns>
    public static List<AuditLog.UpdateValues> GetDifferingProperties<TObj>(TObj value1, TObj value2)
        {
            var type = value1?.GetType();
            var properties = type?.GetProperties();
            var updatedFields = new List<AuditLog.UpdateValues>();

            if (properties == null) return updatedFields;
            
            foreach (var property in properties)
            {
                if (IsComplexType(property.PropertyType))
                {
                    var subValue1 = property.GetValue(value1);
                    var subValue2 = property.GetValue(value2);

                    var subDifferingProperties = GetDifferingProperties(subValue1, subValue2);

                    if (subDifferingProperties.Any())
                    {
                        // Exclude the complex type itself but include differences within its properties
                        updatedFields.AddRange(subDifferingProperties);
                    }
                }
                else
                {
                    var subValue1 = property.GetValue(value1);
                    var subValue2 = property.GetValue(value2);

                    if (AreEqual(subValue1, subValue2)) continue;

                    var fieldUpdateInfo = new AuditLog.UpdateValues()
                    {
                        FieldName = property.Name,
                        ValueBefore = subValue1?.ToString(),
                        ValueAfter = subValue2?.ToString()
                    };

                    updatedFields.Add(fieldUpdateInfo);
                }
            }


            return updatedFields;
        }

    /// <summary>
    /// Determines if two objects are equal.
    /// </summary>
    /// <param name="value1">The first object.</param>
    /// <param name="value2">The second object.</param>
    /// <returns>True if the objects are equal, otherwise false.</returns>
    private static bool AreEqual(object? value1, object? value2)
        {
            if (value1 == null && value2 == null)
            {
                return true;
            }

            if (value1 == null || value2 == null)
            {
                return false;
            }

            return value1.Equals(value2);
        }

    /// <summary>
    /// Determines if a type is a complex type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type is a complex type, otherwise false.</returns>
    private static bool IsComplexType(Type type)
        {
            return type is { IsPrimitive: false, IsValueType: false } && type != typeof(string);
        }
}