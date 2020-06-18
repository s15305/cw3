using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using cw3.DTOs.Requests;
using cw3.Models;

namespace cw3.DAL
{
    public class DbService : IDbService
    {
        private string sqlConnectionData = "Data Source=(localdb)\\db-mssql;Initial Catalog=s15305;Integrated Security=True";
        private Dictionary<string, Claim[]> tokens = new Dictionary<string, Claim[]>();

        //cw4 Miłe wspomnienia 
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
                              BirthDate = new DateTime(),
                              IdEnrollment = int.Parse(dr["IdEnrollment"].ToString()),
                              Password = dr["Password"].ToString(),
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
        //cw5 Miłe wspomnienia
        public Enrollment EnrollStudent(EnrollStudentRequest request)
        {

            var st = new Student();
            st.FirstName = request.FirstName;
            st.FirstName = request.FirstName;
            st.BirthDate = request.BirthDate;
            st.Studies = request.Studies;
            st.Semester = 1;

            var enrollment = new Enrollment();
            enrollment.Semester = 1;

            using (var con = new SqlConnection(sqlConnectionData))
            using (var com = new SqlCommand())
            {
                com.Connection = con;

                con.Open();
                var tran = con.BeginTransaction();

                try
                {
                    //1. Czy studia istnieja?
                    com.CommandText = "SELECT IdStudy FROM Studies WHERE name=@name";
                    com.Parameters.AddWithValue("name", request.Studies);
                    com.Transaction = tran;
                    var dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        dr.Close();
                        tran.Rollback();
                        //return BadRequest("Studia nie istnieja");
                    }
                    int idstudies = (int)dr["IdStudy"];
                    enrollment.IdStudy = idstudies;
                    dr.Close();


                    //2. Czy Nr indexu jest unikalny?
                    com.CommandText = "SELECT INDEXNUMBER FROM STUDENT WHERE INDEXNUMBER = @Index";
                    com.Parameters.AddWithValue("Index", request.IndexNumber);
                    dr = com.ExecuteReader();

                    if (dr.Read())
                    {
                        dr.Close();
                        tran.Rollback();
                        //return BadRequest("W bazie istnieje juz student z takim numberem indeksu."); 
                    }

                    dr.Close();

                    int IdEnrollment = 0;

                    //3. Ostatni wpis w tabeli Enrollments zgodny ze studiami studenta - Semester = 1
                    com.CommandText = "Select IdEnrollment FROM Enrollment WHERE Semester = 1 AND IdStudy =" + idstudies;
                    dr = com.ExecuteReader();
                    if (dr.Read())
                    {

                        IdEnrollment = ((int)dr["IdEnrollment"]);
                        dr.Close();

                    }
                    else if (!dr.Read())
                    {
                        dr.Close();
                        com.CommandText = "Select IdEnrollment FROM Enrollment WHERE IdEnrollment = (Select MAX(IdEnrollment) FROM Enrollment)";
                        dr = com.ExecuteReader();
                        dr.Read();
                        IdEnrollment = ((int)dr["IdEnrollment"]) + 1;
                        dr.Close();
                        com.CommandText = "INSERT INTO ENROLLMENT (IDENROLLMENT,SEMESTER,IDSTUDY,STARTDATE) VALUES (" + IdEnrollment + ",1," + idstudies + ", '2021-09-10')";

                        com.ExecuteNonQuery();
                    }
                    else
                    {
                        tran.Rollback();
                        // return BadRequest("WARNING!");
                    };

                    enrollment.IdEnrollment = IdEnrollment;

                    //Dodanie studenta
                    com.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName, BirthDate, Studies, IdEnrollment) VALUES(@Index, @Fname, @Lname, @BDate, @Studies, @idEnrollment)";
                    com.Parameters.AddWithValue("Index", request.IndexNumber);
                    com.Parameters.AddWithValue("Fname", request.FirstName);
                    com.Parameters.AddWithValue("Lname", request.LastName);
                    com.Parameters.AddWithValue("BDate", request.BirthDate);
                    com.Parameters.AddWithValue("Studies", request.Studies);
                    com.Parameters.AddWithValue("idEnrollment", IdEnrollment);

                    com.ExecuteNonQuery();

                    tran.Commit();

                    dr.Close();
                    com.CommandText = "Select * From Enrollment WHERE IdEnrollment = " + IdEnrollment;
                    dr = com.ExecuteReader();
                    string message = "";
                    while (dr.Read())
                    {
                        message = string.Concat(message, '\n', "Enrollment ID: ", enrollment.IdEnrollment.ToString(), ", Semester: ", enrollment.Semester.ToString(), ", ID Studies: ", enrollment.IdStudy.ToString());
                    }
                }
                catch (SqlException exc)
                {
                    tran.Rollback();
                }
            }
            return enrollment;
        }

        public Enrollment PromoteStudents(int semester, string studies)
        {
            using (var con = new SqlConnection(sqlConnectionData))
            using (var com = new SqlCommand())
            {

                com.Connection = con;
                con.Open();
                var tran = con.BeginTransaction();

                com.CommandText = "EXEC PROMOTESTUDENTS @STUDIES = @studies, @SEMESTER = @semester;";

                com.Parameters.AddWithValue("studies", studies);
                com.Parameters.AddWithValue("semester", semester);

                com.Transaction = tran;
                var dr = com.ExecuteReader();
                int idEnrollment;


                Enrollment enrollment = new Enrollment();

                if (!dr.Read())
                {
                    tran.Rollback();
                    //return BadRequest("Bad request.");
                }
                else
                {

                    enrollment.IdEnrollment = (int)dr["IdEnrollment"];
                    enrollment.IdStudy = (int)dr["IdStudy"];
                    enrollment.Semester = (int)dr["Semester"];

                    idEnrollment = (int)dr[0];

                }
                dr.Close();

                tran.Commit();

                return enrollment;
            }
        }
        //cw6
        public bool IsExistingStudent(string id)
        {

            using (SqlConnection connection = new SqlConnection(sqlConnectionData))
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT FirstName from Student where IndexNumber=@index";
                command.Parameters.AddWithValue("index", id);
                connection.Open();
                SqlDataReader dr = command.ExecuteReader();
                if (dr.Read())
                {
                    return true;
                }
                return false;
            }
        }

        public Dictionary<string, Claim[]> getRefreshTokens()
        {
            return tokens;
        }
    }
}
