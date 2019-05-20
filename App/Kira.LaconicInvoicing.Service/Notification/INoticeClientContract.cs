using Kira.LaconicInvoicing.Notification.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.Notification
{
    public interface INoticeClientContract
    {
        /// <summary>
        /// 添加通知公告
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task AddNoticeAsync(NoticeInputDto dto);

        /// <summary>
        /// 删除用户通知公告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteNoticeByUserAsync(Guid id, string userName);

        /// <summary>
        /// 删除用户所有通知公告
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task DeleteAllNoticeByUserAsync(string userName);

        /// <summary>
        /// 删除通知公告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteNoticeAync(Guid id);

        /// <summary>
        /// 删除所有通知公告
        /// </summary>
        /// <returns></returns>
        Task DeleteAllNoticeAync();

        /// <summary>
        /// 获取所有通知
        /// </summary>
        /// <returns></returns>
        Task<List<NoticeOutputDto>> GetNoticeAllAsync(int page = 1, int pageSize = 5);

        /// <summary>
        /// 获取用户所有通知
        /// </summary>
        /// <returns></returns>
        Task<List<NoticeOutputDto>> GetNoticeAllByUserAsync(string userName, int page = 1, int pageSize = 5);

        /// <summary>
        /// 获取用户所有未读通知
        /// </summary>
        /// <param name="userName"></param>
        Task<List<NoticeOutputDto>> GetAllUnReadAsync(string userName);

        /// <summary>
        /// 获取用户所有未读通知数
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<int> GetAllUnReadCountAsync(string userName);

        /// <summary>
        /// 阅读通知
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task ReadNoticeAsync(string userName, Guid id);
    }
}
