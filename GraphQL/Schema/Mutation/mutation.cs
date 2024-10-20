using DapperAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using GraphQL.DTOs;
using GraphQL.Migrations;
using GraphQL.Models;

namespace GraphQL.Schema.Mutations
{
    public class Mutation
    {
        private readonly DapperRepository _dapperRepository;

        public Mutation( DapperRepository dapperRepository)
        {
            _dapperRepository = dapperRepository;
        }
        /////////////////////////////////////////////////  Course /////////////////////////////////////////////////////
        ///
        public async Task<string> CreateCourse(string name, Subject subject, Guid instrid)
        {
             return await _dapperRepository.creatcourse(name, subject, instrid);
        }
        public async Task<string> UpdateCourse(CourseDTO course)
        {
            return await _dapperRepository.UpdateCourse(course.Id, course.Name, course.Subject, course.InstructorId);
        }
        public async Task<string> DeletCourse(Guid id)
        {
            return await _dapperRepository.DeleteCourse(id);
        }  
      
        /////////////////////////////////////////////////  Instructor /////////////////////////////////////////////////////

        [GraphQLName("createInstructor")]
        public async Task<string> CreateInstructor( string fname, string lname, double salary)
        {
            return await _dapperRepository.creatinstractor(fname, lname, salary);
        }
        public async Task<string> UpdateInstructor(InstructorDTO instructor)
        {
            return await _dapperRepository.UpdateInstructor(instructor.Id, instructor.FirstName,instructor.LastName, instructor .Salary);
        }
        public async Task<string> DeletInstructor(Guid id)
        {
            return await _dapperRepository.DeleteInstractor(id);
        }

        /////////////////////////////////////////////////  Student /////////////////////////////////////////////////////
       
        public async Task<string> CreateStudent( string fname, string lname,double gpa)
        {
            return await _dapperRepository.creatStudent(fname, lname, gpa);
        }
        public async Task<string> UpdateStudent(StudentDTO student)
        {
            return await _dapperRepository.UpdateStudent(student.Id, student.FirstName, student.LastName, student.GPA);
        }
        public async Task<string> DeleteStudent(Guid id)
        {
            return await _dapperRepository.DeleteStudent(id);
        }

        //////////////////////////////////////////////// course student////////////////////////////////////////////////////////////
        
        public async Task<string> AddCourse(Guid studentid,Guid courseid)
        {
            return await _dapperRepository.Addcourse(studentid, courseid);
        }
        public async Task<string> RemoveCourse(Guid studentid,Guid courseid)
        {
            return await _dapperRepository.RemoveCourses(studentid, courseid);
        }
    }
}