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
}
