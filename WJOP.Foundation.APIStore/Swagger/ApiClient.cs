using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WJOP.Foundation.APIStore.Swagger
{
    public class ApiClient
    {
        private JsonSerializerSettings serializerSettings;
        public Configuration Configuration { get; set; }
        public RestClient RestClient { get; set; }

        #region ctor
        public ApiClient()
        {
            this.serializerSettings = new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };
            this.Configuration = Configuration.Default;
            this.RestClient = new RestClient("http:127.0.0.1:8888");
        }
        public ApiClient(Configuration config = null)
        {
            this.serializerSettings = new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };
            if (config != null)
            {
                Configuration = config;
            }
            else
            {
                this.Configuration = Configuration.Default;
            }
            this.RestClient = new RestClient("http://127.0.0.1:8888");
        }
        public ApiClient(string basePath = "http://127.0.0.1:8888")
        {
            this.serializerSettings = new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };
            if (string.IsNullOrEmpty(basePath))
            {
                throw new ArgumentException("basePath cannot be empty");
            }
            this.RestClient = new RestClient(basePath);
            this.Configuration = Configuration.Default;
        }

        #endregion

        public static string Base64Encode(string text)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }
        public static dynamic ConvertType(dynamic source, Type dest)
        {
            return Convert.ChangeType(source, dest);
        }

        #region call api
        public object CallApi(string path, Method method, Dictionary<string, string> queryParams,
            object postBody, Dictionary<string, string> headerParams,
            Dictionary<string, string> formParams, Dictionary<string, FileParameter> fileParams,
            Dictionary<string, string> pathParams, string contentType)
        {
            IntegrateURLWithConsul();
            RestRequest restRequest = PrepareRequest(path, method, queryParams, postBody, headerParams, formParams, fileParams, pathParams, contentType);
            RestClient.Timeout = Configuration.Timeout;
            RestClient.UserAgent = Configuration.UserAgent;
            return this.RestClient.Execute(restRequest);
        }
        public async Task<object> CallApiAsync(string path, Method method,
            Dictionary<string, string> queryParams, object postBody,
            Dictionary<string, string> headerParams, Dictionary<string, string> formParams,
            Dictionary<string, FileParameter> fileParams, Dictionary<string, string> pathParams,
            string contentType)
        {
            this.IntegrateURLWithConsul();
            RestRequest restRequest = this.PrepareRequest(path, method, queryParams, postBody, headerParams, formParams, fileParams, pathParams, contentType);
            return await this.RestClient.ExecuteTaskAsync(restRequest);
        }
        #endregion

        private RestRequest PrepareRequest(string path, Method method, Dictionary<string, string> queryParams, object postBody, Dictionary<string, string> headerParams, Dictionary<string, string> formParams, Dictionary<string, FileParameter> fileParams, Dictionary<string, string> pathParams, string contentType)
        {
            RestRequest restRequest = new RestRequest(path, method);
            foreach (KeyValuePair<string, string> pathParam in pathParams)
            {
                restRequest.AddParameter(pathParam.Key, pathParam.Value, ParameterType.UrlSegment);
            }
            foreach (KeyValuePair<string, string> headerParam in headerParams)
            {
                restRequest.AddHeader(headerParam.Key, headerParam.Value);
            }
            foreach (KeyValuePair<string, string> queryParam in queryParams)
            {
                restRequest.AddQueryParameter(queryParam.Key, queryParam.Value);
            }
            foreach (KeyValuePair<string, string> formParam in formParams)
            {
                restRequest.AddParameter(formParam.Key, formParam.Value);
            }
            foreach (KeyValuePair<string, FileParameter> fileParam in fileParams)
            {
                restRequest.AddFile(fileParam.Value.Name, fileParam.Value.Writer, fileParam.Value.FileName, fileParam.Value.ContentType);
            }
            if (postBody != null)
            {
                if (postBody.GetType() == typeof(string))
                {
                    restRequest.AddParameter("application/json", postBody, ParameterType.RequestBody);
                }
                else if (postBody.GetType() == typeof(byte[]))
                {
                    restRequest.AddParameter(contentType, postBody, ParameterType.RequestBody);
                }
            }
            return restRequest;
        }
        public object Deserialize(IRestResponse response, Type type)
        {
            object rawBytes;
            IList<Parameter> headers = response.Headers;
            if (type == typeof(byte[]))
            {
                rawBytes = response.RawBytes;
            }
            else if (type == typeof(Stream))
            {
                if (headers != null)
                {
                    string str = (string.IsNullOrEmpty(this.Configuration.TempFolderPath) ? Path.GetTempPath() : this.Configuration.TempFolderPath);
                    Regex regex = new Regex("Content-Disposition=.*filename=['\"]?([^'\"\\s]+)['\"]?$");
                    foreach (Parameter header in headers)
                    {
                        Match match = regex.Match(header.ToString());
                        if (match.Success)
                        {
                            string str1 = string.Concat(str, ApiClient.SanitizeFilename(match.Groups[1].Value.Replace("\"", "").Replace("'", "")));
                            File.WriteAllBytes(str1, response.RawBytes);
                            rawBytes = new FileStream(str1, FileMode.Open);
                            return rawBytes;
                        }
                    }
                }
                rawBytes = new MemoryStream(response.RawBytes);
            }
            else if (type.Name.StartsWith("System.Nullable`1[[System.DateTime"))
            {
                rawBytes = DateTime.Parse(response.Content, null, DateTimeStyles.RoundtripKind);
            }
            else if ((type == typeof(string) ? false : !type.Name.StartsWith("System.Nullable")))
            {
                try
                {
                    rawBytes = JsonConvert.DeserializeObject(response.Content, type, this.serializerSettings);
                }
                catch (Exception exception)
                {
                    throw new ApiException(500, exception.Message);
                }
            }
            else
            {
                rawBytes = ApiClient.ConvertType(response.Content, type);
            }
            return rawBytes;
        }

        private void IntegrateURLWithConsul()
        {
            if (this.Configuration.EnableConsul)
            {
                string str = APIStoreHelper.DiscoverService(this.Configuration.ConsulServiceName, this.Configuration.ConsulServiceTag, false, 1000);
                if (!string.IsNullOrEmpty(str))
                {
                    this.RestClient.BaseUrl=new Uri(str);
                }
            }
        }
        public static string SanitizeFilename(string filename)
        {
            string str;
            Match match = Regex.Match(filename, ".*[/\\\\](.*)$");
            str = (!match.Success ? filename : match.Groups[1].Value);
            return str;
        }
        public string EscapeString(string str)
        {
            return ApiClient.UrlEncode(str);
        }
        public static string UrlEncode(string input)
        {
            string str = null;
            string str1;
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (input.Length > 32766)
            {
                StringBuilder stringBuilder = new StringBuilder(input.Length * 2);
                for (int i = 0; i < input.Length; i = i + str.Length)
                {
                    int num = Math.Min(input.Length - i, 32766);
                    str = input.Substring(i, num);
                    stringBuilder.Append(Uri.EscapeDataString(str));
                }
                str1 = stringBuilder.ToString();
            }
            else
            {
                str1 = Uri.EscapeDataString(input);
            }
            return str1;
        }

        public FileParameter ParameterToFile(string name, Stream stream)
        {
            FileParameter fileParameter;
            fileParameter = (!(stream is FileStream) ? FileParameter.Create(name, ApiClient.ReadAsBytes(stream), "no_file_name_provided") : FileParameter.Create(name, ApiClient.ReadAsBytes(stream), Path.GetFileName(((FileStream)stream).Name)));
            return fileParameter;
        }
        public static byte[] ReadAsBytes(Stream input)
        {
            byte[] array;
            byte[] numArray = new byte[16384];
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                while (true)
                {
                    int num = input.Read(numArray, 0, (int)numArray.Length);
                    int num1 = num;
                    if (num <= 0)
                    {
                        break;
                    }
                    memoryStream.Write(numArray, 0, num1);
                }
                array = memoryStream.ToArray();
            }
            finally
            {
                if (memoryStream != null)
                {
                    ((IDisposable)memoryStream).Dispose();
                }
            }
            return array;
        }

        public string ParameterToString(object obj)
        {
            string str;
            if (obj is DateTime)
            {
                str = ((DateTime)obj).ToString(this.Configuration.DateTimeFormat);
            }
            else if (obj is DateTimeOffset)
            {
                str = ((DateTimeOffset)obj).ToString(this.Configuration.DateTimeFormat);
            }
            else if (!(obj is IList))
            {
                str = Convert.ToString(obj);
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (object obj1 in (IList)obj)
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.Append(",");
                    }
                    stringBuilder.Append(obj1);
                }
                str = stringBuilder.ToString();
            }
            return str;
        }
        public string SelectHeaderAccept(string[] accepts)
        {
            string str;
            if ((int)accepts.Length == 0)
            {
                str = null;
            }
            else if (!accepts.Contains<string>("*_/_*"))
            {
                str = (!accepts.Contains<string>("application/json", StringComparer.OrdinalIgnoreCase) ? string.Join(",", accepts) : "application/json");
            }
            else
            {
                str = "application/json";
            }
            return str;
        }
        public string SelectHeaderContentType(string[] contentTypes)
        {
            string str;
            if ((int)contentTypes.Length != 0)
            {
                str = (!contentTypes.Contains<string>("application/json", StringComparer.OrdinalIgnoreCase) ? contentTypes[0] : "application/json");
            }
            else
            {
                str = null;
            }
            return str;
        }

        public string Serialize(object obj)
        {
            string str;
            string str1;
            try
            {
                if (obj != null)
                {
                    str1 = JsonConvert.SerializeObject(obj);
                }
                else
                {
                    str1 = null;
                }
                str = str1;
            }
            catch (Exception exception)
            {
                throw new ApiException(500, exception.Message);
            }
            return str;
        }
    }
}
