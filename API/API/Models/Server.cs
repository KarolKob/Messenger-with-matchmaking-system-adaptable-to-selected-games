using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Server
    {
        public string adress { get; set; } = "127.0.0.1";
        public string lobby_ID { get; set; }
        public int max_size { get; set; }
        public Server(string lobby_id,int lobby_size)
        {
            lobby_ID = lobby_id;
            max_size = lobby_size;
        }
    }
}
