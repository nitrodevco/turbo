using System.Text;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using Turbo.Core.Security;

namespace Turbo.Security;

public class DiffieService : IDiffieService
{
    private const int DhPrimesBitSize = 128;
    private readonly BigInteger _dhGenerator;
    private readonly BigInteger _dhPrime;
    private readonly BigInteger _dhPrivate;
    private readonly BigInteger _dhPublic;
    private readonly IRsaService _rsaService;

    public DiffieService(IRsaService rsaService)
    {
        var random = new SecureRandom();
        _rsaService = rsaService;

        // Generate probable primes for DH parameters
        _dhPrime = BigInteger.ProbablePrime(DhPrimesBitSize, random);
        _dhGenerator = BigInteger.ProbablePrime(DhPrimesBitSize, random);

        while (_dhGenerator.CompareTo(_dhPrime) <= 0)
        {
            _dhPrime = BigInteger.ProbablePrime(DhPrimesBitSize, random);
            _dhGenerator = BigInteger.ProbablePrime(DhPrimesBitSize, random);
        }

        (_dhPrime, _dhGenerator) = (_dhGenerator, _dhPrime);

        _dhPrivate = BigInteger.ProbablePrime(DhPrimesBitSize, random);
        _dhPublic = _dhGenerator.ModPow(_dhPrivate, _dhPrime);
    }

    public byte[] GenerateSharedKey(string publicKey)
    {
        var pubKey = DecryptBigInteger(publicKey);
        var sharedKey = pubKey.ModPow(_dhPrivate, _dhPrime);

        return sharedKey.ToByteArrayUnsigned();
    }

    public BigInteger DecryptBigInteger(string str)
    {
        var bytes = Hex.Decode(str);
        var decrypted = _rsaService.Decrypt(bytes);
        var intStr = Encoding.UTF8.GetString(decrypted);

        return new BigInteger(intStr);
    }

    public string GetSignedPrime()
    {
        return EncryptBigInteger(_dhPrime);
    }

    public string GetSignedGenerator()
    {
        return EncryptBigInteger(_dhGenerator);
    }

    public string EncryptBigInteger(BigInteger integer)
    {
        var str = integer.ToString(10);
        var bytes = Encoding.UTF8.GetBytes(str);

        // Use RSA to sign the byte array
        var encrypted = _rsaService.Sign(bytes);

        return Hex.ToHexString(encrypted).ToLower();
    }

    public byte[] GetSharedKey(string publicKeyStr)
    {
        var publicKey = DecryptBigInteger(publicKeyStr);
        var sharedKey = publicKey.ModPow(_dhPrivate, _dhPrime);

        return sharedKey.ToByteArrayUnsigned();
    }

    public string GetPublicKey()
    {
        return EncryptBigInteger(_dhPublic);
    }
}