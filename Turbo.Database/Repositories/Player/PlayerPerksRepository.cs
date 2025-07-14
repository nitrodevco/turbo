using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Database.Entities.Players;
using Turbo.Database.Context;

namespace Turbo.Database.Repositories.Player;

public class PlayerPerksRepository(IEmulatorContext _context) : IPlayerPerksRepository
{

    public async Task<PlayerPerksEntity> FindAsync(int id)
    {
        return await _context.PlayerPerks
            .FindAsync(id);
    }

    public async Task<List<PlayerPerksEntity>> FindAllByPlayerIdAsync(int playerId)
    {
        return await _context.PlayerPerks
            .Where(entity => entity.PlayerEntityId == playerId)
            .ToListAsync();
    }

    public async Task<bool> IsCitizenAsync(int playerId)
    {
        return await _context.PlayerPerks
            .AnyAsync(entity => entity.PlayerEntityId == playerId && entity.Citizen);
    }


    public Task<bool> IsVoteInCompetitionsAsync(int playerId)
    {
        return _context.PlayerPerks
            .AnyAsync(entity => entity.PlayerEntityId == playerId && entity.VoteInCompetitions);
    }

    public Task<bool> IsTrade(int playerId)
    {
        return _context.PlayerPerks
            .AnyAsync(entity => entity.PlayerEntityId == playerId && entity.Trade);
    }

    public Task<bool> IsCallOnHelpersAsync(int playerId)
    {
        return _context.PlayerPerks
            .AnyAsync(entity => entity.PlayerEntityId == playerId && entity.CallOnHelpers);
    }

    public Task<bool> IsJudgeChatReviewsAsync(int playerId)
    {
        return _context.PlayerPerks
            .AnyAsync(entity => entity.PlayerEntityId == playerId && entity.JudgeChatReviews);
    }

    public Task<bool> IsNavigatorRoomThumbnailCameraAsync(int playerId)
    {
        return _context.PlayerPerks
            .AnyAsync(entity => entity.PlayerEntityId == playerId && entity.NavigatorRoomThumbnailCamera);
    }

    public Task<bool> IsUseGuideToolAsync(int playerId)
    {
        return _context.PlayerPerks
            .AnyAsync(entity => entity.PlayerEntityId == playerId && entity.UseGuideTool);
    }

    public Task<bool> IsMouseZoomAsync(int playerId)
    {
        return _context.PlayerPerks
            .AnyAsync(entity => entity.PlayerEntityId == playerId && entity.MouseZoom);
    }

    public Task<bool> IsHabboClubOfferBetaAsync(int playerId)
    {
        return _context.PlayerPerks
            .AnyAsync(entity => entity.PlayerEntityId == playerId && entity.HabboClubOfferBeta);
    }

    public Task<bool> IsNavigatorPhaseTwo2024Async(int playerId)
    {
        return _context.PlayerPerks
            .AnyAsync(entity => entity.PlayerEntityId == playerId && entity.NavigatorPhaseTwo2014);
    }

    public Task<bool> IsUnityTradeAsync(int playerId)
    {
        return _context.PlayerPerks
            .AnyAsync(entity => entity.PlayerEntityId == playerId && entity.UnityTrade);
    }

    public Task<bool> IsBuilderAtWorkAsync(int playerId)
    {
        return _context.PlayerPerks
            .AnyAsync(entity => entity.PlayerEntityId == playerId && entity.BuilderAtWork);
    }

    public Task<bool> IsCameraAsync(int playerId)
    {
        return _context.PlayerPerks
            .AnyAsync(entity => entity.PlayerEntityId == playerId && entity.Camera);
    }
}