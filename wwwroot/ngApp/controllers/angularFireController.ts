namespace PatientApp.Controllers {

    export class AngularFireController {

        constructor(private $scope, private $firebaseObject) {

            var ref = new Firebase("https://impatient-3b3b4.firebaseio.com/nurses");
            var syncObject = $firebaseObject(ref);
            syncObject.$bindTo($scope, "nurses");

        }


    }

}