namespace PatientApp.Controllers {

    export class NurseCreateController {

        public registerUser;
        public validationErrors;
        private nurseKey: any;
        private ref: any;

        constructor(private nurseServices: PatientApp.Services.NurseServices,
            private $state: angular.ui.IStateService,
            private firebaseService: PatientApp.Services.FirebaseService
        ) {
            this.ref = new Firebase("https://impatient-3b3b4.firebaseio.com/");
        }

        //creates the nurse in SQL and then fire the method to create nurse in Firebase
        registerNurse() {
            this.nurseServices.registerNurse(this.registerUser).then((data) => {
                this.registerUser = data;
                this.$state.go("adminAllNursesActivities");
            }).catch((err) => {
                let validationErrors = [];
                for (let prop in err.data) {
                    let propErrors = err.data[prop];
                    validationErrors = validationErrors.concat(propErrors);
                }
                this.validationErrors = validationErrors;
            });
            this.newFirebaseNurse();
        }

        //adds a new nurse to the database (parent node that contains patients which contain messages)
        newFirebaseNurse() {
            //this.nurseKey = firebase.database().ref().child('nurses').push().key;
            let updates = {};
            updates["/nurses/" + this.registerUser.firstName + this.registerUser.lastName] = this.registerUser;
            return firebase.database().ref().update(updates);
            
        }
       

        cancel() {
            this.$state.go("adminAllNursesActivities");
        }
    }
}