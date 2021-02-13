/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.Auth
{
    using System.Threading.Tasks;

    /// <summary>
    /// Inicio da aplicação
    /// </summary>
    public class Program : Api.Program
    {
        /// <summary>
        /// Nome da aplicação
        /// </summary>
        public static string ApplicationName => "AuthAPI";

        /// <summary>
        /// Construtor padrão
        /// </summary>
        private Program()
            : base(ApplicationName) { }

        /// <summary>
        /// Método inicial da aplicação
        /// </summary>
        /// <param name="args">Parametros de inicialização</param>
        public static async Task Main(
            string[] args)
        {
            Program program = new Program();

            await program.DoAsync(
                args,
                startupFactory => new Startup(program.CreateLogger<Startup>())).ConfigureAwait(false);
        }
    }
}
