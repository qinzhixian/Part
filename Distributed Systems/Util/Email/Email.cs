using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;

namespace Util
{
    /// <summary>
    /// 邮件帮助类
    /// </summary>
    public class Email : IDisposable
    {
        #region 私有成员

        private SmtpClient _Client = new SmtpClient();

        private string useName = string.Empty;
        private string password = string.Empty;
        private string smtp = string.Empty;
        private string sendForm = string.Empty;

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="userName">发送者账号</param>
        /// <param name="password">发送者密码</param>
        /// <param name="smtp">Smtp地址</param>
        /// <param name="port">端口</param>
        public Email(string userName, string password, string smtp, int port)
        {
            this._Client.UseDefaultCredentials = true;
            this._Client.Credentials = new NetworkCredential(userName, password);
            this._Client.Port = port;
            this._Client.Host = smtp;
            this._Client.EnableSsl = true;
        }
        
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="sendForm">发送者</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件正文</param>
        /// <param name="sendTo">收件人</param>
        public void Send(string sendForm, string title, string content, params string[] sendTo)
        {
            using (MailMessage message = new MailMessage())
            {
                foreach (string str in sendTo)
                {
                    message.To.Add(str);
                }
                message.From = new MailAddress(sendForm);
                message.Subject = title;
                message.SubjectEncoding = Encoding.UTF8;
                message.Body = content;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                message.Priority = MailPriority.Normal
;
                try
                {
                    this._Client.Send(message);
                }
                catch (System.Exception ex)
                {
                    throw new Util.Exception("邮件发送失败！", ex);
                }
            }
        }
        

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (this._Client != null)
            {
                this._Client = null;
            }
        }

    }



}
