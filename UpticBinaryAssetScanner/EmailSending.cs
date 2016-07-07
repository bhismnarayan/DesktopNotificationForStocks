using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using AegisImplicitMail;

namespace UpticBinaryAssetScanner
{
    public  class EmailSending
    {
       
        public static void SendEmail(string emailTo, string subject, string body)
        {
            string smtpAddress = "smtp.gmail.com";
            int portNumber = 587;
            bool enableSSL = true;
            string emailFrom = "bhismnarayan@gmail.com";
            string password = "Aricent12#4";
            
            
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                try
                {
                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
//                                          
                        smtp.UseDefaultCredentials = false;
                        smtp.EnableSsl = enableSSL;                        
                        smtp.Credentials = new System.Net.NetworkCredential(emailFrom,password);
                        smtp.Send(mail);
                        //MessageBox.Show("mail Send");
                    }
                }
           catch (Exception ex)
                {

                    throw ex;
                }
                
            }
        }

        public static void EmailSend(string mail, string subject, string body)
        {
            var host = "smtp.gmail.com";
            var user = "uptickscanner@gmail.com ";
            var pass = "Mass1959";
                        //Generate Message 
            var mymessage = new MimeMailMessage();
            mymessage.From = new MimeMailAddress(mail);
            mymessage.To.Add(mail);
            mymessage.Subject = subject;
            mymessage.Body = body;

            //Create Smtp Client
            var mailer = new MimeMailer(host, 465);
            mailer.User = user;
            mailer.Password = pass;
            mailer.SslType = SslMode.Ssl;
            mailer.AuthenticationMode = AuthenticationType.Base64;

            //Set a delegate function for call back
            // mailer.SendCompleted += compEvent;
            try
            {
                mailer.SendMailAsync(mymessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }

        }

    }
}
