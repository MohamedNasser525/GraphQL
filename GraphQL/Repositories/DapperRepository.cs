using Dapper;
using DapperAPI.Models;
using HotChocolate.Types.Pagination;
using Microsoft.IdentityModel.Tokens;
using GraphQL.Migrations;
using GraphQL.Models;
using System.Data;
using System.Numerics;

namespace DapperAPI.Repositories
{
    public class DapperRepository 
    {
        private readonly DapperContext _dbContext;

        public DapperRepository(DapperContext databaseContext)
        {
            _dbContext = databaseContext;
        }


        public async Task<List<Course>> GetCourses()
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var CourseDict = new Dictionary<Guid, Course>();

                var query = @"SELECT * 
                      FROM Courses
                      FULL JOIN CourseStudent ON CourseStudent.CoursesId = Courses.Id
                      LEFT JOIN Students ON CourseStudent.StudentsId = Students.Id
                      LEFT JOIN Instructors ON Courses.Instructorid = Instructors.Id;";

                var courses = await connection.QueryAsync<Course, Student,Instructor, Course>(
                    query,
                    (cour, stu,ins) =>
                    {
                        // Check if the course is already in the dictionary
                        if (!CourseDict.TryGetValue(cour.Id, out var currentCourse))
                        {
                            currentCourse = cour;
                            currentCourse.Students = new List<Student>();  
                            CourseDict.Add(currentCourse.Id, currentCourse);
                        }

                        currentCourse.Instructor = ins;
                        currentCourse.Students.Add(stu);
                        
                      
                        return currentCourse;
                    },
                    splitOn: "Id" 
                );

                return CourseDict.Values.ToList();
            }
        }

        public async Task<List<Student>> GetStudents()
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var instructorDict = new Dictionary<Guid, Student>();
                var query = @"SELECT *
                            FROM Students AS I
                            JOIN CourseStudent AS IC ON I.Id = IC.StudentsId
                            JOIN Courses AS C ON IC.CoursesId = C.Id";
              
                var instructors = await connection.QueryAsync<Student, Course, Student>(query
                    , (inst, cour) =>
                    {
                        if (!instructorDict.TryGetValue(inst.Id, out var currentInstructor))
                        {
                            currentInstructor = inst;
                            currentInstructor.Courses = new List<Course>();
                            instructorDict.Add(inst.Id, currentInstructor);
                        }

                        currentInstructor.Courses.Add(cour);

                        return currentInstructor;
                    },
                   splitOn: "Id"  
                 );

                return instructorDict.Values.ToList();
            }
        }

        public async Task<List<Instructor>> GetInstractors()
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var instructors = await connection.QueryAsync<Instructor,Course,Instructor>(
                    @"SELECT * FROM Instructors as I Join Courses as C on C.Instructorid = I.Id", (inst, cour) =>
                    {
                        inst.Courses.Add(cour) ;
                        return inst;
                    }
                    , splitOn: "Id");

                return instructors.ToList();
            }
        }

        public async Task<string> creatcourse(string name,Subject s,Guid instaid)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var affectedRow = await connection.ExecuteAsync("INSERT INTO Courses (Id, Name,Subject,Instructorid) VALUES (@Id, @Name,@Subject,@Instructorid)", new { Id=Guid.NewGuid(),Name=name,Subject=s,Instructorid= instaid });
                return affectedRow > 1 ? "DONE" : "error";
            }
        }
        public async Task<string> creatinstractor(string fname, string lname,double salary)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var affectedRow = await connection.ExecuteAsync("INSERT INTO Instructors (Id, FirstName,LastName,Salary) VALUES (@Id, @fName,@lName,@s)", new { Id=Guid.NewGuid(),fName=fname,lname=lname,s=salary });
                return affectedRow > 1 ? "DONE" : "error";
            }
        }
        public async Task<string> creatStudent(string fname, string lname,double gpa)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var affectedRow = await connection.ExecuteAsync("INSERT INTO Studebts (Id, FirstName,LastName,GPA) VALUES (@Id, @fName,@lName,@s)", new { Id=Guid.NewGuid(),fName=fname,lname=lname,s=gpa });
                return affectedRow > 1 ? "DONE" : "error";
            }
        }
         public async Task<string> Addcourse(Guid studentid , Guid courseid)
         {
            using (var connection = _dbContext.CreateConnection())
            {
                var affectedRow = await connection.ExecuteAsync("INSERT INTO CourseStudent (CoursesId, StudentsId) VALUES (@cid,@sid)", new { cid=courseid,sid=studentid });
                return affectedRow>1?"DONE":"error";
            }
         }

        public async Task<string> DeleteCourse(Guid id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var affectedRows = await connection.ExecuteAsync("DELETE FROM Courses WHERE Id=@id", new { id = id });
                return affectedRows>0?"DONE":"error";
            }
        }
        public async Task<string> DeleteInstractor(Guid id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var affectedRows = await connection.ExecuteAsync("DELETE FROM Instructors WHERE Id=@id", new { id = id });
                return affectedRows>0?"DONE":"error";
            }
        }
        public async Task<string> DeleteStudent(Guid id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var affectedRows = await connection.ExecuteAsync("DELETE FROM Studebts WHERE Id=@id", new { id = id });
                return affectedRows>0?"DONE":"error";
            }
        }
        public async Task<string> RemoveCourses(Guid studentid, Guid courseid)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var affectedRows = await connection.ExecuteAsync("DELETE FROM CourseStudent (CoursesId, StudentsId) VALUES (@cid,@sid)", new { cid = courseid, sid = studentid });
                return affectedRows>0?"DONE":"error";
            }
        }



        public async Task<string> UpdateCourse(Guid id,string name, Subject s, Guid instaid)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var affectedRow = await connection.ExecuteAsync("UPDATE  Courses SET Name=@Name, Subject=@Subject,Instructorid=@Instructorid WHERE Id =@id", new { Id = id, Name = name, Subject = s, Instructorid = instaid });
                return affectedRow>0?"DONE":"error";
            }
        } 
        public async Task<string> UpdateStudent(Guid id,string fname, string lname,double GPA)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var affectedRow = await connection.ExecuteAsync("UPDATE  Students SET FirstName=@fName,LastName=@lname , GPA=@GPA WHERE Id =@id", new { Id = id, fname = fname, lname=lname,GPA=GPA });
                return affectedRow>0?"DONE":"error";
            }
        } 
        public async Task<string> UpdateInstructor(Guid id,string fname, string lname,double Salary)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var affectedRow = await connection.ExecuteAsync("UPDATE  Instructors SET FirstName=@fName,LastName=@lname , Salary=@Salary WHERE Id =@id", new { Id = id, fname = fname, lname=lname, Salary = Salary });
                return affectedRow>0?"DONE":"error";
            }
        } 
      

    }
}
