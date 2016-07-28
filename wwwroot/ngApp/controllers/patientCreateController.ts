namespace PatientApp.Controllers {

    export class PatientCreateController {

        public patient; //patient to save
        public registerUser; //application user save
        public validationErrors;    //patient table error
        public validationMessages;  //registration error

        public nurses;
        public assignedNurseId;
        private patientKey: any;
        public nurse;
        private firebaseNurseKey;
        private ref: any;
        public patientVM: any;

        constructor(private patientsServices: PatientApp.Services.PatientsServices,
            private $state: angular.ui.IStateService,
            private firebaseService: PatientApp.Services.FirebaseService) {
            this.ref = new Firebase("https://impatient-3b3b4.firebaseio.com/");
            this.clearPageMessages();
            this.getNurses();
            this.patient = {};
            //this.patient.checkInDate = null;
            this.patient.checkInDate = new Date(Date.now());
        }

        getNurses() {
            this.nurses = this.patientsServices.getNurses();
        }

        //saves patient info in the application user table and patient table in SQL
        //then calls the method that saves patient in Firebase database
        addRegisterUser() {
            this.clearPageMessages();
            this.firebaseNurseKey = this.getNurseFirebaseKey();
            this.newFirebasePatient();
            this.patientsServices.registerPatient(this.registerUser, this.patient, this.patientKey).then(() => {
                this.$state.go("admitted");
            }).catch((result) => {
                this.validationMessages = this.flattenValidation(result.data);
            });
        }

        getNurseFirebaseKey() {
            for (let i = 0; i < this.nurses.length; i++) {
                if (this.nurses[i].id == this.patient.nurseId) {
                    return this.nurses[i];
                }
            }
        }

        //adds a new patient to the database under the nurse key defined above (need to change it so you can add to the correct nurse)
        newFirebasePatient() {
            this.patientKey = firebase.database().ref().child('patients').push().key;
            this.patientVM = {};
            this.patientVM.icon = "../../images/greenBedIcon.png";
            this.patientVM.firstName = this.registerUser.firstName;
            this.patientVM.lastName = this.registerUser.lastName;
            this.patientVM.roomNumber = this.patient.roomNumber;
            this.patientVM.bedNumber = this.patient.bedNumber;
            this.patientVM.checkInDate = this.patient.checkInDate;
            this.patientVM.dependency = this.patient.dependency;
            this.patientVM.nurseId = this.patient.nurseId;
            this.patientVM.notes = this.patient.notes;
            //this.patientVM.messages = [];
            let updates = {};
            updates["/nurses/" + this.firebaseNurseKey.applicationUser.firstName + this.firebaseNurseKey.applicationUser.lastName + "/patients/" + this.registerUser.firstName + this.registerUser.lastName] = this.patientVM;
            return firebase.database().ref().update(updates);
        }

        cancel() {
            this.$state.go("admitted");
        }

        clearPageMessages() {
            this.validationErrors = null;
            this.validationMessages = null;
        }

        //method used to extract the modelState errors
        private flattenValidation(modelState) {
            let messages = [];
            for (let prop in modelState) {
                messages = messages.concat(modelState[prop]);
            }
            return messages;
        }
    }
}