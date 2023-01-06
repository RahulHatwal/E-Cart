using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;

namespace E_Cart_WebAPI.Filter
{
    public class NotifyExceptionFilter : ExceptionFilterAttribute, IExceptionFilter
    {
        public override void OnException(ExceptionContext filterContext)
        {
            Debug.WriteLine("Inside OnException");
            if (filterContext.Exception != null)
            {
                string fromEmail = "rahulhatwaldoc@gmail.com";
                string toEmail = "rahulhatwaldoc@gmail.com";
                MailMessage mailMessage = new MailMessage(fromEmail, toEmail, "Error in application", filterContext.Exception.Message.ToString());
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(fromEmail, "mdgomhkvbomebnqf");
                try
                {
                    smtpClient.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            //base.OnException(filterContext);
        }
    }
}
