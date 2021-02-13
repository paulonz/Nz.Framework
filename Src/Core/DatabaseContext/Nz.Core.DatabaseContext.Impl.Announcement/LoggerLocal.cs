/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.DatabaseContext.Impl.Announcement
{
    using System;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Implementação Fake de ILogger para ser usado na criação de migrations
    /// </summary>
    internal class LoggerLocal : ILogger
    {
        /// <summary>
        /// FAKE
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public IDisposable BeginScope<TState>(
            TState state)
        {
            return null;
        }

        /// <summary>
        /// FAKE
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public bool IsEnabled(
            LogLevel logLevel)
        {
            return true;
        }

        /// <summary>
        /// FAKE
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
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
