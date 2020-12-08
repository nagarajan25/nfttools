﻿using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using System.IO;


namespace NFTIntegration.Data
{
    public class DataAdapter
    {
        private string sqliteConnectionString;

        public DataAdapter()
        {
            sqliteConnectionString = $"Data Source={Directory.GetCurrentDirectory()}\\DB\\rAtOOn.db";
        }

        public List<ReportData> GetZapReportList()
        {
            var reportDataList = new List<ReportData>();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand("SELECT ReportId,High,Medium,Low,Information,ReportFileName,RunDate FROM ZapReports ORDER BY ReportId DESC LIMIT 10", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                OpenConnection(sqliteConnection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    reportDataList.Add(new ReportData
                    {
                        ReportId = reader.GetInt64("ReportId"),
                        High = reader.GetInt32("High"),
                        Medium = reader.GetInt32("Medium"),
                        Low = reader.GetInt32("Low"),
                        Information = reader.GetInt32("Information"),
                        ReportFileName = reader.GetString("ReportFileName"),
                        RunDate = reader.GetString("RunDate")
                    });
                }

                CloseConnection(sqliteConnection);

            }

            return reportDataList;
        }

        public ReportData GetLastRunZapReport()
        {
            var reportData = new ReportData();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand("SELECT ReportId,High,Medium,Low,Information,ReportFileName,RunDate FROM ZapReports ORDER BY ReportId DESC LIMIT 1", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                OpenConnection(sqliteConnection);

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    reportData.ReportId = reader.GetInt64("ReportId");
                    reportData.High = reader.GetInt32("High");
                    reportData.Medium = reader.GetInt32("Medium");
                    reportData.Low = reader.GetInt32("Low");
                    reportData.Information = reader.GetInt32("Information");
                    reportData.ReportFileName = reader.GetString("ReportFileName");
                    reportData.RunDate = reader.GetString("RunDate");
                }

                CloseConnection(sqliteConnection);
            }

            return reportData;
        }

        public ReportData GetZapReportDetails(string reportId)
        {
            var reportDetails = new ReportData();

            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand($"SELECT ReportId,High,Medium,Low,Information,ReportFileName,RunDate FROM ZapReports WHERE ReportId={reportId}", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                OpenConnection(sqliteConnection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    reportDetails.ReportId = reader.GetInt64("ReportId");
                    reportDetails.High = reader.GetInt32("High");
                    reportDetails.Medium = reader.GetInt32("Medium");
                    reportDetails.Low = reader.GetInt32("Low");
                    reportDetails.Information = reader.GetInt32("Information");
                    reportDetails.ReportFileName = reader.GetString("ReportFileName");
                    reportDetails.RunDate = reader.GetString("RunDate");
                }

                CloseConnection(sqliteConnection);

            }

            return reportDetails;
        }

        public void AddReportDetails(ReportData reportData)
        {
            using (var sqliteConnection = new SqliteConnection(sqliteConnectionString))
            {
                var command = new SqliteCommand($"INSERT INTO ZapReports(ReportId,High,Medium,Low,Information,ReportFileName,RunDate) " +
                                                $"VALUES({reportData.ReportId},{reportData.High},{reportData.Medium},{reportData.Low}," +
                                                $"{reportData.Information},'{reportData.ReportFileName}','{reportData.RunDate}')", sqliteConnection)
                {
                    CommandType = CommandType.Text
                };

                OpenConnection(sqliteConnection);

                command.ExecuteNonQuery();

                CloseConnection(sqliteConnection);
            }
        }

        private void OpenConnection(SqliteConnection sqliteConnection)
        {
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_winsqlite3());

            if (sqliteConnection.State == ConnectionState.Closed)
            {
                sqliteConnection.Open();
            }
        }

        private void CloseConnection(SqliteConnection sqliteConnection)
        {
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_winsqlite3());

            if (sqliteConnection.State == ConnectionState.Open)
            {
                sqliteConnection.Close();
            }
        }
    }
}