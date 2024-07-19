using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Turbo.Core.Security;

namespace Turbo.Security;

public class Rc4Service : IRc4Service
{
    private readonly RC4Engine _rc4Engine;

    public Rc4Service(byte[] key)
    {
        _rc4Engine = new RC4Engine();
        SetKey(key);
    }

    public void SetKey(byte[] key)
    {
        _rc4Engine.Init(true, new KeyParameter(key));
    }

    public byte[] Encrypt(byte[] inputData)
    {
        var outputData = new byte[inputData.Length];
        _rc4Engine.ProcessBytes(inputData, 0, inputData.Length, outputData, 0);
        return outputData;
    }

    public byte[] Decrypt(byte[] inputData)
    {
        return Encrypt(inputData);
    }
}