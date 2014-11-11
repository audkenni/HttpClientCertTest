# README

Example code on how to access a https site that requires client certs, made for Advania. 

## Overview

There are (at least) two ways to accomplish this. Using "raw" `System.Net.WebRequest", that lacks a lot of conviences methods or using `System.Net.WebClient", that has more conveniances. 

This example focuses on a RESTlike API. Usually, we would be using RestSharp, but it's documentation doesn't explain how to add client certs, although it probably can be done. (I will update this if I figure it out). 

If using `WebClient`, please see the file `ClientCertWebClient.cs`, since we have to override the `GetWebRequest` method to intercept the underlying `WebRequest` object and add the certificate.

See: http://msdn.microsoft.com/en-us/library/system.net.webclient.getwebrequest(v=vs.110).aspx