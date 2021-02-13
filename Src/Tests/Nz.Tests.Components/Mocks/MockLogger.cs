/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Components.Mocks
{
    using System;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Mock para ILogger
    /// </summary>
    /// <typeparam name="T">Tipo do objeto</typeparam>
    public class MockLogger<T> : ILogger<T>
    {
        /// <summary>
        /// FAKE
        /// </summary>
        /// <typeparam name="TState">FAKE</typeparam>
        /// <param name="state">FAKE</param>
        /// <returns>FAKE</returns>
        public IDisposable BeginScope<TState>(
            TState state)
        {
            return null;
        }

        /// <summary>
        /// FAKE
        /// </summary>
        /// <param name="logLevel">FAKE</param>
        /// <returns>FAKE</returns>
        public bool IsEnabled(
            LogLevel logLevel)
        {
            return true;
        }

        /// <summary>
        /// FAKE
        /// </summary>
        /// <typeparam name="TState">FAKE</typeparam>
        /// <param name="logLevel">FAKE</param>
        /// <param name="eventId">FAKE</param>
        /// <param name="state">FAKE</param>
        /// <param name="exception">FAKE</param>
        /// <param name="formatter">FAKE</param>
        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {

        }
    }
}
