namespace PatientApp.Controllers {

    export class PatientDetailsController {
        public patientId;
        public patient;
        public assignedNurse;

        constructor(private patientsServices: PatientApp.Services.PatientsServices,
            private $state: angular.ui.IStateService,
            $stateParams: angular.ui.IStateParamsService
           
        ) {
            this.patientId = $stateParams["id"];
            this.getPatient();
        }

        getPatient() {
            this.patientsServices.getPatient(this.patientId).then((data) => {
                this.patient = data;
            });
            this.getAssignedNurse();
        }

        getAssignedNurse() {
            this.patientsServices.getAssignedNurse(this.patientId).then((data) => {
                this.assignedNurse = data;
            });
        }
    }
}