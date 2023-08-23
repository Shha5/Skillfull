using Microsoft.EntityFrameworkCore;
using SkillfullAPI.Data;
using SkillfullAPI.Models.AuthModels;
using SkillfullAPI.Services.Interfaces;

namespace SkillfullAPI.Services
{
    public class DataAccessService : IDataAccessService
    {
        private readonly ApplicationDbContext _appDbContext;
        public DataAccessService(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task SaveRefreshToken(RefreshTokenModel refreshToken)
        {
            await _appDbContext.RefreshTokens.AddAsync(refreshToken);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<RefreshTokenModel> GetRefreshTokenData(string refreshToken)
        {
            return await _appDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);
        }

        public async Task UpdateRefreshToken(RefreshTokenModel refreshToken)
        {
            _appDbContext.RefreshTokens.Update(refreshToken);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
