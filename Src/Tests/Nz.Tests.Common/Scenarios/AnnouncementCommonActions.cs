/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Common.Scenarios
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Newtonsoft.Json;
    using Nz.Tests.Common.Models.Announcement;

    public class AnnouncementCommonActions
    {
        /// <summary>
        /// Cliente Http
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="httpClient">Cliente Http</param>
        public AnnouncementCommonActions(
            HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Cria um novo anúncio para o usuário autenticado
        /// </summary>
        /// <param name="createMyAnnouncementRequest">Dados para a criação de um novo anúncio</param>
        /// <param name="authToken">Token de autenticação</param>
        /// <returns>Sucesso</returns>
        public async Task<CreateMyAnnouncementResponse> CreateMyAnnouncementAsync(
            CreateMyAnnouncementRequest createMyAnnouncementRequest,
            string authToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            HttpResponseMessage response = await _httpClient.PostAsync(
                Endpoints.Announcement.MyAnnouncementsPost_v1_0,
                createMyAnnouncementRequest.ToStringContent()).ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            string bodyResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            CreateMyAnnouncementResponse createMyAnnouncementResponse = JsonConvert.DeserializeObject<CreateMyAnnouncementResponse>(
                    bodyResponse);

            createMyAnnouncementResponse.Should().NotBeNull();
            createMyAnnouncementResponse.CreatedBy.Should().NotBeNull();

            return createMyAnnouncementResponse;
        }

        /// <summary>
        /// Remove um anúncio do usuário autenticado
        /// </summary>
        /// <param name="id">Identificador do anúncio</param>
        /// <param name="authToken">Autenticação</param>
        /// <returns>Sucesso</returns>
        public async Task<RemoveMyAnnouncementResponse> RemoveMyAnnouncementAsync(
            long id,
            string authToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            HttpResponseMessage response = await _httpClient.DeleteAsync(
                Endpoints.Announcement.MyAnnouncementsDelete_v1_0(id)).ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            string bodyResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            RemoveMyAnnouncementResponse removeMyAnnouncementResponse = JsonConvert.DeserializeObject<RemoveMyAnnouncementResponse>(
                    bodyResponse);

            removeMyAnnouncementResponse.Should().NotBeNull();
            removeMyAnnouncementResponse.Id.Should().Be(id);
            removeMyAnnouncementResponse.ExcludedOn.Should().NotBeNull();

            return removeMyAnnouncementResponse;
        }

        /// <summary>
        /// Atualiza os dados de um anúncio
        /// </summary>
        /// <param name="id">Identificador do anúncio</param>
        /// <param name="updateMyAnnouncementRequest">Novos dados</param>
        /// <param name="authToken">Autenticação</param>
        /// <returns>Sucesso</returns>
        public async Task<UpdateMyAnnouncementResponse> UpdateMyAnnouncementAsync(
            long id,
            UpdateMyAnnouncementRequest updateMyAnnouncementRequest,
            string authToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            HttpResponseMessage response = await _httpClient.PutAsync(
                Endpoints.Announcement.MyAnnouncementsPut_v1_0(id),
                updateMyAnnouncementRequest.ToStringContent()).ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            string bodyResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            UpdateMyAnnouncementResponse updateMyAnnouncementResponse = JsonConvert.DeserializeObject<UpdateMyAnnouncementResponse>(
                    bodyResponse);

            updateMyAnnouncementResponse.Should().NotBeNull();
            updateMyAnnouncementResponse.UpdatedOn.Should().NotBeNull();
            updateMyAnnouncementResponse.Id.Should().Be(id);

            return updateMyAnnouncementResponse;
        }
    }
}
