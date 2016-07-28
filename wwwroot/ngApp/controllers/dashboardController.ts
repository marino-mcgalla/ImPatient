namespace PatientApp.Controllers {

    export class DashboardController{

        public myMessages;
        public nurse;
        public patientMessages = [];
        public validationErrors;
        public nurseNames = [];
    

        constructor(private patientsServices: PatientApp.Services.PatientsServices,
            private moment) {
            this.countMessages();
        }

        //gets the login nurse's patients messages activities
        countMessages() {
            this.patientsServices.getPatientMessages().then((data) => {
                this.patientMessages = data;
                for (let i = 0; i < this.patientMessages.length; i++) {
                    this.nurseNames.push(this.patientMessages[i].nurse.applicationUser.firstName + " " + this.patientMessages[i].nurse.applicationUser.lastName);
                    for (let message of this.patientMessages[i].messages) {
                        //used moment to get the relative time from now
                        message.timeRequested = this.moment(message.timeRequested).fromNow();
                    }
                }
            }).catch((err) => {
                let validationErrors = [];
                for (let prop in err.data) {
                    let propErrors = err.data[prop];
                    validationErrors = validationErrors.concat(propErrors);
                }
                this.validationErrors = validationErrors;
            });
        }

        //dismiss the message by updating the message table time responded field with the current time
        dismissIncident(messageId) {
            this.myMessages = this.patientsServices.dismissIncident(messageId).then((data) => {
                this.myMessages = data.messages;
                this.nurse = data;
                this.countMessages();
            });
        }
    }

    //custom angular filters
    function customUnique() {
        return function (input: string[]) {
            return input.filter((nurseNames: string, index) => input.indexOf(nurseNames) === index);
        };
    }
    angular.module("PatientApp").filter("customUnique", customUnique);

}