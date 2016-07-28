namespace PatientApp.Controllers {

    export class AdminAllNursesActivitiesController {

        public nurses;

        constructor(private patientsServices: PatientApp.Services.PatientsServices,
            private $uibModal: ng.ui.bootstrap.IModalService){
            this.getAllNurses();
        }

        //gets list of all nurses for admin
        getAllNurses() {
            this.nurses = this.patientsServices.getNurses();
        }

        //opens the modal and display the selected nurse patients' activities 
        showDetailsDialog(nurseAppUserId, nurseId) {
            this.$uibModal.open({
                templateUrl: '/ngApp/views/nursePatientsDetailsDialog.html',
                controller: PatientApp.Controllers.AdminNurseDashboardDetailsDialogController,
                controllerAs: 'controller',
                resolve: {
                    nurseAppUserIdFrom: () => nurseAppUserId,  //this eventId is passed from the form
                    nurseIdFrom: () =>  nurseId
                },
                size: 'lg'
            });
        }
    }
}