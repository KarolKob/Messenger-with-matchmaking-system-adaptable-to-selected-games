using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{   //hard coded root - api/Matching
    [Route("api/Matches")]
    [ApiController]
    public class matches : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILiteRepo _repository;
        private readonly IPoczekalnia _poczekalnia;


        //konstruktor jest potrzebny by móc używać dependecy
        //injection
        public matches(ILiteRepo repo,IMapper mapper, IPoczekalnia poczekalnia)
        {
            _mapper = mapper;
            _repository = repo;
            _poczekalnia = poczekalnia;
        }

        //GET api/Matching
        [HttpGet]
        public string test([FromBody] string nick) //zwraca dane serwera do gry
        {
            //var players = _repository.MatchPlayers();//To Be Done
            //return Ok(_mapper.Map<IEnumerable<CommandReadDTO>>(commanditems));
            //return Ok(_mapper.Map<IEnumerable<PlayerReadDTO>>(players));

            //'get player rating'
            //var player = await _repository.GetPlayerInfo(nick);
            //'find him a place in queue'
            //'add him to queue'
            //Lobby lobby=_poczekalnia.find_lobby(player);
            //'send him server adress and lobby id'
            return ("ola");
        }


        //GET api/Matching/{nick}
        //[HttpGet("{nick}", Name = "GetPlayerInfo")]
        //public async Task<ActionResult<PlayerReadDTO>> GetPlayerInfo(string nick) //funkcja do testów
        //{
        //    //var playerinfo = await _repository.GetPlayerInfo(nick);
        //    //var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;



        //    //if (playerinfo != null)
        //    //{
        //        //bez if zwraca 204 no content w przypadku braku wyniku
        //        //return Ok(_mapper.Map<CommandReadDTO>(commanditem));
        //    //    return Ok(_mapper.Map<PlayerReadDTO>(playerinfo));
        //    //}
        //    //zwraca 404 not found
        //    //return NotFound();


        //    //return Ok(nick);
        //}
        //POST api/Matching
        [HttpPost]//tworzenie
        public async Task<ActionResult> CreateMatch(MatchSoloCreate matchsolo) //zapisanie informacji o rozpoczętym meczu
        {
            //async Task<ActionResult<PlayerReadDTO>>
            int matchID = await _repository.Add_Game_Solo(matchsolo.players,matchsolo.ranked);
            _poczekalnia.remove_lobby_from_pool(matchsolo.lobby_id);
            return Ok();
            //zapisz mecz do bazy danych i uaktualnij statystyki graczy
            //return CreatedAtRoute(nameof(GetPlayerInfo), new { nick = playerReadModel.Nickname }, playerReadModel);
        }
        //PUT api/Matching/{id}
        [HttpPut("{lobby_id}")] //update całości
        public ActionResult LobbyRefill(string lobby_id, [FromBody]Lobby NotFullLobby)
        {
            //var commandModel = _repository.GetcommandbyID(id);
            //if (commandModel == null)
            //{
            //    return NotFound();
            //}
            //_mapper.Map(update, commandModel);
            //_repository.UpdateCommand(commandModel);
            //_repository.SaveChanges();
            _poczekalnia.add_lobby_to_pool(NotFullLobby);//ulepsz gdy dodajemy lobby i gdy aktualizujemy lobby bo ktoś jeszcze wyszedł
            return NoContent();//standard zwracany przez put
        }
        //PATH api/Matching/{nick}
        [HttpPatch("{nick}")] //update części
        public ActionResult PatchCommand(string nick, JsonPatchDocument<PlayerUpdateDTO> patch)
        {
            //var commandModel = _repository.GetcommandbyID(id);
            //if (commandModel == null)
            //{
            //    return NotFound();
            //}
            //var commandPatch = _mapper.Map<CommandUpdateDTO>(commandModel);
            //patch.ApplyTo(commandPatch, ModelState);
            //if (!TryValidateModel(commandPatch))
            //{
            //    return ValidationProblem(ModelState);
            //}
            ////spowrotem na model
            //_mapper.Map(commandPatch, commandModel);
            //_repository.UpdateCommand(commandModel);
            //_repository.SaveChanges();
            return NoContent();
        }
        //DELETE api/Matching/{nick}
        [HttpDelete("{nick}")] //usuwanie
        public ActionResult DeleteCommand(string nick)
        {
            //var commandModel = _repository.GetcommandbyID(id);
            //if (commandModel == null)
            //{
            //    return NotFound();
            //}
            //_repository.DeleteCommand(commandModel);
            //_repository.SaveChanges();
            return NoContent();
        }


    }
}
