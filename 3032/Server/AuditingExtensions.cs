using System.Text.Json;
using CampaignManagementTool.Server.Repositories.Interfaces;
using CampaignManagementTool.Shared;

namespace CampaignManagementTool.Server;

public class AuditingExtensions
{
    public static T? DeepCopyJson<T>(T input)
    {
        var jsonString = JsonSerializer.Serialize(input);
        return JsonSerializer.Deserialize<T>(jsonString);
    }
    
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

        private static bool IsComplexType(Type type)
        {
            return type is { IsPrimitive: false, IsValueType: false } && type != typeof(string);
        }
}