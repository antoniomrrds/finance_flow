namespace TestUtilities.Helpers;

public static class FakerHelper
{
    private const string Locale = "pt_BR";

    public static Faker<T> CreateFaker<T>()
        where T : class => new(Locale);
}

public static class FakerConstants
{
    public const int DefaultSeed = 42;
}
