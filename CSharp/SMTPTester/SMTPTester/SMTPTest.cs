using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMTPTester
{
    public partial class SMTPTest : Form
    {
        public SMTPTest()
        {
            InitializeComponent();
        }

        private void btnTestEmail_Click(object sender, EventArgs e)
        {
            try { SendEmailTest(); }
            catch (Exception ex)
            {
                tbResult.Text = "";
                tbResult.AppendText("Error message :\n");
                tbResult.AppendText(ex.Message + "\n");
                if (ex.InnerException != null)
                {
                    tbResult.AppendText("More details :\n");
                    tbResult.AppendText(ex.InnerException + "\n");
                }

            }
        }


        void SendEmailTest()
        {
            MailMessage message = new System.Net.Mail.MailMessage();
            message.To.Add(tbTo.Text);
            message.Subject = tbSubject.Text;
            message.From = new System.Net.Mail.MailAddress(tbFrom.Text, tbFromName.Text);
            SmtpClientEx smtp = new SmtpClientEx(tbServer.Text);

            string messageBodyText = "<br><br>Settings that worked :<br>";
            messageBodyText += "Server : " + tbServer.Text + "<br>";
            messageBodyText += "Port : " + tbPort.Text + "<br>";
            messageBodyText += "SSL/TLS : " + (cbSSL.Checked ? "YES" : "NO") + "<br>";
            messageBodyText += "Username : " + tbUname.Text + "<br>";
            if(!string.IsNullOrEmpty(tbMasquerade.Text)) messageBodyText += "Masquerade : " + tbMasquerade.Text + "<br>";
            messageBodyText += "Machine name : " + smtp.LocalHostName+"<br>";
            message.Body = tbBody.Text +messageBodyText+ "<br>This is a test email to notify that your SMTP settings are correct.<br><br>Thank you<br>https://github.com/VPleckaitis,<br>2016-2019";
            message.IsBodyHtml = true;

            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess | DeliveryNotificationOptions.OnFailure;

            // timeout in milliseconds
            smtp.Timeout = 10000;
            smtp.Port = Convert.ToInt32(tbPort.Text);
            smtp.EnableSsl = cbSSL.Checked;

            if (!String.IsNullOrEmpty(tbMasquerade.Text)) smtp.LocalHostName = tbMasquerade.Text;


            smtp.Credentials = new System.Net.NetworkCredential(tbUname.Text, tbPassword.Text);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            try
            {
                if (cbSSL.Checked)
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
                smtp.Send(message);
                tbResult.Text = "";
                tbResult.AppendText("Email was sent successfully !");
            }
            catch (Exception ex)
            {
                tbResult.Text = "";
                tbResult.AppendText("Error message :\n");
                tbResult.AppendText(ex.Message + "\n");
                if (ex.InnerException != null)
                {
                    tbResult.AppendText("More details :\n");
                    tbResult.AppendText(ex.InnerException + "\n");
                }

            }
        }


        //--------- Test email settings only !!! To check SSL CERT -----------
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;
            else
            {
                if (System.Windows.Forms.MessageBox.Show("The server certificate is not valid.\nAccept?", "Certificate Validation", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    return true;
                }
                else
                    return false;
            }
        }
    }
}
