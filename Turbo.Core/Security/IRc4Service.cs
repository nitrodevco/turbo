namespace Turbo.Core.Security;

public interface IRc4Service
{
    void SetKey(byte[] key);
    byte[] Encrypt(byte[] inputData);
    byte[] Decrypt(byte[] inputData);
}