using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Streaming.Producer.API.Models;
using Streaming.Producer.Application;

[ApiController]
[Route("api/stream")]
public class StreamController : ControllerBase
{

    private readonly IStreamProcessorService _service;

    private readonly ILogger<StreamController> _logger;
    public StreamController(IStreamProcessorService streamProcessorService, ILogger<StreamController> logger)
    {
        _service = streamProcessorService;
        _logger = logger;
    }

    [HttpPost("process")]
    public async Task<IActionResult> Process([FromBody] StreamProcessRequest request)
    {
        _logger.LogInformation("Start Executing Process Method of StreamController");
        var res= await _service.CreateProcess(new StreamProcessDto(request.Data));
        return Accepted(new StreamProcessStatusResponse(ProcessId:res.ProcessId, ProcessStatus:res.ProcessStatus));
    }
    [HttpGet("process/{id}/status")]
    public async Task<IActionResult> ProcessStatus(Guid id)
    {
        var res = await _service.GetProcessData(id);

        if (res == null)
            return NotFound();

        return Ok(new StreamProcessStatusResponse(ProcessId: res.ProcessId, ProcessStatus: res.ProcessStatus));
    }
    [HttpGet("process/{id}")]
    public async Task<IActionResult> ProcessData(Guid id)
    {

        var res = await _service.GetProcessData(id);

        if (res == null)
            return NotFound();

        return Ok(new StreamProcessResponse(ProcessId: res.ProcessId,Data:res.Data,OutputData:res.OutputData, ProcessStatus: res.ProcessStatus));
    }
}
