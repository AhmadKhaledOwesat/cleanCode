

namespace DcpTracker.Domain.Interfaces
{
    public interface IDcpMapper
    {
        TDestination Map<TSource , TDestination>(TSource source);
        void Map<TSource, TDestination>(TSource source , TDestination destination);
        TDestination Map<TDestination>(object source);
    }
}
