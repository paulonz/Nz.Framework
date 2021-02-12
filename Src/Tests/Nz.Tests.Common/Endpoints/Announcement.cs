/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Common.Endpoints
{
    /// <summary>
    /// Lista de endpoints usados pelos testes
    /// </summary>
    public static class Announcement
    {
        /// <summary>
        /// Url base
        /// </summary>
        public static string Base => "https://localhost:5002";

        /// <summary>
        /// Url base
        /// </summary>
        public static string BaseNoPort => "https://localhost";

        /// <summary>
        /// HealthCheck
        /// </summary>
        public static string HealthCheck_v1_0 => "/1.0/HealthCheck";

        /// <summary>
        /// MyAnnouncementsPost
        /// </summary>
        public static string MyAnnouncementsPost_v1_0 => "/1.0/MyAnnouncements";
        /// <summary>
        /// MyAnnouncementsPut
        /// </summary>
        /// <param name="id">Identificador</param>
        public static string MyAnnouncementsPut_v1_0(
            long id)
        {
            return $"/1.0/MyAnnouncements/{id}";
        }
        /// <summary>
        /// MyAnnouncementsDelete
        /// </summary>
        /// <param name="id">Identificador</param>
        public static string MyAnnouncementsDelete_v1_0(
            long id)
        {
            return $"/1.0/MyAnnouncements/{id}";
        }
        /// <summary>
        /// MyAnnouncementsUnDelete
        /// </summary>
        /// <param name="id">Identificador</param>
        public static string MyAnnouncementsUnDelete_v1_0(
            long id)
        {
            return $"/1.0/MyAnnouncements/{id}";
        }
        /// <summary>
        /// MyAnnouncementsGetAll
        /// </summary>
        public static string MyAnnouncementsGetAll_v1_0 => "/1.0/MyAnnouncements/";
        /// <summary>
        /// MyAnnouncementsGetSingle
        /// </summary>
        /// <param name="id">Identificador</param>
        public static string MyAnnouncementsGetSingle_v1_0(
            long id)
        {
            return $"/1.0/MyAnnouncements/{id}";
        }
    }
}
