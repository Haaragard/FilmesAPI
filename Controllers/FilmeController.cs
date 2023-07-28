using FilmesAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private static List<Filme> Filmes = new List<Filme>();
    public static int Id = 0;

    [HttpPost]
    public IActionResult Store([FromBody] Filme filme)
    {
        filme.Id = Id++;
        Filmes.Add(filme);

        return CreatedAtAction(nameof(Get), new { id = filme.Id }, filme);
    }

    [HttpGet]
    public IEnumerable<Filme> List([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        return Filmes.Skip(skip).Take(take);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var filme =  Filmes.FirstOrDefault(filme => filme.Id == id);

        if (filme == null) return NotFound();

        return Ok(filme);
    }
}
