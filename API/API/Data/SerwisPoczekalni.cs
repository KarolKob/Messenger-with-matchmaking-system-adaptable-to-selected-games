using API.DTO;
using API.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class SerwisPoczekalni : IPoczekalnia
    {
        //lista lobby
        //List<Lobby> poczekalnia = new List<Lobby>();

        List<Lobby> poczekalnia = LobbyCache.Instance.Lobbies;
        private readonly int lobby_size;
        //dostęp do kolejki pokoi poprzez dependency injection
        public SerwisPoczekalni()
        {
            lobby_size = 2;
        }
        public Lobby find_lobby(Player player)
        {
            if (poczekalnia.Count > 0)
            {
                Lobby temp = poczekalnia.FirstOrDefault();
                temp.queue.Add(player);
                return temp;
            }
            else
            {
                Lobby temp = new Lobby(player, player.NickName, lobby_size);
                poczekalnia.Add(temp);
                return temp;
            }
        }

    }
}