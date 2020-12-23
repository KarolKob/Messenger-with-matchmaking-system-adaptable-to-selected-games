using API.DTO;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public interface IPoczekalnia
    {
        Lobby find_lobby(Player player);
    }
}
