using AppointmentScheduling.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling.Services
{
    public interface IAppointmentService
    {
        public List<DoctorViewModel> GetDoctorList();

        public List<PatientViewModel> GetPatientList();

        public Task<int> AddUpdate(AppointmentViewMode model);

        public List<AppointmentViewMode> DoctorsEventsById(string doctorId);
        public List<AppointmentViewMode> PatientEventsById(string patientId);

        public AppointmentViewMode GetById(int id);

        public Task<int> Delete(int id);

        public Task<int> Confirm(int id);
    }
}