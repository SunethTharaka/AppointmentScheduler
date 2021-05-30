﻿using AppointmentScheduling.Models;
using AppointmentScheduling.Models.ViewModels;
using AppointmentScheduling.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _db;

        public AppointmentService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<int> AddUpdate(AppointmentViewMode model)
        {

            var startDate = DateTime.Parse(model.StartDate);
            var endDate = DateTime.Parse(model.StartDate).AddMinutes(Convert.ToDouble(model.Duration));

            if (model != null && model.Id > 0)
            {
                var apporintment = _db.Appointments.FirstOrDefault(x => x.Id.Equals(model.Id));
                apporintment.Title = model.Title;
                apporintment.Description = model.Description;
                apporintment.StartDate = startDate;
                apporintment.EndDate = endDate;
                apporintment.Duration = model.Duration;
                apporintment.DoctorId = model.DoctorId;
                apporintment.PatientId = model.PatientId;
                apporintment.IsDoctorApproved = model.IsDoctorApproved;
                apporintment.AdminId = model.AdminId;
                await _db.SaveChangesAsync();
                return 1;
            }
            else
            {
                Appointment appointment = new Appointment()
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartDate = startDate,
                    EndDate = endDate,
                    Duration = model.Duration,
                    DoctorId = model.DoctorId,
                    PatientId = model.PatientId,
                    IsDoctorApproved = model.IsDoctorApproved,
                    AdminId = model.AdminId
                };

                _db.Appointments.Add(appointment);
                await _db.SaveChangesAsync();
                return 2;
            }
        }

        public async Task<int> Confirm(int id)
        {
            var apporintment = _db.Appointments.FirstOrDefault(x => x.Id.Equals(id));
            if (apporintment != null)
            {
                apporintment.IsDoctorApproved = true;
                return await _db.SaveChangesAsync();

            }
            else
                return 0;
        }

        public async Task<int> Delete(int id)
        {
            var apporintment = _db.Appointments.FirstOrDefault(x => x.Id.Equals(id));
            if (apporintment != null)
            {
                _db.Appointments.Remove(apporintment);
                return await _db.SaveChangesAsync();

            }
            else
                return 0;
        }

        public List<AppointmentViewMode> DoctorsEventsById(string doctorId)
        {
            return _db.Appointments.Where(x => x.DoctorId.Equals(doctorId)).ToList().Select(c => new AppointmentViewMode()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Title = c.Title,
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved
            }).ToList();
        }

        public AppointmentViewMode GetById(int id)
        {
            return _db.Appointments.Where(x => x.Id.Equals(id)).ToList().Select(c => new AppointmentViewMode()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Title = c.Title,
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved,
                PatientId = c.PatientId,
                DoctorId = c.DoctorId,
                PatientName = _db.Users.Where(x => x.Id.Equals(c.PatientId)).Select(x => x.Name).FirstOrDefault(),
                DoctorName = _db.Users.Where(x => x.Id.Equals(c.DoctorId)).Select(x => x.Name).FirstOrDefault()
            }).SingleOrDefault();
        }

        public List<DoctorViewModel> GetDoctorList()
        {
            var doctors = (from user in _db.Users
                           join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                           join roles in _db.Roles.Where(x => x.Name.Equals(Helper.Doctor)) on userRoles.RoleId equals roles.Id
                           select new DoctorViewModel
                           {
                               Id = user.Id,
                               Name = user.Name
                           }).ToList();
            return doctors;
        }

        public List<PatientViewModel> GetPatientList()
        {
            var patients = (from user in _db.Users
                            join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                            join roles in _db.Roles.Where(x => x.Name.Equals(Helper.Patient)) on userRoles.RoleId equals roles.Id
                            select new PatientViewModel
                            {
                                Id = user.Id,
                                Name = user.Name
                            }).ToList();
            return patients;
        }

        public List<AppointmentViewMode> PatientEventsById(string patientId)
        {
            return _db.Appointments.Where(x => x.PatientId.Equals(patientId)).ToList().Select(c => new AppointmentViewMode()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Title = c.Title,
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved
            }).ToList();
        }
    }
}