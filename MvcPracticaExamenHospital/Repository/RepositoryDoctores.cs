using Microsoft.Data.SqlClient;
using MvcPracticaExamenHospital.Models;
using System.Data;
using System.Threading.Tasks;

namespace MvcPracticaExamenHospital.Repository
{
    public class RepositoryDoctores
    {
        DataTable tablaDoctores;
        SqlConnection cn;
        SqlCommand com;

        public RepositoryDoctores()
        {
            string connectionString = @"Data Source=LOCALHOST\DEVELOPER;Initial Catalog=HOSPITAL;User ID=sa;Trust Server Certificate=True";
            string sql = "select * from DOCTOR";
            SqlDataAdapter ad = new SqlDataAdapter(sql, connectionString);
            this.tablaDoctores = new DataTable();
            ad.Fill(this.tablaDoctores);
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<Doctor> GetDoctores()
        {
            var consulta = from datos in this.tablaDoctores.AsEnumerable()
                           select datos;
            List<Doctor> doctores = new List<Doctor>();

            foreach (var row in consulta)
            {
                Doctor d = new Doctor();
                d.IdDoctor = row.Field<int>("DOCTOR_NO");
                d.Apellido = row.Field<string>("APELLIDO");
                d.Especialidad = row.Field<string>("ESPECIALIDAD");
                d.Salario = row.Field<int>("SALARIO");
                d.IdHospital = row.Field<int>("HOSPITAL_COD");
                doctores.Add(d);
            }

            return doctores;
        }

        public Doctor GetDetallesDoctor(int id)
        {
            var consulta = from datos in this.tablaDoctores.AsEnumerable()
                           where datos.Field<int>("DOCTOR_NO")==id
                           select datos;
            var row = consulta.First();
            Doctor d = new Doctor();
            d.IdDoctor = row.Field<int>("DOCTOR_NO");
            d.Apellido = row.Field<string>("APELLIDO");
            d.Especialidad = row.Field<string>("ESPECIALIDAD");
            d.Salario = row.Field<int>("SALARIO");
            d.IdHospital = row.Field<int>("HOSPITAL_COD");
            return d;
        }

        public async Task UpsertDoctor(int id, int hospital, string apellido, string especialidad, int salario)
        {
            string sql = "SP_UPSERT_DOCTOR";
            this.com.Parameters.AddWithValue("@id", id);
            this.com.Parameters.AddWithValue("@hospital", hospital);
            this.com.Parameters.AddWithValue("@apellido", apellido);
            this.com.Parameters.AddWithValue("@especialidad", especialidad);
            this.com.Parameters.AddWithValue("@salario", salario);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }
    }
}
