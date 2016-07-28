namespace PatientApp.Controllers {

    export class PatientController {
        public patientUser;
        public patientId;
        public nurseId;
        public message;
        public patient;
        public patient1;
        public assignedNurse;
        public messageKey;
        public messageFromView;
        //public ref;
        public messageText;
        public notificationKey: any;
        public messageVM: any;

        constructor(
            private patientsServices: PatientApp.Services.PatientsServices,
            private $stateParams: angular.ui.IStateParamsService,
            private $state: angular.ui.IStateService,
            private firebaseService: PatientApp.Services.FirebaseService) {

            this.patient = {};
            this.patientsServices.getCurrentPatient().then((data) => {
                this.patientUser = data;
                console.log(this.patientUser);
            });

            this.patientsServices.getCurrentPatient().then((data2) => {
                let self = this;
                self.patient = data2;
                this.setPatientView();
                this.patientId = this.patient.id;
                this.getAssignedNurse();
            });

        }

        //Redirects patient to the proper patient view based on patient dependency
        setPatientView() {
            if (this.patient.dependency == 1) {
                this.$state.go("patient1");
            }
            else if (this.patient.dependency == 2) {
                this.$state.go("patient2");
            }
            else if (this.patient.dependency == 3) {
                this.$state.go("patient3");
            }
        }

        getAssignedNurse() {
            this.patientsServices.getAssignedNurse(this.patientId).then((data) => {
                this.assignedNurse = data;
            });
        }

        //adds message to the SQL database
        sendMessage(messageText) {
            this.messageFromView = messageText;
            this.patientsServices.sendMessage(this.patient.applicationUserId, messageText);
            //calls firebase message send
            this.newNotification();
        }

        //adds new message to the firebase db
        newNotification() {
            this.messageKey = firebase.database().ref().child('notifications').push().key;
            this.messageVM = {};
            let updates = {};

            //binds message from view to a view model to send to firebase
            this.messageVM.message = this.messageFromView;

            //calls a connection to the nurse ref in firebase database
            var patientRef = new Firebase("https://impatient-3b3b4.firebaseio.com/" + "/nurses/" + this.assignedNurse.applicationUser.firstName + this.assignedNurse.applicationUser.lastName + "/patients/" + this.patient.applicationUser.firstName + this.patient.applicationUser.lastName + "/icon");

            //updates status icon based on type of message being sent
            if (this.messageFromView == "emergency") {
                updates["/nurses/" + this.assignedNurse.applicationUser.firstName + this.assignedNurse.applicationUser.lastName + "/patients/" + this.patient.applicationUser.firstName + this.patient.applicationUser.lastName + "/icon"] = "../../images/redBedIcon.png";
                firebase.database().ref().update(updates);
            }
            else {
                updates["/nurses/" + this.assignedNurse.applicationUser.firstName + this.assignedNurse.applicationUser.lastName + "/patients/" + this.patient.applicationUser.firstName + this.patient.applicationUser.lastName + "/icon"] = "../../images/yellowBedIcon.png";
                firebase.database().ref().update(updates);
            }

            //view model to firebase
            this.messageVM.timeRequested = Date.now();
            this.messageVM.timeResponded = (new Date("01/01/0001")).toString();
            this.messageVM.id = this.messageKey;

            //calls a connection to the specific patient ref in firebase database
            var messageListRef = new Firebase("https://impatient-3b3b4.firebaseio.com/" + "/nurses/" + this.assignedNurse.applicationUser.firstName + this.assignedNurse.applicationUser.lastName + "/patients/" + this.patient.applicationUser.firstName + this.patient.applicationUser.lastName + "/messages/" + this.messageKey);
            //updates firebase database with new message
            messageListRef = messageListRef.update(this.messageVM);
            patientRef = patientRef.update(patientRef);
        }

    }

}