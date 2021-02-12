/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Components.Mocks
{
    using System.Security.Cryptography;
    using Nz.Libs.Encryption;

    /// <summary>
    /// Mock para IEncryptionSettings
    /// </summary>
    public class MockEncryptionSettings : IEncryptionSettings
    {
        public HashAlgorithm HashAlgorithm => SHA512.Create();
    }
}
