using System.Reflection;

namespace TestUtilities.Helpers;

public static class TypeAssertExtensions
{
    public static void ShouldHavePrivateConstructor(this Type type)
    {
        bool hasPrivateConstructor = type.GetConstructors(
                BindingFlags.Instance | BindingFlags.NonPublic
            )
            .Any(c => c.IsPrivate);

        hasPrivateConstructor.ShouldBeTrue(
            $"Expected {type.Name} to have at least one private constructor."
        );
    }
}
