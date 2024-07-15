using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Context;
using Turbo.Database.Entities.ChatStyles;

namespace Turbo.Database.Repositories.ChatStyles;

public class ChatStyleRepository(IEmulatorContext context) : IChatStyleRepository
{
    public async Task<List<ChatStyleEntity>> GetAllAsync()
    {
        return await context.ChatStyles.ToListAsync();
    }

    public async Task<ChatStyleEntity> FindAsync(int id)
    {
        return await context.ChatStyles.FirstOrDefaultAsync(e => e.Id == id);
    }
}