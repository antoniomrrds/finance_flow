using System.Reflection;

namespace TestUtilities.Extensions;

public static class ShouldMatchExtensions
{
    public static void ShouldMatch<T>(this T actual, T expected, params string[] propertiesToIgnore)
        where T : class
    {
        actual.ShouldNotBeNull();
        expected.ShouldNotBeNull();

        var ignore = new HashSet<string>(propertiesToIgnore);

        PropertyInfo[] properties =
        [
            .. typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => !ignore.Contains(p.Name)),
        ];

        foreach (PropertyInfo prop in properties)
        {
            object? actualValue = prop.GetValue(actual);
            object? expectedValue = prop.GetValue(expected);
            actualValue.ShouldBe(expectedValue, $"Propriedade '{prop.Name}' não confere");
        }
    }
}
