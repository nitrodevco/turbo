namespace Turbo.Core.Security;

public interface IRsaService
{
    byte[] Encrypt(byte[] data);
    byte[] Decrypt(byte[] data);
    byte[] Sign(byte[] data);
    bool Verify(byte[] data, byte[] signature);
}