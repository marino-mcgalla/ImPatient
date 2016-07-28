namespace PatientApp.Controllers {

    export class ModalController {
        public patient;
        public patients;
        public assignedNurse;

        constructor(private patientIdFrom, private $uibModalInstance: angular.ui.bootstrap.IModalServiceInstance,
            private patientsServices: PatientApp.Services.PatientsServices, private $state: angular.ui.IStateService) {
            this.getPatient();
            this.getAssignedNurse();
        }

        //used to see the patient's details in modal
        getPatient() {
            this.patientsServices.getPatient(this.patientIdFrom).then((data) => {
                this.patient = data;
            });
        }

        //used to show the patient's assigned nurse first and last name in modal
        getAssignedNurse() {
            this.patientsServices.getAssignedNurse(this.patientIdFrom).then((data) => {
                this.assignedNurse = data;
            });
        }

        //use modal to soft delete the patients
        deletePatient() {
            this.patientsServices.deletePatient(this.patientIdFrom).then(() => {
                this.$uibModalInstance.close();
            });
        }

        close() {
            this.$uibModalInstance.close();
        }
    }
}