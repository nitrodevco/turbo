using System;
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Turbo.Core.Security;

namespace Turbo.Security;

public class RsaService : IRsaService
{
    private readonly RsaKeyParameters _publicKey;
    private readonly RsaKeyParameters _privateKey;
    private readonly BigInteger _exponent;
    private readonly BigInteger _modulus;
    private readonly BigInteger _privateExponent;
    private readonly int _blockSize;
    
    public RsaService(string exponent, string modulus, string privateExponent)
    {
        _exponent = new BigInteger(exponent, 16);
        _modulus = new BigInteger(modulus, 16);
        _privateExponent = new BigInteger(privateExponent, 16);
        
        _publicKey = new RsaKeyParameters(false, _modulus, _exponent);
        _privateKey = new RsaKeyParameters(true, _modulus, _privateExponent);
        _blockSize = (_modulus.BitLength + 7) / 8;
    }
    
    public byte[] Encrypt(byte[] data)
    {
        IAsymmetricBlockCipher cipher = new Pkcs1Encoding(new RsaEngine());
        cipher.Init(true, _publicKey);
        return cipher.ProcessBlock(data, 0, data.Length);
    }

    public byte[] Decrypt(byte[] data)
    {
        IAsymmetricBlockCipher cipher = new Pkcs1Encoding(new RsaEngine());
        cipher.Init(false, _privateKey);
        return cipher.ProcessBlock(data, 0, data.Length);
    }

    public byte[] Sign(byte[] data)
    {
        IAsymmetricBlockCipher cipher = new Pkcs1Encoding(new RsaEngine());
        cipher.Init(true, _privateKey); // true for encryption mode, using private key as if "signing"
        return ProcessData(cipher, data);
    }

    public bool Verify(byte[] data, byte[] signature)
    {
        ISigner verifier = new PssSigner(new RsaEngine(), new Sha256Digest(), 20);
        verifier.Init(false, _publicKey);
        verifier.BlockUpdate(data, 0, data.Length);
        return verifier.VerifySignature(signature);
    }
    
    private static byte[] ProcessData(IAsymmetricBlockCipher cipher, byte[] data)
    {
        var outputStream = new MemoryStream();
        var chunkSize = cipher.GetInputBlockSize();

        for (var chunkPosition = 0; chunkPosition < data.Length; chunkPosition += chunkSize)
        {
            var chunkLength = Math.Min(chunkSize, data.Length - chunkPosition);
            var chunkResult = cipher.ProcessBlock(data, chunkPosition, chunkLength);
            outputStream.Write(chunkResult, 0, chunkResult.Length);
        }

        return outputStream.ToArray();
    }
}