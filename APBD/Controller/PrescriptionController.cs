using APBD.Dto;
using APBD.Service;
using Microsoft.AspNetCore.Mvc;

namespace APBD.Controller;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;

    public PrescriptionController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescription(Dtos.PrescriptionDto prescriptionDto)
    {
        try
        {
            await _prescriptionService.AddPrescriptionAsync(prescriptionDto);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
