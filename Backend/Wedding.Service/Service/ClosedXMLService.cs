using ClosedXML.Excel;
using Wedding.Model.DTO;
using Wedding.Service.IService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Wedding.Service.Service;

public class ClosedXMLService : IClosedXMLService
{
    private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _config;

    public ClosedXMLService(IWebHostEnvironment env, IConfiguration config)
    {
        _env = env;
        _config = config;
    }
    

    public async Task<string> ExportCustomerExcel(List<CustomerFullInfoDTO> customerInfoDtos)
    {
    // Tạo đường dẫn đến thư mục lưu trữ file Excel
    string exportFolderPath = Path.Combine(_env.ContentRootPath, _config["FolderPath:CustomerExportFolderPath"]);

    // Tạo thư mục nếu chưa tồn tại
    if (!Directory.Exists(exportFolderPath))
    {
        Directory.CreateDirectory(exportFolderPath);
    }

    // Tạo tên file duy nhất
    string fileNameCustomer = $"Customers_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
    string filePath = Path.Combine(exportFolderPath, fileNameCustomer);

    using (var workBook = new XLWorkbook())
    {
        var workSheet = workBook.Worksheets.Add("Customers");

        // Create header
        workSheet.Cell(1, 1).Value = "Customer Id";
        workSheet.Cell(1, 2).Value = "UserId";
        workSheet.Cell(1, 3).Value = "FullName";
        workSheet.Cell(1, 4).Value = "Email";
        workSheet.Cell(1, 5).Value = "PhoneNumber";
        workSheet.Cell(1, 6).Value = "Gender";
        workSheet.Cell(1, 7).Value = "BirthDate";
        workSheet.Cell(1, 8).Value = "Country";
        workSheet.Cell(1, 9).Value = "Address";
        
        // Create data
        for (int i = 0; i < customerInfoDtos.Count; i++)
        {
            var student = customerInfoDtos[i];

            workSheet.Cell(i + 2, 1).Value = student.CustomerId.ToString();
            workSheet.Cell(i + 2, 2).Value = student.UserId;
            workSheet.Cell(i + 2, 3).Value = student.FullName;
            workSheet.Cell(i + 2, 4).Value = student.Email;
            workSheet.Cell(i + 2, 5).Value = student.PhoneNumber;
            workSheet.Cell(i + 2, 6).Value = student.Gender;
            workSheet.Cell(i + 2, 7).Value = student.BirthDate?.ToString("yyyy-MM-dd");
            workSheet.Cell(i + 2, 8).Value = student.Country;
            workSheet.Cell(i + 2, 9).Value = student.Address;
        }

        workSheet.Columns().AdjustToContents();

        // Export to memory stream
        workBook.SaveAs(filePath);

        return fileNameCustomer;
    }
    }
}