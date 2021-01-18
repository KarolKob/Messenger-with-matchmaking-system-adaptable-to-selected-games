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
        public Lobby find_lobby(Player player, int skilllimit)
        {
            Lobby temp = poczekalnia.Find(lobby=>lobby.queue.Count<lobby.max_size& Math.Abs(lobby.AvgSkill - player.SkillRating) < skilllimit);
            if (temp!=null)
            {
                foreach (var r in poczekalnia)
                {
                    if (Math.Abs(r.AvgSkill - player.SkillRating) < skilllimit & r.queue.Count < r.max_size)
                    {
                        if (Math.Abs(r.AvgSkill - player.SkillRating) < Math.Abs(temp.AvgSkill - player.SkillRating))
                        {
                            temp = r;
                        }

                    }
                }
                temp.queue.Add(player);
                return temp;
            }
            else
            {
                Lobby temp2 = new Lobby(player, player.NickName, lobby_size, player.SkillRating);
                poczekalnia.Add(temp2);
                return temp2;
            }
            //if (poczekalnia.Count > 0)
            //{

            //    //Lobby temp = poczekalnia.FirstOrDefault();
            //    foreach (var r in poczekalnia)
            //    {
            //        if (Math.Abs(r.AvgSkill - player.SkillRating) < skilllimit & r.queue.Count<r.max_size)
            //        {
            //            if (Math.Abs(r.AvgSkill - player.SkillRating) < Math.Abs(temp.AvgSkill-player.SkillRating))
            //            {
            //                temp = r;
            //            }
                        
            //        }
            //    }
            //    temp.queue.Add(player);
            //    return temp;
            //}
            //else
            //{
            //    Lobby temp2 = new Lobby(player, player.NickName, lobby_size,player.SkillRating);
            //    poczekalnia.Add(temp2);
            //    return temp2;
            //}
            
        }
        public void remove_player_from_lobby(string nick,string lobby_id)
        {
            
            var lobby=poczekalnia.Find(lobby => lobby.lobby_ID == lobby_id);
            lobby.queue.Remove(lobby.queue.Find(player=>player.NickName==nick));
            if (lobby.queue.Count() < 1)
            {
                poczekalnia.Remove(lobby);
            }
        }
        public void add_lobby_to_pool(Lobby lobby)
        {
            poczekalnia.Add(lobby);
        }
        public void remove_lobby_from_pool(string lobby_id)
        {
            poczekalnia.Remove(poczekalnia.Find(lobby => lobby.lobby_ID == lobby_id));
        }

    }
}