using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notes.ColorService.Models;
using Notes.Common.Exceptions;
using Notes.Context.Context;

namespace Notes.ColorService;

public class ColorService : IColorService
{
    private readonly IDbContextFactory<MainDbContext> contextFactory;
    private readonly IMapper mapper;


    public ColorService(IDbContextFactory<MainDbContext> contextFactory,
        IMapper mapper
       )
    {
        this.contextFactory = contextFactory;
        this.mapper = mapper;
    }
    public async Task<IEnumerable<ColorModel>> GetColors()
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var colors = context.Colors
            .AsQueryable();
        var data = (await colors.ToListAsync()).Select(x => mapper.Map<ColorModel>(x));
        return data;
    }

    public async Task<ColorModel> GetColorById(int id)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var colors = context.Colors
            .AsQueryable();
        var data = (await colors.ToListAsync()).Select(x => mapper.Map<ColorModel>(x))
            .FirstOrDefault(x => x.Id == id);
        ProcessException.ThrowIf(() => data is null, "The color with this ID was not found in the database");
        return data;
    }
}