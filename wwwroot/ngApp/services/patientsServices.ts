namespace PatientApp.Services {

    export class PatientsServices {

        public messageResources;
        public patientResources;
        public nurseResources;
        public userResources;


        constructor($resource: angular.resource.IResourceService,
            private $q: ng.IQService,
            private $http: ng.IHttpService) {
            this.messageResources = $resource("/api/myMessage", null, {
                SendMessage: {
                    method: 'POST',
                    url: '/api/myMessage/sendmessage/:id/:message',
                    isArray: false
                },
                dismissIncident: {
                    method: 'POST',
                    url: "/api/myMessage/dismissincident"
                },
                getPatientMessages: {
                    method: 'GET',
                    url: '/api/myMessage/getpatientmessages',
                    isArray: true
                },
                getNursePatientsMessages: {
                    method: 'GET',
                    url: '/api/myMessage/getNursePatientsMessages/:nurseAppUserId',
                    isArray: true
                },
                getMyMessages: {
                    method: 'GET',
                    url: '/api/myMessage/getmymessages'
                }
            });
            this.nurseResources = $resource("/api/nurse/:id", null, {
                GetAssignedPatients: {
                    method: 'GET',
                    url: '/api/nurse/getassignedpatients',
                    isArray: true
                }
            });
            this.patientResources = $resource("/api/patient/:id", null, {
                GetAssignedNurse: {
                    method: 'GET',
                    url: '/api/patient/getassignednurse'
                }, 
                getRegisteredPatient: {
                    method: 'GET',
                    url: '/api/patient/getregisteredpatient/:id'
                },
                registerPatient: {
                    method: 'POST',
                    url: '/api/patient/registerpatient'
                }
            });
            this.userResources = $resource("/api/user/:id", null, {
                GetCurrentUser: {
                    method: 'GET',
                    url: '/api/user/getcurrentuser'
                },
                GetCurrentPatient: {
                    method: 'GET',
                    url: '/api/user/getcurrentpatient'
                }
            });
        }

        //this is used to get the messages for the Nurse from the sQL side 
        //UI page has been migrated to Firebase
        getMyMessages() {
            return this.messageResources.getMyMessages().$promise;
        }

        getPatients() {
            return this.patientResources.query().$promise;
        }

        //used for patient edit
        getRegisteredPatient(patientId) {
            return this.patientResources.getRegisteredPatient({ id: patientId }).$promise;
        }

        getPatientMessages() {
            return this.messageResources.getPatientMessages().$promise;
        }

        //used to get the nurse patients messages
        getNursePatientsMessages(nurseAppUserId) {
            return this.messageResources.getNursePatientsMessages({ nurseAppUserId: nurseAppUserId }).$promise;
        }

        //returns a single patient 
        getPatient(patientId) {
            return this.patientResources.get({ id: patientId }).$promise;
        }

        getAssignedNurse(patientId) {
            return this.patientResources.GetAssignedNurse({ id: patientId }).$promise;
        }

        getNurses() {
            return this.nurseResources.query();
        }

        getNurse(nurseId) {
            debugger;
            return this.nurseResources.get(nurseId).$promise;
        }

        getAssignedPatients(nurseId) {
            return this.nurseResources.GetAssignedPatients({ id: nurseId }).$promise;
        }

        //used to update the message table with time responded
        dismissIncident(messageId) {
            return this.messageResources.dismissIncident(messageId).$promise;
        }

        //use vm to separate saving patient appUser and patient table (one to one relationship)
        registerPatient(userLogin, patient, firebasePatientKey) {
            var dataVm: any = {};
            dataVm.email = userLogin.email;
            dataVm.password = userLogin.password;
            dataVm.confirmPassword = userLogin.confirmPassword;
            dataVm.firstName = userLogin.firstName;
            dataVm.lastName = userLogin.lastName;
            dataVm.roomNumber = patient.roomNumber;
            dataVm.bedNumber = patient.bedNumber;
            dataVm.checkInDate = patient.checkInDate;
            dataVm.notes = patient.notes;
            dataVm.dependency = patient.dependency;
            dataVm.nurseId = patient.nurseId;
            dataVm.firebasePatientKey = firebasePatientKey;
            return this.patientResources.registerPatient(dataVm).$promise;
        }

        //edit patient info in both appUser and patient tables
        savePatient(assignedNurseId, registerUser, patient) {
            var dataVm: any = {};
            dataVm.applicationUser = registerUser;
            dataVm.patient = patient;
            dataVm.assignedNurseId = assignedNurseId;
            dataVm.firebasePatientKey = patient.firebasePatientKey;
            return this.patientResources.save(dataVm).$promise;
        }


        private flattenValidation(modelState) {
            let messages = [];
            for (let prop in modelState) {
                messages = messages.concat(modelState[prop]);
            }
            return messages;
        }

        deletePatient(patientId) {
            return this.patientResources.remove({ id: patientId }).$promise;
        }

        getCurrentUser() {
            return this.userResources.GetCurrentUser().$promise;
        }

        getCurrentPatient()
        {
            return this.userResources.GetCurrentPatient().$promise;
        }

        sendMessage(patientId, message) {
            let VM: any = {};
            VM.userId = patientId;
            VM.messageString = message;
            return this.messageResources.SendMessage(VM).$promise;
        }

        countMessages() {
            return this.messageResources.get().$promise;
        }

    }
    angular.module("PatientApp").service("patientsServices", PatientsServices);
}