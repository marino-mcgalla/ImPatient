namespace PatientApp.Controllers {

    export class AdminNurseDashboardDetailsDialogController {

        public patientMessages = [];
        public validationErrors;
        public nurseName;

        constructor(private nurseIdFrom, private nurseAppUserIdFrom, private patientsServices: PatientApp.Services.PatientsServices,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance) {
            this.countMessages();
        }

        //retrieve the nurse patients' messages activities
        countMessages() {
            this.patientsServices.getNursePatientsMessages(this.nurseAppUserIdFrom).then((data) => {
                debugger;
                if (data.length == 0) {
                    data = {};
                }
                this.patientMessages = data;
                this.nurseName = data[0].nurse.applicationUser.firstName + " " + data[0].nurse.applicationUser.lastName;
            }).catch((err) => {
                let validationErrors = [];
                for (let prop in err.data) {
                    let propErrors = err.data[prop];
                    validationErrors = validationErrors.concat(propErrors);
                }
                this.validationErrors = validationErrors;
            });
        }

        close() {
            this.$uibModalInstance.close();
        }
    }
}