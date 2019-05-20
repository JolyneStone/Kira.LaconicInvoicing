using Kira.LaconicInvoicing.Identity.Entities;
using Kira.LaconicInvoicing.Notification.Dtos;
using Kira.LaconicInvoicing.Notification.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OSharp.Entity;
using System;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.Notification
{
    public class NoticeHubService : INoticeHubContract
    {
        private readonly IRepository<Notice, Guid> _noticeRepo;
        private readonly IRepository<NoticeReceiving, Guid> _noticeReceivingRepo;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<NoticeHubService> _logger;

        public NoticeHubService(IRepository<Notice, Guid> noticeRepo,
            IRepository<NoticeReceiving, Guid> noticeReceivingRepo,
            UserManager<User> userManager,
            ILogger<NoticeHubService> logger)
        {
            _noticeRepo = noticeRepo;
            _noticeReceivingRepo = noticeReceivingRepo;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task ReadNoticeAsync(Guid id, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var noticeReceiving = await _noticeReceivingRepo.GetFirstAsync(n => n.NoticeId == id && n.UserId == user.Id);
            noticeReceiving.IsRead = true;
            await _noticeReceivingRepo.UpdateAsync(noticeReceiving);
        }

        public async Task DeleteNoticeAsync(Guid id, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var noticeReceiving = await _noticeReceivingRepo.GetFirstAsync(n => n.NoticeId == id && n.UserId == user.Id);
            await _noticeReceivingRepo.DeleteAsync(noticeReceiving);
        }
        public async Task ClearNoticeAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            await _noticeReceivingRepo.DeleteBatchAsync(n=>n.UserId == user.Id);
        }
    }
}