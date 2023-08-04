using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CinemaController : ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;

    public CinemaController(FilmeContext _context, IMapper _mapper)
    {
        this._context = _context;
        this._mapper = _mapper;
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        Cinema? cinema = _FindCinema(id);
        if (cinema == null) return NotFound();

        var cinemaDto = _mapper.Map<Cinema>(cinema);

        return Ok(cinemaDto);
    }

    [HttpGet]
    public IEnumerable<ReadCinemaDto> List([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        var cinemas = _context.Cinemas.Skip(skip).Take(take);

        return _mapper.Map<List<ReadCinemaDto>>(cinemas);
    }

    [HttpPost]
    public IActionResult Store([FromBody] CreateCinemaDto cinemaDto)
    {
        Cinema cinema = _mapper.Map<Cinema>(cinemaDto);
        _context.Cinemas.Add(cinema);
        _context.SaveChanges();

        return CreatedAtAction(nameof(Get), new { id = cinema.Id }, cinema);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] UpdateCinemaDto cinemaDto)
    {
        Cinema? cinema = _FindCinema(id);
        if (cinema == null) return NotFound();

        _mapper.Map(cinemaDto, cinema);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult Patch(int id, JsonPatchDocument<UpdateCinemaDto> patch)
    {
        Cinema? cinema = _FindCinema(id);
        if (cinema == null) return NotFound();

        var cinemaParaAtualizar = _mapper.Map<UpdateCinemaDto>(cinema);
        patch.ApplyTo(cinemaParaAtualizar, ModelState);
        if (!TryValidateModel(ModelState))
        {
            return ValidationProblem();
        }

        _mapper.Map(cinemaParaAtualizar, cinema);
        _context.SaveChanges();

        return NoContent();
    }
 
    private Cinema? _FindCinema(int id)
    {
        return _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
    }
}
