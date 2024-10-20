using Bogus;
using Bogus.DataSets;
using DapperAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GraphQL.Models;
using System;
using System.Collections.Generic;

namespace GraphQL.Schema.Query
{
    public class Query
    {
       
        private readonly DapperRepository _dapperRepository;

        public Query(DapperRepository dapperRepository)
        {
            _dapperRepository = dapperRepository;
        }
        public IEnumerable<Course> GetCoursesFaker()
        {
            Faker<Course> _CourseTypeFaker;
            Faker<Instructor> _instructorTypeFaker;
            Faker<Student> _StudentTypeFaker;

            Faker<Instructor> instructorTypeFaker = new Faker<Instructor>()
                .RuleFor(c => c.Id, c => Guid.NewGuid())
                .RuleFor(c => c.FirstName, c => c.Name.FirstName())
                .RuleFor(c => c.LastName, c => c.Name.LastName())
                .RuleFor(c => c.Salary, c => c.Random.Double(0, 10000));

            Faker<Student> StudentTypeFaker = new Faker<Student>()
               .RuleFor(c => c.Id, c => Guid.NewGuid())
               .RuleFor(c => c.FirstName, c => c.Name.FirstName())
               .RuleFor(c => c.LastName, c => c.Name.LastName())
               .RuleFor(c => c.GPA, c => c.Random.Double(0, 4));

            Faker<Course> CourseTypeFaker = new Faker<Course>()
               .RuleFor(c => c.Id, c => Guid.NewGuid())
               .RuleFor(c => c.Name, c => c.Name.JobTitle())
               .RuleFor(c => c.Subject, c => c.PickRandom<Subject>())
               .RuleFor(c => c.Students, c => StudentTypeFaker.Generate(3))
               .RuleFor(c => c.Instructor, c => instructorTypeFaker.Generate());


            _CourseTypeFaker = CourseTypeFaker;
            _instructorTypeFaker = instructorTypeFaker;
            _StudentTypeFaker = StudentTypeFaker;

            return _CourseTypeFaker.Generate(3);

        }

        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public async Task<List<Course>> GetcoursesWithEF([Service] AppDbContext dbContext)
        {
             return dbContext.Courses.Include(x=>x.Students ).Include(x=>x.Instructor).ToList();
        }
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public async Task<List<Course>> GetCourses()
        {
            return await _dapperRepository.GetCourses();
        }
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public async Task<List<Instructor>> GetInstractors()
        {
            return await _dapperRepository.GetInstractors();
        }
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public async Task<List<Student>> GetStudents()
        {
            return await _dapperRepository.GetStudents();
        }

    }
}
