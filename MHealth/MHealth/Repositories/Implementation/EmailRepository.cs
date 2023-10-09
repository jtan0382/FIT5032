using MHealth.Repositories.Abstract;
using System.Net;
using System.Net.Mail;

namespace MHealth.Repositories.Implementation
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IConfiguration _configuration;

        public EmailRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(string fromEmail, string toEmail, string subject, string message, string attachmentPath = null)
        {
            //var mail = _configuration["EmailSMTPService"];
            //var pw = _configuration["EmailPasswordSMTPService"];

            //var client = new SmtpClient("smtp.gmail.com", 587)
            //{
            //    Credentials = new NetworkCredential(mail, pw),
            //    EnableSsl = true
            //};

            //return client.SendMailAsync(
            //    new MailMessage(
            //        from: fromEmail,
            //        to: toEmail,
            //        subject,
            //        message
            //        ));

            var mail = _configuration["EmailSMTPService"];
            var pw = _configuration["EmailPasswordSMTPService"];


            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.Credentials = new NetworkCredential(mail, pw);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true, // You can set this to false if you're sending plain text emails
                    To = { new MailAddress(toEmail) }
                };
                //// Attach the file
                //var attachment = new Attachment(attachmentPath);
                //mailMessage.Attachments.Add(attachment);

                if (!string.IsNullOrEmpty(attachmentPath))
                {
                    // Attach the file if attachmentPath is provided
                    var attachment = new Attachment(attachmentPath);
                    mailMessage.Attachments.Add(attachment);
                }


                try
                {
                    // Send the email asynchronously
                    await client.SendMailAsync(mailMessage);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }
                finally
                {
                    if (!string.IsNullOrEmpty(attachmentPath))
                    {
                        mailMessage.Attachments[0].Dispose();
                    }
                    mailMessage.Dispose();
                }
            }
        }
    }
}
