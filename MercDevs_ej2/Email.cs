using Microsoft.Extensions.Options;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
//ccreo una clase totalmente de nue8vo en donde integro los servicios que consumire en mi controlador ,basicamente este vendria siendo la conexion del servidor de gmail uwuw(assi lo entendi yo)
public class Email
{
    private readonly SmtpSettings _smtpSettings;

    public Email(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachmentData, string attachmentName)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_smtpSettings.UserName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        mailMessage.To.Add(to);

        using (var memoryStream = new MemoryStream(attachmentData))
        {
            mailMessage.Attachments.Add(new Attachment(memoryStream, attachmentName));

            using (var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
            {
                smtpClient.Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password);
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
public class SmtpSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}