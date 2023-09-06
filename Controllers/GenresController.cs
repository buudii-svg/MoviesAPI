using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {

        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllGenres()
        {
            var genres = await _genreService.GetAll();
            return Ok(genres);
        }


        [HttpPost]
        public async Task<ActionResult> CreateGenre(CreateGenreDto dto)
        {
            var genre = new Genre { Name = dto.Name };
            await _genreService.Create(genre);
            return Ok(genre);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateGenre(byte id, [FromBody] CreateGenreDto dto)
        {
            var genre = await _genreService.GetById(id);
            if (genre == null)
                return NotFound($"No genre was found with ID: {id}");

            genre.Name = dto.Name;
            _genreService.Update(genre);
            return Ok(genre);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteGenre(byte id)
        {
            var genre = await _genreService.GetById(id);
            if (genre == null)
                return NotFound($"No genre was found with ID: {id}");

            _genreService.Delete(genre);
            return Ok();
        }

    }
}
