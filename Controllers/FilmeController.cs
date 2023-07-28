using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;

    public FilmeController(FilmeContext _context, IMapper _mapper)
    {
        this._context = _context;
        this._mapper = _mapper;
    }

    [HttpPost]
    public IActionResult Store([FromBody] CreateFilmeDto filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes.Add(filme);
        _context.SaveChanges();

        return CreatedAtAction(nameof(Get), new { id = filme.Id }, filme);
    }

    [HttpGet]
    public IEnumerable<ReadFilmeDto> List([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        var filmes = _context.Filmes.Skip(skip).Take(take);

        return _mapper.Map<List<ReadFilmeDto>>(filmes);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        Filme? filme = _FindFilme(id);
        if (filme == null) return NotFound();

        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);

        return Ok(filmeDto);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] UpdateFilmeDto filmeDto)
    {
        Filme? filme = _FindFilme(id);
        if (filme == null) return NotFound();

        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult Patch(int id, JsonPatchDocument<UpdateFilmeDto> patch)
    {
        Filme? filme = _FindFilme(id);
        if (filme == null) return NotFound();

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);
        patch.ApplyTo(filmeParaAtualizar, ModelState);

        if (!TryValidateModel(ModelState))
        {
            return ValidationProblem();
        }

        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        Filme? filme = _FindFilme(id);
        if (filme == null) return NotFound();

        _context.Remove(filme);
        _context.SaveChanges();

        return NoContent();
    }

    private Filme? _FindFilme(int id)
    {
        Filme? filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

        return filme;
    }
}
