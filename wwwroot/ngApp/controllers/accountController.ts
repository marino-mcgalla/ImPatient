namespace PatientApp.Controllers {

    export class AccountController {
        public externalLogins;
        public nurse;

        public getUserName() {
            return this.accountService.getUserName();
        }

        public getClaim(type) {
            return this.accountService.getClaim(type);
        }

        public isLoggedIn() {
            return this.accountService.isLoggedIn();
        }

        public logout() {
            this.accountService.logout();
            this.$location.path('/');
        }

        public getExternalLogins() {
            return this.accountService.getExternalLogins();
        }

        constructor(private accountService: PatientApp.Services.AccountService, private $location: ng.ILocationService,
            private nurseServices: PatientApp.Services.NurseServices,
            private patientsServices: PatientApp.Services.PatientsServices) {
            this.getExternalLogins().then((results) => {
                this.externalLogins = results;
            });
        }

    }

    angular.module('PatientApp').controller('AccountController', AccountController);

    export class LoginController {
        public loginUser;
        public validationMessages;

        //logs a user into the application
        public login() {
            this.accountService.login(this.loginUser).then(() => {

                //if no user is logged in, directs to the homepage
                this.$location.path('/');
                location.reload();

                //reroutes the user to specific page based on their role
                if (this.accountService.getClaim('isNurse')) {
                    this.$location.path('/myMessages');
                }
                if (this.accountService.getClaim('isAdmin')) {
                    this.$location.path('/adminAllNursesActivities');
                }
                if (this.accountService.getClaim('isPatient')) {
                    this.$location.path('/setupView');
                }
            }).catch((results) => {
                this.validationMessages = results;
            });
        }

        constructor(private accountService: PatientApp.Services.AccountService, private $location: ng.ILocationService) {
            var emailTextBox = <HTMLInputElement>document.getElementById("emailLogin");
            emailTextBox.focus();
        }

    }

    export class PatientLoginController {
        public loginUser;
        public validationMessages;
        public patients;
        public selectedPatient;
        public notification: any;
        public notifications;

        public patientLogin() {
            this.loginUser.email = this.selectedPatient.applicationUser.email; //gets patient info from angular
            this.loginUser.password = "Secret123!";
            this.accountService.login(this.loginUser).then(() => {
                this.$location.path('/setupView');
            }).catch((results) => {
                this.validationMessages = results;
            });
        }

        constructor(private accountService: PatientApp.Services.AccountService, private $location: ng.ILocationService, private $http: ng.IHttpService, private firebaseService: PatientApp.Services.FirebaseService) {
            //gets a list of patients for login
            this.$http.get('/api/patient').success((results) => {
                this.patients = results;
                this.loginUser = {};
            });

        }
        firebaseLogin() {
            let self = this;
            firebase.auth().signInWithEmailAndPassword(this.loginUser.email, this.loginUser.password).then(() => {
                console.log("login succeeded");
                self.updateNotifications();
            }).catch((error) => {
                console.log(error.code + " " + error.message);
            });
        }

        //adds new notification to the firebase db
        newNotification() {
            let notificationKey = firebase.database().ref().child('notifications').push().key;
            let updates = {};
            updates["/notifications/" + notificationKey] = this.notification;
            return firebase.database().ref().update(updates);
        }

        //refreshes the db when there is a change
        updateNotifications() {
            firebase.database().ref("notifications").on("value", (snapshot) => {
                this.notifications = snapshot.val();
            })
        }
    }



    export class RegisterController {
        public registerUser;
        public validationMessages;

        public register() {
            this.accountService.register(this.registerUser).then(() => {
                this.$location.path('/');
            }).catch((results) => {
                this.validationMessages = results;
            });
        }

        constructor(private accountService: PatientApp.Services.AccountService, private $location: ng.ILocationService) { }
    }

    export class ExternalRegisterController {
        public registerUser;
        public validationMessages;

        public register() {
            this.accountService.registerExternal(this.registerUser.email)
                .then((result) => {
                    this.$location.path('/');
                }).catch((result) => {
                    this.validationMessages = result;
                });
        }

        constructor(private accountService: PatientApp.Services.AccountService, private $location: ng.ILocationService) { }

    }

    export class ConfirmEmailController {
        public validationMessages;

        constructor(
            private accountService: PatientApp.Services.AccountService,
            private $http: ng.IHttpService,
            private $stateParams: ng.ui.IStateParamsService,
            private $location: ng.ILocationService
        ) {
            let userId = $stateParams['userId'];
            let code = $stateParams['code'];
            accountService.confirmEmail(userId, code)
                .then((result) => {
                    this.$location.path('/');
                }).catch((result) => {
                    this.validationMessages = result;
                });
        }
    }

}
