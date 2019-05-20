using Kira.LaconicInvoicing.Notification.Dtos;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.Notification
{
    public class NoticeHub : Hub
    {
        private readonly IServiceProvider _serviceProvider;

        public NoticeHub(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task NoticeAll(NoticeOutputDto dto)
        {
            await Clients.All.SendAsync("Receiving", dto);
        }

        public async Task Read(Guid id)
        {
            await ExcuteAsync(typeof(NoticeHubService).FullName, "ReadNoticeAsync", new object[] { id, Context.User.Identity.Name });
        }

        public async Task Delete(Guid id)
        {
            await ExcuteAsync(typeof(NoticeHubService).FullName, "DeleteNoticeAsync", new object[] { id, Context.User.Identity.Name });
        }

        public async Task Clear()
        {
            await ExcuteAsync(typeof(NoticeHubService).FullName, "ClearNoticeAsync", new object[] { Context.User.Identity.Name });
        }

        public async Task<dynamic> ExcuteAsync(string serverName, string functionName, object[] parameters)
        {
            return await Task.Factory.StartNew(() =>
            {
                var type = Type.GetType(serverName);
                var service = _serviceProvider.GetRequiredService(type);
                var method = type.GetMethod(functionName);
                var resultTask = method.Invoke(service, parameters) as Task;
                dynamic result = resultTask.GetType().GetProperty("Result").GetValue(resultTask, null);
                return result;
            });
        }

        public async Task ExcuteWithoutResultAsync(string serverName, string functionName, object[] parameters)
        {
            var type = Type.GetType(serverName);
            var service = _serviceProvider.GetRequiredService(type);
            var method = type.GetMethod(functionName);
            var resultTask = method.Invoke(service, parameters) as Task;
            await resultTask;
            var msg = "task done";
            await Clients.Caller.SendAsync("callback", msg);
        }
    }
}
