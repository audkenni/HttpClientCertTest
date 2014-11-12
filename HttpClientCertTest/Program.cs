using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HttpClientCertTest
{
    // All test exmaples send GET /umsokn/1
    class Program
    {
        static string API = "https://kortaumsoknirtest.audkenni.is/api/";

        // There are a couple of ways to do this in .NET. This is using System.Net.WebRequest, the more
        // bare-bone http client. 
        static public JObject GetKortaumsoknWebRequest(int id)
        {    
            X509Certificate2 cert = new X509Certificate2("osk2018test_private.pfx", "");

            // Connect to Auðkenni API
            var uri = string.Format("umsokn/{0}", id);
            var request = (HttpWebRequest)WebRequest.Create(API + uri);
            request.Method = WebRequestMethods.Http.Get;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "application/json";
            request.ClientCertificates.Add(cert);

            using (StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                
                string jsondata = sr.ReadToEnd();
                var json = JsonConvert.DeserializeObject<JObject>(jsondata);
                if (json["error"] != null)
                {
                    throw new InvalidOperationException(json["statuscode"] + ": " + json["error"]);
                }
                return json;
            }
        }

        // This is using System.Net.WebClient, which builds on top of WebRequest and is more handy to use
        static public JObject GetKortaumsoknWebClient(int id)
        {
            var uri = String.Format("umsokn/{0}", id);
            using (ClientCertWebClient client = new ClientCertWebClient())
            {
                client.Headers.Add("User-Agent", "Demo");
                try
                {
                    var jsonstring = client.DownloadString(API + uri);
                    return JsonConvert.DeserializeObject<JObject>(jsonstring);
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        // http error from auðkenni api
                        int statuscode = (int)((HttpWebResponse)ex.Response).StatusCode;
                        throw new InvalidOperationException(String.Format("{0}", statuscode));
                    }
                    // lots of other possible WebExceptionStatus.
                    // choose what you want to handle: http://msdn.microsoft.com/en-us/library/system.net.webexceptionstatus(v=vs.110).aspx
                    throw;
                }
            }
        }

        static void Main(string[] args)
        {
            // Justing printing an arbitary property to do something...
            try
            {
                Console.Write("WebClient: ");
                Console.WriteLine(GetKortaumsoknWebClient(1)["UmNafn"]);
                Console.Write("WebRequest: ");
                Console.WriteLine(GetKortaumsoknWebRequest(1)["UmNafn"]);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
    }
}
