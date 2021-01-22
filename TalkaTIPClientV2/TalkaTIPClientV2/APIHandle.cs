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
            httpClient.BaseAddress = new Uri(apiAddress);//"https://localhost:5001/api/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            this.apiAddress = apiAddress;
            /*string result = ApiPOST().GetAwaiter().GetResult();

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
            }*/
        }

        // TODO: Figure out what is returned and act accordingly
        public async Task<string> ApiPOST()
        {
            Player player;
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
                var objekt = await response.Content.ReadAsStringAsync();
                player = JsonConvert.DeserializeObject<Player>(objekt);
            }
            catch (Exception)
            {
                return null;
            }

            return player.ApiName;
        }

        public async Task<string> ApiGETUserStatistics()
        {
            Player player;
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
                var objekt = await response.Content.ReadAsStringAsync();
                player = JsonConvert.DeserializeObject<Player>(objekt);
            }
            catch (Exception)
            {
                return null;
            }
             
            return player.ToString();
        }

        public async Task<string> ApiGETMatchmaking()
        {
            Server server;
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync("https://localhost:5001/api/Matching/Match/" + Program.userLogin);
                response.EnsureSuccessStatusCode();
                var objekt = await response.Content.ReadAsStringAsync();
                server = JsonConvert.DeserializeObject<Server>(objekt);
            }
            catch (Exception)
            {
                return null;
            }

            return server.lobby_ID;
        }

        public class Server
        {
            public string adress { get; set; }
            public int port { get; set; }
            public string lobby_ID { get; set; }
            public int max_size { get; set; }
        }

        public class Player
        {
            public string Nickname { get; set; }
            public double SkillRating { get; set; }
            public int GamesPlayed { get; set; }
            public int GamesWon { get; set; }
            public int GamesTied { get; set; }
            public int GamesLost { get; set; }
            public double WinRate { get; set; }
            public string Rank { get; set; }
            public string ApiName { get; set; }

            public string ToString()
            {
                string ret = string.Empty;
                return ret;
            }
        }
    }
}
