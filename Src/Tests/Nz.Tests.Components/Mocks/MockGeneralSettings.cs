/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Components.Mocks
{
    using System;
    using System.Text;
    using Nz.Common.GeneralSettings;

    /// <summary>
    /// Mock para IGeneralSettings
    /// </summary>
    public class MockGeneralSettings : IGeneralSettings
    {
        public DateTime CurrentDateTime => DateTime.UtcNow;

        public Encoding DefaultEncoding => Encoding.UTF8;

        public Uri BaseUri => new Uri("http://localhost");
    }
}
