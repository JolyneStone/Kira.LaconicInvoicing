using Kira.LaconicInvoicing.Notification.Dtos;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.Notification
{
    public class NoticeClient
    {
        private readonly HubConnection connection;

        public NoticeClient()
        {
            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:7001/noticehub")
                //.AddMessagePackProtocol()
                .Build();

            connection.Closed += async (error) =>
              {
                  await Task.Delay(new Random().Next(0, 5) * 1000);
                  await connection.StartAsync();
              };

            InitOnMethod();
            connection.StartAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private void InitOnMethod()
        {
            connection.On<string>("callback", (msg) => {
            });
        }

        public async Task AddNoticeAsync(NoticeInputDto dto)
        {
            await connection.InvokeAsync("NoticeAll", dto);
        }

        public async Task<dynamic> ExcuteAsync(string serverName, string functionName, object[] parameters)
        {
            var result = await connection.InvokeAsync<dynamic>("ExcuteAsync", serverName, functionName, parameters);
            return result;
        }

        public async Task RequestWithoutResultAsync(string serverName, string functionName, object[] parameters)
        {
            await connection.InvokeAsync("ExcuteWithoutResultAsync", serverName, functionName, parameters);
        }
    }
}
