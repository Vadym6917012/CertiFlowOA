using Application.Common.Interfaces;
using Domain.Entities.GoogleOAuth;
using Infrastructure.Data;
using System.Data.Entity;
using System.Linq.Expressions;


namespace Infrastructure.Services
{
    public class GoogleTokenService : IGenericRepository<GoogleToken>
    {
        private readonly DataContext _ctx;

        public GoogleTokenService(DataContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<GoogleToken> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _ctx.Set<GoogleToken>().FindAsync(new object [] { id }, cancellationToken);
        }

        public async Task<IEnumerable<GoogleToken>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _ctx.Set<GoogleToken>().ToListAsync(cancellationToken);
        }

        public async Task AddAsync(GoogleToken entity, CancellationToken cancellationToken = default)
        {
            await _ctx.Set<GoogleToken>().AddAsync(entity, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(GoogleToken entity, CancellationToken cancellationToken = default)
        {
            _ctx.Set<GoogleToken>().Update(entity);
            await _ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(GoogleToken entity, CancellationToken cancellationToken = default)
        {
            _ctx.Set<GoogleToken>().Remove(entity);
            await _ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task<GoogleToken> FindAsync(Expression<Func<GoogleToken, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _ctx.Set<GoogleToken>().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        // Специфічний метод для GoogleToken
        public async Task<GoogleToken> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _ctx.Set<GoogleToken>().FirstOrDefaultAsync(t => t.UserId == userId, cancellationToken);
        }
    }
}
