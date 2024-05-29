using Infrastructure.Data.Contexts;
using Infrastructure.Data.Entities;
using Infrastructure.Factories;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public interface ICourseService
{
    Task<Course> CreateCourseAsync(CourseCreateRequest request);
    Task<Course> GetCourseByIdAsync(string id);
    Task<IEnumerable<Course>> GetCoursesAsync();
    Task<Course> UpdateCourseAsync(CourseUpdateRequest request);
    Task<bool> DeleteCourseAsync(string id);
}

public class CourseService(IDbContextFactory<DataContext> contextFactory) : ICourseService
{
    private readonly IDbContextFactory<DataContext> _contextFactory = contextFactory;

    public async Task<Course> CreateCourseAsync(CourseCreateRequest request)
    {
        await using var context = _contextFactory.CreateDbContext();

        var courseEntity = CourseFactory.Create(request);
        context.Courses.Add(courseEntity);
        await context.SaveChangesAsync();

        return CourseFactory.Create(courseEntity);
    }

    public async Task<bool> DeleteCourseAsync(string id)
    {
        await using var context = _contextFactory.CreateDbContext();
        var courseEntity = await context.Courses.FirstOrDefaultAsync(x => x.Id == id);
        if (courseEntity == null) return false;

        context.Courses.Remove(courseEntity);
        await context.SaveChangesAsync();
        return true;

    }

    public async Task<Course> GetCourseByIdAsync(string id)
    {
        await using var context = _contextFactory.CreateDbContext();
        var courseEntity = await context.Courses.FirstOrDefaultAsync(x => x.Id == id);

        return courseEntity == null ? null! : CourseFactory.Create(courseEntity);
    }

    public async Task<IEnumerable<Course>> GetCoursesAsync()
    {
        await using var context = _contextFactory.CreateDbContext();
        var courseEntities = await context.Courses.ToListAsync();

        return courseEntities.Select(CourseFactory.Create);
    }

    public async Task<Course> UpdateCourseAsync(CourseUpdateRequest request)
    {
        //await using var context = _contextFactory.CreateDbContext();
        //var existingCourse = await context.Courses.FirstOrDefaultAsync(x =>x.Id == request.Id);
        //if (existingCourse == null) return null!;

        //var updatedCourseEntity = CourseFactory.Update(request);
        //updatedCourseEntity.Id = existingCourse.Id;
        //context.Entry(existingCourse).CurrentValues.SetValues(updatedCourseEntity);

        //await context.SaveChangesAsync();
        //return CourseFactory.Create(existingCourse);

        await using var context = _contextFactory.CreateDbContext();
        var existingCourse = await context.Courses
            .Include(c => c.Authors)
            .Include(c => c.Prices)
            .Include(c => c.Content)
                .ThenInclude(c => c!.ProgramDetails)
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (existingCourse == null) return null!;

        // Update scalar properties
        context.Entry(existingCourse).CurrentValues.SetValues(request);

        // Authors
        existingCourse.Authors!.Clear();
        existingCourse.Authors.AddRange(request.Authors!.Select(a => new AuthorEntity { Name = a.Name }));

        // Prices
        if (request.Prices != null)
        {
            existingCourse.Prices!.Currency = request.Prices.Currency;
            existingCourse.Prices.Price = request.Prices.Price;
            existingCourse.Prices.Discount = request.Prices.Discount;
        }

        // Content
        if (request.Content != null)
        {
            context.Entry(existingCourse.Content!).CurrentValues.SetValues(request.Content);
        }

        await context.SaveChangesAsync();
        return CourseFactory.Create(existingCourse);
    }
}
