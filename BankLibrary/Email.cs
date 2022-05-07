using System.Net;
using System.Net.Mail;

namespace BankLibrary;
public class Email
{
    public const int Port = 587;
    public const string Host = "smtp.gmail.com";
    public static void Send(string toMailAddress, string htmlMessage, string title)
    {
        try
        {
            var message = new MailMessage();
            var smtp = new SmtpClient();
            message.From = new MailAddress("kolpashikovaleksey@gmail.com");
            message.To.Add(new MailAddress(toMailAddress));
            message.Subject = title;
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = htmlMessage;
            smtp.Port = Port;
            smtp.Host = Host;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }
        catch (Exception) { }
    }
}