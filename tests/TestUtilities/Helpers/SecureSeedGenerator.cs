using System.Security.Cryptography;

namespace TestUtilities.Helpers;

public static class SecureSeedGenerator
{
    public static int Generate() => RandomNumberGenerator.GetInt32(1, int.MaxValue);
}
