(function () {
    'use strict';

    /*global angular */

    agGrid.initialiseAgGridWithAngular1(angular);

    angular
        .module('app', ['ngRoute', 'ngCookies', 'toastr', 'agGrid'])
        .config(config)
        .run(run);
    
    
    config.$inject = ['$routeProvider', '$locationProvider'];

    function config($routeProvider, $locationProvider) {
        
        $routeProvider
            .when('/', {
                templateUrl: 'templates/home.html',
                controllerAs: 'vm'
            })
            .when('/login', {
                templateUrl: 'templates/login.html',
                controller: 'LoginController',
                controllerAs: 'vm'
            })
            .when('/register', {
                templateUrl: 'templates/register.html',
                controller: 'RegisterController',
                controllerAs: 'vm'
            })
            .when('/data', {
                templateUrl: 'templates/data.html',
                controller: 'dataCtrl',
                controllerAs: 'vm'
            })
            .when('/clients', {
                templateUrl: 'templates/clients.html',
                controller: 'clientsCtrl',
                controllerAs: 'vm'
            })
            .when('/clients/:id', {
                templateUrl: 'templates/client.html',
                controller: 'clientCtrl',
                controllerAs: 'vm'
            })
            .otherwise({ redirectTo: '/' });
    }


    run.$inject = ['$rootScope', '$location', '$cookies', '$http'];

    function run($rootScope, $location, $cookies, $http) {
        // keep user logged in after page refresh
        $rootScope.globals = $cookies.getObject('globals') || {};
        if ($rootScope.globals.currentUser) {
            $http.defaults.headers.common['Authorization'] = 'Basic ' + $rootScope.globals.currentUser.authdata;
        }

        $rootScope.$on('$locationChangeStart', function (event, next, current) {
            // redirect to login page if not logged in and trying to access a restricted page
            var restrictedPage = $.inArray($location.path(), ['/login', '/register']) === -1;
            var loggedIn = $rootScope.globals.currentUser;
            if (restrictedPage && !loggedIn) {
                $location.path('/login');
            }
        });
    }
    
})();