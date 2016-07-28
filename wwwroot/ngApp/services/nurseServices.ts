namespace PatientApp.Services {

    export class NurseServices {
        public nurseResources;

        constructor($resource: angular.resource.IResourceService) {
            this.nurseResources = $resource("/api/nurse", null, {
                GetAssignedPatients: {
                    method: 'GET',
                    url: '/api/nurse/getassignedpatients',
                    isArray: true
                },
                registerNurse: {
                    method: 'POST',
                    url: '/api/nurse/registernurse'
                },
                getNurseKeyByLoginNurse: {
                    method: 'GET',
                    url: '/api/nurse/getnurseKeybyloginnurse'
                }
            })
        }

        getAssignedPatients(nurseId) {
            return this.nurseResources.GetAssignedPatients(nurseId);
        }

        getNurseKeyByLoginNurse() {
            return this.nurseResources.getNurseKeyByLoginNurse().$promise;
        }

        registerNurse(nurse) {
            debugger;
            let dataVm: any = {};
            dataVm.email = nurse.email;
            dataVm.password = nurse.password;
            dataVm.confirmPassword = nurse.confirmPassword;
            dataVm.firstName = nurse.firstName;
            dataVm.lastName = nurse.lastName;
            return this.nurseResources.registerNurse(dataVm).$promise;
        }
    }

    angular.module('PatientApp').service('nurseServices', NurseServices)

}