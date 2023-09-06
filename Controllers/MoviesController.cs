using Microsoft.AspNetCore.Mvc;


namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IGenreService _genreService;
        private new readonly List<string> _allowedExtensions = new List<string> { ".jpg", ".png" };
        private readonly long _fileSizeLimit = 1048576;  // 1MB

        public MoviesController(IMovieService movieService, IGenreService genreService)
        {
            _movieService = movieService;
            _genreService = genreService;
        }


        [HttpGet]
        public async Task<ActionResult> GetAllMovies()
        {
            var movies = await _movieService.GetAll();
            return Ok(movies);
        }

        //this function will take genreId from Url if i want to take parameter from postman i will remove /{genreId}
        [HttpGet("GetMoviesByGenre/{genreId}")]
        public async Task<ActionResult> GetMoviesByGenre([FromRoute] byte genreId)
        {
            var movies = await _movieService.GetAll(genreId);
            return Ok(movies);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetMovieById(int id)
        {
            var movie = await _movieService.GetById(id);
            if (movie == null)
                return NotFound($"No movie was found with ID: {id}");

            return Ok(movie);
        }

        [HttpPost]
        public async Task<ActionResult> CreateMovie([FromForm] MovieDto dto)
        {
            if (dto.Poster == null || dto.Poster.Length == 0)
                return BadRequest("Poster is required!!!");

            if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg extensions are allowed!!!");

            if (dto.Poster.Length > _fileSizeLimit)
                return BadRequest("File size is too big!!!");

            var IsValid = await _genreService.IsValid(dto.GenreId);

            if (!IsValid)
                return BadRequest("Genre ID is not valid!!!");

            using var memoryStream = new MemoryStream();
            await dto.Poster.CopyToAsync(memoryStream);

            var movie = new Movie
            {
                Title = dto.Title,
                Year = dto.Year,
                Rating = dto.Rating,
                StoryLine = dto.StoryLine,
                Poster = memoryStream.ToArray(),
                GenreId = dto.GenreId
            };
            _movieService.Create(movie);
            return Ok(movie);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateMovie(int id, [FromForm] MovieDto dto)
        {
            var movie = await _movieService.GetById(id);
            if (movie == null)
                return NotFound($"No movie was found with ID: {id}");

            if (dto.Poster != null)
            {
                if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg extensions are allowed!!!");

                if (dto.Poster.Length > _fileSizeLimit)
                    return BadRequest("File size is too big!!!");

                using var memoryStream = new MemoryStream();
                await dto.Poster.CopyToAsync(memoryStream);
                movie.Poster = memoryStream.ToArray();
            }

            var IsValid = await _genreService.IsValid(dto.GenreId);

            if (!IsValid)
                return BadRequest("Genre ID is not valid!!!");

            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.Rating = dto.Rating;
            movie.StoryLine = dto.StoryLine;
            movie.GenreId = dto.GenreId;
            _movieService.Update(movie);
            return Ok(movie);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteMovie(int id)
        {
            var movie = await _movieService.GetById(id);
            if (movie == null)
                return NotFound($"No movie was found with ID: {id}");

            _movieService.Delete(movie);
            return Ok();
        }
    }
}
