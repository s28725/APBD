using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace APBD
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly List<string> allowedFields = new List<string> { "name", "description", "category", "area" };

        public AnimalsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private List<Dictionary<string, object>> ConvertDataTableToList(DataTable dt)
        {
            var columns = dt.Columns.Cast<DataColumn>();
            return dt.Rows.Cast<DataRow>()
                .Select(row => columns.ToDictionary(column => column.ColumnName, column => row[column])).ToList();
        }

        // GET: api/animals
        [HttpGet]
        public IActionResult GetAnimals(string orderBy = "name")
        {
            if (!allowedFields.Contains(orderBy.ToLower()))
            {
                return BadRequest("Invalid field for sorting. Allowed fields are: name, description, category, area");
            }

            string query = $"SELECT * FROM Animals ORDER BY {orderBy}";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        table.Load(myReader);
                    }
                }
                myCon.Close();
            }

            var list = ConvertDataTableToList(table);
            return Ok(list);
        }

        // POST: api/animals
        [HttpPost]
        public IActionResult PostAnimal(Animal animal)
        {
            string query = @"
               INSERT INTO Animals (Name, Description, Category, Area) 
               VALUES (@Name, @Description, @Category, @Area)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Name", animal.Name);
                    myCommand.Parameters.AddWithValue("@Description", animal.Description);
                    myCommand.Parameters.AddWithValue("@Category", animal.Category);
                    myCommand.Parameters.AddWithValue("@Area", animal.Area);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }

        // Further methods for PUT and DELETE to be added here

        // PUT: api/animals/{idAnimal}
        [HttpPut("{idAnimal}")]
        public IActionResult PutAnimal(int idAnimal, Animal animal)
        {
            if (animal == null)
            {
                return BadRequest("Animal object is null");
            }

            string query = @"
                    UPDATE Animals 
                    SET Name = @Name, 
                        Description = @Description, 
                        Category = @Category, 
                        Area = @Area 
                    WHERE IdAnimal = @IdAnimal";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@IdAnimal", idAnimal);
                    myCommand.Parameters.AddWithValue("@Name", animal.Name);
                    myCommand.Parameters.AddWithValue("@Description", animal.Description);
                    myCommand.Parameters.AddWithValue("@Category", animal.Category);
                    myCommand.Parameters.AddWithValue("@Area", animal.Area);
                    myCommand.ExecuteNonQuery();
                }
                myCon.Close();
            }

            return new JsonResult("Updated Successfully");
        }

        // DELETE: api/animals/{idAnimal}
        [HttpDelete("{idAnimal}")]
        public IActionResult DeleteAnimal(int idAnimal)
        {
            string query = @"
                   DELETE FROM Animals 
                   WHERE IdAnimal = @IdAnimal";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@IdAnimal", idAnimal);
                    int result = myCommand.ExecuteNonQuery();
                    if (result == 0)
                    {
                        return NotFound("Animal not found");
                    }
                }
                myCon.Close();
            }

            return new JsonResult("Deleted Successfully");
        }
    }
}
