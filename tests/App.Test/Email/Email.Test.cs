using OSharp.Extensions;
using System.Net;
using System.Net.Mail;
using Xunit;

namespace App.Tests.Email
{
    public class EmailTest
    {
        [Fact]
        public void SendTest()
        {
            string host = "smtp.qq.com",
                displayName = "Kira Yoshikage",
                userName = "kirayoshikage@qq.com",
                password = "lcqluyurtcxgbbah";
            SmtpClient client = new SmtpClient(host)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(userName, password)
            };

            string fromEmail = userName.Contains("@") ? userName : "{0}@{1}".FormatWith(userName, client.Host.Replace("smtp.", ""));
            MailMessage mail = new MailMessage
            {
                From = new MailAddress(fromEmail, displayName),
                Subject = "�ʼ����Ͳ���",
                Body = $"�װ����û� <strong>Kira Yoshikage</strong>Kira Yoshikage�����ã�<br>"
                    + $"��ӭע�ᣬ���������� <a href=\"http://localhost:7201\" target=\"_blank\"><strong>�������</strong></a><br>"
                    + $"�������������޷�����������Ը������µ�ַ����ճ����������ĵ�ַ���д򿪡�<br>"
                    + $"http://localhost:4201<br>"
                    + $"ף��ʹ����죡",
                IsBodyHtml = true
            };

            mail.To.Add("forestw@qq.com");
            client.SendMailAsync(mail).GetAwaiter().GetResult();
        }
    }
}   
