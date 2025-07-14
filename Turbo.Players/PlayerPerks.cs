using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Game.Players;
using Turbo.Core.Utilities;
using Turbo.Database.Repositories.Player;

namespace Turbo.Players;

public class PlayerPerks(
    IPlayer _player,
    IServiceScopeFactory _serviceScopeFactory) : Component, IPlayerPerks
{

    protected override async Task OnInit()
    {
        await LoadPerks();
    }

    protected override async Task OnDispose()
    {
    }

    private async Task LoadPerks()
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var perksRepository = scope.ServiceProvider.GetService<IPlayerPerksRepository>();

        var entities = await perksRepository.FindAllByPlayerIdAsync(_player.Id);

        if (entities != null)
            foreach (var perkEntity in entities)
            {
            }
    }

    public async Task<bool> HasPerkAsync(string perk)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var perksRepository = scope.ServiceProvider.GetService<IPlayerPerksRepository>();

        switch (perk)
        {
            // TODO: Find a better way to do this. I'm sorry...
            case "CITIZEN":
                return await perksRepository.IsCitizenAsync(_player.Id);
            case "VOTE_IN_COMPETITIONS":
                return await perksRepository.IsVoteInCompetitionsAsync(_player.Id);
            case "TRADE":
                return await perksRepository.IsTrade(_player.Id);
            case "CALL_ON_HELPERS":
                return await perksRepository.IsCallOnHelpersAsync(_player.Id);
            case "JUDGE_CHAT_REVIEWS":
                return await perksRepository.IsJudgeChatReviewsAsync(_player.Id);
            case "NAVIGATOR_ROOM_THUMBNAIL_CAMERA":
                return await perksRepository.IsNavigatorRoomThumbnailCameraAsync(_player.Id);
            case "USE_GUIDE_TOOL":
                return await perksRepository.IsUseGuideToolAsync(_player.Id);
            case "MOUSE_ZOOM":
                return await perksRepository.IsMouseZoomAsync(_player.Id);
            case "HABBO_CLUB_OFFER_BETA":
                return await perksRepository.IsHabboClubOfferBetaAsync(_player.Id);
            case "NAVIGATOR_PHASE_TWO_2014":
                return await perksRepository.IsNavigatorPhaseTwo2024Async(_player.Id);
            case "UNITY_TRADE":
                return await perksRepository.IsUnityTradeAsync(_player.Id);
            case "BUILDER_AT_WORK":
                return await perksRepository.IsBuilderAtWorkAsync(_player.Id);
            case "CAMERA":
                return await perksRepository.IsCameraAsync(_player.Id);
            default:
                return false;

        }
    }
}