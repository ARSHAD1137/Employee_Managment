using Employee_Managment.SalaryModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Managment
{
    public class Salary
    {
        private static SqlConnection ConnectionSetup()
        {
            return new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=payroll_service16;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        public int UpdateEmployeeSalary(SalaryUpdateModel salaryUpdateModel)
        {
            SqlConnection SalaryConnection = ConnectionSetup();
            int salary = 0;
            try
            {
                using (SalaryConnection)
                {
                    string id = "2";
                    //string id=Console.ReadLine();
                    string query = @"select * from Employee where id=" + Convert.ToInt32(id);
                    SalaryDetailModel displayModel = new SalaryDetailModel();
                    SqlCommand command = new SqlCommand("spUpdateEmployeeSalary", SalaryConnection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", salaryUpdateModel.SalaryId);
                    command.Parameters.AddWithValue("@month", salaryUpdateModel.Month);
                    command.Parameters.AddWithValue("@salary", salaryUpdateModel.EmployeeSalary);
                    command.Parameters.AddWithValue("@EmpId", salaryUpdateModel.EmployeeId);
                    SalaryConnection.Open();
                    SqlDataReader dr = command.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            displayModel.EmployeeId = Convert.ToInt32(dr["EmpId"]);
                            displayModel.EmployeeName = dr["EName"].ToString();
                            displayModel.JobDescription = dr["JobDiscription"].ToString();
                            displayModel.EmployeeSalary = Convert.ToInt32(dr["EmpSal"]);
                            displayModel.Month = dr["SalaryMonth"].ToString();
                            displayModel.SalaryId = Convert.ToInt32(dr["SalaryId"]);
                            Console.WriteLine(displayModel.EmployeeName + " " + displayModel.EmployeeSalary + " " + displayModel.Month);
                            Console.WriteLine("\n");
                            salary = displayModel.EmployeeSalary;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found.");
                    }

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                SalaryConnection.Close();
            }
            return salary;
        }
        public void InsertEmployeeRecord(Employee employee)
        {
            SqlConnection connection = ConnectionSetup();
            employee.deduction = Convert.ToInt32(0.2 * employee.basicPay);
            employee.taxablePay = employee.basicPay - employee.deduction;
            employee.incomeTax = Convert.ToInt32(0.1 * employee.taxablePay);
            employee.netPay = employee.basicPay - employee.incomeTax;
            

            string storedProcedure = "SpAddEmployeeDetails";
            string storedProcedurePayroll = "sp_InsertPayrollDetails";
            using (connection)
            {
                connection.Open();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Insert Employee Transaction");
                try
                {
                    SqlCommand sqlCommand = new SqlCommand(storedProcedure, connection, transaction);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@StartDate", employee.startDate);
                    sqlCommand.Parameters.AddWithValue("@EmployeeName", employee.name);
                    sqlCommand.Parameters.AddWithValue("@Gender", employee.gender);
                    sqlCommand.Parameters.AddWithValue("@PhoneNumber", employee.phoneNumber);
                    sqlCommand.Parameters.AddWithValue("@Address", employee.address);
                    SqlParameter outPutVal = new SqlParameter("@scopeIdentifier", SqlDbType.Int);
                    outPutVal.Direction = ParameterDirection.Output;
                    sqlCommand.Parameters.Add(outPutVal);

                    sqlCommand.ExecuteNonQuery();
                    SqlCommand sqlCommand1 = new SqlCommand(storedProcedurePayroll, connection, transaction);
                    sqlCommand1.CommandType = CommandType.StoredProcedure;
                    sqlCommand1.Parameters.AddWithValue("@ID", outPutVal.Value);
                    sqlCommand1.Parameters.AddWithValue("@BasicPay", employee.basicPay);
                    sqlCommand1.Parameters.AddWithValue("@Deduction", employee.deduction);
                    sqlCommand1.Parameters.AddWithValue("@TaxablePay", employee.taxablePay);
                    sqlCommand1.Parameters.AddWithValue("@IncomeTax", employee.incomeTax);
                    sqlCommand1.Parameters.AddWithValue("@NetPay", employee.netPay);
                    sqlCommand1.ExecuteNonQuery();
                    transaction.Commit();
                    connection.Close();
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {

                        Console.WriteLine(ex2.Message);
                    }
                }
            }

        }
        public void AddEmployeeRecord(Employee employee)
        {
           SqlConnection Addconnection = ConnectionSetup();
           string StoreEmp = "SpAddEmployeeDetails";
           using (Addconnection)
           {
                Addconnection.Open();
                SqlTransaction transaction;
                transaction = Addconnection.BeginTransaction("Add Employee Details");
                try
                {
                    SqlCommand sqlCommand = new SqlCommand(StoreEmp, Addconnection, transaction);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@EmployeeName", employee.name);
                    sqlCommand.Parameters.AddWithValue("@PhoneNumber", employee.phoneNumber);
                    sqlCommand.Parameters.AddWithValue("@Gender", employee.gender);
                    sqlCommand.Parameters.AddWithValue("@Address", employee.address);
                    sqlCommand.Parameters.AddWithValue("@StartDate", employee.startDate);
                    sqlCommand.Parameters.AddWithValue("@BasicPay", employee.basicPay);
                    sqlCommand.Parameters.AddWithValue("@Deduction", employee.deduction);
                    sqlCommand.Parameters.AddWithValue("@TaxablePay", employee.taxablePay);
                    sqlCommand.Parameters.AddWithValue("@IncomeTax", employee.incomeTax);
                    sqlCommand.Parameters.AddWithValue("@NetPay", employee.netPay);
                }
                catch (Exception e2)
                {
                    throw new Exception(e2.Message);
                }
                finally
                {
                    Addconnection.Close();
                }

           }

        }
    }

}