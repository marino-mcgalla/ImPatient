namespace PatientApp.Controllers {

    export class MessageCountController{  
        public nurseUser;
        public patients;

        constructor(private patientsServices: PatientApp.Services.PatientsServices,
            private $window) {

            this.patientsServices.getCurrentUser().then((data) => {
                this.nurseUser = data;
                this.getAssignedPatients();
            });
        }

        returnPatients() {
            return this.patients;
        }

        getAssignedPatients() {
            this.patientsServices.getAssignedPatients(this.nurseUser.id).then((data) => {
                this.patients = data;
            });
        }
    }

    angular.module('PatientApp').controller('MessageCountController', MessageCountController);
}