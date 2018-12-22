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

        private SmtpClient _Client;

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
            this._Client = new SmtpClient();
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
        public string Send(string sendForm, string title, string content, params string[] sendTo)
        {
            using (MailMessage message = new MailMessage())
            {
                try
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
                    message.Priority = MailPriority.Normal;

                    this._Client.Send(message);
                    return string.Empty;
                }
                catch (System.Exception ex)
                {
                    return string.Format("邮件发送失败，错误原因：{0}", ex.Message);
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
