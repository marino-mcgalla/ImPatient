declare let firebase: any;
declare let Firebase: any;
namespace PatientApp.Services {

    export class FirebaseService {
        private database;

        constructor() {
            firebase.initializeApp(this.config);
            this.database = firebase.database();
        }

        private config = {
            apiKey: "AIzaSyAZmrEBQZxEQuyIGBb9o8OqhfnSLZRMDgY",
            authDomain: "impatient-3b3b4.firebaseapp.com",
            databaseURL: "https://impatient-3b3b4.firebaseio.com",
            storageBucket: "impatient-3b3b4.appspot.com"
        };
    }
    angular.module('PatientApp').service('firebaseService', FirebaseService);
}

