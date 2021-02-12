/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Classe base para models
    /// </summary>
    public class ModelBase : IModel
    {
        /// <summary>
        /// Identificador único do registro
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.ModelBase_Id))]
        public long Id { get; set; }

        /// <summary>
        /// Data em que o objeto foi criado
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.ModelBase_CreatedOn))]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Usuário que criou o objeto
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.ModelBase_CreatedBy))]
        public long? CreatedBy { get; set; }

        /// <summary>
        /// Data em que o registro foi excluído
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.ModelBase_ExcludedOn))]
        [DataType(DataType.DateTime)]
        public DateTime? ExcludedOn { get; set; }

        /// <summary>
        /// Usuário que excluíu o objeto
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.ModelBase_ExcludedBy))]
        public long? ExcludedBy { get; set; }

        /// <summary>
        /// Data em que o objeto foi atualizado pela última vez
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.ModelBase_UpdatedOn))]
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// Último usuário que atualizou o objeto
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.ModelBase_UpdatedBy))]
        public long? UpdatedBy { get; set; }

        /// <summary>
        /// Objeto para string (json)
        /// </summary>
        /// <returns>String json do objeto</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(
                    this,
                    Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    });
        }
    }
}
