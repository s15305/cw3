using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cw3.Models;

namespace cw3.DAL
{
    public class DbService : IDbService
    {
        private string sqlConnectionData = "Data Source=(localdb)\\db-mssql;Initial Catalog=s15305;Integrated Security=True";

        public IEnumerable<Student> GetStudents()
        {
            var output = new List<Student>();
            using (var connection = new SqlConnection(sqlConnectionData))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Student";
                    connection.Open();
                    var dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        output.Add(new Student
                        {
                            IndexNumber = dr["IndexNumber"].ToString(),
                            FirstName = dr["FirstName"].ToString(),
                            LastName = dr["LastName"].ToString(),
                            BirthDate = DateTime.Parse(dr["BirthDate"].ToString()),
                            IdEnrollment = int.Parse(dr["IdEnrollment"].ToString()),
                        });
                    }
                }
            }
            return output;
        }

        public IEnumerable<Enrollment> GetStudent(int indexNumber)
        {
            List<Enrollment> enrollments = new List<Enrollment>();
            using (var connection = new SqlConnection(sqlConnectionData))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Enrollment e " +
                                          "INNER JOIN Student s " +
                                          "ON e.IdEnrollment = s.IdEnrollment " +
                                          "WHERE s.IndexNumber = @indexNumber;";

                    command.Parameters.AddWithValue("indexNumber", indexNumber);
                    connection.Open();
                    var dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        var enrollment = new Enrollment();
                        enrollment.IdEnrollment = int.Parse(dr["IdEnrollment"].ToString());
                        enrollment.Semester = int.Parse(dr["Semester"].ToString());
                        enrollment.IdStudy = int.Parse(dr["IdStudy"].ToString());
                        enrollment.StartDate = DateTime.Parse(dr["StartDate"].ToString());
                        enrollments.Add(enrollment);
                    }
                    return enrollments;
                }
            }
        }
    }
}
