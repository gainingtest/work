using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace TianQin365.Common.Encrypt
{
    public class EncryptionProcessingHandler : MessageProcessingHandler
    {
        private TripleDES _3des = new TripleDES();

        protected override HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Content.IsMimeMultipartContent())
                return request;

            var encryptionType = HttpContext.Current.Request.Headers.GetValues("encryptiontype")?.First().ToUpper();
            if (!string.IsNullOrEmpty(encryptionType))
            {
                if (request.Method == HttpMethod.Get)
                {
                    if (!string.IsNullOrEmpty(request.RequestUri.Query))
                    {
                        var baseQuery = this.decryptString(request.RequestUri.Query.Substring(3), getKey(), encryptionType);
                        request.RequestUri = new Uri($"{request.RequestUri.AbsoluteUri.Split('?')[0]}?{baseQuery}"); // 重置解密后的URL请求
                    }
                }
                else
                {
                    var baseContent = request.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrEmpty(baseContent))
                    {
                        baseContent = this.decryptString(baseContent.Substring(2), getKey(), encryptionType);
                        request.Content = new StringContent(baseContent, System.Text.Encoding.UTF8, "application/json");
                    }
                }
            }

            return request;
        }

        protected override HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var encryptionType = HttpContext.Current.Request.Headers.GetValues("encryptiontype")?.First().ToUpper();
                if (!string.IsNullOrEmpty(encryptionType))
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var encodeResult = this.encryptString(result, getKey());
                    response.Content = new StringContent(encodeResult);
                }
            }

            return response;
        }

        private string getKey()
        {
            // 3des key为accesstoken的前24个字母
            var auth = HttpContext.Current.Request.Headers.GetValues("authorization")?.First();
            return string.Join("", auth.Skip(7).Where(m => (m >= 'a' && m <= 'z') || (m >= 'A' && m <= 'Z')).Take(24)); 
        }
        private string decryptString(string datas, string key, string encrypt = "3DES")
        {
            datas = Regex.Match(datas, "(code=)*(?<code>[\\S]+)").Groups[2].Value;
            datas = HttpUtility.UrlDecode(datas);

            return this._3des.Decrypt(datas, key);
        }
        private string encryptString(string datas, string key, string encrypt = "3DES")
        {
            return this._3des.Encrypt(datas, key);
        }
    }
}
