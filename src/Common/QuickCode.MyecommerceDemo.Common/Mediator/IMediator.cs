using System.Threading;
using System.Threading.Tasks;

namespace QuickCode.MyecommerceDemo.Common.Mediator
{
    public interface IMediator
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
        Task Publish<TNotification>(INotification notification, CancellationToken cancellationToken = default);
    }
} 