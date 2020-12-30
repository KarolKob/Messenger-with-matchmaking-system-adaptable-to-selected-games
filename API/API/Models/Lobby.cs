﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Lobby
    {
        public string lobby_ID { get; set; }
        public int max_size { get; set; }
        int cur_size;
        public List<Player> queue { get; set; }
        public Lobby(Player player, string id,int size)
        {
            lobby_ID = id;
            max_size = size;
            //queue = new List<Player>();
            queue=new List<Player> { player };

        }
    }
}