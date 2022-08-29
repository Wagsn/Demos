using FastReport;
using IntelligentHardware.Domain.Response;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;

namespace Wagsn.Device.Reporter
{
    /// <summary>
    /// FastReport帮助器
    /// </summary>
    public class FastReportHelper
    {
        /// <summary>
        /// 打印模板
        /// </summary>
        /// <param name="reportFrxUri">
        /// 报表模板文件URI，格式如下：<br/>
        /// http://localhost:7855/userresource/text.pdf <br/>
        /// file:///E:/Code/text.pdf</param>
        /// </param>
        /// <param name="printerName"></param>
        /// <param name="conStr">SQLServer数据库连接字符串</param>
        /// <param name="dataIds">数据筛选条件</param>
        /// <param name="outMsg">提示信息</param>
        /// <returns></returns>
        public static void PrintTemplate(string reportFrxUri, string printerName, string conStr, string dataIds)
        {
            //using (PDFExport pdfExport = new PDFExport()) { }
            
            if ((string.IsNullOrWhiteSpace(reportFrxUri)) || !Uri.IsWellFormedUriString(reportFrxUri, UriKind.RelativeOrAbsolute))
            {
                throw new Exception($"报表模板有问题{reportFrxUri}");
            }
            using (Report report = new Report())
            {
                using (WebClient webClient = new WebClient())
                {
                    var frxText = webClient.DownloadString(reportFrxUri);
                    report.LoadFromString(frxText);

                    // .frx 模板  .fpx 数据准备好的  .pdf PDF文件
                    var reportXmlUri = reportFrxUri.Replace(".frx", ".xml");
                    var reportBytes = webClient.DownloadData(reportXmlUri);
                    var reportJson = Encoding.UTF8.GetString(reportBytes);
                    ReportModel reportDataSouuce = Newtonsoft.Json.JsonConvert.DeserializeObject<ReportModel>(reportJson);
                    DataSet ds = new DataSet();
                    foreach (var item in reportDataSouuce.DataSource)
                    {
                        var value = item.DefaultValue;
                        if (!string.IsNullOrEmpty(dataIds))
                        {
                            value = dataIds;
                        }
                        if (string.IsNullOrEmpty(value))
                        {
                            throw new Exception("sql 语句没有得到where 值");
                        }
                        var sql = item.SelectCommand.Replace("#V#", value);

                        var dt = GetDataTable(conStr, sql);
                        dt.TableName = item.DataName;
                        ds.Tables.Add(dt);
                    }
                    report.RegisterData(ds, "Data");

                    report.PrintSettings.ShowDialog = false;
                    report.PrintSettings.Printer = printerName;

                    (new EnvironmentSettings()).ReportSettings.ShowProgress = false;

                    report.Print();
                }
            }
        }


        private static DataTable GetDataTable(string connStr, string sql)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    DataTable ds = new DataTable();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);
                    return ds;
                }
            }
        }

        public static byte[] BuildPdf(string reportFrxUri, string conStr, string dataIds)
        {
            if (string.IsNullOrWhiteSpace(reportFrxUri) || !Uri.IsWellFormedUriString(reportFrxUri, UriKind.RelativeOrAbsolute))
            {
                throw new ArgumentException("报表模板地址有误：" + reportFrxUri, nameof(reportFrxUri));
            }
            using (Report report = new Report())
            {
                using (WebClient webClient = new WebClient())
                {
                    var frxText = webClient.DownloadString(reportFrxUri);
                    report.LoadFromString(frxText);

                    // .frx 模板  .fpx 数据准备好的  .pdf PDF文件
                    var reportXmlUri = reportFrxUri.Replace(".frx", ".xml");
                    var reportBytes = webClient.DownloadData(reportXmlUri);
                    var reportJson = Encoding.UTF8.GetString(reportBytes);
                    ReportModel reportDataSouuce = Newtonsoft.Json.JsonConvert.DeserializeObject<ReportModel>(reportJson);
                    DataSet ds = new DataSet();
                    foreach (var item in reportDataSouuce.DataSource)
                    {
                        var value = item.DefaultValue;
                        if (!string.IsNullOrEmpty(dataIds))
                        {
                            value = dataIds;
                        }
                        if (string.IsNullOrEmpty(value))
                        {
                            throw new Exception("sql 语句没有得到where 值");
                        }
                        var sql = item.SelectCommand.Replace("#V#", value);

                        var dt = GetDataTable(conStr, sql);
                        dt.TableName = item.DataName;
                        ds.Tables.Add(dt);
                    }
                    report.RegisterData(ds, "Data");
                    report.Prepare();

                    using (MemoryStream ms = new MemoryStream())
                    {
                        FastReport.Export.Pdf.PDFExport pdfExport = new FastReport.Export.Pdf.PDFExport();
                        pdfExport.Export(report, ms);
                        return ms.ToArray();
                    }
                }
            }
        }

        public static byte[] BuildFpx(string reportFrxUri, string conStr, string dataIds)
        {
            if (string.IsNullOrWhiteSpace(reportFrxUri) || !Uri.IsWellFormedUriString(reportFrxUri, UriKind.RelativeOrAbsolute))
            {
                throw new ArgumentException("报表模板地址有误：" + reportFrxUri, nameof(reportFrxUri));
            }
            using (Report report = new Report())
            {
                using (WebClient webClient = new WebClient())
                {
                    var frxText = webClient.DownloadString(reportFrxUri);
                    report.LoadFromString(frxText);

                    // .frx 模板  .fpx 数据准备好的  .pdf PDF文件
                    var reportXmlUri = reportFrxUri.Replace(".frx", ".xml");
                    var reportBytes = webClient.DownloadData(reportXmlUri);
                    var reportJson = Encoding.UTF8.GetString(reportBytes);
                    ReportModel reportDataSouuce = Newtonsoft.Json.JsonConvert.DeserializeObject<ReportModel>(reportJson);
                    DataSet ds = new DataSet();
                    foreach (var item in reportDataSouuce.DataSource)
                    {
                        var value = item.DefaultValue;
                        if (!string.IsNullOrEmpty(dataIds))
                        {
                            value = dataIds;
                        }
                        if (string.IsNullOrEmpty(value))
                        {
                            throw new Exception("sql 语句没有得到where 值");
                        }
                        var sql = item.SelectCommand.Replace("#V#", value);

                        var dt = GetDataTable(conStr, sql);
                        dt.TableName = item.DataName;
                        ds.Tables.Add(dt);
                    }
                    report.RegisterData(ds, "Data");
                    report.Prepare();

                    using (MemoryStream ms = new MemoryStream())
                    {
                        report.SavePrepared(ms);
                        return ms.ToArray();
                    }
                }
            }
        }
    }
}
