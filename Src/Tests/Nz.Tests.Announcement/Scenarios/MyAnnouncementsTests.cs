/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Announcements.Scenarios
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Nz.Tests.Common.Models.Announcement;
    using Nz.Tests.Common.Scenarios;
    using Xunit;

    /// <summary>
    /// Testes MyAnnouncements
    /// </summary>
    public class MyAnnouncementsTests : AnnouncementTestsBase
    {
        /// <summary>
        /// Ações comuns entre cenários
        /// </summary>
        private readonly AnnouncementCommonActions _announcementCommonActions;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public MyAnnouncementsTests()
        {
            _announcementCommonActions = new AnnouncementCommonActions(HttpClient);
        }

        /// <summary>
        /// Usuário com a conta validada cria um novo anúncio para um produto que deseja vender
        /// </summary>
        /// <returns>Sucesso</returns>
        [Fact]
        public async Task create_my_announcements_happy_path()
        {
            await _announcementCommonActions.CreateMyAnnouncementAsync(new CreateMyAnnouncementRequest()
            {
                Description = "I am selling an almost new ps3 video game.",
                Price = 199,
                Title = "Almost new ps3"
            }, NoRoleToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Usuário remove um de seus anúncios
        /// </summary>
        /// <returns>Sucesso</returns>
        [Fact]
        public async Task remove_my_announcements_happy_path()
        {
            CreateMyAnnouncementResponse createMyAnnouncementResponse = await _announcementCommonActions.CreateMyAnnouncementAsync(new CreateMyAnnouncementRequest()
            {
                Description = "I am selling an almost new ps3 video game.",
                Price = 199,
                Title = "Almost new ps3"
            }, NoRoleToken).ConfigureAwait(false);

            await _announcementCommonActions.RemoveMyAnnouncementAsync(createMyAnnouncementResponse.Id, NoRoleToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Usuário atualiza o preço para um de seus anúncios
        /// </summary>
        /// <returns>Sucesso</returns>
        [Fact]
        public async Task update_my_announcements_happy_path()
        {
            CreateMyAnnouncementResponse createMyAnnouncementResponse = await _announcementCommonActions.CreateMyAnnouncementAsync(new CreateMyAnnouncementRequest()
            {
                Description = "I am selling an almost new ps3 video game.",
                Price = 199,
                Title = "Almost new ps3"
            }, NoRoleToken).ConfigureAwait(false);

            UpdateMyAnnouncementResponse updateMyAnnouncementResponse = await _announcementCommonActions.UpdateMyAnnouncementAsync(createMyAnnouncementResponse.Id, new UpdateMyAnnouncementRequest()
            {
                Description = "I am selling an almost new ps3 video game.",
                Price = 99,
                Title = "Almost new ps3"
            }, NoRoleToken).ConfigureAwait(false);

            updateMyAnnouncementResponse.Price.Should().Be(99);
        }
    }
}
