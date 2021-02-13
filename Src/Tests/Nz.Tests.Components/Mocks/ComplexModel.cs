/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Components.Mocks
{
    /// <summary>
    /// Mock de objeto complexo
    /// </summary>
    public class ComplexModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Array de SimpleModel
        /// </summary>
        public long Code { get; set; }
        public SimpleModel[] SimpleModels { get; set; }
    }
}
