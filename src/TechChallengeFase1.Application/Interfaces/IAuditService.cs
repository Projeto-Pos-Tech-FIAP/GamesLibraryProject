using TechChallengeFase1.Domain.Entities;

namespace TechChallengeFase1.Application.Interfaces;

public interface IAuditService
{
    Task SaveAuditLogsAsync(IEnumerable<AuditLog> logs, CancellationToken cancellationToken = default);
}
