using System;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

public class EmailController : Controller
{
    public ActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public ActionResult SendEmail(string recipient, string subject, string body)
    {
        try
        {
            // Gmail SMTP settings
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("jehezkiel1699@gmail.com", "urzxhdcaifigtoly"),
                EnableSsl = true,
            };

            // Create a new email message
            var message = new MailMessage("jehezkiel1699@gmail.com", recipient)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            // Send the email
            smtpClient.Send(message);

            ViewBag.Message = "Email sent successfully!";
        }
        catch (Exception ex)
        {
            ViewBag.Error = "An error occurred: " + ex.Message;
        }

        return View("Index");
    }
}
