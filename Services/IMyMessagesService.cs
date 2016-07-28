using PatientApp.Models;
using PatientApp.ViewModels;
using System.Collections.Generic;

namespace PatientApp.Services
{
    public interface IMyMessagesService
    {
        //NursePatientsMessagesVm GetMyMessages(string appUserId);
        Nurse GetMyMessages(string appUserId);
        Nurse DismissIncident(string appUserId, int messageId);
        void SendMessage(string appUserId, string message);
        List<PatientMessagesVm> GetPatientsMessages(string appUserId);
    }
}