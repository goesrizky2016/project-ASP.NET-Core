using ExportDemo.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using ClosedXML.Excel;
using System.IO;
using System.Linq;

namespace ExportDemo.Controllers
{
    public class ExportController : Controller
    {
        private readonly IPersonRepository _personRepository;

        public ExportController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DownloadExcel()
        {
            var persons = _personRepository.GetPersons().ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Data Persons");

            // Header
            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Nama";
            worksheet.Cell(1, 3).Value = "Umur";

            // Data
            for (int i = 0; i < persons.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = persons[i].Id;
                worksheet.Cell(i + 2, 2).Value = persons[i].Nama;
                worksheet.Cell(i + 2, 3).Value = persons[i].Umur;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "PersonsData.xlsx";

            return File(stream.ToArray(), contentType, fileName);
        }

        public IActionResult DownloadPdf()
        {
            var persons = _personRepository.GetPersons().ToList();

            return new ViewAsPdf("PdfView", persons)
            {
                FileName = "PersonsData.pdf"
            };
        }
    }
}
