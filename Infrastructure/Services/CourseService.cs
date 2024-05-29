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

    //public async Task<Course> UpdateCourseAsync(CourseUpdateRequest request)
    //{
    //    //await using var context = _contextFactory.CreateDbContext();
    //    //var existingCourse = await context.Courses.FirstOrDefaultAsync(x =>x.Id == request.Id);
    //    //if (existingCourse == null) return null!;

    //    //var updatedCourseEntity = CourseFactory.Update(request);
    //    //updatedCourseEntity.Id = existingCourse.Id;
    //    //context.Entry(existingCourse).CurrentValues.SetValues(updatedCourseEntity);

    //    //await context.SaveChangesAsync();
    //    //return CourseFactory.Create(existingCourse);

    //    await using var context = _contextFactory.CreateDbContext();
    //    var existingCourse = await context.Courses
    //        .Include(c => c.Authors)
    //        .Include(c => c.Prices)
    //        .Include(c => c.Content)
    //        .Include(c => c.ProgramDetails)
    //        .FirstOrDefaultAsync(x => x.Id == request.Id);

    //    if (existingCourse == null) return null!;

    //    // Update scalar properties
    //    context.Entry(existingCourse).CurrentValues.SetValues(request);

    //    // Authors
    //    existingCourse.Authors!.Clear();
    //    existingCourse.Authors.AddRange(request.Authors!.Select(a => new AuthorEntity { Name = a.Name }));

    //    // Prices
    //    if (request.Prices != null)
    //    {
    //        existingCourse.Prices!.Currency = request.Prices.Currency;
    //        existingCourse.Prices.Price = request.Prices.Price;
    //        existingCourse.Prices.Discount = request.Prices.Discount;
    //    }

    //    // Content
    //    if (request.Content != null)
    //    {
    //        context.Entry(existingCourse.Content!).CurrentValues.SetValues(request.Content);
    //    }

    //    if (request.ProgramDetails != null)
    //    {
    //        existingCourse.ProgramDetails!.Title_1 = request.ProgramDetails.Title_1;
    //        existingCourse.ProgramDetails.Description_1 = request.ProgramDetails.Description_1;
    //        existingCourse.ProgramDetails.Title_2 = request.ProgramDetails.Title_2;
    //        existingCourse.ProgramDetails.Description_2 = request.ProgramDetails.Description_2;
    //        existingCourse.ProgramDetails.Title_3 = request.ProgramDetails.Title_3;
    //        existingCourse.ProgramDetails.Description_3 = request.ProgramDetails.Description_3;
    //    }

    //    await context.SaveChangesAsync();
    //    return CourseFactory.Create(existingCourse);
    //}

    public async Task<Course> UpdateCourseAsync(CourseUpdateRequest request)
    {
        await using var context = _contextFactory.CreateDbContext();
        var existingCourse = await context.Courses
            .Include(c => c.Authors)
            .Include(c => c.Prices)
            .Include(c => c.Content)
            .Include(c => c.ProgramDetails)
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (existingCourse == null) return null!;

        // Update scalar properties
        context.Entry(existingCourse).CurrentValues.SetValues(request);

        // Authors
        if (request.Authors != null)
        {
            existingCourse.Authors!.Clear();
            existingCourse.Authors.AddRange(request.Authors.Select(a => new AuthorEntity { Name = a.Name }));
        }

        // Prices
        if (request.Prices != null)
        {
            if (existingCourse.Prices == null)
            {
                existingCourse.Prices = new PricesEntity();
            }
            existingCourse.Prices.Currency = request.Prices.Currency;
            existingCourse.Prices.Price = request.Prices.Price;
            existingCourse.Prices.Discount = request.Prices.Discount;
        }

        // Content
        if (request.Content != null)
        {
            if (existingCourse.Content == null)
            {
                existingCourse.Content = new ContentEntity();
            }
            context.Entry(existingCourse.Content).CurrentValues.SetValues(request.Content);
        }

        // ProgramDetails
        if (request.ProgramDetails != null)
        {
            if (existingCourse.ProgramDetails == null)
            {
                existingCourse.ProgramDetails = new ProgramDetailItemEntity();
            }
            existingCourse.ProgramDetails.Title_1 = request.ProgramDetails.Title_1;
            existingCourse.ProgramDetails.Description_1 = request.ProgramDetails.Description_1;
            existingCourse.ProgramDetails.Title_2 = request.ProgramDetails.Title_2;
            existingCourse.ProgramDetails.Description_2 = request.ProgramDetails.Description_2;
            existingCourse.ProgramDetails.Title_3 = request.ProgramDetails.Title_3;
            existingCourse.ProgramDetails.Description_3 = request.ProgramDetails.Description_3;
        }

        await context.SaveChangesAsync();
        return CourseFactory.Create(existingCourse);
    }
}
