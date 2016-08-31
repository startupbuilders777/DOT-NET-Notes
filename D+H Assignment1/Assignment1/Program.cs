using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Data.SqlClient;

using System.Data.SqlTypes;

namespace Assignment1
{

    class Program
    {
        static int NUMOFTASKS = 10;
        static int NUMOFEXECUTIONSPERTASK = 10;

        static int skipRows = 0;
        static int pageSize = 20;
        static string orderBy = "OrderId-desc~TransactionId-desc";
        static int isAdmin = 1; //maybe should be bool
        static long userCorporationId = 2;
        static string laguage = "en";
        static long orderId = 0;
        static string orderIdOperator = "";
        static long transactionId = 0;
        static string trasactionIdOperator = "";
        static string jurisdiction = "";
        static string jurisdictionOperator = "";
        static string reference1 = "";
        static string reference1Operator = "";
        static string reference2 = "";
        static string reference2Operator = "";
        static string reference3 = "";
        static string reference3Operator = "";
        static string transactionType = "";
        static string transactionTypeOperator = "";
        static string serviceType = "";
        static string serviceTypeOperator = "";
        static string transactionStatus = "";
        static string transactionStatusOperator = "";
        static DateTime dateCreated = new DateTime(2015,5,6);
        static string dateCreatedOperator = "";
        static string owner = "";
        static string ownerOperator = "";
        static long searchRegistrationTrasactionId = 0;
        static string searchRegistrationTrasactionIdOperator = "";
        static long corporationId = 0;
        static string corporationIdOperator = "";
        static long branchId = 0;
        static string branchIdOperator = "";
        static string searchCriteriaOrPrimaryDebtor = "";
        static string searchCriteriaPrimaryDebtorOperator = "";

        static void Main(string[] args)
        {

           
            string connectionString = "Server=ZEBRADB-DM13-TOR.dhltd.corp;Database=CGE;User Id=DIT_CGe; Password=password;MultipleActiveResultSets=True";
            Stopwatch stopWatch = new Stopwatch();

            long[,,] data = new long[10, 10, 2];

            SqlConnection testConnection = new SqlConnection(connectionString);

            try
            {
                testConnection.Open();
                Console.WriteLine($"Server Version: {testConnection.ServerVersion}");
                Console.WriteLine($"Connection is: {testConnection.State.ToString()}");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Connecting To Database: " + e.Message);
            }
            finally
            {
                testConnection.Close();
            }


            for (int a = 5; a != data.GetLength(0); ++a)
            {
                Task[] taskArr = new Task[a];
                for (int b = 1; b != data.GetLength(1); ++b)
                {
                    stopWatch.Start();
                    for (int i = 0; i != a; ++i)
                    {
                        taskArr[i] = new Task(() =>
                        {
                            var userId = i;
                            Stopwatch taskWatch = new Stopwatch();
                            taskWatch.Start();
                            Console.WriteLine($"User {userId} started.");
                            
                            SqlConnection connection = new SqlConnection(connectionString);
                            SqlCommand cmd = new SqlCommand();
                            SqlDataReader reader;
                            cmd.CommandText = "dbo.MyQueueList";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = connection;

                            cmd.Parameters.AddWithValue("@SkipRows", skipRows);
                            cmd.Parameters.AddWithValue("@PageSize", pageSize);
                            cmd.Parameters.AddWithValue("@OrderBy", orderBy);
                            cmd.Parameters.AddWithValue("@IsAdmin", isAdmin);
                            cmd.Parameters.AddWithValue("@UserCorporationId", userCorporationId);
                            cmd.Parameters.AddWithValue("@Language", laguage);
                            cmd.Parameters.AddWithValue("@OrderId", orderId);
                            cmd.Parameters.AddWithValue("@OrderIdOperator", null);
                            cmd.Parameters.AddWithValue("@TransactionId", null);
                            cmd.Parameters.AddWithValue("@TransactionIdOperator", null);
                            cmd.Parameters.AddWithValue("@Jurisdiction", null);
                            cmd.Parameters.AddWithValue("@JurisdictionOperator", null);
                            cmd.Parameters.AddWithValue("@Reference1", null);
                            cmd.Parameters.AddWithValue("@Reference1Operator", null);
                            cmd.Parameters.AddWithValue("@Reference2", null);
                            cmd.Parameters.AddWithValue("@Reference2Operator", null);
                            cmd.Parameters.AddWithValue("@Reference3", null);
                            cmd.Parameters.AddWithValue("@Reference3Operator", null);
                            cmd.Parameters.AddWithValue("@TransactionType", null);
                            cmd.Parameters.AddWithValue("@TransactionTypeOperator", null);
                            cmd.Parameters.AddWithValue("@ServiceType", null);
                            cmd.Parameters.AddWithValue("@ServiceTypeOperator", null);
                            cmd.Parameters.AddWithValue("@TransactionStatus", null);
                            cmd.Parameters.AddWithValue("@TransactionStatusOperator", null);
                            cmd.Parameters.AddWithValue("@DateCreated", null);
                            cmd.Parameters.AddWithValue("@DateCreatedOperator", null);
                            cmd.Parameters.AddWithValue("@Owner", null);
                            cmd.Parameters.AddWithValue("@OwnerOperator", null);
                            cmd.Parameters.AddWithValue("@SearchRegistrationTransactionId", null);
                            cmd.Parameters.AddWithValue("@SearchRegistrationTransactionIdOperator", null);
                            cmd.Parameters.AddWithValue("@CorporationId", null);
                            cmd.Parameters.AddWithValue("@CorporationIdOperator", null);
                            cmd.Parameters.AddWithValue("@BranchId", null);
                            cmd.Parameters.AddWithValue("@BranchIdOperator", null);
                            cmd.Parameters.AddWithValue("@SearchCriteriaOrPrimaryDebtor", null);
                            cmd.Parameters.AddWithValue("@SearchCriteriaOrPrimaryDebtorOperator", null);
                            /*cmd.Parameters.AddWithValue("@OrderIdOperator", orderIdOperator);
                            cmd.Parameters.AddWithValue("@TransactionId", transactionId);
                            cmd.Parameters.AddWithValue("@TransactionIdOperator", trasactionIdOperator);
                            cmd.Parameters.AddWithValue("@Jurisdiction", jurisdiction);
                            cmd.Parameters.AddWithValue("@JurisdictionOperator", jurisdictionOperator);
                            cmd.Parameters.AddWithValue("@Reference1", reference1);
                            cmd.Parameters.AddWithValue("@Reference1Operator", reference1Operator);
                            cmd.Parameters.AddWithValue("@Reference2", reference2);
                            cmd.Parameters.AddWithValue("@Reference2Operator", reference2Operator);
                            cmd.Parameters.AddWithValue("@Reference3", reference3);
                            cmd.Parameters.AddWithValue("@Reference3Operator", reference3Operator);
                            cmd.Parameters.AddWithValue("@TransactionType", transactionType);
                            cmd.Parameters.AddWithValue("@TransactionTypeOperator", transactionTypeOperator);
                            cmd.Parameters.AddWithValue("@ServiceType", serviceType);
                            cmd.Parameters.AddWithValue("@ServiceTypeOperator", serviceTypeOperator);
                            cmd.Parameters.AddWithValue("@TransactionStatus", transactionStatus);
                            cmd.Parameters.AddWithValue("@TransactionStatusOperator", transactionStatusOperator);
                            cmd.Parameters.AddWithValue("@DateCreated", dateCreated);
                            cmd.Parameters.AddWithValue("@DateCreatedOperator", dateCreatedOperator);
                            cmd.Parameters.AddWithValue("@Owner", owner);
                            cmd.Parameters.AddWithValue("@OwnerOperator", ownerOperator);
                            cmd.Parameters.AddWithValue("@SearchRegistrationTransactionId", searchRegistrationTrasactionId);
                            cmd.Parameters.AddWithValue("@SearchRegistrationTransactionIdOperator", searchRegistrationTrasactionIdOperator);
                            cmd.Parameters.AddWithValue("@CorporationId", corporationId);
                            cmd.Parameters.AddWithValue("@CorporationIdOperator", corporationIdOperator);
                            cmd.Parameters.AddWithValue("@BranchId", branchId);
                            cmd.Parameters.AddWithValue("@BranchIdOperator", branchIdOperator);
                            cmd.Parameters.AddWithValue("@SearchCriteriaOrPrimaryDebtor", searchCriteriaOrPrimaryDebtor);
                            cmd.Parameters.AddWithValue("@SearchCriteriaOrPrimaryDebtorOperator", searchCriteriaPrimaryDebtorOperator);*/
                          
                            for (int j = 0; j != b; ++j)
                            {
                                connection.Open();
                                reader = cmd.ExecuteReader();
                                connection.Close();
                                reader.Close();
                            }
                            Console.WriteLine();
                            Console.WriteLine($"User {userId} ended. Time Elapsed: {taskWatch.Elapsed}");
                        });
                        taskArr[i].Start();
                        Console.WriteLine();
                    }
                    Task.WaitAll(taskArr);
                    Console.WriteLine();
                    stopWatch.Stop();
                    Console.WriteLine($"With {a} users, and {b} executions/user, time:{stopWatch.ElapsedMilliseconds} ");
                    data[a, b, 0] = stopWatch.ElapsedMilliseconds;
                    stopWatch.Reset();
                }
                Console.WriteLine();
            }
        }
       }
    }
