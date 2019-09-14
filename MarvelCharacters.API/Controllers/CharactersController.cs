using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MarvelCharacters.API.Models;
using System.Collections.Generic;
using MarvelCharacters.API.Services.Http;
using MarvelCharacters.API.Services.Db;
using System.Threading.Tasks;
using System.Linq;

namespace MarvelCharacters.API.Controllers
{
    [Route("api/[controller]")]
    public class CharactersController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]string searchString,
            [FromServices] MarvelApi marvelApi,
            [FromServices] MongoDatabase mongoDatabase)
        {
            var apiCharacters = await marvelApi.GetCharacters(searchString);
            var likedCharacters = await mongoDatabase.GetLikes();

            for (int i = 0; i < apiCharacters.Count; i++)
            {
                var currentCharacter = apiCharacters.ToArray()[i];

                var liked = likedCharacters.FirstOrDefault(x => x.Id == currentCharacter.Id);
                if(liked != null)
                {
                    //foi liked
                    currentCharacter.Liked = true;
                }
            }

            return Ok(apiCharacters);
        }


        [HttpPost("{id}/likes")]
        public async Task<IActionResult> CreateLike(
            [FromRoute]int? id, 
            [FromBody]Character character,
            [FromServices] MongoDatabase mongoDatabase)
        {
            if (id.HasValue == false || character == null)
                return BadRequest();

            var data = await mongoDatabase.AddLike(character);

            return Ok(data);
        }
        [HttpDelete("{id}/likes")]
        public async Task<IActionResult> DeleteLike(
            [FromRoute]int? id, 
            [FromServices] MongoDatabase mongoDatabase)
        {
            if (id.HasValue == false)
                return BadRequest();

            await mongoDatabase.RemoveLike(id.Value);

            return Ok();
        }

    }
}
