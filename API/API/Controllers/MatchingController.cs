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
    [Route("api/Matching")]
    [ApiController]
    public class matching : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILiteRepo _repository;
        private readonly IPoczekalnia _poczekalnia;
        //private readonly IHttpContextAccessor _httpContextAccessor;


        //konstruktor jest potrzebny by móc używać dependecy
        //injection
        public matching(ILiteRepo repo, IMapper mapper, IPoczekalnia poczekalnia)
        {
            _mapper = mapper;
            _repository = repo;
            _poczekalnia = poczekalnia;
           //_httpContextAccessor = httpContextAccessor;
            //poczekalnia = new List<Lobby>();
            
        }

        //GET api/Matching
        [HttpGet]
        public async Task<ActionResult<Server>> GetMatch([FromBody] string nick) //zwraca dane serwera do gry
        {
            //var players = _repository.MatchPlayers();//To Be Done
            //return Ok(_mapper.Map<IEnumerable<CommandReadDTO>>(commanditems));
            //return Ok(_mapper.Map<IEnumerable<PlayerReadDTO>>(players));

            //'get player rating'
            var player = await _repository.GetPlayerInfo(nick);
            //'find him a place in queue'
            //'add him to queue'
            Lobby lobby=_poczekalnia.find_lobby(player);
            //'send him server adress and lobby id'
            return (new Server(lobby.lobby_ID,lobby.max_size));
        }
        //GET api/Matching/{nick}
        [HttpGet("{nick}", Name = "GetPlayerInfo")]
        public async Task<ActionResult<PlayerReadDTO>> GetPlayerInfo(string nick) //funkcja do testów
        {
            var playerinfo = await _repository.GetPlayerInfo(nick);
            //var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;



            if (playerinfo != null)
            {
                //bez if zwraca 204 no content w przypadku braku wyniku
                //return Ok(_mapper.Map<CommandReadDTO>(commanditem));
                return Ok(_mapper.Map<PlayerReadDTO>(playerinfo));
            }
            //zwraca 404 not found
            return NotFound();


            //return Ok(nick);
        }
        //POST api/Matching
        [HttpPost]//tworzenie
        public async Task<ActionResult<PlayerReadDTO>> CreatePlayer(PlayerCreateDTO playerCreate) //dodawanie gracza to bazy danych
        {
            Player response;
            var playerModel = _mapper.Map<Player>(playerCreate);
            try
            {
                response = await _repository.AddPlayer(playerModel);
            }
            catch (Exception ex)
            {
                return BadRequest("Nick jest już zajęty");
            }
            var playerReadModel = _mapper.Map<PlayerReadDTO>(response);
            //return Ok(_mapper.Map<PlayerReadDTO>(playerModel)); 
            return CreatedAtRoute(nameof(GetPlayerInfo), new { nick = playerReadModel.Nickname }, playerReadModel);
            //return NoContent();
        }
        //PUT api/Matching/{id}
        [HttpPut("{id}")] //update całości
        public ActionResult UpdateCommand(int id, PlayerUpdateDTO update)
        {
            //var commandModel = _repository.GetcommandbyID(id);
            //if (commandModel == null)
            //{
            //    return NotFound();
            //}
            //_mapper.Map(update, commandModel);
            //_repository.UpdateCommand(commandModel);
            //_repository.SaveChanges();
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
