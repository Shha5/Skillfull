using SkillfullAPI.Models.AuthModels;

namespace SkillfullAPI.Services.Interfaces
{
    public interface IDataAccessService
    {
        Task<RefreshTokenModel> GetRefreshTokenData(string refreshToken);
        Task SaveRefreshToken(RefreshTokenModel refreshToken);
        Task UpdateRefreshToken(RefreshTokenModel refreshToken);
    }
}