using TechChallengeFase1.Domain.Entities;

namespace TechChallengeFase1.Domain.Interfaces;

public interface ICurrencyRepository
{
    Task<Currency?> GetByIdAsync(int currencyId);
    Task<List<Currency>> GetAllAsync();
}
