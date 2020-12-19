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
            string apiName = apiPOST().GetAwaiter().GetResult();

            if(apiName != null)
            {
                Program.mainWindow.Invoke((System.Windows.Forms.MethodInvoker)delegate
                {
                    if (!Program.apiNameAndHandle.ContainsKey(apiName))
                    {
                        Program.apiNameAndHandle.Add(apiName, this);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Adding API failed. Try again.", "Error");
                    }
                });
            }
        }

        // TODO: Figure out what is returned and act accordingly
        public async Task<string> apiPOST()
        {
            string name;
            try
            {
                var my_jsondata = new
                {
                    NickName = Program.userLogin
                };
                string json = JsonConvert.SerializeObject(my_jsondata);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                //var response = await httpClient.PostAsync("http://www.sample.com/write", stringContent);  new StringContent(Program.userLogin)
                HttpResponseMessage response = await httpClient.PostAsync("Matching", stringContent).ConfigureAwait(continueOnCapturedContext: false);
                response.EnsureSuccessStatusCode();
                name = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                return null;
            }

            return name;
        }
    }
}
