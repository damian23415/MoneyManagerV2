namespace MoneyManager.Domain.GrpcClients;

public interface IUserGrpcClient
{
    Task<bool> CheckUserExistsAsync(Guid userId);
}