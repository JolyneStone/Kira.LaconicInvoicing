using System;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.Notification
{
    public interface INoticeHubContract
    {
        Task ReadNoticeAsync(Guid id, string userName);

        Task DeleteNoticeAsync(Guid id, string userName);
        Task ClearNoticeAsync(string userName);
    }
}