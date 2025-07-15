using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Database.Entities.Players;

namespace Turbo.Database.Repositories.Player;

public interface IPlayerPerksRepository : IBaseRepository<PlayerPerksEntity>
{
    public Task<List<PlayerPerksEntity>> FindAllByPlayerIdAsync(int playerId);

    public Task<bool> IsCitizenAsync(int playerId);
    public Task<bool> IsVoteInCompetitionsAsync(int playerId);
    public Task<bool> IsTrade(int playerId);
    public Task<bool> IsCallOnHelpersAsync(int playerId);
    public Task<bool> IsJudgeChatReviewsAsync(int playerId);
    public Task<bool> IsNavigatorRoomThumbnailCameraAsync(int playerId);
    public Task<bool> IsUseGuideToolAsync(int playerId);
    public Task<bool> IsMouseZoomAsync(int playerId);
    public Task<bool> IsHabboClubOfferBetaAsync(int playerId);
    public Task<bool> IsNavigatorPhaseTwo2014Async(int playerId);
    public Task<bool> IsUnityTradeAsync(int playerId);
    public Task<bool> IsBuilderAtWorkAsync(int playerId);
    public Task<bool> IsCameraAsync(int playerId);
}