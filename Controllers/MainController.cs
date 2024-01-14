using CrudApiAssignment.Interfaces;
using CrudApiAssignment.Models;
using CrudApiAssignment.Queries;
using CrudApiAssignment.Utilities;
using FluentValidation;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Security.Claims;

namespace CrudApiAssignment.Controllers
{
    [Route("simpleCrudApi")]
    [ApiController]
    [Authorize]
    public class MainController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
        public MainController(IMediator mediator, IUserRepository userRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
        }

        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var token = await _mediator.Send(loginRequest);
            return token.Result;

        }

        [Route("allUsers")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var data = await _mediator.Send(new GetAllUsersQuery());
            return data.Result;
        }

        [Route("user/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetSingleUser(string userId)
        {
            var data = await _mediator.Send(new GetSingleUserQuery(userId));
            return data.Result;
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] UserRequest request)
        {
            var data = await _mediator.Send(request);
            return data.Result;
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UserUpdateRequest request)
        {
            var data = await _mediator.Send(request);
            return data.Result;
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var data = await _mediator.Send(new DeleteUserQuery(id));
            return data.Result;
        }

        [HttpPost]
        [Route("search")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Search([FromBody] SearchUserRequest searchUserRequest) 
        { 
            var data = await _mediator.Send(searchUserRequest);
            return data.Result;
        }

        [HttpPost("search/export/{type}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Export([FromBody] SearchUserRequest searchUserRequest, [FromRoute] string type) 
        {
            var searchData = await this.Search(searchUserRequest);
            if (searchData is ObjectResult objectResult && objectResult.Value is SearchUserResponse exportData)
            {
                if (type == "excel")
                {
                    var excelStream = ExportToExcel(exportData);
                    return File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "exported_users.xlsx");
                }
                else if (type == "pdf")
                {
                    var pdfStream = ExportToPdf(exportData);
                    return File(pdfStream, "application/pdf", "exported_users.pdf");
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest("Unexpected result from search operation.");
            }
        }

        private MemoryStream ExportToPdf(SearchUserResponse searchUserResponse)
        {
            var users = searchUserResponse.Users;
            var memoryStream = new MemoryStream();

            using (var pdfWriter = new PdfWriter(memoryStream))
            {
                using (var pdfDocument = new PdfDocument(pdfWriter))
                {
                    var document = new Document(pdfDocument);

                    document.Add(new Paragraph($"{DateTime.UtcNow.ToShortDateString()}").SetBold().SetFontSize(24).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                    var table = new Table(6);
                    table.AddHeaderCell("User ID");
                    table.AddHeaderCell("Username");
                    table.AddHeaderCell("Password");
                    table.AddHeaderCell("IsAdmin");
                    table.AddHeaderCell("Age");
                    table.AddHeaderCell("Hobbies");

                    foreach (var user in users)
                    {
                        table.AddCell(user.Id.ToString());
                        table.AddCell(user.Username);
                        table.AddCell(user.Password);
                        table.AddCell(user.IsAdmin.ToString());
                        table.AddCell(user.Age.ToString());
                        table.AddCell(string.Join(", ", user.Hobbies));
                    }

                    document.Add(table);

                    document.Add(new Paragraph($"Page {searchUserResponse.CurrentPage} of {searchUserResponse.TotalPages}").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                }
                
            }

            var copiedMemoryStream = new MemoryStream(memoryStream.ToArray());
            copiedMemoryStream.Seek(0, SeekOrigin.Begin);
            return copiedMemoryStream;
        }

        private MemoryStream ExportToExcel(SearchUserResponse searchUserResponse)
        {
            var users = searchUserResponse.Users;
            var excelPackage = new ExcelPackage();
            var worksheet = excelPackage.Workbook.Worksheets.Add("Users");

            worksheet.Cells[1, 1].Value = "User ID";
            worksheet.Cells[1, 2].Value = "Username";
            worksheet.Cells[1, 3].Value = "Password";
            worksheet.Cells[1, 4].Value = "IsAdmin";
            worksheet.Cells[1, 5].Value = "Age";
            worksheet.Cells[1, 6].Value = "Hobbies";
           
            for (int i = 0; i < users.Count; i++)
            {
                worksheet.Cells[i + 3, 1].Value = users[i].Id;
                worksheet.Cells[i + 3, 2].Value = users[i].Username;
                worksheet.Cells[i + 3, 3].Value = users[i].Password;
                worksheet.Cells[i + 3, 4].Value = users[i].IsAdmin;
                worksheet.Cells[i + 3, 5].Value = users[i].Age;
                worksheet.Cells[i + 3, 6].Value = string.Join(", ", users[i].Hobbies);
            }

            worksheet.HeaderFooter.OddHeader.CenteredText = $"&24&U&\"Arial,Regular Bold\" {DateTime.UtcNow.ToShortDateString()}";
            worksheet.HeaderFooter.OddFooter.CenteredText = $"&8&\"Arial,Regular Bold\" Page {searchUserResponse.CurrentPage} of {searchUserResponse.TotalPages}";

            return new MemoryStream(excelPackage.GetAsByteArray());
        }
    }
}
