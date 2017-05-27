
    'use strict';

    /*global angular */
    var app = angular.module('app', ['ui.router', 'ngCookies'])
    .run(run);
    
    
    app.config(['$urlRouterProvider', '$locationProvider', '$stateProvider', configRoutes]);
    
//    app.controller('homeCtrl', homeCtrl);
//
//    function homeCtrl(){
//        
//    }

    function configRoutes($urlRouterProvider, $stateProvider) {
        $urlRouterProvider.otherwise('/home');

        $stateProvider
            .state('home', {
                url: '/home',
                templateUrl: '~/templates/home.html',
                controllerAs: 'vm'
            })
            .state('login', {
                url: '/login',
                templateUrl: '~/templates/login.html',
                controller: 'loginCtrl',
                controllerAs: 'vm'
            })
            .state('register', {
                url: '/register',
                templateUrl: '~/templates/register.html',
                controller: 'registerCtrl',
                controllerAs: 'vm'
            })
            .state('data', {
                url: '/data',
                templateUrl: '~/templates/data.html',
                controller: 'dataCtrl',
                controllerAs: 'vm'
            })
            .state('clients', {
                url: '/clients',
                templateUrl: '~/templates/clients.html',
                controller: 'clientsCtrl',
                controllerAs: 'vm'
            })
            .state('clients/:id', {
                url: '/clients/:id',
                templateUrl: "~/templates/client.html",
                controller: 'clientCtrl',
                controllerAs: 'vm'
            });
    }

    
    function configureStateWithScope($rootScope, $state) {
        $rootScope.$state = $state;
    }


run.$inject = ['$rootScope','$state', '$location', '$cookies', '$http',configureStateWithScope];
    function run($rootScope, $state, $location, $cookies, $http, configureStateWithScope) {
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
    