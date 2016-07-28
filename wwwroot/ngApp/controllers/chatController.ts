namespace PatientApp.Controllers {


    export class ChatController {
        public value;
        public textInput;
        public msgSender;
        public msgReciever;
        public msgText;
        public usernameInput;
        public postButton;
        public notification;
        public ref: any;
        public nurseUser;
        public nurseName;
        public nurses;
        public selectedNurse;
        public username;
        public myFirebase = new Firebase('https://impatient-f2cf2.firebaseio.com/');

        constructor(private patientsServices: PatientApp.Services.PatientsServices, private firebaseService: PatientApp.Services.FirebaseService, private $scope, private $firebaseObject, private $location: ng.ILocationService) {
            this.startListening();
            this.getCurrentNurse();
            this.getAllNurses();

                        }
        //sends message to reciepnt
        sendMessage(value) {

            this.usernameInput = document.querySelector('#username');
            this.textInput = document.querySelector('#text');
            this.postButton = document.querySelector('#post');

            this.username = this.nurseUser.applicationUser.firstName;
            var loginButton = document.querySelector('#login');
            
            


            this.postButton.addEventListener;
            this.msgSender = this.username;
            this.msgSender = this.usernameInput.value;
            this.msgReciever = this.selectedNurse.applicationUser.firstName;
            this.msgText = this.textInput.value;
            this.myFirebase.push({ username: this.msgSender, text: this.msgText, messageTo: this.msgReciever });
            this.textInput = "";
            
            location.reload();
            
        }
        // listens for any changes in the database and post them
        startListening() {
           
            this.myFirebase.on('child_added', (snapshot) => {
                    var msg = snapshot.val();
                    if (this.username == this.msgReciever) {
                       
                        var msgUsernameElement = document.createElement("b");
                        msgUsernameElement.textContent = msg.username;

                        var msgTextElement = document.createElement("p");
                        msgTextElement.textContent = msg.text;

                        var msgElement = document.createElement("div");
                        msgElement.appendChild(msgUsernameElement);
                        msgElement.appendChild(msgTextElement);
                      
                        msgElement.className = "msg";
                        document.getElementById("results").appendChild(msgElement);

                    }

                });
            
            
            
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
        getAllNurses() {
           
            this.nurses = this.patientsServices.getNurses();

       }
        newFirebaseNurse() {
            //this.nurseKey = firebase.database().ref().child('nurses').push().key;
            let updates = {};
            updates["/nurses/" + this.nurses.firstName + this.nurses.lastName];
            return this.myFirebase.database().ref().update(updates);
        }
    }


}


