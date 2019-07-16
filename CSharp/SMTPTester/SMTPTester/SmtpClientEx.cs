using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SMTPTester
{  // SmtpClient override
    /// <summary>
    /// based on https://social.msdn.microsoft.com/Forums/en-US/77f45c5f-76be-400c-a529-a1e49d6d8e62/systemnetmailsmtpclient-fqdn-required?forum=netfxnetcom

    ///Tweaked for .net 4+ by :
    ///http://www.codingtiger.com/questions/.net/SmtpException-(Syntactically-invalid-EHLO-argument).html
    /// </summary>
    public class SmtpClientEx : SmtpClient
    {
        // Private data
        private static readonly FieldInfo localHostName = GetLocalHostNameField();

        public SmtpClientEx(string host, int port)
            : base(host, port)
        {
            Initialize();
        }

        public SmtpClientEx(string host)
            : base(host)
        {
            Initialize();
        }

        public SmtpClientEx()
        {
            Initialize();
        }

        public string LocalHostName
        {
            get
            {
                if (null == localHostName) return null;
                return (string)localHostName.GetValue(this);
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                { throw new ArgumentNullException("value"); }
                if (null != localHostName)
                    localHostName.SetValue(this, value);
            }
        }

        //methods
        private static FieldInfo GetLocalHostNameField()
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            System.Reflection.FieldInfo result = typeof(SmtpClient).GetField("clientDomain", flags);
            if (null == result)
                result = typeof(SmtpClient).GetField("localHostName", flags);
            return result;
        }

        private void Initialize()
        {
            IPGlobalProperties ip = IPGlobalProperties.GetIPGlobalProperties();
            if (!string.IsNullOrEmpty(ip.HostName) && !string.IsNullOrEmpty(ip.DomainName))
                this.LocalHostName = ip.HostName + "." + ip.DomainName;

        }
    }
}
