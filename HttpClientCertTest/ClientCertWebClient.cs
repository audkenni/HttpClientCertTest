using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace HttpClientCertTest
{
    class ClientCertWebClient : WebClient
    {
        public string certfile = "osk2018test_private.pfx";
        public string certpass = "";

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.ClientCertificates.Add(new X509Certificate2(certfile, certpass));
            return request;
        }
    }
}
