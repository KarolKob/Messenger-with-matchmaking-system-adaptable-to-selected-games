using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;

namespace TalkaTIPClientV2
{
    class APIHandle
    {
        private HttpClient httpClient;
        string apiAddress;

        public APIHandle(string apiAddress)
        {
            httpClient = new HttpClient();
            //httpClient.BaseAddress = new Uri(apiAddress);
            httpClient.BaseAddress = new Uri("https://localhost:5001/api/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            this.apiAddress = apiAddress;
            string result = ApiPOST().GetAwaiter().GetResult();

            if(result != null)
            {
                Program.mainWindow.Invoke((System.Windows.Forms.MethodInvoker)delegate
                {
                    if (!Program.apiNameAndHandle.ContainsKey(result))
                    {
                        //Program.apiNameAndHandle.Add(apiName, this);
                        System.Windows.Forms.MessageBox.Show(result, "OK");
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Adding API failed. Try again.", "Error");
                    }
                });
            }

            result = ApiPOST().GetAwaiter().GetResult();

            if (result != null)
            {
                Program.mainWindow.Invoke((System.Windows.Forms.MethodInvoker)delegate
                {
                    if (!Program.apiNameAndHandle.ContainsKey(result))
                    {
                        //Program.apiNameAndHandle.Add(apiName, this);
                        System.Windows.Forms.MessageBox.Show(result, "OK");
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Adding API failed. Try again.", "Error");
                    }
                });
            }
        }

        // TODO: Figure out what is returned and act accordingly
        public async Task<string> ApiPOST()
        {
            string result;
            try
            {
                var my_jsondata = new
                {
                    NickName = Program.userLogin
                };
                string json = JsonConvert.SerializeObject(my_jsondata);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                //var response = await httpClient.PostAsync("http://www.sample.com/write", stringContent);  new StringContent(Program.userLogin)
                HttpResponseMessage response = await httpClient.PostAsync("Matching", stringContent)
                    .ConfigureAwait(continueOnCapturedContext: false);
                response.EnsureSuccessStatusCode();
                result = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                return null;
            }

            return result;
        }

        public async Task<string> ApiGETData()
        {
            string result;
            try
            {
                /*var my_jsondata = new
                {
                    NickName = Program.userLogin
                };
                string json = JsonConvert.SerializeObject(my_jsondata);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");*/
                //var response = await httpClient.PostAsync("http://www.sample.com/write", stringContent);  new StringContent(Program.userLogin)
                HttpResponseMessage response = await httpClient.GetAsync("Matching/" + Program.userLogin)
                    .ConfigureAwait(continueOnCapturedContext: false);
                response.EnsureSuccessStatusCode();
                result = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                return null;
            }
             
            return result;
        }

        public async Task<string> ApiGETPlayerInfo()
        {
            string result = null;
            try
            {
                var my_jsondata = new
                {
                    NickName = Program.userLogin
                };
                string json = JsonConvert.SerializeObject(my_jsondata);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://localhost:5001/api/Matching"),
                    Content = new StringContent(json, Encoding.UTF8, "application/json"),

                    //setRequestProperty("Content-Type", "application/json; charset=utf8")

                };
                //var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                //var response = await httpClient.PostAsync("http://www.sample.com/write", stringContent);  new StringContent(Program.userLogin)
                /*HttpResponseMessage response = await httpClient.Get("Matching", stringContent).ConfigureAwait(continueOnCapturedContext: false);
                response.EnsureSuccessStatusCode();
                name = await response.Content.ReadAsStringAsync();*/
                HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(continueOnCapturedContext: false);
                response.EnsureSuccessStatusCode();
                var name = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                return null;
            }

            return result;
        }
    }
}
