using System.Threading;
using System.Threading.Tasks;

namespace QuickCode.MyecommerceDemo.Common.Mediator
{
    public interface INotificationHandler<TNotification>
        where TNotification : INotification
    {
        Task Handle(TNotification notification, CancellationToken cancellationToken);
    }
} 