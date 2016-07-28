namespace PatientApp {

    angular.module('PatientApp', ['ui.router', 'ngResource', 'ui.bootstrap', 'angular.filter', 'firebase', 'angularMoment', 'yaru22.angular-timeago']).config((
        $stateProvider: ng.ui.IStateProvider,
        $urlRouterProvider: ng.ui.IUrlRouterProvider,
        $locationProvider: ng.ILocationProvider
    ) => {
        // Define routes
        $stateProvider
            .state('home', {
                url: '/',
                templateUrl: '/ngApp/views/home.html',
                controller: PatientApp.Controllers.HomeController,
                controllerAs: 'controller'
            })
            .state('adminAllNursesActivities', {
                url: '/adminAllNursesActivities',
                templateUrl: '/ngApp/views/adminAllNursesActivities.html',
                controller: PatientApp.Controllers.AdminAllNursesActivitiesController,
                controllerAs: 'controller'
            })
            .state('secret', {
                url: '/secret',
                templateUrl: '/ngApp/views/secret.html',
                controller: PatientApp.Controllers.SecretController,
                controllerAs: 'controller'
            })
            .state('patientLogin', {
                url: '/patientLogin',
                templateUrl: '/ngApp/views/patientLogin.html',
                controller: PatientApp.Controllers.PatientLoginController,
                controllerAs: 'controller'
            })
            .state('register', {
                url: '/register',
                templateUrl: '/ngApp/views/register.html',
                controller: PatientApp.Controllers.RegisterController,
                controllerAs: 'controller'
            })
            .state('patient1', {
                url: '/patient1',
                templateUrl: '/ngApp/views/patient1.html',
                controller: PatientApp.Controllers.PatientController,
                controllerAs: 'controller'
            })
            .state('patient2', {
                url: '/patient2',
                templateUrl: '/ngApp/views/patient2.html',
                controller: PatientApp.Controllers.PatientController,
                controllerAs: 'controller'
            })
            .state('patient3', {
                url: '/patient3',
                templateUrl: '/ngApp/views/patient3.html',
                controller: PatientApp.Controllers.PatientController,
                controllerAs: 'controller'
            })
            .state('delete', {
                url: '/patient/delete/:id',
                templateUrl: '/ngApp/views/deletePatient.html',
                controller: PatientApp.Controllers.PatientsController,
                controllerAs: 'controller'

            })
            .state('details', {
                url: '/details/:id',
                templateUrl: '/ngApp/views/patientDetails.html',
                controller: PatientApp.Controllers.PatientDetailsController,
                controllerAs: 'controller'
            })
            .state('nurseCreate', {
                url: '/nurse/create',
                templateUrl: '/ngApp/views/nurseCreate.html',
                controller: PatientApp.Controllers.NurseCreateController,
                controllerAs: 'controller'
            })
            .state('patientCreate', {
                url: '/patient/create',
                templateUrl: '/ngApp/views/patientCreate.html',
                controller: PatientApp.Controllers.PatientCreateController,
                controllerAs: 'controller'
            })
            .state('patientEdit', {
                url: '/patient/edit/:id',
                templateUrl: '/ngApp/views/patientEdit.html',
                controller: PatientApp.Controllers.PatientEditController,
                controllerAs: 'controller'
            })
            .state('myMessages', {
                url: '/myMessages',
                templateUrl: '/ngApp/views/myMessages.html',
                controller: PatientApp.Controllers.MyMessageController,
                controllerAs: 'controller'
            })
            .state('externalRegister', {
                url: '/externalRegister',
                templateUrl: '/ngApp/views/externalRegister.html',
                controller: PatientApp.Controllers.ExternalRegisterController,
                controllerAs: 'controller'
            })
            .state('chat', {
                url: '/chat',
                templateUrl: '/ngApp/views/chat.html',
                controller: PatientApp.Controllers.ChatController,
                controllerAs: 'controller'
            })
            .state('about', {
                url: '/about',
                templateUrl: '/ngApp/views/about.html',
                controller: PatientApp.Controllers.AboutController,
                controllerAs: 'controller'
            })
            .state('admitted', {
                url: '/admitted',
                templateUrl: '/ngApp/views/patientsAdmitted.html',
                controller: PatientApp.Controllers.PatientsController,
                controllerAs: 'controller'
            })
            .state('setupView', {
                url: '/setupView',
                templateUrl: '/ngApp/views/setupPatientView.html',
                controller: PatientApp.Controllers.PatientController,
                controllerAs: 'controller'
            })
            .state('dashboard', {
                url: '/dashboard',
                templateUrl: '/ngApp/views/dashboard.html',
                controller: PatientApp.Controllers.DashboardController,
                controllerAs: 'controller'
            })
            .state('nodeSetup', {
                url: '/nodeSetup',
                templateUrl: '/ngApp/views/nodeSetup.html',
                controller: PatientApp.Controllers.MyMessageController,
                controllerAs: 'controller'
            })

            .state('login', {
                url: '/login',
                templateUrl: '/ngApp/views/login.html',
                controller: PatientApp.Controllers.LoginController,
                controllerAs: 'controller'
            })
            .state('notFound', {
                url: '/notFound',
                templateUrl: '/ngApp/views/notFound.html'
            });
        //    .state('notFound', {
        //        url: '/notFound',
        //        templateUrl: '/ngApp/views/notFound.html'
        //    });

        ////// Handle request for non-existent route
        //$urlRouterProvider.otherwise('/notFound');

        // Enable HTML5 navigation
        $locationProvider.html5Mode(true);
    });


    angular.module('PatientApp').factory('authInterceptor', (
        $q: ng.IQService,
        $window: ng.IWindowService,
        $location: ng.ILocationService
    ) =>
        ({
            request: function (config) {
                config.headers = config.headers || {};
                config.headers['X-Requested-With'] = 'XMLHttpRequest';
                return config;
            },
            responseError: function (rejection) {
                if (rejection.status === 401 || rejection.status === 403) {
                    $location.path('/login');
                }
                return $q.reject(rejection);
            }
        })
    );

    angular.module('PatientApp').config(function ($httpProvider) {
        $httpProvider.interceptors.push('authInterceptor');
    });

}
