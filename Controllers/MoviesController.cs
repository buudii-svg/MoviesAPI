using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private new readonly List<string> _allowedExtensions = new List<string> { ".jpg", ".png" };
        private readonly long _fileSizeLimit = 1048576;  // 1MB

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllMovies()
        {
            var movies = await _context.Movies
                .Include(g => g.Genre)
                .OrderByDescending(m => m.Rating)
                .ToListAsync();
            return Ok(movies);
        }

        //this function will take genreId from Url if i want to take parameter from postman i will remove /{genreId}
        [HttpGet("GetMoviesByGenre/{genreId}")]
        public async Task<ActionResult> GetMoviesByGenre([FromRoute] byte genreId)
        {
            var movies = await _context.Movies
                .Where(m => m.GenreId == genreId)
                .OrderByDescending(m => m.Rating)
                .Include(g => g.Genre)
                .ToListAsync();
            return Ok(movies);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetMovieById(int id)
        {
            var movie = await _context.Movies.Include(g => g.Genre).FirstOrDefaultAsync(m => m.Id == id);
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

            var IsValid = await _context.Genres.AnyAsync(m => m.Id == dto.GenreId);

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
            await _context.AddAsync(movie);
            _context.SaveChanges();
            return Ok(movie);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateMovie(int id, [FromForm] MovieDto dto)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
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

            var IsValid = await _context.Genres.AnyAsync(m => m.Id == dto.GenreId);

            if (!IsValid)
                return BadRequest("Genre ID is not valid!!!");

            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.Rating = dto.Rating;
            movie.StoryLine = dto.StoryLine;
            movie.GenreId = dto.GenreId;
            _context.SaveChanges();
            return Ok(movie);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
                return NotFound($"No movie was found with ID: {id}");

            _context.Remove(movie);
            _context.SaveChanges();
            return Ok();
        }
    }
}
