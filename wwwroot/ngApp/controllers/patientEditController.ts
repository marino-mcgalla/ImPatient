namespace PatientApp.Controllers {

    export class PatientEditController {

        private patientId;
        public patient;  //patient to 
        public validationErrors;
        public nurses;
        public assignedNurseId;
        public assignedNurse;
        private ref: any;
        private firebaseNurseKey;
        private patientKey: any;

        constructor(private patientsServices: PatientApp.Services.PatientsServices,
            private $state: angular.ui.IStateService,
            private $stateParams: angular.ui.IStateParamsService,
            private firebaseService: PatientApp.Services.FirebaseService
        ) {
            this.ref = new Firebase("https://impatient-3b3b4.firebaseio.com/");
            this.patientId = $stateParams["id"];
            this.getNurses();
            this.getPatient();
        }

        //gets the patient info from database to populate the fields for edit
        getPatient() {
            this.patientsServices.getRegisteredPatient(this.patientId).then((data) => {
                this.patient = data.patient;
                this.assignedNurse = data.primaryNurseFullName;
                this.patient.checkInDate = new Date(this.patient.checkInDate);
                this.patient.checkOutDate = new Date(this.patient.checkOutDate);
                this.patient.dependency = data.patient.dependency;
                this.assignedNurseId = this.patient.nurseId;
            });
        }

        //saves patient info in the Patient table
        editPatient() {
            this.editFirebasePatient(); 
            this.clearPageMessages();
            this.patientsServices.savePatient(this.assignedNurseId, this.patient.applicationUser,
                this.patient).then((data) => {
                    this.patient = data;
                    this.$state.go("admitted");
                }).catch((results) => {
                    this.validationErrors = results;
                });
        }

        getNurseFirebaseKey() {
            for (let i = 0; i < this.nurses.length; i++) {
                if (this.nurses[i].id == this.patient.nurseId) {
                    return this.nurses[i].firebaseNurseKey;
                }
            }
        }

        editFirebasePatient() {
            let updates = {};
            updates["/nurses/" + this.firebaseNurseKey + "/patients/" + this.patientKey] = this.patient, this.assignedNurse;
            return firebase.database().ref().update(updates);
        }

        getNurses() {
            this.nurses = this.patientsServices.getNurses();
        }

        cancel() {
            this.$state.go("admitted");
        }

        clearPageMessages() {
            this.validationErrors = null;
        }
    }
}