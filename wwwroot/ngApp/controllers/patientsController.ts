namespace PatientApp.Controllers {

    export class PatientsController {
        public patients;
        public patientId;
        public patient;

        constructor(private patientsServices: PatientApp.Services.PatientsServices,
            private $state: angular.ui.IStateService,
            $stateParams: angular.ui.IStateParamsService,
            private $uibModal: angular.ui.bootstrap.IModalService
        ) {
            this.patientId = $stateParams["id"];
            this.getPatients();
        }

        getPatient() {
            this.patientsServices.getPatient(this.patientId).then((data) => {
                this.patient = data;
            });
        }

        getPatients() {
            this.patientsServices.getPatients().then((data) => {
                this.patients = data;
                });
        }

        cancel() {
            this.$state.go("admitted");
        }

        //opens the patient details in a modal
        patientDetails(patientId) {
            this.$uibModal.open({
                templateUrl: '/ngApp/views/patientDetailsDialog.html',
                controller: PatientApp.Controllers.ModalController,
                controllerAs: 'controller',
                resolve: {
                    patientIdFrom: () => patientId
                },
                size: 'md'
            });
        }

        //opens the delete function page in a modal
        deletePatient(patientId) {
            this.$uibModal.open({
                templateUrl: '/ngApp/views/deletePatientDialog.html',
                controller: PatientApp.Controllers.ModalController,
                controllerAs: 'controller',
                resolve: {
                    patientIdFrom: () => patientId
                },
                size: 'md'
            }).result.then(() => {
                this.getPatients();
                });
        }
    }


}