using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;
using Microsoft.Extensions.Options;

using MailKit.Net.Smtp;
using MimeKit;
using MailKit;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Policy;

////using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace ProjectMangaSmurf.Repository
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = "mangasmurf@outlook.com";
            var password = "Phong2107@";

            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(email)); // Cập nhật ở đây
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;

            var builder = new BodyBuilder
            {
                TextBody = body,
                HtmlBody = "<!DOCTYPE html>\r\n<html>\r\n  <head>\r\n    <meta charset=\"utf-8\">\r\n    <meta http-equiv=\"x-ua-compatible\" content=\"ie=edge\">\r\n    <title>Email Receipt</title>\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n    <style type=\"text/css\">\r\n      /**\r\n   * Google webfonts. Recommended to include the .woff version for cross-client compatibility.\r\n   */\r\n      /* media screen {\r\n    font-face {\r\n      font-family: 'Source Sans Pro';\r\n      font-style: normal;\r\n      font-weight: 400;\r\n      src: local('Source Sans Pro Regular'), local('SourceSansPro-Regular'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/ODelI1aHBYDBqgeIAH2zlBM0YzuT7MdOe03otPbuUS0.woff) format('woff');\r\n    }\r\n\r\n    font-face {\r\n      font-family: 'Source Sans Pro';\r\n      font-style: normal;\r\n      font-weight: 700;\r\n      src: local('Source Sans Pro Bold'), local('SourceSansPro-Bold'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/toadOcfmlt9b38dHJxOBGFkQc6VGVFSmCnC_l7QZG60.woff) format('woff');\r\n    }\r\n  } */\r\n      /**\r\n   * Avoid browser level font resizing.\r\n   * 1. Windows Mobile\r\n   * 2. iOS / OSX\r\n   */\r\n      body,\r\n      table,\r\n      td,\r\n      a {\r\n        -ms-text-size-adjust: 100%;\r\n        /* 1 */\r\n        -webkit-text-size-adjust: 100%;\r\n        /* 2 */\r\n      }\r\n\r\n      /**\r\n   * Remove extra space added to tables and cells in Outlook.\r\n   */\r\n      table,\r\n      td {\r\n        mso-table-rspace: 0pt;\r\n        mso-table-lspace: 0pt;\r\n      }\r\n\r\n      /**\r\n   * Better fluid images in Internet Explorer.\r\n   */\r\n      img {\r\n        -ms-interpolation-mode: bicubic;\r\n      }\r\n\r\n      /**\r\n   * Remove blue links for iOS devices.\r\n   */\r\n      a[x-apple-data-detectors] {\r\n        font-family: inherit !important;\r\n        font-size: inherit !important;\r\n        font-weight: inherit !important;\r\n        line-height: inherit !important;\r\n        color: inherit !important;\r\n        text-decoration: none !important;\r\n      }\r\n\r\n      /**\r\n   * Fix centering issues in Android 4.4.\r\n   */\r\n      div[style*=\"margin: 16px 0;\"] {\r\n        margin: 0 !important;\r\n      }\r\n\r\n      body {\r\n        width: 100% !important;\r\n        height: 100% !important;\r\n        padding: 0 !important;\r\n        margin: 0 !important;\r\n      }\r\n\r\n      /**\r\n   * Collapse table borders to avoid space between cells.\r\n   */\r\n      table {\r\n        border-collapse: collapse !important;\r\n      }\r\n\r\n      a {\r\n        color: #1a82e2;\r\n      }\r\n\r\n      img {\r\n        height: auto;\r\n        line-height: 100%;\r\n        text-decoration: none;\r\n        border: 0;\r\n        outline: none;\r\n      }\r\n    </style>\r\n  </head>\r\n  <body style=\"height:100vh;\">\r\n    <!-- start preheader -->\r\n    <div class=\"preheader\" style=\"display: none; max-width: 0; max-height: 0; overflow: hidden; font-size: 1px; line-height: 1px; color: #fff; opacity: 0;\"> A preheader is the short summary text that follows the subject line when an email is viewed in the inbox. </div>\r\n    <!-- end preheader -->\r\n    <!-- start body -->\r\n    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"\">\r\n      <!-- start logo\r\n    <tr><td align=\"center\" bgcolor=\"#D2C7BA\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px; min-height: 00px;\"><tr><td align=\"center\" valign=\"top\" style=\"padding: 36px 24px;\"><a href=\"#\" target=\"_blank\" style=\"display: inline-block;\"><img src=\"~/images/logo2.png\" alt=\"Logo\" border=\"0\" width=\"48\" style=\"display: block; width: 48px; max-width: 48px; min-width: 48px;\"></a></td></tr></table></td></tr> -->\r\n      <tr>\r\n        <td align=\"center\" bgcolor=\"#D2C7BA\">\r\n          <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n            <tr>\r\n              <td align=\"left\" bgcolor=\"#ffffff\" style=\"padding: 36px 24px 0; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; border-top: 3px solid #d4dadf;\">\r\n                <h1 style=\"margin: 0; font-size: 32px; font-weight: 700; letter-spacing: -1px; line-height: 48px;\">THANH TOÁN THÀNH CÔNG</h1>\r\n              </td>\r\n            </tr>\r\n          </table>\r\n        </td>\r\n      </tr>\r\n      <tr>\r\n        <td align=\"center\" bgcolor=\"#D2C7BA\">\r\n          <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n            <tr>\r\n              <td align=\"left\" bgcolor=\"#ffffff\" style=\"padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\">\r\n                <p style=\"margin: 0;\">Đăng kí thành công gói hội viên, mọi thắc mắc về chúng tôi vui lòng liên hệ <a href=\"#\">contact us</a>. </p>\r\n              </td>\r\n            </tr>\r\n            <tr>\r\n              <td align=\"left\" bgcolor=\"#ffffff\" style=\"padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\">\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                  <tr>\r\n                    <td align=\"left\" bgcolor=\"#D2C7BA\" width=\"75%\" style=\"padding: 12px;font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\">\r\n                      <strong>Order #</strong>\r\n                    </td>\r\n                    <td align=\"left\" bgcolor=\"#D2C7BA\" width=\"25%\" style=\"padding: 12px;font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\">\r\n                      <strong>Value</strong>\r\n                    </td>\r\n                  </tr>\r\n                  <tr>\r\n                    <td align=\"left\" width=\"75%\" style=\"padding: 6px 12px;font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\">Premium Member</td>\r\n                    <td align=\"left\" width=\"25%\" style=\"padding: 6px 12px;font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\">69.000 VND</td>\r\n                  </tr>\r\n                </table>\r\n              </td>\r\n            </tr>\r\n            <tr>\r\n              <td align=\"left\" bgcolor=\"#ffffff\" style=\"padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px; display:flex; flex-direction:column; justify-content:center; align-item:center;\">\r\n                <a href=\"#\" style=\"padding: 1rem 2rem; background-color:#1a82e2; color:#fff; font-size: 18px; text-decoration:none; margin:0 auto;\" class=\"btn-home\">Trang Chủ</a>\r\n              </td>\r\n            </tr>\r\n          </table>\r\n        </td>\r\n      </tr>\r\n      <tr></tr>\r\n      <tr>\r\n        <td align=\"center\" bgcolor=\"#D2C7BA\" style=\"padding: 24px;\">\r\n          <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\"></table>\r\n      </tr>\r\n    </table>\r\n  </body>\r\n</html>"
            };

            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(email, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }


    }
}
