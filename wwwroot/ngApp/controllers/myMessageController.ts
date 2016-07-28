namespace PatientApp.Controllers {

    export class MyMessageController {

        public myMessages;
        public nurse;
        public validationErrors;
        public ref: any;
        public nurseUser;
        public nurseName;
        public test;

        constructor(private patientsServices: PatientApp.Services.PatientsServices,
            private firebaseService: PatientApp.Services.FirebaseService,
            private $scope, private $firebaseObject, private moment) {

            this.getCurrentNurse();
            this.getMyMessages();
        }

        getCurrentNurse() {
            let self = this;
            this.patientsServices.getCurrentUser().then((data) => {
                self.nurseUser = data;
                this.nurseName = this.nurseUser.applicationUser.firstName + this.nurseUser.applicationUser.lastName; //used to access nurse node
                this.ref = new Firebase("https://impatient-3b3b4.firebaseio.com/nurses/" + this.nurseName);
                var syncObject = this.$firebaseObject(this.ref);
                syncObject.$bindTo(this.$scope, "nurse");

            });
        }

        getMyMessages() {
            this.myMessages = this.patientsServices.getMyMessages()
                .then((data) => {
                    this.myMessages = data.messages;
                    this.nurse = data;
                }).catch((err) => {
                    let validationErrors = [];
                    for (let prop in err.data) {
                        let propErrors = err.data[prop];
                        validationErrors = validationErrors.concat(propErrors);
                    }
                    this.validationErrors = validationErrors;
                });
        }

        dismissFirebase(messageKey, nameKey) {
            var ref = new Firebase("https://impatient-3b3b4.firebaseio.com/nurses/" + this.nurseName + "/patients/" + nameKey + "/messages/" + messageKey);

            ref.orderByChild("message").on("value", function (snapshot) {
                var snapshot1 = snapshot.val();
                console.log(snapshot.val());

                for (var k in snapshot1) {
                    if (!snapshot1.hasOwnProperty(k)) continue;
                    if (snapshot1[k].message == "emergency") {
                        console.log("there is an emergency!");
                    }
                    else {
                        let updates = {};
                        var iconRef = new Firebase("https://impatient-3b3b4.firebaseio.com/nurses/" + this.nurseName + "/patients/" + nameKey + "/icon");
                        updates["/nurses/" + this.nurseName + "/patients/" + nameKey + "/icon"] = "../../images/yellowBedIcon.png";
                        console.log("everything is fine");
                    }
                }
            });
            ref.remove();
        }

        dismissAll(nameKey) {
            let updates = {};
            var messageRef = new Firebase("https://impatient-3b3b4.firebaseio.com/nurses/" + this.nurseName + "/patients/" + nameKey + "/messages/");
            var iconRef = new Firebase("https://impatient-3b3b4.firebaseio.com/nurses/" + this.nurseName + "/patients/" + nameKey + "/icon");
            updates["/nurses/" + this.nurseName + "/patients/" + nameKey + "/icon"] = "../../images/greenBedIcon.png";
            firebase.database().ref().update(updates);

            messageRef.remove();
        }

    }
}