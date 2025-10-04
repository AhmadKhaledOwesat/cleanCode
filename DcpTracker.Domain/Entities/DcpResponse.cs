namespace MobCentra.Domain.Entities
{
    public record DcpResponse<T>(T Data , string Message = "" , bool IsSuccess=true);
}
