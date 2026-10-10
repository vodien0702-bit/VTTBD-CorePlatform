using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VTTBD_CorePlatform.Application.Features.WebApp.V1.StudentManagement.StudentSchedule.Queries.GetScheduleBySemester;

namespace VTTBD_CorePlatform.API.Controllers;

[ApiController]
[Route("api/v1/student-schedules")]
public class StudentScheduleController : ControllerBase
{
    private readonly IMediator _mediator;

    public StudentScheduleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Chức năng 8.1: Lọc Thời khóa biểu / Đăng ký học phần theo Học kỳ
    /// </summary>
    [HttpGet("filter-by-semester")]
    public async Task<IActionResult> GetBySemester([FromQuery] Guid semesterId, [FromQuery] Guid studentId)
    {
        var query = new GetScheduleBySemesterQuery(semesterId, studentId);
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}