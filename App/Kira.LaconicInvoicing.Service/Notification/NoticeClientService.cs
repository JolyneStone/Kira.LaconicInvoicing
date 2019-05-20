using Kira.LaconicInvoicing.Identity.Entities;
using Kira.LaconicInvoicing.Notification.Dtos;
using Kira.LaconicInvoicing.Notification.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OSharp.Entity;
using OSharp.Mapping;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.Notification
{
    /// <summary>
    /// 处理通知业务，发送消息
    /// </summary>
    public class NoticeClientService : INoticeClientContract
    {
        private readonly IRepository<Notice, Guid> _noticeRepo;
        private readonly IRepository<NoticeReceiving, Guid> _noticeReceivingRepo;
        private readonly UserManager<User> _userManager;
        private readonly NoticeClient _client;
        private readonly ILogger<NoticeClientService> _logger;

        public NoticeClientService(IRepository<Notice, Guid> noticeRepo,
              IRepository<NoticeReceiving, Guid> noticeReceivingRepo,
              UserManager<User> userManager,
              NoticeClient client,
              ILogger<NoticeClientService> logger)
        {
            _noticeRepo = noticeRepo;
            _noticeReceivingRepo = noticeReceivingRepo;
            _userManager = userManager;
            _client = client;
            _logger = logger;
        }

        public async Task AddNoticeAsync(NoticeInputDto dto)
        {
            var notice = dto.MapTo<Notice>();
            await _noticeRepo.InsertAsync(notice);
            var userIds = await _userManager.Users.Select(u => u.Id).ToListAsync();
            var noticeReceivings = new NoticeReceiving[userIds.Count];
            for (var i = 0; i < userIds.Count; i++)
            {
                noticeReceivings[i] = new NoticeReceiving
                {
                    Id = Guid.NewGuid(),
                    UserId = userIds[i],
                    NoticeId = dto.Id,
                    IsRead = false
                };
            }

            await _noticeReceivingRepo.InsertAsync(noticeReceivings);
            await _client.AddNoticeAsync(dto);
        }

        public async Task DeleteNoticeByUserAsync(Guid id, string userName)
        {
            var user = _userManager.FindByNameAsync(userName);
            await _noticeReceivingRepo.DeleteBatchAsync(n => n.NoticeId == id && n.UserId == user.Id);
        }

        public async Task DeleteAllNoticeByUserAsync(string userName)
        {
            var user = _userManager.FindByNameAsync(userName);
            await _noticeReceivingRepo.DeleteBatchAsync(n => n.UserId == user.Id);
        }

        public async Task DeleteNoticeAync(Guid id)
        {
            await _noticeReceivingRepo.DeleteBatchAsync(n => n.NoticeId == id);
            await _noticeRepo.DeleteAsync(id);
        }

        public async Task DeleteAllNoticeAync()
        {
            await _noticeReceivingRepo.DeleteBatchAsync(n => true);
            await _noticeRepo.DeleteBatchAsync(n => true);
        }

        public async Task<List<NoticeOutputDto>> GetNoticeAllAsync(int count = 0, int size = 5)
        {
            if (count < 0)
            {
                count = 0;
            }

            if (size < 1)
            {
                size = 1;
            }

            return await _noticeRepo.Query().Skip(count).Take(size).Select(n => n.MapTo<NoticeOutputDto>()).ToListAsync();
        }

        public async Task<List<NoticeOutputDto>> GetNoticeAllByUserAsync(string userName, int count = 0, int size = 5)
        {
            if (count < 0)
            {
                count = 0;
            }

            if (size < 1)
            {
                size = 1;
            }

            var user = await _userManager.FindByNameAsync(userName);
            return await (from notice in _noticeRepo.Query()
                          join noticeReceiving in _noticeReceivingRepo.Query()
                          on notice.Id equals noticeReceiving.NoticeId
                          where noticeReceiving.UserId == user.Id
                          orderby notice.DateTime
                          select new NoticeOutputDto()
                          {
                              Id = notice.Id,
                              Author = notice.Author,
                              Content = notice.Content,
                              DateTime = notice.DateTime,
                              IsRead = noticeReceiving.IsRead
                          })
                          .Skip(count)
                          .Take(size)
                          .ToListAsync();
        }

        public async Task<List<NoticeOutputDto>> GetAllUnReadAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return await (from notice in _noticeRepo.Query()
                          join noticeReceiving in _noticeReceivingRepo.Query()
                          on notice.Id equals noticeReceiving.NoticeId
                          where noticeReceiving.UserId == user.Id
                          orderby notice.DateTime
                          select new NoticeOutputDto()
                          {
                              Id = notice.Id,
                              Author = notice.Author,
                              Content = notice.Content,
                              DateTime = notice.DateTime,
                              IsRead = noticeReceiving.IsRead
                          })
                          .ToListAsync();
        }

        public async Task<int> GetAllUnReadCountAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return await _noticeReceivingRepo.Query().Where(n => n.UserId == user.Id && n.IsRead == false).CountAsync();
        }

        public async Task ReadNoticeAsync(string userName, Guid id)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var noticeReceiving = await _noticeReceivingRepo.GetFirstAsync(n => n.UserId == user.Id && n.NoticeId == id && n.IsRead == false);
            if (noticeReceiving != null) {
                noticeReceiving.IsRead = true;
                await _noticeReceivingRepo.UpdateAsync(noticeReceiving);
            }
        }
    }
}
