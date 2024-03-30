using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Semana03
{
    public partial class MainWindow : Window
    {
        private const string ConnectionString = "Data Source=LAB1504-27\\SQLEXPRESS;Initial Catalog=Tecsupdb;Integrated Security=True";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnListDataTable_Click(object sender, RoutedEventArgs e)
        {
            List<Student> students = GetStudentsUsingDataTable();
            dataGrid.ItemsSource = students;
        }

        private void btnListDataReader_Click(object sender, RoutedEventArgs e)
        {
            List<Student> students = GetStudentsUsingDataReader();
            dataGrid.ItemsSource = students;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text;
            List<Student> students = SearchStudentsByName(searchText);
            dataGrid.ItemsSource = students;
        }

        private List<Student> GetStudentsUsingDataTable()
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Students", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    students.Add(new Student
                    {
                        StudentId = Convert.ToInt32(row["StudentId"]),
                        FirstName = row["FirstName"].ToString(),
                        LastName = row["LastName"].ToString()
                    });
                }
            }

            return students;
        }

        private List<Student> GetStudentsUsingDataReader()
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Students", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    students.Add(new Student
                    {
                        StudentId = Convert.ToInt32(reader["StudentId"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString()
                    });
                }
            }

            return students;
        }

        private List<Student> SearchStudentsByName(string name)
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Students WHERE FirstName LIKE @Name OR LastName LIKE @Name", connection);
                command.Parameters.AddWithValue("@Name", "%" + name + "%");
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    students.Add(new Student
                    {
                        StudentId = Convert.ToInt32(row["StudentId"]),
                        FirstName = row["FirstName"].ToString(),
                        LastName = row["LastName"].ToString()
                    });
                }
            }

            return students;
        }
    }
}
