using System.Collections.Generic;
using PatientApp.Models;
using System;
using PatientApp.ViewModels;

namespace PatientApp.Services
{
    public interface INurseServices
    {
        List<Nurse> GetNurses();
        List<PatientAppUserVm> GetAssignedPatients(int id);
        Nurse CreateRegisteredNurse(RegisteredNurseVm registeredUser);
        Nurse GetNurse(int id);
        Nurse GetNurseKeyByLoginNurse(string appUserId);
    }
}