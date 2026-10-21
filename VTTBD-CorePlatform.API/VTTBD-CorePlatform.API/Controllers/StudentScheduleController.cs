using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VTTBD_CorePlatform.Application.Features.WebApp.V1.StudentManagement.StudentSchedule.Queries.GetAllCourses;
using VTTBD_CorePlatform.Application.Features.WebApp.V1.StudentManagement.StudentSchedule.Queries.GetScheduleBySemester;
using VTTBD_CorePlatform.Application.Features.WebApp.V1.StudentManagement.StudentSchedule.Queries.GetSemesters;
using VTTBD_CorePlatform.Application.Features.WebApp.V1.StudentManagement.StudentSchedule.Queries.GetStudents;

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
    /// 1. Lấy danh sách tất cả các Học kỳ
    /// </summary>
    [HttpGet("semesters")]
    public async Task<IActionResult> GetSemesters()
    {
        var query = new GetSemestersQuery();
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// 2. Lấy danh sách tất cả các Sinh viên
    /// </summary>
    [HttpGet("students")]
    public async Task<IActionResult> GetStudents()
    {
        var query = new GetStudentsQuery();
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// 3. Lấy danh sách tất cả các Môn học (RegisteredCourses) trong hệ thống
    /// </summary>
    [HttpGet("courses")]
    public async Task<IActionResult> GetAllCourses()
    {
        var query = new GetAllCoursesQuery();
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// 4. Lọc Thời khóa biểu / Đăng ký môn học theo Sinh viên và Học kỳ
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