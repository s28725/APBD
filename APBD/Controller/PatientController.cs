using APBD.Dto;
using APBD.Repository;
using Microsoft.AspNetCore.Mvc;

namespace APBD.Controller;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly IPatientRepository _patientRepository;

    public PatientController(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatient(int id)
    {
        var patient = await _patientRepository.GetPatientByIdAsync(id);
        if (patient == null)
        {
            return NotFound();
        }

        var response = new Dtos.PatientResponseDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate)
                .Select(p => new Dtos.PrescriptionResponseDto
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new Dtos.DoctorResponseDto
                    {
                        IdDoctor = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName
                    },
                    Medicaments = p.PrescriptionMedicaments.Select(pm => new Dtos.MedicamentResponseDto
                    {
                        IdMedicament = pm.Medicament.IdMedicament,
                        Name = pm.Medicament.Name,
                        Dose = pm.Dose,
                        Description = pm.Description
                    }).ToList()
                }).ToList()
        };

        return Ok(response);
    }
}