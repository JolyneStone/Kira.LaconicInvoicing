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
                Subject = "邮件发送测试",
                Body = $"亲爱的用户 <strong>Kira Yoshikage</strong>Kira Yoshikage，您好！<br>"
                    + $"欢迎注册，激活邮箱请 <a href=\"http://localhost:7201\" target=\"_blank\"><strong>点击这里</strong></a><br>"
                    + $"如果上面的链接无法点击，您可以复制以下地址，并粘贴到浏览器的地址栏中打开。<br>"
                    + $"http://localhost:4201<br>"
                    + $"祝您使用愉快！",
                IsBodyHtml = true
            };

            mail.To.Add("forestw@qq.com");
            client.SendMailAsync(mail).GetAwaiter().GetResult();
        }
    }
}   
