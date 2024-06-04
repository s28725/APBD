using APBD.Models;

namespace APBD.Repository;

public class PatientRepository : IPatientRepository
{
    private readonly DbContext.DbContext _context;

    public PatientRepository(DbContext.DbContext context)
    {
        _context = context;
    }
    
    public async Task<Patient> GetPatientByIdAsync(int id)
    {
        return await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .FirstOrDefaultAsync(p => p.IdPatient == id);
    }
}

public interface IPatientRepository
{
    Task<Patient> GetPatientByIdAsync(int id);
}